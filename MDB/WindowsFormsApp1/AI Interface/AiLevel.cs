using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MDB.AIInterface
{
    // Uniform structure of a single table level (columns + types + descent info),
    // whether the level is static (defined in currentData) or script-constructed
    // (defined by an Auto Table Constructor Script).
    internal class LevelInfo
    {
        public bool Ok = true;
        public string Error;                 // reason when !Ok, or note for an empty level
        public bool IsConstructed;           // derived from a script vs static currentData
        public bool ReadOnly;                // regex-reference table
        public bool SingleRowRestriction;
        public string TableKey;              // structural key (static) or dir-derived display key

        public List<string> Columns = new List<string>();
        public Dictionary<string, string> Types = new Dictionary<string, string>();

        // descent metadata
        public Dictionary<string, string> SubtableScript = new Dictionary<string, string>();     // S column -> subscript (constructed)
        public Dictionary<string, List<string>> ReceiverLinks = new Dictionary<string, List<string>>();
        public Dictionary<string, int> ReceiverTypeIndex = new Dictionary<string, int>();
        public Dictionary<string, string> FkRefTable = new Dictionary<string, string>();          // F column -> referenced table
        public List<string[]> DisablerArrays = new List<string[]>();                              // constructed-level disablers
        public HashSet<string> DisablerSourceCols = new HashSet<string>();                         // static disabler-source columns
    }

    // Resolves the LevelInfo at any dir level and supports script-constructed subtables.
    internal static class AiLevel
    {
        static readonly Regex ReceiverColRegex = new Regex(@"^Auto Table Constructor Script Receiver\d*$");

        internal static bool IsSubtableType(string type)
        {
            return type == "SubTable" || ReceiverColRegex.IsMatch(type);
        }

        // columns mutually exclusive with `col` at this level (its disabler partners; filling one
        // locks/clears the others), or an empty list. Works for static and constructed levels.
        internal static List<string> DisablerPartners(LevelInfo lvl, string col)
        {
            var result = new List<string>();
            if (lvl.IsConstructed)
            {
                string[] arr = AutoTableConstructorScriptFunct
                    .GetColumnDisablerArrayFromConstructedTableDisablerArrays(col, lvl.DisablerArrays);
                if (arr != null)
                    foreach (string c in arr)
                        if (c != col && !result.Contains(c)) result.Add(c);
            }
            else if (DatabaseFunct.currentData.ContainsKey(lvl.TableKey))
            {
                var table = (Dictionary<string, dynamic>)DatabaseFunct.currentData[lvl.TableKey];
                string key = col + DatabaseFunct.ColumnDisablerArrayExt;
                if (table.ContainsKey(key) && table[key] != null)
                    foreach (var c in (IEnumerable)table[key])
                    {
                        string s = c as string;
                        if (s != null && s != col && !result.Contains(s)) result.Add(s);
                    }
            }
            return result;
        }

        static LevelInfo Fail(string msg)
        {
            return new LevelInfo { Ok = false, Error = msg };
        }

        // walk the dir path from the main table down, resolving each level
        internal static LevelInfo Resolve(string cwd)
        {
            string[] segs = cwd.Split('/');
            string mainTable = segs[0];
            if (!DatabaseFunct.currentData.ContainsKey(mainTable))
                return Fail("no table '" + mainTable + "'");

            LevelInfo current = StaticLevel(mainTable);
            string dirSoFar = mainTable;

            for (int s = 1; s < segs.Length; s++)
            {
                int comma = segs[s].IndexOf(',');
                if (comma < 0) return Fail("malformed dir segment '" + segs[s] + "'");
                int rowIndex;
                if (!int.TryParse(segs[s].Substring(0, comma), out rowIndex))
                    return Fail("malformed row index in '" + segs[s] + "'");
                string col = segs[s].Substring(comma + 1);

                if (!current.Columns.Contains(col))
                    return Fail("no column '" + col + "' at " + dirSoFar);
                string type = current.Types[col];

                Dictionary<int, Dictionary<string, dynamic>> curData;
                try { curData = DatabaseFunct.GetTableDataFromDir(dirSoFar); }
                catch (Exception ex) { return Fail(ex.Message); }
                if (!curData.ContainsKey(rowIndex))
                    return Fail("row " + rowIndex + " missing at " + dirSoFar);
                var rowData = curData[rowIndex];

                LevelInfo child;
                string childKey = DatabaseFunct.ConvertDirToTableKey(dirSoFar) + "/" + col;

                if (type == "SubTable")
                {
                    if (current.IsConstructed)
                        child = ParseScriptLevel(current.SubtableScript.ContainsKey(col) ? current.SubtableScript[col] : "", childKey);
                    else
                        child = StaticLevel(current.TableKey + "/" + col);
                }
                else if (ReceiverColRegex.IsMatch(type))
                {
                    List<string> links = current.ReceiverLinks.ContainsKey(col) ? current.ReceiverLinks[col] : new List<string>();
                    int typeIndex = current.ReceiverTypeIndex.ContainsKey(col) ? current.ReceiverTypeIndex[col] : -1;
                    string script = FetchReceiverScript(rowData, links, typeIndex, current);
                    if (string.IsNullOrEmpty(script))
                    {
                        child = new LevelInfo { Ok = true, IsConstructed = true, TableKey = childKey,
                            Error = "receiver '" + col + "' has no linked script (set its foreign key first)" };
                    }
                    else
                    {
                        child = ParseScriptLevel(script, childKey);
                    }
                }
                else
                {
                    return Fail("column '" + col + "' is type '" + type + "', not a subtable");
                }

                if (!child.Ok) return child;
                current = child;
                dirSoFar = dirSoFar + "/" + rowIndex + "," + col;
            }

            return current;
        }

        // static level: columns + metadata from currentData[tableKey]
        static LevelInfo StaticLevel(string tableKey)
        {
            var info = new LevelInfo { Ok = true, IsConstructed = false, TableKey = tableKey };
            if (!DatabaseFunct.currentData.ContainsKey(tableKey))
            {
                info.Ok = false;
                info.Error = "no static structure for '" + tableKey + "'";
                return info;
            }

            var table = (Dictionary<string, dynamic>)DatabaseFunct.currentData[tableKey];
            info.ReadOnly = table.ContainsKey(DatabaseFunct.RegexRefrenceTableConstructorDataRefrence);
            List<string> order = table[DatabaseFunct.ColumnOrderRefrence];

            foreach (string col in order)
            {
                dynamic t = table[col];
                if (!(t is string) || !ColumnTypes.Types.ContainsKey((string)t)) continue;
                string type = (string)t;

                info.Columns.Add(col);
                info.Types[col] = type;

                if (table.ContainsKey(col + DatabaseFunct.ColumnDisablerArrayExt))
                    info.DisablerSourceCols.Add(col);

                if (ReceiverColRegex.IsMatch(type))
                {
                    var refs = new List<string>();
                    string linkKey = col + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt;
                    if (table.ContainsKey(linkKey))
                    {
                        dynamic ld = table[linkKey];
                        if (ld is string) refs.Add((string)ld);
                        else if (ld != null) foreach (var x in (IEnumerable)ld) refs.Add((string)x);
                    }
                    info.ReceiverLinks[col] = refs;
                    info.ReceiverTypeIndex[col] = TrailingIndex(type);
                }
                else if (type == "Foreign Key Refrence")
                {
                    string refKey = col + DatabaseFunct.RefrenceColumnKeyExt;
                    if (table.ContainsKey(refKey)) info.FkRefTable[col] = (string)table[refKey];
                }
            }
            return info;
        }

        // constructed level: columns + metadata parsed from a constructor script
        static LevelInfo ParseScriptLevel(string script, string tableKey)
        {
            var info = new LevelInfo { Ok = true, IsConstructed = true, TableKey = tableKey };
            if (string.IsNullOrEmpty(script))
            {
                info.Error = "empty constructed subtable";
                return info;
            }

            script = AutoTableConstructorScriptFunct.LoadScriptPefabs(script);
            var dat = AutoTableConstructorScriptFunct.FetchTopLevelScriptData(script);
            MatchCollection subtableScripts = dat.Item1;
            info.SingleRowRestriction = dat.Item2;
            string[] columnScripts = dat.Item3;
            info.DisablerArrays = dat.Item4;

            int subIdx = 0;
            foreach (string columnScript in columnScripts)
            {
                if (string.IsNullOrWhiteSpace(columnScript)) continue;
                string[] parts = columnScript.Split(':');
                if (parts.Length < 2) continue;

                string name = Regex.Match(parts[0], @"(?<=\<)[^<>]*(?=\>)").Value;
                string shorthand = parts[1].Trim();
                if (name == "" || !AutoTableConstructorScriptFunct.columnTypeShorthandDict.ContainsKey(shorthand)) continue;
                string type = AutoTableConstructorScriptFunct.columnTypeShorthandDict[shorthand];

                info.Columns.Add(name);
                info.Types[name] = type;

                if (type == "SubTable")
                {
                    if (subIdx < subtableScripts.Count)
                        info.SubtableScript[name] = subtableScripts[subIdx].Value;
                    subIdx++;
                }
                else if (ReceiverColRegex.IsMatch(type))
                {
                    var refs = new List<string>();
                    if (parts.Length > 2)
                        foreach (Match m in Regex.Matches(parts[2].Trim(), @"(?<=\<)[^<>]*(?=\>)"))
                            refs.Add(m.Value);
                    info.ReceiverLinks[name] = refs;
                    info.ReceiverTypeIndex[name] = TrailingIndex(type);
                }
                else if (type == "Foreign Key Refrence")
                {
                    if (parts.Length > 2)
                        info.FkRefTable[name] = Regex.Match(parts[2].Trim(), @"(?<=\<)[^<>]*(?=\>)").Value;
                }
            }
            return info;
        }

        // grid-free equivalent of FetchTableConstructorScriptForReceiverColumn's currentData path:
        // resolve a receiver cell's merged constructor script from its linked foreign keys
        static string FetchReceiverScript(Dictionary<string, dynamic> rowData, List<string> links, int typeIndex, LevelInfo current)
        {
            var scriptsToMerge = new List<string>();
            string scriptColType = "Auto Table Constructor Script" + (typeIndex == -1 ? "" : typeIndex.ToString());

            foreach (string link in links)
            {
                if (!current.FkRefTable.ContainsKey(link)) continue;
                string refTable = current.FkRefTable[link];
                string pkSel = (rowData.ContainsKey(link) ? rowData[link] : null) as string;
                if (pkSel == null) continue;
                if (!DatabaseFunct.currentData.ContainsKey(refTable)) continue;

                string pkCol = null, scriptCol = null;
                foreach (KeyValuePair<string, dynamic> kv in (Dictionary<string, dynamic>)DatabaseFunct.currentData[refTable])
                {
                    if (kv.Value is string)
                    {
                        if ((string)kv.Value == "Primary Key") pkCol = kv.Key;
                        else if ((string)kv.Value == scriptColType) scriptCol = kv.Key;
                    }
                }
                if (pkCol == null || scriptCol == null) continue;

                var refData = (Dictionary<int, Dictionary<string, dynamic>>)DatabaseFunct.currentData[refTable][DatabaseFunct.RowEntryRefrence];
                for (int i = 0; i < refData.Count; i++)
                {
                    if (!refData.ContainsKey(i)) continue;
                    string rowPk = (refData[i].ContainsKey(pkCol) ? refData[i][pkCol] : null) as string;
                    if (rowPk == pkSel)
                    {
                        string sc = (refData[i].ContainsKey(scriptCol) ? refData[i][scriptCol] : null) as string;
                        if (!string.IsNullOrEmpty(sc) && AutoTableConstructorScriptFunct.ValidateScript(sc) == "")
                            scriptsToMerge.Add(sc);
                        break;
                    }
                }
            }

            string merged = AutoTableConstructorScriptFunct.MergeScripts(scriptsToMerge.ToArray());
            if (AutoTableConstructorScriptFunct.ValidateScript(merged) != "") return "";
            return merged;
        }

        static int TrailingIndex(string type)
        {
            return Regex.IsMatch(type, @"\d+$") ? int.Parse(Regex.Match(type, @"\d+$").Value) : -1;
        }
    }
}
