using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MDB.AIInterface
{
    // Read-only, grid-free text views of the model at a given dir level.
    // Structure comes from AiLevel.Resolve (static or script-constructed); data comes
    // from DatabaseFunct.GetTableDataFromDir.
    internal static class AiRender
    {
        // the script-holding text column (e.g. "Auto Table Constructor Script", "...Script2")
        static readonly Regex ScriptColRegex = new Regex(@"^Auto Table Constructor Script\d*$");

        // the Primary Key column at a level, or null
        static string PrimaryKeyColumn(LevelInfo lvl)
        {
            foreach (string col in lvl.Columns)
                if (lvl.Types[col] == "Primary Key") return col;
            return null;
        }

        // compact one-line rendering of a cell value
        static string CellString(dynamic v)
        {
            if (v == null) return "null";
            Type t = v.GetType();
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                int n = 0;
                try { n = ((ICollection)v).Count; } catch { }
                return "{subtable: " + n + " rows}";
            }
            return v.ToString();
        }

        internal static string Cols(string cwd)
        {
            var lvl = AiLevel.Resolve(cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            var sb = new StringBuilder();
            sb.AppendLine("cols @ " + cwd
                + (lvl.ReadOnly ? "   [READ-ONLY table]" : "")
                + (lvl.IsConstructed ? "   [script-constructed]" : "")
                + (lvl.SingleRowRestriction ? "   [single-row]" : ""));
            if (lvl.Columns.Count == 0)
                return sb.ToString().TrimEnd() + "\n  (no columns" + (lvl.Error != null ? ": " + lvl.Error : "") + ")";

            foreach (string col in lvl.Columns)
            {
                string type = lvl.Types[col];
                var flags = new List<string>();
                if (type == "Primary Key") flags.Add("PK");
                if (AiLevel.IsSubtableType(type)) flags.Add("subtable");
                if (ScriptColRegex.IsMatch(type)) flags.Add("script");
                if (lvl.DisablerSourceCols.Contains(col)) flags.Add("disabler-source");
                string flagStr = flags.Count > 0 ? "  [" + string.Join(",", flags) + "]" : "";

                // expose "what influences what": FK target, receiver's driving FK column(s),
                // and disabler partners (mutually-exclusive columns)
                var notes = new List<string>();
                if (type == "Foreign Key Refrence")
                    notes.Add("references " + (lvl.FkRefTable.ContainsKey(col) ? lvl.FkRefTable[col] : "?"));
                if (lvl.ReceiverLinks.ContainsKey(col))
                {
                    var links = lvl.ReceiverLinks[col];
                    notes.Add(links.Count > 0
                        ? "structure from FK: " + string.Join(", ", links)
                        : "structure from FK: (none linked)");
                }
                var partners = AiLevel.DisablerPartners(lvl, col);
                if (partners.Count > 0)
                    notes.Add("exclusive with: " + string.Join(", ", partners));
                string noteStr = notes.Count > 0 ? "   (" + string.Join("; ", notes) + ")" : "";

                sb.AppendLine("  - " + col + " : " + type + flagStr + noteStr);
            }
            return sb.ToString().TrimEnd();
        }

        internal static string Rows(string cwd, int start, int end)
        {
            var lvl = AiLevel.Resolve(cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            Dictionary<int, Dictionary<string, dynamic>> data;
            try { data = DatabaseFunct.GetTableDataFromDir(cwd); }
            catch (Exception ex) { return "error reading rows: " + ex.Message; }

            string pk = PrimaryKeyColumn(lvl);
            string previewCol = pk ?? (lvl.Columns.Count > 0 ? lvl.Columns[0] : null);

            var sb = new StringBuilder();
            sb.AppendLine("rows @ " + cwd + "  (" + data.Count + " total)");
            for (int i = start; i <= end && i < data.Count; i++)
            {
                if (!data.ContainsKey(i)) continue;
                var rowData = data[i];
                int filled = lvl.Columns.Count(c => rowData.ContainsKey(c) && rowData[c] != null);
                string preview = (previewCol != null && rowData.ContainsKey(previewCol))
                    ? CellString(rowData[previewCol]) : "";
                sb.AppendLine("  [" + i + "] " + preview + "   (" + filled + "/" + lvl.Columns.Count + " filled)");
            }
            return sb.ToString().TrimEnd();
        }

        // full dump of one row at the current level
        internal static string Row(string cwd, int index)
        {
            var lvl = AiLevel.Resolve(cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;

            Dictionary<int, Dictionary<string, dynamic>> data;
            try { data = DatabaseFunct.GetTableDataFromDir(cwd); }
            catch (Exception ex) { return "error: " + ex.Message; }
            if (!data.ContainsKey(index))
                return "error: row " + index + " does not exist (rows 0.." + (data.Count - 1) + ")";

            var rowData = data[index];
            var kv = new KeyValuePair<int, Dictionary<string, dynamic>>(index, rowData);
            var sb = new StringBuilder();
            sb.AppendLine("row [" + index + "] @ " + cwd);
            foreach (string col in lvl.Columns)
            {
                dynamic val = rowData.ContainsKey(col) ? rowData[col] : null;
                string by = DisabledBy(lvl, kv, col);
                string status = by != null ? "   [DISABLED by: " + by + "]" : "";
                sb.AppendLine("  " + col + " (" + lvl.Types[col] + "): " + CellString(val) + status);
            }
            return sb.ToString().TrimEnd();
        }

        // the disabler partner currently locking this cell in this row, or null if enabled.
        // A cell is disabled when a mutually-exclusive sibling column holds data.
        static string DisabledBy(LevelInfo lvl, KeyValuePair<int, Dictionary<string, dynamic>> kv, string col)
        {
            foreach (string p in AiLevel.DisablerPartners(lvl, col))
                if (kv.Value.ContainsKey(p) && DatabaseFunct.DoesDataRowCellContainData(kv, p))
                    return p;
            return null;
        }

        // single cell value, untruncated
        internal static string Cell(string cwd, int index, string col)
        {
            var lvl = AiLevel.Resolve(cwd);
            if (!lvl.Ok) return "error: " + lvl.Error;
            if (!lvl.Columns.Contains(col))
                return "error: no column '" + col + "' at this level";

            Dictionary<int, Dictionary<string, dynamic>> data;
            try { data = DatabaseFunct.GetTableDataFromDir(cwd); }
            catch (Exception ex) { return "error: " + ex.Message; }
            if (!data.ContainsKey(index))
                return "error: row " + index + " does not exist (highest is " + (data.Count - 1) + ")";

            dynamic val = data[index].ContainsKey(col) ? data[index][col] : null;
            return CellString(val);
        }
    }
}
