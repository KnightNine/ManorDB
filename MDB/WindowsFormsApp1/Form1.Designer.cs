using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace MDB
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideUnhideColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.customTabControl1 = new System.Windows.Forms.CustomTabControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.newTableToolStripMenuItem,
            this.removeTableToolStripMenuItem,
            this.hideUnhideColumnsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2517, 60);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.appendFileToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(87, 56);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(343, 54);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // appendFileToolStripMenuItem
            // 
            this.appendFileToolStripMenuItem.Name = "appendFileToolStripMenuItem";
            this.appendFileToolStripMenuItem.Size = new System.Drawing.Size(343, 54);
            this.appendFileToolStripMenuItem.Text = "Append File";
            this.appendFileToolStripMenuItem.Click += new System.EventHandler(this.appendFileToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(343, 54);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // newTableToolStripMenuItem
            // 
            this.newTableToolStripMenuItem.Name = "newTableToolStripMenuItem";
            this.newTableToolStripMenuItem.Size = new System.Drawing.Size(179, 56);
            this.newTableToolStripMenuItem.Text = "New Table";
            this.newTableToolStripMenuItem.Click += new System.EventHandler(this.newTableToolStripMenuItem_Click);
            // 
            // removeTableToolStripMenuItem
            // 
            this.removeTableToolStripMenuItem.Name = "removeTableToolStripMenuItem";
            this.removeTableToolStripMenuItem.Size = new System.Drawing.Size(226, 56);
            this.removeTableToolStripMenuItem.Text = "Remove Table";
            this.removeTableToolStripMenuItem.Click += new System.EventHandler(this.removeTableToolStripMenuItem_Click);
            // 
            // hideUnhideColumnsToolStripMenuItem
            // 
            this.hideUnhideColumnsToolStripMenuItem.Name = "hideUnhideColumnsToolStripMenuItem";
            this.hideUnhideColumnsToolStripMenuItem.Size = new System.Drawing.Size(337, 56);
            this.hideUnhideColumnsToolStripMenuItem.Text = "Hide/Unhide Columns";
            this.hideUnhideColumnsToolStripMenuItem.Click += new System.EventHandler(this.hideUnhideColumnsToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(864, 69);
            this.label1.TabIndex = 1;
            this.label1.Text = "Open or create a new MDB File";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(982, 392);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 0);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            this.panel1.SizeChanged += new System.EventHandler(this.Panel_SizeChanged);
            this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel_Scroll);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(2474, 60);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.MaximumSize = new System.Drawing.Size(43, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(43, 1031);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.ScrollBar_ValueChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(0, 0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(200, 100);
            this.tabPage1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(0, 0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(200, 100);
            this.tabPage2.TabIndex = 0;
            // 
            // customTabControl1
            // 
            this.customTabControl1.DisplayStyle = System.Windows.Forms.TabStyle.ManorDB;
            // 
            // 
            // 
            this.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.customTabControl1.DisplayStyleProvider.FocusColor = System.Drawing.Color.LightGreen;
            this.customTabControl1.DisplayStyleProvider.FocusTrack = false;
            this.customTabControl1.DisplayStyleProvider.HotTrack = true;
            this.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.customTabControl1.DisplayStyleProvider.Opacity = 1F;
            this.customTabControl1.DisplayStyleProvider.Overlap = 9;
            this.customTabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(14, 1);
            this.customTabControl1.DisplayStyleProvider.ShowTabCloser = false;
            this.customTabControl1.DisplayStyleProvider.TabBookmarkColorsByName = ((System.Collections.Generic.Dictionary<string, System.Drawing.Color>)(resources.GetObject("resource.TabBookmarkColorsByName")));
            this.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.Color.White;
            this.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.Color.WhiteSmoke;
            this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.Color.White;
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customTabControl1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabControl1.HotTrack = true;
            this.customTabControl1.Location = new System.Drawing.Point(0, 1031);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(2474, 60);
            this.customTabControl1.TabIndex = 0;
            this.customTabControl1.SelectedIndexChanged += new System.EventHandler(this.customTabControl1_SelectedIndexChanged);
            this.customTabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTabControl1_MouseDown);
            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(2517, 1091);
            this.Controls.Add(this.customTabControl1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ManorDB";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       
        #endregion

        public System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem newTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTableToolStripMenuItem;

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem newColumnToolStripMenuItem;
        private ToolStripMenuItem newRowToolStripMenuItem;
        public Panel panel1;

        //added in Main
        public CustomDataGridView TableMainGridView;

        private ToolStripMenuItem appendFileToolStripMenuItem;
        public ToolStripMenuItem hideUnhideColumnsToolStripMenuItem;
        public VScrollBar vScrollBar1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        public CustomTabControl customTabControl1;
    }



    public static class Prompt
    {



        public static string[] ShowDialog(string text, string caption, bool addTextBox, bool addListBox, string[] listBoxArr)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormBack"],
                ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormFore"],
            };



            Label textLabel = new Label() { Left = 50, Top = 10, Text = text, Width = 400, Height = 40 };

            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            if (addTextBox)
            {

                prompt.Controls.Add(textBox);
            }

            ComboBox ComboBox1 = new ComboBox() { Left = 50, Top = 70, Width = 400, AutoCompleteMode = AutoCompleteMode.Suggest };
            if (addListBox)
            {

                foreach (string key in listBoxArr)
                {
                    ComboBox1.Items.Add(key);
                }
                ComboBox1.SelectedIndex = 0;

                prompt.Controls.Add(ComboBox1);
            }


            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 90, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { "T", textBox.Text, ComboBox1.SelectedItem != null ? ComboBox1.SelectedItem.ToString() : "" } : new string[] { "F", "", "" };
        }




        public static dynamic ShowColorDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormBack"],
                ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormFore"],
            };



            Label textLabel = new Label() { Left = 50, Top = 60, Text = text, Width = 400, Height = 40 };

            ColorPicker ColorDropDownButton = new ColorPicker() { Left = 20, Top = 90, Width = 300, Height = 40 };
            ColorDropDownButton.AddStandardColors();

            



            Button confirmation = new Button() { Text = "Confirm", Left = 320, Width = 130, Top = 90, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };


            prompt.Controls.Add(ColorDropDownButton);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;
            prompt.AcceptButton = confirmation;

            
            



            return prompt.ShowDialog() == DialogResult.OK ? ColorDropDownButton.SelectedItem : null;



        }

    }
    


    public static class ContextMenuPrompt
    {
        static CustomDataGridView DGV;
        static int rowIndex;
        static ContextMenu cMenu = new ContextMenu();
        static string colName;


        public static void ShowRowContextMenu(CustomDataGridView _DGV, int _rowIndex)
        {
            cMenu.MenuItems.Clear();
            DGV = _DGV;
            rowIndex = _rowIndex;

            

            List<MenuItem> options = new List<MenuItem>(){ new MenuItem(){ Name = "Shift Row Up", Text = "Shift Row Up" }, new MenuItem() { Name = "Shift Row Down", Text = "Shift Row Down" }, new MenuItem() { Name = "Delete Row", Text = "Delete Row" } };
            options[0].Click += new System.EventHandler(shiftUp);
            options[1].Click += new System.EventHandler(shiftDown);
            options[2].Click += new System.EventHandler(deleteRow);

            MenuItem insertRowMenuItem = new MenuItem() { Name = "Insert Row", Text = "Insert Row" };
            MenuItem duplicateRowMenuItem = new MenuItem() { Name = "Duplicate Row", Text = "Duplicate Row" };
            insertRowMenuItem.Click += new System.EventHandler(insertRow);
            duplicateRowMenuItem.Click += new System.EventHandler(duplicateRow);

            //if restricted to a single row, don't add the above two menu items:
            string tableKey = DatabaseFunct.ConvertDirToTableKey(DGV.Name);
            if (!DatabaseFunct.DoesSubTableMeetOrExceedRowRestriction(DGV))
            {
                options.AddRange(new MenuItem[] { insertRowMenuItem, duplicateRowMenuItem });
            }


            cMenu.MenuItems.AddRange(options.ToArray());
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));

        }
        public static void ShowColumnContextMenu(CustomDataGridView _DGV, string _colName)
        {
            cMenu.MenuItems.Clear();
            DGV = _DGV;
            colName = _colName;


            List<MenuItem> options = new List<MenuItem>{ new MenuItem() { Name = "Shift Column Left", Text = "Shift Column Left" }, new MenuItem() { Name = "Shift Column Right", Text = "Shift Column Right" }, new MenuItem() { Name = "Rename Column", Text = "Rename Column", Tag = new dynamic[] { _DGV, _colName } }, new MenuItem() { Name = "Delete Column", Text = "Delete Column" }, new MenuItem() { Name = "Adjacent Column Disabler Settings", Text = "Adjacent Column Disabler Settings" , Tag = new dynamic[] {_DGV, _colName} } };
            options[0].Click += new System.EventHandler(shiftLeft);
            options[1].Click += new System.EventHandler(shiftRight);
            options[2].Click += new System.EventHandler(renameColumn);
            options[3].Click += new System.EventHandler(deleteColumn);
            options[4].Click += new System.EventHandler(openColumnDisabler);

            string tableKey = DatabaseFunct.ConvertDirToTableKey(_DGV.Name);
            string colType = DatabaseFunct.currentData[tableKey][_colName];
            
            if (colType == "Numerical")
            {
                options.Add(new MenuItem() { Name = "Convert To Integer Type Column", Text = "Convert To Integer Type Column", Tag = new dynamic[] { _DGV, _colName } });
                options[5].Click += new System.EventHandler(convertNumericalColumnToIntegerColumn);
            }
            else if (colType == "Integer")
            {
                options.Add(new MenuItem() { Name = "Convert To Numerical Type Column", Text = "Convert To Numerical Type Column", Tag = new dynamic[] { _DGV, _colName } });
                options[5].Click += new System.EventHandler(convertIntegerColumnToNumericalColumn);
            }
            else if (colType == "SubTable")
            {
                //if row restriction already applied to this column
                if (DatabaseFunct.currentData[tableKey].ContainsKey(_colName + DatabaseFunct.SubtableRowRestrictionExt))
                {
                    options.Add(new MenuItem() { Name = "Remove Subable Single Row Restriction", Text = "Remove SubTable Single Row Restriction", Tag = new dynamic[] { _DGV, _colName } });
                    options[5].Click += new System.EventHandler(removeRowRestrictionFromSubtableColumn);
                }
                else //if row restriction not applied
                {
                    options.Add(new MenuItem() { Name = "Apply Subable Single Row Restriction", Text = "Apply SubTable Single Row Restriction", Tag = new dynamic[] { _DGV, _colName } });
                    options[5].Click += new System.EventHandler(addRowRestrictionToSubtableColumn);
                    
                }
                
            }


            cMenu.MenuItems.AddRange(options.ToArray());
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));

        }

        public static void ShowColumnDisablerContextMenu(CustomDataGridView _DGV, string _colName)
        {
            cMenu.MenuItems.Clear();
            DGV = _DGV;
            colName = _colName;

            List<MenuItem> optionsList = new List<MenuItem>();
            MenuItem[] options = { };

            int i = 0;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(_DGV.Name);
            string[] tagData = new string[] { _colName, tableKey };

            foreach (DataGridViewColumn col in _DGV.Columns) 
            {
                if (col.Name != _colName)
                {
                    //show if this column is already within the disabler
                    bool isInDisablerArr = false;
                    if (DatabaseFunct.currentData[tableKey].ContainsKey(_colName + DatabaseFunct.ColumnDisablerArrayExt))
                    {
                        
                        if (((List<string>)DatabaseFunct.currentData[tableKey][_colName + DatabaseFunct.ColumnDisablerArrayExt]).Contains(col.Name))
                        {
                            isInDisablerArr = true;
                        }
                        else
                        {
                            Console.WriteLine(_colName + DatabaseFunct.ColumnDisablerArrayExt + " of key "+ tableKey + " Does not contain: " + col.Name);
                            
                        }

                    }
                    else
                    {
                        Console.WriteLine(_colName + DatabaseFunct.ColumnDisablerArrayExt +" Does not exist within tablekey: " + tableKey);
                    }

                    string selectedColumnKey = col.Name;
                    string selectedColumnText = col.HeaderText;
                    
                    
                    optionsList.Add(new MenuItem() { Name = selectedColumnKey, Text = selectedColumnText, Tag = tagData, Checked = isInDisablerArr });
                    //assign event to each option
                    optionsList[i].Click += new System.EventHandler(addColToDisablerArrEvent);

                    i += 1;
                }
                
            }
            options = optionsList.ToArray();




            cMenu.MenuItems.AddRange(options);
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));
        }


        public static void ShowTableContextMenu(string _tableName)
        {
            cMenu.MenuItems.Clear();


            MenuItem[] options = { new MenuItem() { Name = "Rename Table", Text = "Rename Table", Tag = _tableName }, new MenuItem() { Name = "Bookmark Table", Text = "Bookmark Table", Tag = _tableName } };
            options[0].Click += new System.EventHandler(renameTable);
            options[1].Click += new System.EventHandler(bookmarkTable);

            cMenu.MenuItems.AddRange(options);
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));

        }

        private static void renameTable(object sender, System.EventArgs e)
        {
            MenuItem senderItem = (MenuItem)sender;
            string tableName = (string)senderItem.Tag;
            string[] input = Prompt.ShowDialog("Enter New Table Name:", "Rename \""+ tableName +"\" Table", true, false, null);

            if (input[0] == "T")
            {
                DatabaseFunct.ChangeTableName(tableName,input[1]);
            }

        }

        private static void bookmarkTable(object sender, System.EventArgs e)
        {
            MenuItem senderItem = (MenuItem)sender;
            string tableName = (string)senderItem.Tag;
            dynamic input = Prompt.ShowColorDialog("Add a Bookmark color to the \"" + tableName + "\" Table:", "Bookmark Color Selection" );

            if (input == null)
            {
                //do nothing
            }
            else
            {
                ColorPicker.ColorInfo selectedItem = input as ColorPicker.ColorInfo;

                if (selectedItem.Text == "No Bookmark")
                {
                    DatabaseFunct.RemoveOrChangeTableBookmark(tableName, null);
                }
                else
                {
                    DatabaseFunct.BookmarkTable(tableName, selectedItem.Color);
                }    


            }
            
        }


        private static void renameColumn(object sender, System.EventArgs e)
        {
            
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            string[] input = Prompt.ShowDialog("Enter New Column Name:", "Rename \"" + _colName + "\" Column", true, false,  null);
            
            if (input[0] == "T")
            {
                DatabaseFunct.ChangeColumnName(_colName, input[1],_DGV);
            }
        }

        private static void convertNumericalColumnToIntegerColumn(object sender, System.EventArgs e)
        {
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            

            var confirmResult = MessageBox.Show("Are you sure to convert " + _colName + " to an Integer type column ?",
                                    "Confirm:",
                                    MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.ConvertNumericalColumnToIntegerColumn(_colName, _DGV);
            }
        }

        private static void convertIntegerColumnToNumericalColumn(object sender, System.EventArgs e)
        {
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            var confirmResult = MessageBox.Show("Are you sure to convert " + _colName + " to an Numerical (double) type column ?",
                                    "Confirm:",
                                    MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.ConvertIntegerColumnToNumericalColumn(_colName, _DGV);
            }
        }


        private static void addRowRestrictionToSubtableColumn(object sender, System.EventArgs e)
        {
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            var confirmResult = MessageBox.Show("Are you sure to restrict " + _colName + " to a single row entry? \n This will erase all rows after the first row in all subtables of this column!",
                                    "Confirm:",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.AddRowRestrictionToSubtableColumn(_DGV, _colName, 1);


            }
        }

        private static void removeRowRestrictionFromSubtableColumn(object sender, System.EventArgs e)
        {
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            var confirmResult = MessageBox.Show("Are you sure to remove row restriction from " + _colName + "? \n This will allow the subtables of this column to have multiple rows",
                                    "Confirm:",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.RemoveRowRestrictionFromSubtableColumn(_DGV, _colName);


            }
        }



        private static void addColToDisablerArrEvent(object sender, System.EventArgs e)
        {
            //add or remove to Disabler Arr
            MenuItem senderItem = (MenuItem)sender;
            string tableKey = ((string[])senderItem.Tag)[1];
            string selectedColKey1 = ((string[])senderItem.Tag)[0];
            string selectedColKey2 = senderItem.Name;

            if (senderItem.Checked)
            {
                DatabaseFunct.addColToDisablerArr(tableKey, selectedColKey1, selectedColKey2);
            }
            else if (MessageBox.Show("This will make rows of columns \"" +selectedColKey1+"\" and \"" +selectedColKey2+"\" not allow eachother to contain data at the same time. For each row, data from the cell of the column mentioned second will be deleted if there is data in the first column's cell.\n Are you sure you want to initiate this?", "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)

            {

                DatabaseFunct.addColToDisablerArr(tableKey, selectedColKey1, selectedColKey2);

            }

            cMenu.Dispose();
        }


        private static void insertRow(object sender, System.EventArgs e)
        {
            DatabaseFunct.AddRow(DGV, true, rowIndex);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();

        }

        private static void duplicateRow(object sender, System.EventArgs e)
        {
            DatabaseFunct.DuplicateRow(DGV,rowIndex);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();
        }

        private static void shiftUp(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftRow(DGV, rowIndex, true);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();

        }
        private static void shiftDown(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftRow(DGV, rowIndex, false);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();
        }

        private static void deleteRow(object sender, System.EventArgs e)
        {

            var confirmResult = MessageBox.Show("Are you sure to delete row at index " + rowIndex + " ?",
                                     "Confirm:",
                                     MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.RemoveRow(DGV, rowIndex);
                Program.mainForm.RecenterSubTables();
            }
            


        }
        private static void shiftLeft(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftColumn(colName, DGV, true);
            
        }
        private static void shiftRight(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftColumn(colName, DGV, false);
            

        }

        private static void openColumnDisabler(object sender, System.EventArgs e)
        {
            MenuItem senderMI = sender as MenuItem;
            dynamic[] dat = senderMI.Tag as dynamic[];
            CustomDataGridView _DGV = dat[0];
            string _colName = dat[1];

            ShowColumnDisablerContextMenu(_DGV, _colName);
        }

        private static void deleteColumn(object sender, System.EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete "+colName+" Column?",
                                     "Confirm:",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DatabaseFunct.RemoveColumn(colName, DGV);
                Program.mainForm.RecenterSubTables();
            }
            

            
            
        }

        
    }


    static partial class ComboListBox
    {
        

        /*public static void ShowComboBox(int colIndex, int rowIndex, string[] arr)
        {
            x = DialogResult.No;
            ComboBox ComboBoxNew = new ComboBox();
            
            ComboBoxNew.Name = "ComboBox1";
            ComboBoxNew.Size = Program.mainForm.TableMainGridView.Rows[rowIndex].Cells[colIndex].Size;
            ComboBoxNew.Location = new System.Drawing.Point(Control.MousePosition.X - ComboBoxNew.Size.Width/2, Control.MousePosition.Y - ComboBoxNew.Size.Height / 2);
            ComboBoxNew.TabIndex = 0;
            ComboBoxNew.Text = "Select Foreign Key";
            ComboBoxNew.Items.AddRange(arr);
            Program.mainForm.TableMainGridView.Controls.Add(ComboBoxNew);

            // Hook up the event handler.
            //ComboBoxNew.DropDown +=
            //  new System.EventHandler(ComboBoxNew_DropDown);
            ComboBoxNew.DroppedDown = true;
            ComboBoxNew.SelectionChangeCommitted += new System.EventHandler(ComboBoxNew_SelectValue);


            //return ComboBoxNew.SelectionChangeCommitted? :
        }*/

        



    }


}

