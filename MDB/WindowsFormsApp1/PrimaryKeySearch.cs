using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MDB
{
    // Primary-key search bar shown on the right of the top menu for main tables that have a
    // primary key column. Typing does a case-insensitive substring match against the PK values
    // and shows a suggestion dropdown ranked by how early the match occurs in each value.
    // Selecting a suggestion highlights that row's PK cell and scrolls it into view.
    internal static class PrimaryKeySearch
    {
        private static ToolStripTextBox searchBox;
        private static SuggestionListView list;

        // PK column of the table currently shown, or null when the table has none
        private static string currentPkColumn;

        // suggestion list index -> grid row index of the matching entry
        private static readonly List<int> rowIndexBySuggestion = new List<int>();

        // cap on how many matches are added to the list, to keep each keystroke responsive
        private const int MaxSuggestions = 500;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;
        private const int WM_MOUSEWHEEL = 0x020A;

        public static void Init(ToolStripTextBox box)
        {
            searchBox = box;
            searchBox.Visible = false;

            list = new SuggestionListView();
            list.BorderStyle = BorderStyle.FixedSingle;
            list.Font = searchBox.Font;
            list.Visible = false;
            list.Click += (s, e) => CommitSelection();

            searchBox.TextChanged += (s, e) => UpdateSuggestions();
            searchBox.KeyDown += SearchBox_KeyDown;
            searchBox.LostFocus += (s, e) => DeferredHideIfUnfocused();
            list.LostFocus += (s, e) => DeferredHideIfUnfocused();

            // greyed-out cue shown while the box is empty; reapply if the handle is recreated
            searchBox.TextBox.HandleCreated += (s, e) => SetCueBanner();
            SetCueBanner();

            // scroll the dropdown with the wheel while hovering it, without it needing focus
            Application.AddMessageFilter(new WheelMessageFilter());

            // when the menu runs out of room, drop the search box first so the menu items survive
            Program.mainForm.menuStrip1.SizeChanged += (s, e) => UpdateSearchBoxFit();
        }

        private static void SetCueBanner()
        {
            if (searchBox != null && searchBox.TextBox != null && searchBox.TextBox.IsHandleCreated)
            {
                SendMessage(searchBox.TextBox.Handle, EM_SETCUEBANNER, (IntPtr)1, "Primary Key Search");
            }
        }

        // A ListView is used instead of a ListBox on purpose: the ListBox class applies Windows'
        // animated "smooth-scroll list boxes" effect even to a single wheel step (a ~half-second
        // glide), and there is no per-control way to turn it off. ListView scrolls instantly.
        // Wheel handling is one line per notch via LVM_SCROLL, so it never animates.
        private class SuggestionListView : ListView
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            private const int LVM_FIRST = 0x1000;
            private const int LVM_SCROLL = LVM_FIRST + 20;

            public SuggestionListView()
            {
                DoubleBuffered = true;
                View = View.Details;
                HeaderStyle = ColumnHeaderStyle.None;
                FullRowSelect = true;
                MultiSelect = false;
                HideSelection = false;
                Columns.Add(string.Empty);
            }

            public int LineHeight
            {
                get { return Items.Count > 0 ? GetItemRect(0).Height : Font.Height + 2; }
            }

            public void ScrollByLines(int lines)
            {
                if (IsHandleCreated) SendMessage(Handle, LVM_SCROLL, IntPtr.Zero, (IntPtr)(lines * LineHeight));
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_MOUSEWHEEL)
                {
                    int delta = (short)((m.WParam.ToInt64() >> 16) & 0xffff);
                    ScrollByLines(delta > 0 ? -1 : 1);
                    return;
                }
                base.WndProc(ref m);
            }
        }

        // intercepts the mouse wheel so hovering the dropdown scrolls it even though the search box
        // keeps keyboard focus (covers the case where the wheel goes to the focused box, not the list)
        private class WheelMessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg != WM_MOUSEWHEEL || list == null || !list.Visible) return false;
                if (!list.RectangleToScreen(list.ClientRectangle).Contains(Cursor.Position)) return false;

                int delta = (short)((m.WParam.ToInt64() >> 16) & 0xffff);
                list.ScrollByLines(delta > 0 ? -1 : 1);
                return true;
            }
        }

        // Called when the active main table changes: toggles the search bar based on whether
        // the table has a PK column and resets any in-progress search.
        public static void UpdateForCurrentTable()
        {
            if (searchBox == null) return;

            currentPkColumn = GetPrimaryKeyColumn(DatabaseFunct.selectedTable);
            searchBox.Text = "";
            HideList();
            UpdateSearchBoxFit();
        }

        // Show the search box only when the table has a PK column and the menu has room for it;
        // otherwise hide it so the actual menu items (Add Column / Add Row / ...) keep their space.
        private static void UpdateSearchBoxFit()
        {
            if (searchBox == null) return;

            bool shouldShow = currentPkColumn != null;
            if (shouldShow)
            {
                MenuStrip strip = Program.mainForm.menuStrip1;
                int menuItemsWidth = 0;
                foreach (ToolStripItem item in strip.Items)
                {
                    if (item == searchBox) continue;
                    menuItemsWidth += item.GetPreferredSize(Size.Empty).Width + item.Margin.Horizontal;
                }

                int searchWidth = searchBox.GetPreferredSize(Size.Empty).Width + searchBox.Margin.Horizontal;
                int available = strip.ClientSize.Width - strip.Padding.Horizontal;
                shouldShow = menuItemsWidth + searchWidth <= available;
            }

            if (searchBox.Visible != shouldShow) searchBox.Visible = shouldShow;
        }

        private static string GetPrimaryKeyColumn(string table)
        {
            if (string.IsNullOrEmpty(table) || !DatabaseFunct.currentData.ContainsKey(table)) return null;

            foreach (KeyValuePair<string, dynamic> col in DatabaseFunct.currentData[table])
            {
                if (col.Value is string && col.Value == "Primary Key") return col.Key;
            }
            return null;
        }

        private static void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!list.Visible) return;

            if (e.KeyCode == Keys.Down) { MoveSelection(1); }
            else if (e.KeyCode == Keys.Up) { MoveSelection(-1); }
            else if (e.KeyCode == Keys.Enter) { CommitSelection(); }
            else if (e.KeyCode == Keys.Escape) { HideList(); }
            else { return; }

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private static void MoveSelection(int delta)
        {
            int count = list.Items.Count;
            if (count == 0) return;

            int index = GetSelectedIndex();
            index = index < 0 ? 0 : index + delta;
            if (index < 0) index = 0;
            if (index >= count) index = count - 1;
            SetSelectedIndex(index);
        }

        private static int GetSelectedIndex()
        {
            return list.SelectedIndices.Count > 0 ? list.SelectedIndices[0] : -1;
        }

        private static void SetSelectedIndex(int index)
        {
            if (index < 0 || index >= list.Items.Count) return;
            list.SelectedIndices.Clear();
            list.Items[index].Selected = true;
            list.Items[index].EnsureVisible();
        }

        private static void UpdateSuggestions()
        {
            list.Items.Clear();
            rowIndexBySuggestion.Clear();

            string query = searchBox.Text;
            string table = DatabaseFunct.selectedTable;

            if (string.IsNullOrEmpty(query) || currentPkColumn == null
                || !DatabaseFunct.currentData.ContainsKey(table)
                || !DatabaseFunct.currentData[table].ContainsKey(DatabaseFunct.RowEntryRefrence))
            {
                HideList();
                return;
            }

            var entries = DatabaseFunct.currentData[table][DatabaseFunct.RowEntryRefrence]
                as Dictionary<int, Dictionary<string, dynamic>>;
            if (entries == null)
            {
                HideList();
                return;
            }

            // matchIndex, rowIndex, displayText
            var matches = new List<Tuple<int, int, string>>();
            foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in entries)
            {
                if (!entry.Value.ContainsKey(currentPkColumn)) continue;
                dynamic value = entry.Value[currentPkColumn];
                if (value == null) continue;

                string text = value.ToString();
                int matchIndex = text.IndexOf(query, StringComparison.OrdinalIgnoreCase);
                if (matchIndex < 0) continue;

                matches.Add(Tuple.Create(matchIndex, entry.Key, text));
            }

            // rank by earliest match, then alphabetically, then by row index for stability
            matches.Sort((a, b) =>
            {
                int c = a.Item1.CompareTo(b.Item1);
                if (c != 0) return c;
                c = string.Compare(a.Item3, b.Item3, StringComparison.OrdinalIgnoreCase);
                if (c != 0) return c;
                return a.Item2.CompareTo(b.Item2);
            });

            if (matches.Count == 0)
            {
                HideList();
                return;
            }

            int shown = Math.Min(matches.Count, MaxSuggestions);
            for (int i = 0; i < shown; i++)
            {
                list.Items.Add(matches[i].Item3);
                rowIndexBySuggestion.Add(matches[i].Item2);
            }

            ShowList();
        }

        private static void ShowList()
        {
            Form form = Program.mainForm;

            Rectangle boxScreen = searchBox.Owner.RectangleToScreen(searchBox.Bounds);
            Point topLeft = form.PointToClient(new Point(boxScreen.Left, boxScreen.Bottom));

            list.Font = searchBox.Font;
            int width = Math.Max(searchBox.Width, 160);

            // show it first so the handle exists and item height can be measured
            if (!form.Controls.Contains(list)) form.Controls.Add(list);
            list.Bounds = new Rectangle(topLeft.X, topLeft.Y, width, list.Font.Height + 8);
            list.Visible = true;
            list.BringToFront();

            // grow with the matches but never past the bottom of the window; the list scrolls beyond that
            int itemHeight = list.LineHeight;
            int contentHeight = list.Items.Count * itemHeight + 4;
            int maxHeight = form.ClientSize.Height - topLeft.Y - 4;
            if (maxHeight < itemHeight) maxHeight = itemHeight;
            int height = Math.Min(contentHeight, maxHeight);

            list.Bounds = new Rectangle(topLeft.X, topLeft.Y, width, height);
            list.Columns[0].Width = list.ClientSize.Width;

            SetSelectedIndex(0);
        }

        private static void HideList()
        {
            if (list == null) return;
            list.Visible = false;
            list.Items.Clear();
            rowIndexBySuggestion.Clear();
        }

        // Hide and clear the search once focus has settled away from both the search box and the list.
        private static void DeferredHideIfUnfocused()
        {
            Program.mainForm.BeginInvoke((Action)(() =>
            {
                if (!list.Focused && !searchBox.Focused)
                {
                    HideList();
                    ClearSearch();
                }
            }));
        }

        private static void ClearSearch()
        {
            if (searchBox != null && searchBox.Text.Length > 0) searchBox.Text = "";
        }

        private static void CommitSelection()
        {
            int selected = GetSelectedIndex();
            if (selected < 0 || selected >= rowIndexBySuggestion.Count)
            {
                HideList();
                return;
            }

            int rowIndex = rowIndexBySuggestion[selected];
            HideList();
            ClearSearch();

            CustomDataGridView grid = Program.mainForm.TableMainGridView;
            if (currentPkColumn == null || !grid.Columns.Contains(currentPkColumn)) return;
            if (rowIndex < 0 || rowIndex >= grid.Rows.Count) return;

            DataGridViewCell cell = grid.Rows[rowIndex].Cells[currentPkColumn];

            grid.ClearSelection();
            try { grid.CurrentCell = cell; } catch { }
            cell.Selected = true;

            // this grid never scrolls internally; the viewport is moved by repositioning panel1
            // through vScrollBar1 (panel1.Y = menuStrip1.Height - value * scrollSensitivity, where
            // scrollSensitivity is 10). Only scroll when the row isn't already fully in view, and
            // then put the row's top at the viewport top.
            try
            {
                Form1 form = Program.mainForm;
                VScrollBar scrollBar = form.vScrollBar1;
                Rectangle rowRect = grid.GetRowDisplayRectangle(rowIndex, false);

                int menuHeight = form.MainMenuStrip != null ? form.MainMenuStrip.Height : 0;
                int viewportHeight = form.ClientRectangle.Height - form.customTabControl1.Height - menuHeight;
                int offset = scrollBar.Value * 10;

                bool alreadyVisible = rowRect.Top >= offset && rowRect.Bottom <= offset + viewportHeight;
                if (!alreadyVisible)
                {
                    int target = (int)Math.Round(rowRect.Top / 10.0);
                    if (target < 0) target = 0;
                    if (target > scrollBar.Maximum) target = scrollBar.Maximum;
                    scrollBar.Value = target;
                }
            }
            catch { }

            grid.Focus();
        }
    }
}
