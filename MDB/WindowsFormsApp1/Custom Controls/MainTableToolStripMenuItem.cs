using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;



namespace MDB
{






    //class containing ToolStripMenuItems that are added and removed from the MenuStrip
    class MainTableToolStripMenuItem : ToolStripMenuItem
    {
        private Dictionary<string, EventHandler> MainTableFunctions = new Dictionary<string, EventHandler>()
        {
            {"Add Column", Program.mainForm.newColumnToolStripMenuItem_Click },
            {"Add Row", Program.mainForm.newRowToolStripMenuItem_Click },
            {"Edit Regex Refrence Table Constructor", Program.mainForm.editRegexRefrenceTableConstructorToolStripMenuItem_Click }
        };

        // MainTableFunctionsKey must be from dictionary above
        public MainTableToolStripMenuItem(string MainTableFunctionsKey)
        {
            InitializeComponent();
            this.Text = MainTableFunctionsKey;
            this.Name = MainTableFunctionsKey + "_ToolStripMenuItem";
            this.Size = new System.Drawing.Size(0, 48);
            this.AutoSize = true;
            if (MainTableFunctions.ContainsKey(MainTableFunctionsKey))
            {
                this.Click += MainTableFunctions[MainTableFunctionsKey];
            }
            else
            {
                MessageBox.Show("Error: No event associated with " + MainTableFunctionsKey);
            }


        }


        private void InitializeComponent()
        {

            //
            // ToolStripMenuItem
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;




        }

    }
}