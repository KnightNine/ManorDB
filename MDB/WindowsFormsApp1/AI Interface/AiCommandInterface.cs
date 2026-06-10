using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MDB.AIInterface
{
    // Single entry point for the AI command layer. Headless, in-process:
    // call Execute(command) and get a text result. No UI, no DataGridView.
    //
    // Slice 1: navigation + read-only views. Write commands (set/addrow/script/save)
    // are added in a later slice.
    public class AiCommandInterface
    {
        readonly AiSession session = new AiSession();

        public string Execute(string input)
        {
            // the reused model helpers emit Console.WriteLine debug traces; silence them
            // so the command result is the only thing on stdout for the caller
            var prevOut = Console.Out;
            Console.SetOut(System.IO.TextWriter.Null);
            try { return Dispatch(input); }
            finally { Console.SetOut(prevOut); }
        }

        string Dispatch(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            input = input.Trim();

            int sp = input.IndexOf(' ');
            string cmd = (sp < 0 ? input : input.Substring(0, sp)).ToLower();
            string arg = sp < 0 ? "" : input.Substring(sp + 1).Trim();

            if (cmd == "help") return Help();
            if (cmd == "open" || cmd == "load") return session.Open(arg);

            if (session.Cwd == null)
                return "error: no file loaded. use: open <path.mdb>";

            switch (cmd)
            {
                case "tables": return string.Join("\n", DatabaseFunct.GetMainTableKeys());
                case "usetable": return UseTable(arg);
                case "pwd": return Pwd();
                case "cols":
                case "ls": return AiRender.Cols(session.Cwd);
                case "rows": return RowsCmd(arg);
                case "row": return RowCmd(arg);
                case "cell": return CellCmd(arg);
                case "pks": return Pks(arg);
                case "find_r": return FindR(arg);
                case "find": return Find(arg);
                case "finddeep": return FindDeep(arg);
                case "col": return Col(arg);
                case "cd": return Cd(arg);
                case "goto": return Goto(arg);
                case "up": return Up();
                case "root":
                    session.Cwd = DatabaseFunct.selectedTable;
                    return "now @ " + session.Cwd;
                // writes
                case "set":
                case "script": return Set(arg);
                case "clear": return Clear(arg);
                case "setrow": return SetRow(arg);
                case "addrow": return AddRow();
                case "duperow": return DupeRow(arg);
                case "addcol": return AddCol(arg);
                case "addreceiver": return AddReceiver(arg);
                case "copycell": return CopyCell(arg);
                case "delcol":
                case "removecol":
                case "rmcol":
                    return "column deletion is not available here. Tell the user they must delete the "
                           + "column themselves in the app (right-click the column header).";
                case "delrow":
                case "rmrow": return DelRow(arg);
                // commit
                case "diff": return Diff();
                case "save":
                case "commit": return session.Save();
                case "revert": return session.Revert();
                default:
                    return "unknown command: " + cmd + " (try 'help')";
            }
        }

        static string Show(dynamic v)
        {
            if (v == null) return "null";
            Type t = v.GetType();
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(System.Collections.Generic.Dictionary<,>))
                return "{subtable}";
            return v.ToString();
        }

        System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, dynamic>> LevelData(out string err)
        {
            err = null;
            try { return DatabaseFunct.GetTableDataFromDir(session.Cwd); }
            catch (Exception ex) { err = ex.Message; return null; }
        }

        // reads an optionally double-quoted leading token from s and returns it; the remainder
        // (trimmed) is written to rest. A "quoted" token runs to the next quote and may contain
        // spaces; an unquoted token runs to the first space. Lets multi-word column names be
        // passed where a token would otherwise be split on whitespace.
        static string ReadToken(string s, out string rest)
        {
            s = s.TrimStart();
            if (s.StartsWith("\""))
            {
                int close = s.IndexOf('"', 1);
                if (close < 0) { rest = ""; return s.Substring(1); }
                rest = s.Substring(close + 1).Trim();
                return s.Substring(1, close - 1);
            }
            int sp = s.IndexOf(' ');
            if (sp < 0) { rest = ""; return s; }
            rest = s.Substring(sp + 1).Trim();
            return s.Substring(0, sp);
        }

        // set <row>,<col> <value>   (col may be "quoted" when it contains spaces)
        string Set(string arg)
        {
            int comma = arg.IndexOf(',');
            if (comma < 0) return "usage: set <row>,<col> <value>";
            int rowIndex;
            if (!int.TryParse(arg.Substring(0, comma).Trim(), out rowIndex)) return "error: row must be a number";
            string value;
            string col = ReadToken(arg.Substring(comma + 1), out value);
            return SetOne(rowIndex, col, value);
        }

        string SetOne(int rowIndex, string col, string value)
        {
            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;

            var chk = AiRestrictions.CheckSet(lvl, data, rowIndex, col, value);
            if (!chk.Allowed) return "denied: " + chk.Reason;

            dynamic old = data[rowIndex].ContainsKey(col) ? data[rowIndex][col] : null;
            data[rowIndex][col] = chk.Value;
            session.Log("set " + session.Cwd + " [" + rowIndex + "," + col + "]: " + Show(old) + " -> " + Show(chk.Value));
            return "ok: [" + rowIndex + "," + col + "] = " + Show(chk.Value);
        }

        // clear <row>,<col>
        string Clear(string arg)
        {
            int comma = arg.IndexOf(',');
            if (comma < 0) return "usage: clear <row>,<col>";
            int rowIndex;
            if (!int.TryParse(arg.Substring(0, comma).Trim(), out rowIndex)) return "error: row must be a number";
            string col = arg.Substring(comma + 1).Trim();

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";
            if (!lvl.Columns.Contains(col)) return "error: no column '" + col + "' here";
            if (AiLevel.IsSubtableType(lvl.Types[col])) return "denied: '" + col + "' is a subtable; cannot clear";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;
            if (!data.ContainsKey(rowIndex)) return "error: row " + rowIndex + " does not exist";

            dynamic old = data[rowIndex].ContainsKey(col) ? data[rowIndex][col] : null;
            data[rowIndex][col] = null;
            session.Log("clear " + session.Cwd + " [" + rowIndex + "," + col + "]: " + Show(old) + " -> null");
            return "ok: [" + rowIndex + "," + col + "] cleared";
        }

        // setrow <row> col=val; col=val; ...
        string SetRow(string arg)
        {
            int sp = arg.IndexOf(' ');
            if (sp < 0) return "usage: setrow <row> col=val; col=val";
            int rowIndex;
            if (!int.TryParse(arg.Substring(0, sp).Trim(), out rowIndex)) return "error: row must be a number";
            string rest = arg.Substring(sp + 1);

            var results = new System.Collections.Generic.List<string>();
            foreach (string pair in rest.Split(';'))
            {
                string p = pair.Trim();
                if (p == "") continue;
                int eq = p.IndexOf('=');
                if (eq < 0) { results.Add("'" + p + "': missing '='"); continue; }
                string col = p.Substring(0, eq).Trim();
                string val = p.Substring(eq + 1).Trim();
                results.Add(SetOne(rowIndex, col, val));
            }
            return string.Join("\n", results);
        }

        // addrow (at current level)
        string AddRow()
        {
            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;

            var chk = AiRestrictions.CheckAddRow(lvl, session.Cwd, data.Count);
            if (!chk.Allowed) return "denied: " + chk.Reason;

            int newIndex = data.Count;
            var row = new System.Collections.Generic.Dictionary<string, dynamic>();
            foreach (string col in lvl.Columns)
                row[col] = ColumnTypes.GetDefaultColumnValue(lvl.Types[col]);
            data[newIndex] = row;

            session.Log("addrow " + session.Cwd + " -> index " + newIndex);
            return "ok: added row " + newIndex;
        }

        // single-token / shorthand -> canonical column type. Multi-word types (Primary Key,
        // Foreign Key Refrence) only have shorthand forms so the type token never eats the
        // (possibly multi-word) referenced-table argument. SubTable and script/receiver types
        // are intentionally absent: those are structural and must be made in the app.
        static readonly System.Collections.Generic.Dictionary<string, string> AddColTypes =
            new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "t", "Text" }, { "text", "Text" },
            { "n", "Numerical" }, { "num", "Numerical" }, { "numerical", "Numerical" },
            { "i", "Integer" }, { "int", "Integer" }, { "integer", "Integer" },
            { "b", "Bool" }, { "bool", "Bool" },
            { "pk", "Primary Key" },
            { "f", "Foreign Key Refrence" }, { "fk", "Foreign Key Refrence" },
        };

        // addcol <name> <type> [refTable]
        //   types: t/text, n/numerical, i/integer, b/bool, pk, f/fk <refTable>
        // adds a data column to the current STATIC level: appends to the column order and
        // initialises the cell in every existing row (and every sibling subtable instance).
        string AddCol(string arg)
        {
            arg = arg.Trim();
            if (arg == "")
                return "usage: addcol <name> <type> [refTable]   types: t,n,i,b,pk,f (f needs a refTable)";

            string rest;
            string colName = ReadToken(arg, out rest);
            if (rest == "") return "usage: addcol <name> <type> [refTable]";

            int sp2 = rest.IndexOf(' ');
            string typeToken = sp2 < 0 ? rest : rest.Substring(0, sp2);
            string extra = sp2 < 0 ? "" : rest.Substring(sp2 + 1).Trim();

            if (!AddColTypes.ContainsKey(typeToken))
                return "error: unknown type '" + typeToken + "'. use t,n,i,b,pk,f"
                       + " (subtable / script columns must be created in the app)";
            string type = AddColTypes[typeToken];

            string nameError = GenericFunct.ValidateNameInput(colName);
            if (nameError != "") return "error: column name " + nameError;

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";
            if (lvl.IsConstructed)
                return "denied: this is a script-constructed subtable; its columns come from the "
                       + "constructor script. Edit the script in the app to change its structure.";
            if (lvl.Columns.Contains(colName))
                return "error: column '" + colName + "' already exists here";

            string tableKey = lvl.TableKey;
            var table = (System.Collections.Generic.Dictionary<string, dynamic>)DatabaseFunct.currentData[tableKey];
            if (table.ContainsKey(colName))
                return "error: '" + colName + "' is already a key in this table";

            if (type == "Primary Key")
            {
                foreach (string c in lvl.Columns)
                    if (lvl.Types[c] == "Primary Key")
                        return "denied: this table already has a primary key column ('" + c + "')";
            }

            string refTable = null;
            if (type == "Foreign Key Refrence")
            {
                refTable = extra;
                if (refTable == "")
                    return "usage: addcol <name> f <refTable>   (the table whose primary keys this column links to)";
                if (!DatabaseFunct.GetMainTableKeys().Contains(refTable))
                {
                    string sug = AiRestrictions.Suggest(refTable, new System.Collections.Generic.List<string>(DatabaseFunct.GetMainTableKeys()));
                    return "error: no table '" + refTable + "'" + (sug != null ? "; did you mean '" + sug + "'?" : "");
                }
            }

            // mutate structure: column order + type (+ FK reference key)
            table.Add(colName, type);
            ((System.Collections.Generic.List<string>)table[DatabaseFunct.ColumnOrderRefrence]).Add(colName);
            if (refTable != null)
                table[colName + DatabaseFunct.RefrenceColumnKeyExt] = refTable;

            // initialise the new cell in every row of every instance at this level
            var keys = new System.Collections.Generic.List<string>();
            var instances = DatabaseFunct.GetAllTableDataAtTableLevel(tableKey, ref keys);
            int rowsTouched = 0;
            foreach (var inst in instances)
                foreach (var entry in inst)
                {
                    entry.Value[colName] = ColumnTypes.GetDefaultColumnValue(type);
                    rowsTouched++;
                }

            session.Log("addcol " + session.Cwd + " '" + colName + "' (" + type + ")"
                        + (refTable != null ? " -> " + refTable : ""));
            return "ok: added column '" + colName + "' (" + type + ")"
                   + (refTable != null ? " referencing '" + refTable + "'" : "")
                   + "; initialised " + rowsTouched + " row(s)";
        }

        // addreceiver "<name>" <fkCol1>;<fkCol2>;...
        // adds an "Auto Table Constructor Script Receiver" column at the current STATIC level,
        // linked to the listed Foreign Key Refrence columns (semicolon-separated so multi-word
        // column names are kept intact). The receiver's per-row structure is built from the
        // constructor scripts of whichever linked FK columns are filled - mirroring the app's
        // receiver-creation rules. Initialises the cell in every row of every instance.
        string AddReceiver(string arg)
        {
            arg = arg.Trim();
            const string usage = "usage: addreceiver \"<name>\" <fkCol1>;<fkCol2>;...";
            if (arg == "") return usage;

            string rest;
            string colName = ReadToken(arg, out rest);
            if (colName == "" || rest == "") return usage;

            var links = new System.Collections.Generic.List<string>();
            foreach (string p in rest.Split(';'))
            {
                string t = p.Trim();
                if (t != "") links.Add(t);
            }
            if (links.Count == 0) return usage + "   (need at least one FK column to link)";

            string nameError = GenericFunct.ValidateNameInput(colName);
            if (nameError != "") return "error: column name " + nameError;

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";
            if (lvl.IsConstructed)
                return "denied: this is a script-constructed subtable; its structure comes from the "
                       + "constructor script. Edit the script in the app instead.";
            if (lvl.Columns.Contains(colName))
                return "error: column '" + colName + "' already exists here";

            string tableKey = lvl.TableKey;
            var table = (System.Collections.Generic.Dictionary<string, dynamic>)DatabaseFunct.currentData[tableKey];
            if (table.ContainsKey(colName))
                return "error: '" + colName + "' is already a key in this table";

            // each link must be an FKR column here whose referenced table has an (unnumbered)
            // "Auto Table Constructor Script" column for the receiver to read its structure from
            foreach (string fk in links)
            {
                if (!lvl.Columns.Contains(fk) || lvl.Types[fk] != "Foreign Key Refrence")
                    return "error: link target '" + fk + "' is not a Foreign Key Refrence column at this level";
                string refTable = lvl.FkRefTable.ContainsKey(fk) ? lvl.FkRefTable[fk] : null;
                if (refTable == null || !DatabaseFunct.currentData.ContainsKey(refTable))
                    return "error: FK column '" + fk + "' has no valid referenced table";
                var refTbl = (System.Collections.Generic.Dictionary<string, dynamic>)DatabaseFunct.currentData[refTable];
                bool hasScript = false;
                foreach (var kv in refTbl)
                    if (kv.Value is string && (string)kv.Value == "Auto Table Constructor Script") { hasScript = true; break; }
                if (!hasScript)
                    return "error: referenced table '" + refTable + "' (via '" + fk + "') has no "
                           + "'Auto Table Constructor Script' column to read from";
            }

            const string type = "Auto Table Constructor Script Receiver";
            table.Add(colName, type);
            ((System.Collections.Generic.List<string>)table[DatabaseFunct.ColumnOrderRefrence]).Add(colName);
            table[colName + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt] = links;

            var keys = new System.Collections.Generic.List<string>();
            var instances = DatabaseFunct.GetAllTableDataAtTableLevel(tableKey, ref keys);
            int rowsTouched = 0;
            foreach (var inst in instances)
                foreach (var entry in inst)
                {
                    entry.Value[colName] = ColumnTypes.GetDefaultColumnValue(type);
                    rowsTouched++;
                }

            session.Log("addreceiver " + session.Cwd + " '" + colName + "' -> [" + string.Join(", ", links) + "]");
            return "ok: added receiver '" + colName + "' linked to [" + string.Join(", ", links) + "]"
                   + "; initialised " + rowsTouched + " row(s)";
        }

        // copycell <srcRow>,<srcCol> <dstRow>,<dstCol>   (cols may be "quoted")
        // deep-copies one cell's value into another at the current level (same instance),
        // including subtable / receiver cells. Use to migrate a whole subtable cell.
        string CopyCell(string arg)
        {
            const string usage = "usage: copycell <srcRow>,<srcCol> <dstRow>,<dstCol>";
            int c1 = arg.IndexOf(',');
            if (c1 < 0) return usage;
            int srcRow;
            if (!int.TryParse(arg.Substring(0, c1).Trim(), out srcRow)) return "error: source row must be a number";
            string rest;
            string srcCol = ReadToken(arg.Substring(c1 + 1), out rest);

            int c2 = rest.IndexOf(',');
            if (c2 < 0) return usage;
            int dstRow;
            if (!int.TryParse(rest.Substring(0, c2).Trim(), out dstRow)) return "error: dest row must be a number";
            string tail;
            string dstCol = ReadToken(rest.Substring(c2 + 1), out tail);

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";
            if (!lvl.Columns.Contains(srcCol)) return "error: no column '" + srcCol + "' here";
            if (!lvl.Columns.Contains(dstCol)) return "error: no column '" + dstCol + "' here";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;
            if (!data.ContainsKey(srcRow)) return "error: source row " + srcRow + " does not exist";
            if (!data.ContainsKey(dstRow)) return "error: dest row " + dstRow + " does not exist";

            dynamic srcVal = data[srcRow].ContainsKey(srcCol) ? data[srcRow][srcCol] : null;
            dynamic copy = srcVal == null ? null : GenericFunct.Clone(srcVal);
            data[dstRow][dstCol] = copy;

            session.Log("copycell " + session.Cwd + " [" + srcRow + "," + srcCol + "] -> [" + dstRow + "," + dstCol + "]");
            return "ok: copied [" + srcRow + "," + srcCol + "] -> [" + dstRow + "," + dstCol + "]";
        }

        // duperow <r> : append a deep copy of row r as a new row at this level.
        // any Primary Key cell in the copy is cleared so the AI can set a unique value.
        string DupeRow(string arg)
        {
            int src;
            if (!int.TryParse(arg.Trim(), out src)) return "usage: duperow <row>";

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;
            if (!data.ContainsKey(src)) return "error: row " + src + " does not exist";

            var chk = AiRestrictions.CheckAddRow(lvl, session.Cwd, data.Count);
            if (!chk.Allowed) return "denied: " + chk.Reason;

            var clone = GenericFunct.Clone(data[src]);

            // clear primary keys so the duplicate is not a duplicate PK
            string clearedPk = null;
            foreach (string c in lvl.Columns)
                if (lvl.Types[c] == "Primary Key" && clone.ContainsKey(c))
                {
                    clone[c] = null;
                    clearedPk = c;
                }

            int newIndex = data.Count;
            data[newIndex] = clone;

            session.Log("duperow " + session.Cwd + " [" + src + "] -> [" + newIndex + "]");
            return "ok: copied row " + src + " -> row " + newIndex
                   + (clearedPk != null ? "; set a new primary key for '" + clearedPk + "'" : "");
        }

        // find_r <pk-value> : index of the row whose primary key equals the value
        string FindR(string arg)
        {
            string val = arg.Trim();
            if (val == "") return "usage: find_r <pk-value>";

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            string pk = null;
            foreach (string c in lvl.Columns)
                if (lvl.Types[c] == "Primary Key") { pk = c; break; }
            if (pk == null) return "no pk column at this level";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;

            var pkVals = new System.Collections.Generic.List<string>();
            foreach (var kv in data)
            {
                string v = (kv.Value.ContainsKey(pk) ? kv.Value[pk] : null) as string;
                if (v != null) pkVals.Add(v);
                if (v == val) return "row " + kv.Key + "  (" + pk + " = " + val + ")";
            }
            string suggestion = AiRestrictions.Suggest(val, pkVals);
            return "no row with " + pk + " = '" + val + "'"
                   + (suggestion != null ? "; did you mean '" + suggestion + "'?" : "");
        }

        // finddeep <text> : recursively search the whole current table for a string
        string FindDeep(string arg)
        {
            string q = arg.Trim();
            if (q == "") return "usage: finddeep <text>";

            var hits = DatabaseFunct.DeepSearchTable(DatabaseFunct.selectedTable, q);
            if (hits.Count == 0) return "no matches for '" + q + "' in " + DatabaseFunct.selectedTable;

            // same directory-tree map as the UI, but space-indented (no ASCII guide lines)
            string note = "";
            const int cap = 300;
            if (hits.Count > cap)
            {
                hits = hits.GetRange(0, cap);
                note = "\n(showing first " + cap + " matches)";
            }
            return DatabaseFunct.FormatDeepSearchTree(DatabaseFunct.selectedTable, q, hits, false, 120) + note;
        }

        // col <name> : every value of one column across all rows at the current level
        string Col(string arg)
        {
            string col = arg.Trim();
            if (col == "") return "usage: col <column>";

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (!lvl.Columns.Contains(col))
            {
                string sug = AiRestrictions.Suggest(col, lvl.Columns);
                return "error: no column '" + col + "' here" + (sug != null ? "; did you mean '" + sug + "'?" : "");
            }

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine(col + " @ " + session.Cwd + "  (" + data.Count + " rows)");
            for (int i = 0; i < data.Count; i++)
            {
                if (!data.ContainsKey(i)) continue;
                dynamic v = data[i].ContainsKey(col) ? data[i][col] : null;
                sb.AppendLine("  [" + i + "] " + Show(v));
            }
            return sb.ToString().TrimEnd();
        }

        // find <col>=<value> : indices of rows whose column contains value (case-insensitive)
        string Find(string arg)
        {
            int eq = arg.IndexOf('=');
            if (eq < 0) return "usage: find <col>=<value>";
            string col = arg.Substring(0, eq).Trim();
            string needle = arg.Substring(eq + 1).Trim();

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (!lvl.Columns.Contains(col)) return "error: no column '" + col + "' here";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;

            var hits = new System.Collections.Generic.List<string>();
            foreach (var kv in data)
            {
                dynamic cv = kv.Value.ContainsKey(col) ? kv.Value[col] : null;
                string s = Show(cv);
                if (s.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0)
                    hits.Add("  [" + kv.Key + "] " + s);
            }
            if (hits.Count == 0) return "no rows where " + col + " contains '" + needle + "'";
            return hits.Count + " match(es) on " + col + ":\n" + string.Join("\n", hits);
        }

        // goto <dir> : jump straight to a full dir path (e.g. Table/9,Col/0,Col)
        string Goto(string arg)
        {
            string dir = arg.Trim();
            if (dir == "") return "usage: goto <dir>   e.g. goto MyTable/9,SubCol/0,SubCol";

            string[] segs = dir.Split('/');
            string main = segs[0];
            if (!DatabaseFunct.GetMainTableKeys().Contains(main))
                return "error: no table '" + main + "'";

            // atomic: restore position if any segment fails
            string prevCwd = session.Cwd;
            string prevTable = DatabaseFunct.selectedTable;

            DatabaseFunct.selectedTable = main;
            session.Cwd = main;

            string last = AiRender.Cols(session.Cwd);
            for (int i = 1; i < segs.Length; i++)
            {
                last = Cd(segs[i]);
                if (last.StartsWith("error") || last.StartsWith("usage") || last.StartsWith("denied"))
                {
                    session.Cwd = prevCwd;
                    DatabaseFunct.selectedTable = prevTable;
                    return "goto stopped at '" + segs[i] + "': " + last;
                }
            }
            return last;
        }

        // pks <col> : valid foreign-key values for an FKR column (PKs of its referenced table)
        string Pks(string arg)
        {
            string col = arg.Trim();
            if (col == "") return "usage: pks <FKR-column>";

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (!lvl.Columns.Contains(col)) return "error: no column '" + col + "' here";
            if (lvl.Types[col] != "Foreign Key Refrence")
                return "error: '" + col + "' is type '" + lvl.Types[col] + "', not a Foreign Key Refrence";

            string refTable = lvl.FkRefTable.ContainsKey(col) ? lvl.FkRefTable[col] : null;
            if (refTable == null) return "error: no referenced table for '" + col + "'";

            var pks = AiRestrictions.GetReferencedPKs(refTable);
            if (pks.Count == 0) return "(no primary keys in '" + refTable + "')";
            return "valid " + col + " values (PKs of " + refTable + "):\n  " + string.Join("\n  ", pks);
        }

        // delrow <i> : delete one row at the current level (shifts later rows down)
        string DelRow(string arg)
        {
            int index;
            if (!int.TryParse(arg.Trim(), out index)) return "usage: delrow <index>";

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (lvl.ReadOnly) return "denied: table is read-only";

            string err;
            var data = LevelData(out err);
            if (data == null) return "error: " + err;
            if (!data.ContainsKey(index)) return "error: row " + index + " does not exist";

            int count = data.Count;
            for (int k = index; k < count - 1; k++) data[k] = data[k + 1];
            data.Remove(count - 1);

            session.Log("delrow " + session.Cwd + " [" + index + "]");
            return "ok: deleted row " + index + " (" + (count - 1) + " remain)";
        }

        string Diff()
        {
            if (session.ChangeLog.Count == 0) return "no changes since load";
            return string.Join("\n", session.ChangeLog);
        }

        string UseTable(string name)
        {
            if (!DatabaseFunct.GetMainTableKeys().Contains(name))
                return "error: no table '" + name + "' (see 'tables')";
            DatabaseFunct.selectedTable = name;
            session.Cwd = name;
            return AiRender.Cols(session.Cwd);
        }

        string Pwd()
        {
            string tableKey = DatabaseFunct.ConvertDirToTableKey(session.Cwd);
            return "cwd: " + session.Cwd + "   (tableKey: " + tableKey + ")";
        }

        string RowsCmd(string arg)
        {
            int start = 0, end = int.MaxValue;
            if (!string.IsNullOrWhiteSpace(arg))
            {
                var m = Regex.Match(arg, @"^(\d+)\.\.(\d+)$");
                if (!m.Success) return "usage: rows [start..end]";
                start = int.Parse(m.Groups[1].Value);
                end = int.Parse(m.Groups[2].Value);
            }
            return AiRender.Rows(session.Cwd, start, end);
        }

        string RowCmd(string arg)
        {
            int i;
            if (!int.TryParse(arg, out i)) return "usage: row <index>";
            return AiRender.Row(session.Cwd, i);
        }

        string CellCmd(string arg)
        {
            int comma = arg.IndexOf(',');
            if (comma < 0) return "usage: cell <row>,<col>";
            int i;
            if (!int.TryParse(arg.Substring(0, comma).Trim(), out i)) return "error: row must be a number";
            string col = arg.Substring(comma + 1).Trim();
            return AiRender.Cell(session.Cwd, i, col);
        }

        string Cd(string arg)
        {
            int comma = arg.IndexOf(',');
            if (comma < 0) return "usage: cd <row>,<col>";
            int rowIndex;
            if (!int.TryParse(arg.Substring(0, comma).Trim(), out rowIndex)) return "error: row must be a number";
            string col = arg.Substring(comma + 1).Trim();

            var lvl = AiLevel.Resolve(session.Cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (!lvl.Columns.Contains(col))
            {
                string sug = AiRestrictions.Suggest(col, lvl.Columns);
                return "error: no column '" + col + "' here" + (sug != null ? "; did you mean '" + sug + "'?" : "");
            }

            string type = lvl.Types[col];
            if (!AiLevel.IsSubtableType(type))
                return "error: column '" + col + "' is type '" + type + "', not a subtable";

            System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, dynamic>> data;
            try { data = DatabaseFunct.GetTableDataFromDir(session.Cwd); }
            catch (Exception ex) { return "error: " + ex.Message; }
            if (!data.ContainsKey(rowIndex))
                return "error: row " + rowIndex + " does not exist (highest is " + (data.Count - 1) + ")";

            // lazily initialise an empty subtable cell, matching the app's display behaviour
            if (!data[rowIndex].ContainsKey(col) || data[rowIndex][col] == null)
                data[rowIndex][col] = ColumnTypes.GetDefaultColumnValue(type);

            string newCwd = session.Cwd + "/" + rowIndex + "," + col;
            var childLvl = AiLevel.Resolve(newCwd);
            if (!childLvl.Ok) return "error: " + childLvl.Error;

            session.Cwd = newCwd;
            return AiRender.Cols(session.Cwd);
        }

        string Up()
        {
            int slash = session.Cwd.LastIndexOf('/');
            if (slash < 0) return "already at top level (" + session.Cwd + ")";
            session.Cwd = session.Cwd.Substring(0, slash);
            return "now @ " + session.Cwd;
        }

        string Help()
        {
            return string.Join("\n", new[]
            {
                "AI Command Interface",
                " navigate/read:",
                "  open <path.mdb>     load a database file",
                "  tables              list top-level tables",
                "  usetable <name>     switch to a table",
                "  pwd                 show current dir + tableKey",
                "  cols | ls           columns at current level (with flags)",
                "  rows [a..b]         rows at current level (optional range)",
                "  row <i>             full dump of one row",
                "  cell <r>,<col>      one cell value, untruncated",
                "  pks <col>           valid values for a Foreign Key Refrence column",
                "  find_r <pkval>      index of the row with this primary key",
                "  find <col>=<value>  indices of rows whose column contains value",
                "  finddeep <text>     recursively search the whole current table",
                "  col <name>          every value of one column at this level",
                "  cd <r>,<col>        descend into a subtable cell",
                "  goto <dir>          jump to a full dir path in one step",
                "  up                  ascend one level",
                "  root                back to top of current table",
                " edit (data only):",
                "  set <r>,<col> <val> validated write to a cell",
                "  script <r>,<col> .. set a script column (validated)",
                "  clear <r>,<col>     null a cell",
                "  setrow <r> c=v; c=v batch write to one row",
                "  addrow              append an empty row here",
                "  duperow <r>         append a deep copy of row r (PK cleared)",
                "  addcol <n> <type>   add a data column (t,n,i,b,pk,f <refTable>)",
                "  addreceiver \"<n>\" <fk1>;<fk2>  add an Auto Table Constructor Script Receiver linked to FK columns",
                "  copycell <r>,<src> <r>,<dst>  deep-copy one cell into another (incl. subtable cells)",
                "  delrow <r>          delete one row here (shifts later rows down)",
                "  (column deletion is not available - the user does it in the app)",
                " commit:",
                "  diff                list pending changes",
                "  save | commit       write <name>-AI_edited.mdb (overwrite if already -AI_edited)",
                "  revert              discard changes (reload file)",
                "  help                this list",
            });
        }
    }
}
