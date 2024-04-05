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
            this.scriptSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editScriptPrefabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setScriptColumnDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRegexReferenceTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.hideUnhideColumnsToolStripMenuItem,
            this.scriptSettingsToolStripMenuItem,
            this.updateRegexReferenceTablesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(2, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(1005, 47);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(87, 45);
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
            this.newTableToolStripMenuItem.Size = new System.Drawing.Size(179, 45);
            this.newTableToolStripMenuItem.Text = "New Table";
            this.newTableToolStripMenuItem.Click += new System.EventHandler(this.newTableToolStripMenuItem_Click);
            // 
            // removeTableToolStripMenuItem
            // 
            this.removeTableToolStripMenuItem.Name = "removeTableToolStripMenuItem";
            this.removeTableToolStripMenuItem.Size = new System.Drawing.Size(226, 45);
            this.removeTableToolStripMenuItem.Text = "Remove Table";
            this.removeTableToolStripMenuItem.Click += new System.EventHandler(this.removeTableToolStripMenuItem_Click);
            // 
            // hideUnhideColumnsToolStripMenuItem
            // 
            this.hideUnhideColumnsToolStripMenuItem.Name = "hideUnhideColumnsToolStripMenuItem";
            this.hideUnhideColumnsToolStripMenuItem.Size = new System.Drawing.Size(337, 45);
            this.hideUnhideColumnsToolStripMenuItem.Text = "Hide/Unhide Columns";
            this.hideUnhideColumnsToolStripMenuItem.Click += new System.EventHandler(this.hideUnhideColumnsToolStripMenuItem_Click);
            // 
            // scriptSettingsToolStripMenuItem
            // 
            this.scriptSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editScriptPrefabsToolStripMenuItem,
            this.setScriptColumnDuplicatesToolStripMenuItem});
            this.scriptSettingsToolStripMenuItem.Name = "scriptSettingsToolStripMenuItem";
            this.scriptSettingsToolStripMenuItem.Size = new System.Drawing.Size(232, 45);
            this.scriptSettingsToolStripMenuItem.Text = "Script Settings";
            // 
            // editScriptPrefabsToolStripMenuItem
            // 
            this.editScriptPrefabsToolStripMenuItem.Name = "editScriptPrefabsToolStripMenuItem";
            this.editScriptPrefabsToolStripMenuItem.Size = new System.Drawing.Size(638, 54);
            this.editScriptPrefabsToolStripMenuItem.Text = "Edit Script Prefabs";
            this.editScriptPrefabsToolStripMenuItem.Click += new System.EventHandler(this.editScriptPrefabsToolStripMenuItem_Click);
            // 
            // setScriptColumnDuplicatesToolStripMenuItem
            // 
            this.setScriptColumnDuplicatesToolStripMenuItem.Name = "setScriptColumnDuplicatesToolStripMenuItem";
            this.setScriptColumnDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(638, 54);
            this.setScriptColumnDuplicatesToolStripMenuItem.Text = "Set Script Column Type Duplicates";
            this.setScriptColumnDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.setScriptColumnTypeDuplicatesToolStripMenuItem_Click);
            // 
            // updateRegexReferenceTablesToolStripMenuItem
            // 
            this.updateRegexReferenceTablesToolStripMenuItem.Name = "updateRegexReferenceTablesToolStripMenuItem";
            this.updateRegexReferenceTablesToolStripMenuItem.Size = new System.Drawing.Size(466, 45);
            this.updateRegexReferenceTablesToolStripMenuItem.Text = "Update Regex Reference Tables ";
            this.updateRegexReferenceTablesToolStripMenuItem.Click += new System.EventHandler(this.updateRegexReferenceTablesToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
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
            this.panel1.Location = new System.Drawing.Point(368, 164);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
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
            this.vScrollBar1.Location = new System.Drawing.Point(989, 47);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.MaximumSize = new System.Drawing.Size(16, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(16, 252);
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
            this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.Color.Black;
            this.customTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customTabControl1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customTabControl1.HotTrack = true;
            this.customTabControl1.Location = new System.Drawing.Point(0, 274);
            this.customTabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(989, 25);
            this.customTabControl1.TabIndex = 0;
            this.customTabControl1.SelectedIndexChanged += new System.EventHandler(this.customTabControl1_SelectedIndexChanged);
            this.customTabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.customTabControl1_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1005, 299);
            this.Controls.Add(this.customTabControl1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(1);
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
        private ToolStripMenuItem scriptSettingsToolStripMenuItem;
        private ToolStripMenuItem editScriptPrefabsToolStripMenuItem;
        private ToolStripMenuItem setScriptColumnDuplicatesToolStripMenuItem;
        private ToolStripMenuItem updateRegexReferenceTablesToolStripMenuItem;
    }















    public static class DictionaryWriterPrompt
    {
        public static Tuple<string, Dictionary<string, string>> Show(string text,string keyText,string valueText, string caption, string addButtonText, Dictionary<string,string> storedData)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 430,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormBack"],
                ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormFore"],
            };

            Label textLabel = new Label() { Left = 50, Top = 10, Text = text, Width = 400, Height = 40 };
            Label KeyLabel = new Label() { Left = 50, Top = 65, Text = keyText, Width = 275, Height = 14 };
            TextBox KeyBox = new TextBox() { Left = 50, Top = 80, Width = 275 };
            Label ValueLabel = new Label() { Left = 50, Top = 105, Text = valueText, Width = 275, Height = 14 };
            TextBox ValueBox = new TextBox() { Left = 50, Top = 120, Width = 275 };
            DataGridView dataGridView = new DataGridView() { Left = 50, Top = 155, Width = 400, Height = 200, ScrollBars = ScrollBars.Vertical, ColumnHeadersVisible = false, RowHeadersVisible = false, AllowUserToResizeRows = false, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToOrderColumns = false };
            Button addButton = new Button() { Left = 355, Top = 80, Width = 90, Height = 70, Text = addButtonText, AutoSizeMode = AutoSizeMode.GrowOnly, Anchor = AnchorStyles.Left };
            Button confirmation = new Button() { Text = "Done", Left = 350, Width = 100, Top = 360, DialogResult = DialogResult.OK };

            //---Key
            prompt.Controls.Add(KeyLabel);
            prompt.Controls.Add(KeyBox);
            //---Value
            prompt.Controls.Add(ValueLabel);
            prompt.Controls.Add(ValueBox);

            //---dataGridView

            dataGridView.Columns.Add("Key", "Key");
            dataGridView.Columns.Add("Val", "Val");
            dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView.BackgroundColor = SystemColors.ControlDarkDark;

            dataGridView.Tag = new Dictionary<string, dynamic>() { { "addButton", addButton }, { " KeyBox", KeyBox },{ "ValueBox", ValueBox } };
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();

            //not sure why but DefaultCellStyle doesn't seem to change anything here
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.CellMouseEnter += DataGridView_CellMouseEnter;
            dataGridView.CellMouseLeave += DataGridView_CellMouseLeave;



            prompt.Controls.Add(dataGridView);

            //load existing dict
            foreach (KeyValuePair<string,string> kv in storedData)
            {
                dataGridView.Rows.Add();
                DataGridViewRow row = dataGridView.Rows[dataGridView.Rows.Count - 1];
                row.Cells[0].Value = kv.Key;
                row.Cells[0].Tag = kv.Key;
                row.Cells[1].Value = kv.Value;
                row.Cells[1].Tag = kv.Value;

                SetRowInitialCellStyles(row);
            }

            //----addButton

            addButton.Tag = new Dictionary<string, dynamic>() { { "dataGridView", dataGridView },  { "KeyBox", KeyBox }, { "ValueBox", ValueBox } };
            addButton.Click += AddButton_Click;

            prompt.Controls.Add(addButton);
            //----

            Dictionary<string,string> getTableData()
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    result.Add(row.Cells[0].Tag as string, row.Cells[1].Tag as string);
                }
                return result;
            }

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new Tuple<string, Dictionary<string, string>>("T", getTableData()) : new Tuple<string, Dictionary<string, string>>("F", null);
        }

        private static void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            foreach (DataGridViewCell cell in senderDGV.Rows[e.RowIndex].Cells)
            {
                //revert to original value and color
                cell.Style.BackColor = System.Drawing.SystemColors.Window;
                cell.Style.ForeColor = System.Drawing.SystemColors.ControlText;
                cell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
                cell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;
                cell.Value = cell.Tag;
            }
        }

            private static void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            foreach(DataGridViewCell cell in senderDGV.Rows[e.RowIndex].Cells)
            {
                cell.Style.BackColor = Color.Red;
                cell.Style.ForeColor = Color.White;
                cell.Style.SelectionBackColor = Color.Red;
                cell.Style.SelectionForeColor = Color.White;
                cell.Value = "Remove";
            }
            
        }

        private static void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            Dictionary<string, dynamic> tagDict = senderDGV.Tag as Dictionary<string, dynamic>;

            

            senderDGV.Rows.RemoveAt(e.RowIndex);
        }
        private static void AddButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            Dictionary<string, dynamic> tagDict = senderButton.Tag as Dictionary<string, dynamic>;

            DataGridView dataGridView = tagDict["dataGridView"];
            TextBox KeyBox = tagDict["KeyBox"];
            TextBox ValueBox = tagDict["ValueBox"];

            string Key = KeyBox.Text;
            string Value = ValueBox.Text;

            bool validKey = true;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                
                if (row.Cells[0].Tag as string == Key)
                {
                    validKey = false;
                }
                
            }

            if (validKey)
            {
                if (!String.IsNullOrEmpty(KeyBox.Text) && !String.IsNullOrEmpty(ValueBox.Text))
                {



                    dataGridView.Rows.Add();

                    DataGridViewRow row = dataGridView.Rows[dataGridView.Rows.Count - 1];


                    DataGridViewCell newCell = row.Cells[0];
                    newCell.Value = Key;
                    newCell.Tag = Key;

                    DataGridViewCell newCell2 = row.Cells[1];
                    newCell2.Value = Value;
                    newCell2.Tag = Value;

                    SetRowInitialCellStyles(row);
                    
                    //empty boxes
                    KeyBox.Text = "";
                    ValueBox.Text = "";

                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Key already exists in table!");
            }
           


        }


        private static void SetRowInitialCellStyles(DataGridViewRow row)
        {
            //KEY---------------
            DataGridViewCell newCell = row.Cells[0];
            //this has to be set manually due to some issue
            newCell.Style.BackColor = System.Drawing.SystemColors.Window;
            newCell.Style.ForeColor = System.Drawing.SystemColors.ControlText;
            newCell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
            newCell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;

            

            //Value---------------
            DataGridViewCell newCell2 = row.Cells[1];
            //this has to be set manually due to some issue
            newCell2.Style.BackColor = System.Drawing.SystemColors.Window;
            newCell2.Style.ForeColor = System.Drawing.SystemColors.ControlText;
            newCell2.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
            newCell2.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;

            
        }

    }


    public static class MultiSelectPrompt
    {
        public static Tuple<string, List<string>> Show(string text, string caption, string[] listBoxArr, string addButtonText)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormBack"],
                ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormFore"],
            };

            Label textLabel = new Label() { Left = 50, Top = 10, Text = text, Width = 400, Height = 40 };
            ComboBox ComboBox1 = new ComboBox() { Left = 50, Top = 50, Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            DataGridView dataGridView = new DataGridView() { Left = 50, Top = 75, Width = 400, Height = 200, ScrollBars = ScrollBars.Vertical, ColumnHeadersVisible = false, RowHeadersVisible = false, AllowUserToResizeRows = false, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToOrderColumns = false };
            Button addButton = new Button() { Left = 300, Top = 50, Width = 100, Text = addButtonText, AutoSizeMode = AutoSizeMode.GrowOnly, Anchor = AnchorStyles.Left };
            Button confirmation = new Button() { Text = "Done", Left = 350, Width = 100, Top = 300, DialogResult = DialogResult.OK };

            //---ComboBox1


            foreach (string key in listBoxArr)
            {
                ComboBox1.Items.Add(key);
            }
            ComboBox1.SelectedIndex = 0;

            prompt.Controls.Add(ComboBox1);
            //---dataGridView

            dataGridView.Columns.Add("default", "default");
            dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView.BackgroundColor = SystemColors.ControlDarkDark;

            dataGridView.Tag = new Dictionary<string, dynamic>() { { "addButton", addButton }, { "ComboBox1", ComboBox1 } };
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();

            //not sure why but DefaultCellStyle doesn't seem to change anything here
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            dataGridView.CellClick += DataGridView_CellClick;
            dataGridView.CellMouseEnter += DataGridView_CellMouseEnter;
            dataGridView.CellMouseLeave += DataGridView_CellMouseLeave;



            prompt.Controls.Add(dataGridView);

            //----addButton

            addButton.Tag = new Dictionary<string, dynamic>() { { "dataGridView", dataGridView }, { "ComboBox1", ComboBox1 } };
            addButton.Click += AddButton_Click;

            prompt.Controls.Add(addButton);
            //----

            List<string> getSelectedItems()
            {
                List<string> result = new List<string>();
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    result.Add(row.Cells[0].Tag as string);
                }
                return result;
            }

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new Tuple<string, List<string>>("T", getSelectedItems()) : new Tuple<string, List<string>>("F", null);
        }

        private static void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            DataGridViewCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //revert to original value and color
            cell.Style.BackColor = System.Drawing.SystemColors.Window;
            cell.Style.ForeColor = System.Drawing.SystemColors.ControlText;
            cell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
            cell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            cell.Value = cell.Tag;
        }

        private static void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            DataGridViewCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.Style.BackColor = Color.Red;
            cell.Style.ForeColor = Color.White;
            cell.Style.SelectionBackColor = Color.Red;
            cell.Style.SelectionForeColor = Color.White;
            cell.Value = "Remove";
        }

        private static void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            Dictionary<string, dynamic> tagDict = senderDGV.Tag as Dictionary<string, dynamic>;

            ComboBox ComboBox1 = tagDict["ComboBox1"];

            //return item to combo box
            DataGridViewCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];


            ComboBox1.Items.Add(cell.Tag);

            senderDGV.Rows.RemoveAt(e.RowIndex);
        }
        private static void AddButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            Dictionary<string, dynamic> tagDict = senderButton.Tag as Dictionary<string, dynamic>;

            DataGridView dataGridView = tagDict["dataGridView"];
            ComboBox ComboBox1 = tagDict["ComboBox1"];





            if (ComboBox1.SelectedItem != null)
            {
                string item = ComboBox1.SelectedItem.ToString();


                dataGridView.Rows.Add();
                DataGridViewCell newCell = dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[0];
                //this has to be set manually due to some issue
                newCell.Style.BackColor = System.Drawing.SystemColors.Window;
                newCell.Style.ForeColor = System.Drawing.SystemColors.ControlText;
                newCell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
                newCell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;

                newCell.Value = item;
                newCell.Tag = item;


                //remove item from comboBox items
                ComboBox1.SelectedItem = null;
                ComboBox1.Items.Remove(item);

            }


        }
    }


    public static class Prompt
    {

        

        



        public static string[] ShowDialog(string text, string caption, bool addTextBox, string initialTextBoxText, bool addListBox, string[] listBoxArr, bool addCheckBox, string checkBoxText)
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
                textBox.Text = initialTextBoxText;
            }

            ComboBox ComboBox1 = new ComboBox() { Left = 50, Top = 70, Width = 400, AutoCompleteMode = AutoCompleteMode.Suggest };
            CheckBox CheckBox1 = new CheckBox() { Left = 50, Top = 70, Width = 400 };
            if (addListBox)
            {

                foreach (string key in listBoxArr)
                {
                    ComboBox1.Items.Add(key);
                }
                ComboBox1.SelectedIndex = 0;

                prompt.Controls.Add(ComboBox1);
            }
            else if (addCheckBox)
            {
                CheckBox1.Text = checkBoxText;
                prompt.Controls.Add(CheckBox1);
            }
            else
            {
                //add more room for label above
                textBox.Top += 20;
            }


            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 90, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);

            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { "T", textBox.Text, ComboBox1.SelectedItem != null ? ComboBox1.SelectedItem.ToString() : "" , CheckBox1.Checked?"T":"F"} : new string[] { "F", "", "" ,""};
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
            string[] input = Prompt.ShowDialog("Enter New Table Name:", "Rename \""+ tableName +"\" Table", true, tableName, false, null, false, null);

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

                if (selectedItem.Text == "No Bookmark" )
                {
                    DatabaseFunct.RemoveBookmarkOrTransferBookmarkToNewTableName(tableName, null);
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

            string[] input = Prompt.ShowDialog("Enter New Column Name:", "Rename \"" + _colName + "\" Column", true, _colName, false,  null,false,null);
            
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

