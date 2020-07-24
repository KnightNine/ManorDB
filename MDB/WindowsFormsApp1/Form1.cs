using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MDB
{

    public partial class Form1 : Form
    {
        //extra spacing within open subtables
        static int subTableSpacing = 10;
        //indent value for subtable depth
        static int indentationValue = 10;
        //stored scroll point used to retain the panel1 scroll position when contents of panel are redrawn
        Point scrollValue;
        public Form1()
        {
            InitializeComponent();
        }

        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("New Table",
                       "Table Name:",
                       "Table",
                       0,
                       0);
            DatabaseFunct.AddTable(input);
        }

        private void removeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Remove Table",
                       "Table Name:",
                       "Table",
                       0,
                       0);
            DatabaseFunct.RemoveTable(input);
        }

        private void newColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scrollValue = panel1.AutoScrollPosition;
            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, true, ColumnTypes.Types.Keys.ToArray<string>());
                DatabaseFunct.AddColumn(input[0], input[1], false, Program.mainForm.TableMainGridView);
                panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X), Math.Abs(scrollValue.Y));
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }



        private void newRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scrollValue = panel1.AutoScrollPosition;
            if (DatabaseFunct.selectedTable != "")
            {
                if (Program.mainForm.TableMainGridView.Columns.Count > 0)
                {
                    DatabaseFunct.AddRow(Program.mainForm.TableMainGridView);
                    panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X), Math.Abs(scrollValue.Y));
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("add a column first");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }

        public void subTableNewColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem senderTSMI = sender as ToolStripMenuItem;
            DataGridView senderDGV = senderTSMI.GetCurrentParent().Parent as DataGridView;
            scrollValue = panel1.AutoScrollPosition;

            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, true, ColumnTypes.Types.Keys.ToArray<string>());


                DatabaseFunct.AddColumn(input[0], input[1], false, senderDGV);

                panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X), Math.Abs(scrollValue.Y));
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }

        public void subTableNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem senderTSMI = sender as ToolStripMenuItem;
            DataGridView senderDGV = senderTSMI.GetCurrentParent().Parent as DataGridView;
            scrollValue = panel1.AutoScrollPosition;

            if (DatabaseFunct.selectedTable != "")
            {
                if (senderDGV.Columns.Count > 0)
                {
                    DatabaseFunct.AddRow(senderDGV);
                    panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X), Math.Abs(scrollValue.Y));
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("add a column first");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DatabaseFunct.loadingTable)
            {
                DatabaseFunct.ChangeMainTable(Program.mainForm.tabControl1.SelectedTab.Name);
            }

        }

        public void TableMainGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            /*if (e.RowIndex < 0) return;
            //if it is a button cell
            var colType = DatabaseFunct.currentData[DatabaseFunct.selectedTable][Program.mainForm.TableMainGridView.Columns[e.ColumnIndex].Name];
            if (colType == "Foreign Key Refrence")
            {
                

                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                cell.Value = null;

                Program.mainForm.TableMainGridView.Rows[e.RowIndex].Cells[e.ColumnIndex] = cell;




            }*/
        }

        public void TableMainGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {



            DataGridView senderDGV = sender as DataGridView;

            string tableDir = senderDGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            if (e.RowIndex > -1 && e.ColumnIndex > -1 && !DatabaseFunct.loadingTable)
            {

                //validate input of column type and return dynamic type var:
                dynamic val = ColumnTypes.ValidateInput(e, senderDGV);
                string displayVal = Convert.ToString(val);



                //don't want to trigger CellValueChanged a second time for checkboxes
                if (!(val is bool))
                {
                    Console.WriteLine("rewriting cell display to... " + Convert.ToString(val));
                    //corrected string value is applied to textbox
                    senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = displayVal;
                }

                //add data to database
                var colName = senderDGV.Columns[e.ColumnIndex].Name;
                var rowIndex = e.RowIndex;


                //apply to data
                tableData[rowIndex][colName] = val;


                Console.WriteLine("value of entry " + rowIndex + " of column " + colName + " changed to: " + displayVal);
            }



        }

        public void TableMainGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1)
                {
                    ContextMenuPrompt.ShowRowContextMenu(senderDGV, e.RowIndex);
                }

                else if (e.RowIndex == -1 && e.ColumnIndex > -1)
                {
                    ContextMenuPrompt.ShowColumnContextMenu(senderDGV, senderDGV.Columns[e.ColumnIndex].Name);
                }
            }


            else if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;


                string tableDir = senderDGV.Name;
                string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

                Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;


                var colType = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name];
                if (colType == "Foreign Key Refrence")
                {
                    //refrenced tables are always main tables
                    var refrencedTable = DatabaseFunct.currentData[DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name + DatabaseFunct.RefrenceColumnKeyExt]];
                    string primaryKeyCol = "";

                    //find primary key column name
                    foreach (KeyValuePair<string, dynamic> KV in refrencedTable)
                    {

                        if (KV.Value is string && KV.Value == "Primary Key")
                        {
                            primaryKeyCol = KV.Key;
                        }
                    }

                    if (primaryKeyCol != "")
                    {
                        List<string> primaryKeys = new List<string>();
                        primaryKeys.Add("");
                        foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in refrencedTable[DatabaseFunct.RowEntryRefrence])
                        {
                            if (entry.Value[primaryKeyCol] != null)
                            {
                                primaryKeys.Add(entry.Value[primaryKeyCol]);
                            }
                            
                        }

                        //clear value if invalid
                        if (!primaryKeys.Contains(senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        {
                            senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                        }

                        var prevSel = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                        DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();


                        cell.Items.Clear();
                        cell.Items.AddRange(primaryKeys.ToArray());
                        cell.Value = prevSel;


                        senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex] = cell;




                    }
                    else
                    {

                        senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                        System.Windows.Forms.MessageBox.Show("Primary key no longer exists in that table");

                    }


                }
            }

        }


        public void TableMainGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        public void TableMainGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            string tableDir = senderDGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            // Ignore clicks that are on an empty table
            if (e.RowIndex < 0) return;
            //if it is a button cell
            var colType = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name];

            if (colType == "SubTable")
            {

                void colorTabOfOpenTable(int selectedColIndex)
                {



                    foreach (DataGridViewCell cell in senderDGV.Rows[e.RowIndex].Cells)
                    {

                        if (cell is DataGridViewButtonCell)
                        {
                            DataGridViewButtonCell bCell = cell as DataGridViewButtonCell;
                            if (cell.ColumnIndex == selectedColIndex)
                            {
                                //change cell to where color is visible
                                bCell.FlatStyle = FlatStyle.Popup;
                                bCell.Style.BackColor = Color.LightGreen;
                                bCell.Style.SelectionBackColor = Color.LightGreen;
                            }
                            else
                            {
                                //change cell to default
                                bCell.FlatStyle = FlatStyle.Standard;
                                //default style
                                if ((double)e.RowIndex % 2 == 0)
                                {
                                    bCell.Style.BackColor = senderDGV.DefaultCellStyle.BackColor;
                                }
                                else
                                {
                                    bCell.Style.BackColor = senderDGV.AlternatingRowsDefaultCellStyle.BackColor;
                                }

                                bCell.Style.SelectionBackColor = Color.LightCyan;


                            }


                        }
                    }
                }


                DatabaseFunct.loadingTable = true;


                scrollValue = panel1.AutoScrollPosition;
                panel1.SuspendLayout();

                Tuple<DataGridView, int> subTableKey = new Tuple<DataGridView, int>(senderDGV, e.RowIndex);

                //add or replace subtable below row
                if (!Program.openSubTables.ContainsKey(subTableKey) || Program.openSubTables[subTableKey].Item1 != senderDGV.Columns[e.ColumnIndex].Name)
                {


                    if (Program.openSubTables.ContainsKey(subTableKey))
                    {
                        DatabaseFunct.RemoveSubtableFromOpenSubtables(subTableKey);



                    }


                    DataGridView newDGV = Program.GetGridView();
                    //-------------------------------------------------setting edits-----
                    newDGV.Dock = DockStyle.None;


                    newDGV.Name = senderDGV.Name + "/" + e.RowIndex.ToString() + "," + senderDGV.Columns[e.ColumnIndex].Name;
                    //-------------------------------------------------------------------
                    MenuStrip newMenuStrip = Program.GetSubMenuStrip();

                    //add newdgv to parent
                    senderDGV.Controls.Add(newDGV);

                    DatabaseFunct.LoadTable(newDGV);

                    //add menu strip to new dgv
                    newDGV.Controls.Add(newMenuStrip);

                    //add to open subtables
                    Program.openSubTables.Add(subTableKey, new Tuple<string, DataGridView>(senderDGV.Columns[e.ColumnIndex].Name, newDGV));

                    //change color
                    colorTabOfOpenTable(e.ColumnIndex);


                }
                else // close table
                {
                    Console.WriteLine("close subtable");

                    DatabaseFunct.RemoveSubtableFromOpenSubtables(subTableKey);
                    senderDGV.Rows[e.RowIndex].Height = senderDGV.RowTemplate.Height;
                    senderDGV.Rows[e.RowIndex].DividerHeight = 0;

                    //change color of all to default
                    colorTabOfOpenTable(-1);

                }
                RecenterSubTables();
                DatabaseFunct.loadingTable = false;

            }
            else if (colType == "Bool")
            {
                senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToBoolean(senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
            }

        }

        public void MainForm_Resize(object sender, EventArgs e)
        {
            RecenterSubTables();
        }

        public void TableMainGridView_Scroll(object sender, ScrollEventArgs e)
        {
            //RecenterSubTables();
        }
        public void RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //RecenterSubTables();
        }
        public void RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RecenterSubTables();
        }



        /*public void ControlAddedToPanel(object sender, ControlEventArgs e)
        {
            Console.WriteLine(panel1.AutoScrollPosition.Y.ToString() + " but changed back to:");
            panel1.AutoScrollPosition = new Point(panel1.AutoScrollPosition.X, scrollYValue);
            Console.WriteLine(panel1.AutoScrollPosition.Y);
        }
        public void ControlRemovedFromPanel(object sender, ControlEventArgs e)
        {
           
        }*/

        public double GetDataGridViewHeightAtRow(DataGridView DGV, int index)
        {
            double height = DGV.ColumnHeadersHeight;
            if (index <= DGV.Rows.Count - 1 && index > -1)
            {
                for (int i = 0; i <= index; i++)
                {
                    height += DGV.Rows[i].Height;
                }
            }
            //collect all rows
            else if (index == -1)
            {
                int i = 0;
                while (i < DGV.Rows.Count)
                {
                    height += DGV.Rows[i].Height;
                    i++;
                }
            }
            else
            {
                Console.WriteLine("invalid row index call");
            }
            return height;

        }

        public void RecenterSubTables()
        {

            //Console.WriteLine(scrollValue.Y + " and then: ");

            void subFunctExpandParentLoop(DataGridView DGV)
            {

                DataGridView parentDGV = DGV.Parent as DataGridView;
                string[] TupleDataArr = DGV.Name.Split('/');
                Console.WriteLine(DGV.Name + " is the directory being expanded");

                int row = Convert.ToInt32(TupleDataArr.Last().Split(',')[0]);
                Console.WriteLine("expanding row: " + row.ToString());

                int subDGVHeight = (int)GetDataGridViewHeightAtRow(DGV, -1);
                //test


                parentDGV.Rows[row].DividerHeight = subDGVHeight + subTableSpacing;
                parentDGV.Rows[row].Height = parentDGV.RowTemplate.Height + subDGVHeight + subTableSpacing;

                DGV.Height = subDGVHeight + 5;

                if (parentDGV != Program.mainForm.TableMainGridView)
                {
                    subFunctExpandParentLoop(parentDGV);
                }

            }

            //Resize rows first
            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
            {


                DataGridView subDGV = openSubTable.Value.Item2;
                
                subFunctExpandParentLoop(subDGV);

                /*DataGridView parentDGV = openSubTable.Key.Item1;
                int row = openSubTable.Key.Item2;

                int subDGVHeight = (int)GetDataGridViewHeightAtRow(subDGV, -1);
                //test
                parentDGV.Rows[row].DividerHeight = subDGVHeight + subTableSpacing;
                parentDGV.Rows[row].Height = parentDGV.RowTemplate.Height + subDGVHeight + subTableSpacing;

                subDGV.Height = subDGVHeight+5;*/




            }

            Program.mainForm.TableMainGridView.Height = (int)GetDataGridViewHeightAtRow(Program.mainForm.TableMainGridView, -1) + 5;
            Program.mainForm.TableMainGridView.Width = panel1.Width - SystemInformation.VerticalScrollBarWidth;

            //Then move tables into position
            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
            {
                DataGridView subDGV = openSubTable.Value.Item2;
                DataGridView parentDGV = openSubTable.Key.Item1;
                int row = openSubTable.Key.Item2;


                //get x value relative to table key depth
                int xOffset = DatabaseFunct.ConvertDirToTableKey(subDGV.Name).Split('/').Count() - 1;
                xOffset *= indentationValue;


                //place subtable below row relative to parent table
                //adjust relative to TableMainGridView scroll
                subDGV.Location = new Point(xOffset, (int)(row != 0 ? GetDataGridViewHeightAtRow(parentDGV, row - 1) : subDGV.ColumnHeadersHeight) + parentDGV.RowTemplate.Height);//- Program.mainForm.TableMainGridView.VerticalScrollingOffset);

                subDGV.Width = panel1.Width - SystemInformation.VerticalScrollBarWidth - xOffset;

            }

            panel1.ResumeLayout();

            //Console.WriteLine(panel1.AutoScrollPosition.Y.ToString() + " but changed back to:");
            panel1.AutoScrollPosition = new Point(Math.Abs(panel1.AutoScrollPosition.X), Math.Abs(scrollValue.Y));




        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputOutput.ImportMDBFile(true);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputOutput.ExportMDBFile();
        }

        private void appendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputOutput.ImportMDBFile(false);
        }

        private void hideUnhideColumnToolStripMenuItem(object sender, System.EventArgs e)
        {
            ToolStripMenuItem menuItemSender = sender as ToolStripMenuItem;
            DatabaseFunct.HideUnhideColumn(menuItemSender.Name, TableMainGridView);
            menuItemSender.Checked = !menuItemSender.Checked;
            hideUnhideColumnsToolStripMenuItem.ShowDropDown();


        }

        private void hideUnhideColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableMainGridView.Columns.Count > 0)
            {
                //ContextMenuPrompt.ShowColumnHideContextMenu(TableMainGridView);

                hideUnhideColumnsToolStripMenuItem.DropDownItems.Clear();

                System.Collections.Generic.List<ToolStripItem> options = new System.Collections.Generic.List<ToolStripItem>();

                int i = 0;

                foreach (DataGridViewColumn col in TableMainGridView.Columns)
                {
                    bool check = false;
                    if (col.MinimumWidth != 2)
                    {
                        check = true;
                    }

                    options.Add(new ToolStripMenuItem() { Name = col.Name, Text = col.Name, Checked = check, ImageScaling = ToolStripItemImageScaling.None });
                    options[i].Click += new System.EventHandler(hideUnhideColumnToolStripMenuItem);
                    i += 1;
                }

                hideUnhideColumnsToolStripMenuItem.DropDownItems.AddRange(options.ToArray());
            }
            else
            {
                MessageBox.Show("No columns exist");
            }
        }

        /*public void hideUnhideColumnsToolStripMenuItemLostFocus(object sender, EventArgs e)
        {
           hideUnhideColumnsToolStripMenuItem.DropDown.Close();
            

            
        }*/
        

    }
    


    /* public static partial class ComboListBox
     {
         static DialogResult x;
         private static void ComboBoxNew_SelectValue(object sender, EventArgs e)
         {
             x = DialogResult.OK;

         }

     }*/




}
