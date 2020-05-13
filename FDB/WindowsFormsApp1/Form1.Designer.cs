using System.Drawing;
using System.Windows.Forms;

namespace FDB
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
            this.newColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideUnhideColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
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
            this.newColumnToolStripMenuItem,
            this.newLineToolStripMenuItem,
            this.hideUnhideColumnsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2517, 49);
            this.menuStrip1.TabIndex = 0;
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
            // newColumnToolStripMenuItem
            // 
            this.newColumnToolStripMenuItem.Name = "newColumnToolStripMenuItem";
            this.newColumnToolStripMenuItem.Size = new System.Drawing.Size(214, 45);
            this.newColumnToolStripMenuItem.Text = "New Column";
            this.newColumnToolStripMenuItem.Click += new System.EventHandler(this.newColumnToolStripMenuItem_Click);
            // 
            // newLineToolStripMenuItem
            // 
            this.newLineToolStripMenuItem.Name = "newLineToolStripMenuItem";
            this.newLineToolStripMenuItem.Size = new System.Drawing.Size(164, 45);
            this.newLineToolStripMenuItem.Text = "New Line";
            this.newLineToolStripMenuItem.Click += new System.EventHandler(this.newLineToolStripMenuItem_Click);
            // 
            // hideUnhideColumnsToolStripMenuItem
            // 
            this.hideUnhideColumnsToolStripMenuItem.Name = "hideUnhideColumnsToolStripMenuItem";
            this.hideUnhideColumnsToolStripMenuItem.Size = new System.Drawing.Size(337, 45);
            this.hideUnhideColumnsToolStripMenuItem.Text = "Hide/Unhide Columns";
            this.hideUnhideColumnsToolStripMenuItem.Click += new System.EventHandler(this.hideUnhideColumnsToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(998, 69);
            this.label1.TabIndex = 1;
            this.label1.Text = "Open or create a new FDB database";
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 991);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(2517, 100);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2517, 942);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.TableMainGridView_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(2517, 1091);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "FortressDB";
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
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem newTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTableToolStripMenuItem;

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem newColumnToolStripMenuItem;
        private ToolStripMenuItem newLineToolStripMenuItem;
        public Panel panel1;

        //added in Main
        public System.Windows.Forms.DataGridView TableMainGridView;

        private ToolStripMenuItem appendFileToolStripMenuItem;
        public ToolStripMenuItem hideUnhideColumnsToolStripMenuItem;
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
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 10, Text = text, Width = 400, Height = 40 };

            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            if (addTextBox)
            {
                
                prompt.Controls.Add(textBox);
            }

            ComboBox ComboBox1 = new ComboBox() { Left = 50, Top = 70, Width = 400 , AutoCompleteMode = AutoCompleteMode.Suggest};
            if (addListBox)
            {
                
                foreach (string key in listBoxArr)
                {
                    ComboBox1.Items.Add(key);
                }
                prompt.Controls.Add(ComboBox1);
            }

                
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 90, DialogResult = DialogResult.OK };

            
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { textBox.Text, ComboBox1.SelectedItem != null? ComboBox1.SelectedItem.ToString():"" }  : new string[] { "", "" };
        }
    }

    public static class ContextMenuPrompt
    {
        static DataGridView DGV;
        static int rowIndex;
        static ContextMenu cMenu = new ContextMenu();
        static string colName;


        public static void ShowRowContextMenu(DataGridView _DGV, int _rowIndex)
        {
            cMenu.MenuItems.Clear();
            DGV = _DGV;
            rowIndex = _rowIndex;

            

            MenuItem[] options = { new MenuItem() { Name = "Shift Row Up", Text = "Shift Row Up" }, new MenuItem() { Name = "Shift Row Down", Text = "Shift Row Down" }, new MenuItem() { Name = "Delete Row", Text = "Delete Row" } };
            options[0].Click += new System.EventHandler(shiftUp);
            options[1].Click += new System.EventHandler(shiftDown);
            options[2].Click += new System.EventHandler(deleteRow);


            cMenu.MenuItems.AddRange(options);
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));

        }
        public static void ShowColumnContextMenu(DataGridView _DGV, string _colName)
        {
            cMenu.MenuItems.Clear();
            DGV = _DGV;
            colName = _colName;


            MenuItem[] options = { new MenuItem() { Name = "Shift Column Left", Text = "Shift Column Left" }, new MenuItem() { Name = "Shift Column Right", Text = "Shift Column Right" }, new MenuItem() { Name = "Delete Column", Text = "Delete Column" } };
            options[0].Click += new System.EventHandler(shiftLeft);
            options[1].Click += new System.EventHandler(shiftRight);
            options[2].Click += new System.EventHandler(deleteColumn);


            cMenu.MenuItems.AddRange(options);
            Rectangle screenRectangle = Program.mainForm.RectangleToScreen(Program.mainForm.ClientRectangle);

            int titleHeight = screenRectangle.Top - Program.mainForm.Top;

            cMenu.Show(Program.mainForm, new Point(System.Windows.Forms.Cursor.Position.X - Program.mainForm.Location.X, System.Windows.Forms.Cursor.Position.Y - Program.mainForm.Location.Y - titleHeight));

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
            DatabaseFunct.RemoveRow(DGV, rowIndex);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();


        }
        private static void shiftLeft(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftColumn(colName, DGV, true);
            cMenu.Dispose();
        }
        private static void shiftRight(object sender, System.EventArgs e)
        {
            DatabaseFunct.ShiftColumn(colName, DGV, false);
            cMenu.Dispose();

        }

        private static void deleteColumn(object sender, System.EventArgs e)
        {
            DatabaseFunct.RemoveColumn(colName, DGV);
            Program.mainForm.RecenterSubTables();
            cMenu.Dispose();
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

