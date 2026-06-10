using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDB
{
    // Read-only "Deep Search" window: prompts for a string, then shows a directory map of
    // every subtable > column > row index in the current table where the string was found.
    // Closable at any time; holds no state beyond the displayed text.
    public class DeepSearchForm : Form
    {
        readonly TextBox queryBox;
        readonly RichTextBox resultsBox;
        readonly TableLayoutPanel top;
        readonly Button searchBtn;

        public DeepSearchForm()
        {
            Text = "Deep Search";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(760, 560);
            MinimumSize = new Size(420, 300);

            // 3-column row (label | textbox-fill | button) so the textbox can never
            // overlap the button regardless of form width
            top = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 42,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(8, 8, 8, 8)
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var label = new Label { Text = "Search current table:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 5, 6, 0) };
            queryBox = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 2, 6, 2) };
            searchBtn = new Button { Text = "Search", Width = 90, Anchor = AnchorStyles.Right, Margin = new Padding(0) };

            top.Controls.Add(label, 0, 0);
            top.Controls.Add(queryBox, 1, 0);
            top.Controls.Add(searchBtn, 2, 0);

            resultsBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                WordWrap = false,
                Font = new Font("Consolas", 9.75f),
                DetectUrls = false,
                // keep the click-drag selection visible when the window loses focus (Notepad++ style)
                HideSelection = false,
                Text = "Enter a string and press Search."
            };

            Controls.Add(resultsBox);
            Controls.Add(top);

            searchBtn.Click += (s, e) => RunSearch();
            queryBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; RunSearch(); }
            };

            ApplyTheme();
            Shown += (s, e) => queryBox.Focus();
        }

        void RunSearch()
        {
            string q = queryBox.Text;
            if (string.IsNullOrEmpty(q)) { resultsBox.Text = "Enter a string to search for."; return; }

            string table = DatabaseFunct.selectedTable;
            if (string.IsNullOrEmpty(table)) { resultsBox.Text = "No table is open."; return; }

            var hits = DatabaseFunct.DeepSearchTable(table, q);
            resultsBox.Text = DatabaseFunct.FormatDeepSearchTree(table, q, hits, true, 200);
        }

        void ApplyTheme()
        {
            try
            {
                var t = ColorThemes.Themes[ColorThemes.currentTheme];
                // NOTE: PanelFore/PanelBack are both black in the Dark theme (the grid draws
                // cell text with a separate style), so use FormFore/FormBack here for readable
                // text. ElseBack/ElseFore give the button visible contrast on a black form.
                Color back = t["FormBack"];
                Color fore = t["FormFore"];

                BackColor = back;
                ForeColor = fore;
                top.BackColor = back;

                resultsBox.BackColor = back;
                resultsBox.ForeColor = fore;
                queryBox.BackColor = back;
                queryBox.ForeColor = fore;

                searchBtn.BackColor = t["ElseBack"];
                searchBtn.ForeColor = t["ElseFore"];
                searchBtn.FlatStyle = FlatStyle.Flat;
            }
            catch
            {
                // theme key missing - leave default colors
            }
        }
    }
}
