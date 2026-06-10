using System;
using System.Collections.Generic;
using System.Text;

namespace MDB
{
    public static partial class DatabaseFunct
    {
        // One location where a search string was found during a deep search.
        public class DeepSearchHit
        {
            public string Dir;        // dir string of the table level, e.g. "Main/9,Col/0,Sub"
            public int RowIndex;
            public string Column;
            public string Value;      // the matched cell's string form
        }

        // Recursively search every subtable/column/row under one main table for a string.
        // Walks the nested row data directly (a subtable cell is a
        // Dictionary<int,Dictionary<string,dynamic>>), so it needs no column metadata and
        // works for both static and script-constructed subtables. Case-insensitive.
        public static List<DeepSearchHit> DeepSearchTable(string mainTableKey, string query)
        {
            var hits = new List<DeepSearchHit>();
            if (string.IsNullOrEmpty(query) || !currentData.ContainsKey(mainTableKey)) return hits;

            var rootData = currentData[mainTableKey][RowEntryRefrence] as Dictionary<int, Dictionary<string, dynamic>>;
            if (rootData == null) return hits;

            DeepSearchWalk(mainTableKey, rootData, query, hits);
            return hits;
        }

        static void DeepSearchWalk(string dir, Dictionary<int, Dictionary<string, dynamic>> tableData,
                                   string query, List<DeepSearchHit> hits)
        {
            foreach (KeyValuePair<int, Dictionary<string, dynamic>> rowKv in tableData)
            {
                foreach (KeyValuePair<string, dynamic> cell in rowKv.Value)
                {
                    dynamic val = cell.Value;
                    if (val is Dictionary<int, Dictionary<string, dynamic>>)
                    {
                        DeepSearchWalk(dir + "/" + rowKv.Key + "," + cell.Key, val, query, hits);
                    }
                    else if (val != null)
                    {
                        string s = val.ToString();
                        if (s.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                            hits.Add(new DeepSearchHit { Dir = dir, RowIndex = rowKv.Key, Column = cell.Key, Value = s });
                    }
                }
            }
        }

        // ----- directory-tree rendering of deep-search hits -----
        // A node in the merged path tree. Three kinds: a row (a row in some table), a column
        // (a subtable column descended into), or a leaf column (the column the match was in,
        // holding the matched [row]->value entries).
        class LeafEntry
        {
            public int Row;
            public string Pk;     // primary key of this row in the matched table, if any
            public string Value;
        }

        class DeepNode
        {
            public string ColName;                              // for column / leaf nodes
            public int RowIndex = -1;                           // >= 0 for row nodes (sort key)
            public string Pk;                                   // primary-key value of a row node, if any
            public bool IsLeaf;                                 // a |column| node with match entries
            public int Order;                                  // first-seen order (tie-break for sorting)
            public List<LeafEntry> Entries;                     // leaf only
            public readonly List<DeepNode> Children = new List<DeepNode>();
            readonly Dictionary<string, DeepNode> index = new Dictionary<string, DeepNode>();
            int counter;

            public DeepNode Child(string key, string colName, int rowIndex, string pk)
            {
                DeepNode n;
                if (index.TryGetValue(key, out n)) return n;
                n = new DeepNode { ColName = colName, RowIndex = rowIndex, Pk = pk, Order = counter++ };
                index[key] = n;
                Children.Add(n);
                return n;
            }
        }

        // Render the hits as a directory tree. useLines draws the ASCII branch guides for the UI;
        // when false the same tree is indented with spaces (and "row" implied) for an AI command.
        public static string FormatDeepSearchTree(string mainTable, string query,
                                                  List<DeepSearchHit> hits, bool useLines, int maxValueLen)
        {
            if (hits.Count == 0)
                return "No matches for \"" + query + "\" in " + mainTable + ".";

            var root = new DeepNode();
            foreach (var h in hits)
            {
                string[] segs = h.Dir.Split('/');
                string tableDir = segs[0];           // main table (its name is the implicit root)
                DeepNode cur = root;

                for (int i = 1; i < segs.Length; i++)
                {
                    int comma = segs[i].IndexOf(',');
                    if (comma < 0) continue;
                    int r;
                    if (!int.TryParse(segs[i].Substring(0, comma), out r)) continue;
                    string col = segs[i].Substring(comma + 1);

                    string pk = GetRowPkValue(tableDir, r);
                    cur = cur.Child("R" + r, null, r, pk);       // row node (merge by row index)
                    cur = cur.Child("C|" + col, col, -1, null);  // subtable-column node
                    tableDir = tableDir + "/" + r + "," + col;
                }

                var leaf = cur.Child("L|" + h.Column, h.Column, -1, null);
                leaf.IsLeaf = true;
                if (leaf.Entries == null) leaf.Entries = new List<LeafEntry>();
                leaf.Entries.Add(new LeafEntry
                {
                    Row = h.RowIndex,
                    Pk = GetRowPkValue(h.Dir, h.RowIndex),
                    Value = Sanitize(h.Value, maxValueLen)
                });
            }

            var sb = new StringBuilder();
            sb.AppendLine(hits.Count + " match(es) for \"" + query + "\" in " + mainTable + ":");
            sb.AppendLine();
            RenderChildren(sb, root, "", useLines, maxValueLen);
            return sb.ToString().TrimEnd();
        }

        static void RenderChildren(StringBuilder sb, DeepNode node, string prefix, bool useLines, int maxValueLen)
        {
            node.Children.Sort((a, b) =>
            {
                int ra = a.RowIndex < 0 ? int.MaxValue : a.RowIndex;
                int rb = b.RowIndex < 0 ? int.MaxValue : b.RowIndex;
                return ra != rb ? ra.CompareTo(rb) : a.Order.CompareTo(b.Order);
            });

            for (int i = 0; i < node.Children.Count; i++)
            {
                DeepNode c = node.Children[i];
                bool last = i == node.Children.Count - 1;
                string connector = useLines ? (last ? "└─" : "├─") : "";
                string childPrefix = useLines ? prefix + (last ? "   " : "│  ") : prefix + "  ";

                sb.AppendLine(prefix + connector + Label(c, useLines));

                if (c.IsLeaf)
                {
                    c.Entries.Sort((a, b) => a.Row.CompareTo(b.Row));
                    foreach (var e in c.Entries)
                    {
                        string rowWord = useLines ? "row " : "";
                        string rowTag = "[" + rowWord + e.Row + (e.Pk != null ? ": " + e.Pk : "") + "] ";
                        sb.AppendLine(childPrefix + "- " + rowTag + e.Value);
                    }
                }
                else
                {
                    RenderChildren(sb, c, childPrefix, useLines, maxValueLen);
                }

                // blank guide line between sibling subtrees, mirroring a directory listing
                if (useLines && !last) sb.AppendLine(prefix + "│");
            }
        }

        static string Label(DeepNode n, bool useLines)
        {
            if (n.IsLeaf) return "|" + n.ColName + "|";                        // matched column
            if (n.RowIndex >= 0)                                               // row node
                return (useLines ? "row " : "") + n.RowIndex + (n.Pk != null ? " [" + n.Pk + "]" : "") + "/";
            return n.ColName + "/";                                            // subtable column
        }

        static string Sanitize(string v, int maxLen)
        {
            if (v == null) return "";
            v = v.Replace("\r", " ").Replace("\n", " ");
            if (maxLen > 0 && v.Length > maxLen) v = v.Substring(0, maxLen) + "...";
            return v;
        }

        // Primary-key value of row rowIndex in the table at tableDir, or null if that table has
        // no primary-key column (or it is a script-constructed table not present in currentData).
        static string GetRowPkValue(string tableDir, int rowIndex)
        {
            try
            {
                string tableKey = ConvertDirToTableKey(tableDir);
                if (!currentData.ContainsKey(tableKey)) return null;
                var table = currentData[tableKey] as Dictionary<string, dynamic>;
                if (table == null) return null;

                string pkCol = null;
                foreach (var kv in table)
                    if (kv.Value is string && (string)kv.Value == "Primary Key") { pkCol = kv.Key; break; }
                if (pkCol == null) return null;

                var data = GetTableDataFromDir(tableDir);
                if (data == null || !data.ContainsKey(rowIndex)) return null;
                var row = data[rowIndex];
                if (row != null && row.ContainsKey(pkCol) && row[pkCol] != null)
                    return row[pkCol].ToString();
                return null;
            }
            catch { return null; }
        }
    }
}
