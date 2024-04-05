using System;
using System.Windows.Forms;

namespace MDB
{
    partial class EditRegexRefrenceTableConstructorPrompt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRegexRefrenceTableConstructorPrompt));
            this.panel1 = new System.Windows.Forms.Panel();
            this.addColumnButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.isAutoTableConstructorScriptCheckBox = new System.Windows.Forms.CheckBox();
            this.isPrimaryKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.regexTextBox = new System.Windows.Forms.TextBox();
            this.columnNameTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.columnNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isPrimaryKeyColumn = new CustomDataGridViewCheckBoxColumn();
            this.isAutoTableConstructorScriptColumn = new CustomDataGridViewCheckBoxColumn();
            this.regexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.fileDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fileDirectoryButton = new System.Windows.Forms.Button();
            this.constructTableButton = new System.Windows.Forms.Button();
            this.autoResizePanel = new System.Windows.Forms.Panel();
            this.folderDirectoryButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.autoResizePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.addColumnButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.isAutoTableConstructorScriptCheckBox);
            this.panel1.Controls.Add(this.isPrimaryKeyCheckBox);
            this.panel1.Controls.Add(this.regexTextBox);
            this.panel1.Controls.Add(this.columnNameTextBox);
            this.panel1.Controls.Add(this.dataGridView);
            this.panel1.Location = new System.Drawing.Point(20, 154);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(719, 356);
            this.panel1.TabIndex = 0;
            // 
            // addColumnButton
            // 
            this.addColumnButton.AutoSize = true;
            this.addColumnButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.addColumnButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addColumnButton.Font = new System.Drawing.Font("Arial", 11.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addColumnButton.Location = new System.Drawing.Point(239, 56);
            this.addColumnButton.Margin = new System.Windows.Forms.Padding(1);
            this.addColumnButton.Name = "addColumnButton";
            this.addColumnButton.Size = new System.Drawing.Size(246, 28);
            this.addColumnButton.TabIndex = 7;
            this.addColumnButton.Text = "Add Column";
            this.addColumnButton.UseVisualStyleBackColor = false;
            this.addColumnButton.Click += new System.EventHandler(this.addColumnButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(534, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Regex:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Column Name:";
            // 
            // isAutoTableConstructorScriptCheckBox
            // 
            this.isAutoTableConstructorScriptCheckBox.AutoSize = true;
            this.isAutoTableConstructorScriptCheckBox.Location = new System.Drawing.Point(317, 28);
            this.isAutoTableConstructorScriptCheckBox.Margin = new System.Windows.Forms.Padding(1);
            this.isAutoTableConstructorScriptCheckBox.Name = "isAutoTableConstructorScriptCheckBox";
            this.isAutoTableConstructorScriptCheckBox.Size = new System.Drawing.Size(205, 17);
            this.isAutoTableConstructorScriptCheckBox.TabIndex = 4;
            this.isAutoTableConstructorScriptCheckBox.Text = "Is AutoTableConstructorScript Column";
            this.isAutoTableConstructorScriptCheckBox.UseVisualStyleBackColor = true;
            // 
            // isPrimaryKeyCheckBox
            // 
            this.isPrimaryKeyCheckBox.AutoSize = true;
            this.isPrimaryKeyCheckBox.Location = new System.Drawing.Point(182, 27);
            this.isPrimaryKeyCheckBox.Margin = new System.Windows.Forms.Padding(1);
            this.isPrimaryKeyCheckBox.Name = "isPrimaryKeyCheckBox";
            this.isPrimaryKeyCheckBox.Size = new System.Drawing.Size(130, 17);
            this.isPrimaryKeyCheckBox.TabIndex = 3;
            this.isPrimaryKeyCheckBox.Text = "Is Primary Key Column";
            this.isPrimaryKeyCheckBox.UseVisualStyleBackColor = true;
            // 
            // regexTextBox
            // 
            this.regexTextBox.Location = new System.Drawing.Point(530, 27);
            this.regexTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.regexTextBox.Name = "regexTextBox";
            this.regexTextBox.Size = new System.Drawing.Size(165, 20);
            this.regexTextBox.TabIndex = 2;
            this.regexTextBox.WordWrap = false;
            // 
            // columnNameTextBox
            // 
            this.columnNameTextBox.Location = new System.Drawing.Point(7, 28);
            this.columnNameTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.columnNameTextBox.Name = "columnNameTextBox";
            this.columnNameTextBox.Size = new System.Drawing.Size(165, 20);
            this.columnNameTextBox.TabIndex = 1;
            this.columnNameTextBox.WordWrap = false;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeight = 100;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNameColumn,
            this.isPrimaryKeyColumn,
            this.isAutoTableConstructorScriptColumn,
            this.regexColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView.EnableHeadersVisualStyles = false;
            this.dataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView.Location = new System.Drawing.Point(0, 87);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkRed;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.RowHeadersWidth = 102;
            this.dataGridView.RowTemplate.Height = 40;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView.Size = new System.Drawing.Size(719, 269);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_RowHeaderMouseClick);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // columnNameColumn
            // 
            this.columnNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnNameColumn.HeaderText = "Column Name";
            this.columnNameColumn.MinimumWidth = 12;
            this.columnNameColumn.Name = "columnNameColumn";
            this.columnNameColumn.ReadOnly = true;
            // 
            // isPrimaryKeyColumn
            // 
            this.isPrimaryKeyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isPrimaryKeyColumn.HeaderText = "Is Primary Key";
            this.isPrimaryKeyColumn.MinimumWidth = 12;
            this.isPrimaryKeyColumn.Name = "isPrimaryKeyColumn";
            this.isPrimaryKeyColumn.ReadOnly = true;
            // 
            // isAutoTableConstructorScriptColumn
            // 
            this.isAutoTableConstructorScriptColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.isAutoTableConstructorScriptColumn.HeaderText = "Is Auto Table Constructor Script";
            this.isAutoTableConstructorScriptColumn.MinimumWidth = 12;
            this.isAutoTableConstructorScriptColumn.Name = "isAutoTableConstructorScriptColumn";
            this.isAutoTableConstructorScriptColumn.ReadOnly = true;
            // 
            // regexColumn
            // 
            this.regexColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.regexColumn.HeaderText = "Regex";
            this.regexColumn.MinimumWidth = 12;
            this.regexColumn.Name = "regexColumn";
            this.regexColumn.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(744, 39);
            this.label3.TabIndex = 1;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // fileDirectoryTextBox
            // 
            this.fileDirectoryTextBox.Location = new System.Drawing.Point(147, 37);
            this.fileDirectoryTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.fileDirectoryTextBox.Name = "fileDirectoryTextBox";
            this.fileDirectoryTextBox.ReadOnly = true;
            this.fileDirectoryTextBox.Size = new System.Drawing.Size(473, 20);
            this.fileDirectoryTextBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 38);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "File/Folder Directory:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.900001F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(343, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Add columns to your File Regex Refrence table Here:";
            // 
            // fileDirectoryButton
            // 
            this.fileDirectoryButton.AutoSize = true;
            this.fileDirectoryButton.Location = new System.Drawing.Point(628, 37);
            this.fileDirectoryButton.Margin = new System.Windows.Forms.Padding(1);
            this.fileDirectoryButton.Name = "fileDirectoryButton";
            this.fileDirectoryButton.Size = new System.Drawing.Size(118, 23);
            this.fileDirectoryButton.TabIndex = 5;
            this.fileDirectoryButton.Text = "Search for File";
            this.fileDirectoryButton.UseVisualStyleBackColor = true;
            this.fileDirectoryButton.Click += new System.EventHandler(this.fileDirectoryButton_Click);
            // 
            // constructTableButton
            // 
            this.constructTableButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.constructTableButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constructTableButton.Location = new System.Drawing.Point(495, 519);
            this.constructTableButton.Margin = new System.Windows.Forms.Padding(1);
            this.constructTableButton.Name = "constructTableButton";
            this.constructTableButton.Size = new System.Drawing.Size(244, 30);
            this.constructTableButton.TabIndex = 0;
            this.constructTableButton.Text = "Construct Table";
            this.constructTableButton.UseVisualStyleBackColor = true;
            this.constructTableButton.Click += new System.EventHandler(this.constructTableButton_Click);
            // 
            // autoResizePanel
            // 
            this.autoResizePanel.Controls.Add(this.folderDirectoryButton);
            this.autoResizePanel.Controls.Add(this.label5);
            this.autoResizePanel.Controls.Add(this.label3);
            this.autoResizePanel.Controls.Add(this.fileDirectoryButton);
            this.autoResizePanel.Controls.Add(this.label6);
            this.autoResizePanel.Controls.Add(this.fileDirectoryTextBox);
            this.autoResizePanel.Controls.Add(this.label4);
            this.autoResizePanel.Location = new System.Drawing.Point(0, 0);
            this.autoResizePanel.Margin = new System.Windows.Forms.Padding(1);
            this.autoResizePanel.Name = "autoResizePanel";
            this.autoResizePanel.Size = new System.Drawing.Size(764, 559);
            this.autoResizePanel.TabIndex = 6;
            // 
            // folderDirectoryButton
            // 
            this.folderDirectoryButton.AutoSize = true;
            this.folderDirectoryButton.Location = new System.Drawing.Point(571, 62);
            this.folderDirectoryButton.Margin = new System.Windows.Forms.Padding(1);
            this.folderDirectoryButton.Name = "folderDirectoryButton";
            this.folderDirectoryButton.Size = new System.Drawing.Size(175, 23);
            this.folderDirectoryButton.TabIndex = 7;
            this.folderDirectoryButton.Text = "Search for Folder Containing Files";
            this.folderDirectoryButton.UseVisualStyleBackColor = true;
            this.folderDirectoryButton.Click += new System.EventHandler(this.folderDirectoryButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(106, 15);
            this.label6.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(282, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "This directory exists relative to to last Database file loaded.";
            // 
            // EditRegexRefrenceTableConstructorPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(763, 558);
            this.Controls.Add(this.constructTableButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.autoResizePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.MinimumSize = new System.Drawing.Size(729, 475);
            this.Name = "EditRegexRefrenceTableConstructorPrompt";
            this.Text = "Regex Refrence Table Constructor Editor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.autoResizePanel.ResumeLayout(false);
            this.autoResizePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        







        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.CheckBox isAutoTableConstructorScriptCheckBox;
        private System.Windows.Forms.CheckBox isPrimaryKeyCheckBox;
        private System.Windows.Forms.TextBox regexTextBox;
        private System.Windows.Forms.TextBox columnNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameColumn;
        private CustomDataGridViewCheckBoxColumn isPrimaryKeyColumn;
        private CustomDataGridViewCheckBoxColumn isAutoTableConstructorScriptColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn regexColumn;
        private System.Windows.Forms.Button addColumnButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox fileDirectoryTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button fileDirectoryButton;
        private System.Windows.Forms.Button constructTableButton;
        private System.Windows.Forms.Panel autoResizePanel;
        private System.Windows.Forms.Label label6;
        private Button folderDirectoryButton;
    }
}