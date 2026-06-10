using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MDB.AIInterface
{
    // The single gate run before any data write. Grid-free reimplementation of the
    // type/PK rules in ColumnTypes.ValidateCellInput plus disabler, read-only and
    // row-restriction enforcement. Returns a result instead of popping a dialog.
    internal static class AiRestrictions
    {
        internal class Result
        {
            public bool Allowed;
            public string Reason;
            public dynamic Value;   // coerced value to write when Allowed
        }

        static Result Deny(string reason) { return new Result { Allowed = false, Reason = reason }; }
        static Result Ok(dynamic value) { return new Result { Allowed = true, Value = value }; }

        static readonly Regex ScriptColRegex = new Regex(@"^Auto Table Constructor Script\d*$");

        // validate + gate a single-cell write
        internal static Result CheckSet(LevelInfo lvl, Dictionary<int, Dictionary<string, dynamic>> tableData,
                                        int rowIndex, string col, string raw)
        {
            if (lvl.ReadOnly) return Deny("table is read-only");
            if (!lvl.Columns.Contains(col)) return Deny("no column '" + col + "' at this level");
            string type = lvl.Types[col];

            if (AiLevel.IsSubtableType(type))
                return Deny("'" + col + "' is a " + type + "; use 'cd' to enter it, not 'set'");

            if (!tableData.ContainsKey(rowIndex)) return Deny("row " + rowIndex + " does not exist");
            var kvRow = new KeyValuePair<int, Dictionary<string, dynamic>>(rowIndex, tableData[rowIndex]);

            // adjacent-column disabler gate
            string[] disablerArr = lvl.IsConstructed
                ? AutoTableConstructorScriptFunct.GetColumnDisablerArrayFromConstructedTableDisablerArrays(col, lvl.DisablerArrays)
                : null;
            if (DatabaseFunct.IsDataRowCellStillDisabled(lvl.TableKey, kvRow, col, disablerArr))
            {
                string by = null;
                foreach (string p in AiLevel.DisablerPartners(lvl, col))
                    if (kvRow.Value.ContainsKey(p) && DatabaseFunct.DoesDataRowCellContainData(kvRow, p)) { by = p; break; }
                return Deny("cell is disabled by '" + (by ?? "an adjacent column") + "' (clear it first)");
            }

            return ValidateValue(lvl, tableData, col, type, raw);
        }

        static Result ValidateValue(LevelInfo lvl, Dictionary<int, Dictionary<string, dynamic>> tableData,
                                    string col, string type, string raw)
        {
            bool empty = string.IsNullOrEmpty(raw);

            if (type == "Primary Key")
            {
                if (empty) return Ok(null);
                foreach (var e in tableData)
                    if (e.Value.ContainsKey(col) && (e.Value[col] as string) == raw)
                        return Deny("duplicate primary key '" + raw + "' at row " + e.Key);
                return Ok(raw);
            }
            if (type == "Numerical")
            {
                if (empty) return Ok(null);
                double d;
                if (!double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                    return Deny("'" + raw + "' is not a numerical value");
                return Ok(d);
            }
            if (type == "Integer")
            {
                if (empty) return Ok(null);
                double d;
                if (!double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                    return Deny("'" + raw + "' is not an integer");
                return Ok((int)d);
            }
            if (type == "Bool")
            {
                if (empty) return Ok(false);
                bool b;
                if (bool.TryParse(raw, out b)) return Ok(b);
                string r = raw.Trim().ToLower();
                if (r == "1" || r == "yes" || r == "y" || r == "t") return Ok(true);
                if (r == "0" || r == "no" || r == "n" || r == "f") return Ok(false);
                return Deny("'" + raw + "' is not a bool (true/false)");
            }
            if (type == "Foreign Key Refrence")
            {
                if (empty) return Ok(null);
                string refTable = lvl.FkRefTable.ContainsKey(col) ? lvl.FkRefTable[col] : null;
                if (refTable == null || !DatabaseFunct.currentData.ContainsKey(refTable))
                    return Deny("foreign key target table not found");
                var pks = GetReferencedPKs(refTable);
                if (!pks.Contains(raw))
                {
                    string msg = "'" + raw + "' is not a primary key in '" + refTable + "'";
                    string suggestion = Suggest(raw, pks);
                    if (suggestion != null) msg += "; did you mean '" + suggestion + "'?";
                    return Deny(msg);
                }
                return Ok(raw);
            }
            if (ScriptColRegex.IsMatch(type))
            {
                if (empty) return Ok(null);
                string err = AutoTableConstructorScriptFunct.ValidateScript(raw);
                if (err != "") return Deny("script error: " + err);
                return Ok(raw);
            }
            if (type == "Parent Subtable Foreign Key Refrence")
            {
                // a link to a parent subtable PK; accepted as-is (not deep-validated in this slice)
                return Ok(empty ? null : raw);
            }
            // Text and any other text-backed type
            return Ok(empty ? null : raw);
        }

        // gate appending a row at the current level
        internal static Result CheckAddRow(LevelInfo lvl, string cwd, int currentRowCount)
        {
            if (lvl.ReadOnly) return Deny("table is read-only");

            int restrict = 0;
            if (lvl.IsConstructed)
            {
                restrict = lvl.SingleRowRestriction ? 1 : 0;
            }
            else if (cwd.Contains("/"))
            {
                // static subtable: restriction lives on the parent table under the subtable column
                int lastSlash = cwd.LastIndexOf('/');
                string parentDir = cwd.Substring(0, lastSlash);
                string lastSeg = cwd.Substring(lastSlash + 1);
                int comma = lastSeg.IndexOf(',');
                string col = comma >= 0 ? lastSeg.Substring(comma + 1) : lastSeg;
                string parentKey = DatabaseFunct.ConvertDirToTableKey(parentDir);
                if (DatabaseFunct.currentData.ContainsKey(parentKey)
                    && ((Dictionary<string, dynamic>)DatabaseFunct.currentData[parentKey]).ContainsKey(col + DatabaseFunct.SubtableRowRestrictionExt))
                {
                    restrict = (int)DatabaseFunct.currentData[parentKey][col + DatabaseFunct.SubtableRowRestrictionExt];
                }
            }

            if (restrict != 0 && currentRowCount >= restrict)
                return Deny("row restriction reached (" + restrict + ")");

            return Ok(null);
        }

        // all primary-key values a Foreign Key Refrence column may point at
        internal static List<string> GetReferencedPKs(string refTable)
        {
            var result = new List<string>();
            if (!DatabaseFunct.currentData.ContainsKey(refTable)) return result;

            var table = (Dictionary<string, dynamic>)DatabaseFunct.currentData[refTable];
            string pkCol = null;
            foreach (var kv in table)
                if (kv.Value is string && (string)kv.Value == "Primary Key") { pkCol = kv.Key; break; }
            if (pkCol == null) return result;

            var data = (Dictionary<int, Dictionary<string, dynamic>>)table[DatabaseFunct.RowEntryRefrence];
            foreach (var row in data.Values)
            {
                string v = (row.ContainsKey(pkCol) ? row[pkCol] : null) as string;
                if (v != null) result.Add(v);
            }
            return result;
        }

        // similarity needed before suggesting a near-match PK on a failed FK write.
        // NOTE: a literal ">90%" rarely fires for common typos (e.g. "Order"/"Orders" is
        // ~0.83), so this is set lower to actually be helpful; change here to retune.
        const double SuggestionThreshold = 0.6;

        internal static string Suggest(string input, List<string> candidates)
        {
            string best = null;
            double bestScore = 0;
            foreach (string c in candidates)
            {
                double s = Similarity(input, c);
                if (s > bestScore) { bestScore = s; best = c; }
            }
            return bestScore >= SuggestionThreshold ? best : null;
        }

        static double Similarity(string a, string b)
        {
            string x = a.ToLower(), y = b.ToLower();
            if (x == y) return 1.0;
            int max = Math.Max(x.Length, y.Length);
            if (max == 0) return 1.0;
            return 1.0 - (double)Levenshtein(x, y) / max;
        }

        static int Levenshtein(string a, string b)
        {
            int[,] d = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) d[0, j] = j;
            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            return d[a.Length, b.Length];
        }
    }
}
