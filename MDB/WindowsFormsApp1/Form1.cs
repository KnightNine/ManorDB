using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace MDB
{

    public partial class Form1 : Form
    {
        //extra spacing within open subtables
        static int subTableSpacing = 10;
        //indent value for subtable depth
        static int indentationValue = 10;
        //stored scroll point used to retain the panel1 scroll position when contents of panel are redrawn
        //Point scrollValue;


        

        public Form1()
        {
            InitializeComponent();
        }

        private DataGridView lastFocusedDGV = null;

        //focus on cell hover:
        public void TableMainGridView_Focus(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;

            //if cell content was in focus instead, winforms doesn't know if it was from this DGV or not
            //so instead of checking Focused, i'll check if the last focused DGV was this one
            //the ideal solution would be to disable this all together when cell content is brought into focus, but i'm unsure of when that happens so this will do for now.

            

            if (lastFocusedDGV != senderDGV)
            {
                Console.WriteLine("TMGV Focus");
                lastFocusedDGV = senderDGV;

                senderDGV.Focus();
            }
            
        }


        public void TableMainGridView_Got_Focus(object sender, EventArgs e)
        {
            
            DataGridView senderDGV = sender as DataGridView;
            
        }

        public void TableMainGridView_Click(object sender, EventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            senderDGV.Focus();
        }


        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            string[] input = Prompt.ShowDialog("Table Name:", "New Table", true, false, null);

            if (input[0] == "T")
            {
                DatabaseFunct.AddTable(input[1]);
            }
            
        }

        private void removeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[] input = Prompt.ShowDialog("Table Name:", "Remove Table", true, false, null);

            if (input[0] == "T")
            {
                DatabaseFunct.RemoveTable(input[1]);
            }
            
        }

        private void newColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //store scroll value for later
            //scrollValue = panel1.AutoScrollPosition;

            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, true, ColumnTypes.Types.Keys.ToArray<string>());
                if (input[0] == "T")
                {
                    DatabaseFunct.AddColumn(input[1], input[2], false, Program.mainForm.TableMainGridView);
                }
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }
        
        public void TableMainGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            /* me trying to increase the bool cell checkbox size:
             * 
            if (e.ColumnIndex > -1)
            {
                //increase size of check boxes of 
                DataGridView senderDGV = sender as DataGridView;


                if (senderDGV.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    e.PaintBackground(e.CellBounds, true);
                    ControlPaint.DrawCheckBox(e.Graphics, 11, 88, 44, 44, (bool)e.FormattedValue ? ButtonState.Checked : ButtonState.Normal);
                    e.Handled = true;
                }
            }
            
            */

        }

        private void newRowToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (DatabaseFunct.selectedTable != "")
            {
                if (Program.mainForm.TableMainGridView.Columns.Count > 0)
                {
                    DatabaseFunct.AddRow(Program.mainForm.TableMainGridView,false,0);
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

        public void subTableNewColumnButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            DataGridView senderDGV = senderButton.Parent as DataGridView;

            

            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, true, ColumnTypes.Types.Keys.ToArray<string>());

                if (input[0] == "T")
                {
                    DatabaseFunct.AddColumn(input[1], input[2], false, senderDGV);
                }
                

                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("no table selected");
            }
        }



        public void subTableNewRowButton_Click(object sender, EventArgs e)
        {

            Button senderButton = sender as Button;
            DataGridView senderDGV = senderButton.Parent as DataGridView;

            if (DatabaseFunct.selectedTable != "")
            {
                if (senderDGV.Columns.Count > 0)
                {
                    DatabaseFunct.AddRow(senderDGV, false, 0);
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

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("tabcontrol mouse down");
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl1.TabCount; ++i)
                {
                    if (tabControl1.GetTabRect(i).Contains(e.Location))
                    {
                        //tabs.Controls[i]; // this is your tab
                        var tableName = tabControl1.Controls[i].Text;
                        ContextMenuPrompt.ShowTableContextMenu(tableName);

                    }
                }

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
                dynamic val = ColumnTypes.ValidateCellInput(e, senderDGV);
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


                Console.WriteLine("value of entry " + rowIndex + " of column \"" + colName + "\" changed to: " + displayVal);

                KeyValuePair<int, Dictionary<string, dynamic>> KVRow = new KeyValuePair<int, Dictionary<string, dynamic>>(rowIndex, tableData[rowIndex]);
                DatabaseFunct.UpdateStatusOfAllRowCellsInDisablerArrayOfCell(senderDGV, tableKey, KVRow, colName);
            }

            


        }

        public void TableMainGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public void TableMainGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //scroll value here is re-added after cellvaluechanged
            //store scroll value for later
            //scrollValue = panel1.AutoScrollPosition;

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
                    string error = "";

                    string refrencedTableKey = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name + DatabaseFunct.RefrenceColumnKeyExt];

                    //make sure table still exists and that table still has a primary key col
                    if (!DatabaseFunct.currentData.ContainsKey(refrencedTableKey))
                    {

                        error += "The table being refrenced, \"" + refrencedTableKey + "\" no longer exists. ";
                    }
                    else if (!DatabaseFunct.currentData[refrencedTableKey].ContainsValue("Primary Key"))
                    {
                        error += "The table being refrenced, \"" + refrencedTableKey + "\", no longer contains a primary key columnn, add one or delete this column.";

                    }

                    if (error == "")
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

                            DataGridViewComboBoxCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;


                            cell.Items.Clear();
                            cell.Items.AddRange(primaryKeys.ToArray());
                            cell.Value = prevSel;







                        }
                    }
                    else
                    {
                        senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                        System.Windows.Forms.MessageBox.Show(error);
                    }

                }
                else if (colType == "Parent Subtable Foreign Key Refrence")
                {
                    string error = "";

                    string regex = "/(?!.*/).*";

                    string refrencedSubTableKey = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name + DatabaseFunct.ParentSubTableRefrenceColumnKeyExt];

                    string tableKeyContainingRefrencedSubTable = Regex.Replace(refrencedSubTableKey, regex, "");
                    string refrencedSubTableCol = Regex.Matches(refrencedSubTableKey, regex)[0].ToString().TrimStart('/');


                    string rowIndexOfRefrencedSubTable = "";
                    string subjectDirectory = tableDir.Clone().ToString();

                    //remove from subject until dir is equivalent to tableKeyContainingRefrencedSubTable

                    while (DatabaseFunct.ConvertDirToTableKey(subjectDirectory) != tableKeyContainingRefrencedSubTable )
                    {
                        if (!subjectDirectory.Contains("/"))
                        {
                            throw new Exception("subject directory not found");
                        }

                        //i also need to get the index for the row of this subtable 
                        //get after '/'
                            regex = @"([^\/]+$)";
                        string output = Regex.Matches(subjectDirectory, regex)[0].ToString();
                        //get before ','
                        regex = @"^[^,]+";
                        rowIndexOfRefrencedSubTable = Regex.Matches(output, regex)[0].ToString();
                        
                        // remove past and including last '/'
                        regex = "/(?!.*/).*";
                        subjectDirectory = Regex.Replace(subjectDirectory, regex, "");
                        

                    }

                    

                    

                    //make sure table still exists and that table still has a primary key col
                    if (!DatabaseFunct.currentData[tableKeyContainingRefrencedSubTable].ContainsKey(refrencedSubTableCol))
                    {

                        error += "The subtable column being refrenced, \"" + refrencedSubTableCol + "\" within \""+ tableKeyContainingRefrencedSubTable + " no longer exists.";
                    }
                    else if (!DatabaseFunct.currentData[refrencedSubTableKey].ContainsValue("Primary Key"))
                    {
                        error += "The subtable being refrenced, \"" + refrencedSubTableKey + "\", no longer contains a primary key columnn, add one or delete this column.";

                    }

                    if (error == "")
                    {

                        
                        var refrencedSubTable = DatabaseFunct.currentData[refrencedSubTableKey];
                        string primaryKeyCol = "";

                        //find primary key column name
                        foreach (KeyValuePair<string, dynamic> KV in refrencedSubTable)
                        {

                            if (KV.Value is string && KV.Value == "Primary Key")
                            {
                                primaryKeyCol = KV.Key;
                            }
                        }

                        if (primaryKeyCol != "")
                        {
                            // append the missing directory string of the same row and get the data
                            Dictionary<int, Dictionary<string, dynamic>> refrencedTableData = DatabaseFunct.GetTableDataFromDir(subjectDirectory + "/" + rowIndexOfRefrencedSubTable + "," + refrencedSubTableCol) as Dictionary<int, Dictionary<string, dynamic>>;

                            List<string> primaryKeys = new List<string>();
                            primaryKeys.Add("");
                            foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in refrencedTableData)
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

                            DataGridViewComboBoxCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewComboBoxCell;


                            cell.Items.Clear();
                            cell.Items.AddRange(primaryKeys.ToArray());
                            cell.Value = prevSel;







                        }
                    }
                    else
                    {
                        senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                        System.Windows.Forms.MessageBox.Show(error);
                    }

                }
            }

        }


        private DataGridView lastDGVClicked = null;

        public void TableMainGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            //deselect cells from last selected table (so that their selection doesn't intervene with copy and paste)
            if (lastDGVClicked != null && lastDGVClicked != senderDGV)
            {
                lastDGVClicked.ClearSelection();
            }
            lastDGVClicked = senderDGV;

        }

        public void TableMainGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderDGV = sender as DataGridView;
            string tableDir = senderDGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            // Ignore clicks that are on an empty table or a num column
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;

            //if it is a button cell
            var colType = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name];

            bool isEnabled = false;
            var selcell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (selcell is DataGridViewButtonCell)
            {
                DataGridViewButtonCell bcell = (DataGridViewButtonCell)selcell;
                isEnabled = ((Dictionary<string, dynamic>)bcell.Tag)["Enabled"];
            }
            else if (selcell is DataGridViewCheckBoxCell)
            {
                DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)selcell;
                isEnabled = ((Dictionary<string, dynamic>)cbcell.Tag)["Enabled"];
            }
                
            if (isEnabled)
            {
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
                                    //bCell.FlatStyle = FlatStyle.Popup;
                                    bCell.Style.BackColor = Color.LightGreen;
                                    bCell.Style.SelectionBackColor = Color.LightGreen;
                                }
                                else if (!bCell.ReadOnly)//change cell to default if not disabled
                                {
                                    
                                    //bCell.FlatStyle = FlatStyle.Standard;
                                    //default style
                                    if ((double)e.RowIndex % 2 == 0)
                                    {
                                        bCell.Style.BackColor = senderDGV.RowsDefaultCellStyle.BackColor;
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

                    //store scroll value for later
                    //scrollValue = panel1.AutoScrollPosition;
                    

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
                        Button[] newButtonArr = Program.GetSubTableButtons();

                        //add newdgv to parent
                        senderDGV.Controls.Add(newDGV);

                        DatabaseFunct.LoadTable(newDGV);

                        //add menu strip to new dgv
                        foreach (Button b in newButtonArr)
                        {
                            newDGV.Controls.Add(b);
                        }
                        

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

                        //update displayValue of button cell
                        Dictionary<int, Dictionary<string, dynamic>> subtableData = tableData[e.RowIndex][senderDGV.Columns[e.ColumnIndex].Name];

                        selcell.Value = ColumnTypes.GetSubTableCellDisplay(subtableData, senderDGV.Columns[e.ColumnIndex].Name, tableKey);

                    }
                    RecenterSubTables();
                    DatabaseFunct.loadingTable = false;

                }
                else if (colType == "Bool")
                {
                    //bools don't disable other cells since they have no null state
                    senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToBoolean(senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                }
            }
            

        }

        public void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateScrollBar();
            RecenterSubTables();
        }

        public void Panel_SizeChanged(object sender,EventArgs e)
        {
            UpdateScrollBar();
            Console.WriteLine("size_changed");
        }

        //scrolling from within the panel
        public void Panel_Scroll(object sender, MouseEventArgs e)
        {
            
            int x = vScrollBar1.Value + 0;
            if (e.Delta > 0)
            {
                //up
                x -= 1;
                if (x < 0)
                {
                    x = 0;
                }
            }
            else
            {
                //down
                x += 1;

                if (x > vScrollBar1.Maximum)
                {
                    x = vScrollBar1.Maximum;
                }
            }

            vScrollBar1.Value = x;



        }
        

        //sensitivity = pixels shifted per scroll value
        const double scrollSensitivity = 10;
        public void UpdateScrollBar()
        {
            int panelHeight = panel1.Size.Height;
            int windowHeight = ClientRectangle.Height;
            int space = (windowHeight - tabControl1.Height) - menuStrip1.Height;


            if (panelHeight > space)
            {
                vScrollBar1.Visible = true;
                int maxRange = panelHeight - space;
                vScrollBar1.Maximum = (int)(maxRange / scrollSensitivity);

            }
            else
            {
                vScrollBar1.Maximum = 0;
                vScrollBar1.Visible = false;
            }

            //if the scroll value changes due to maximum change
            SetPanelToScrollValue();
        }


        public void ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            SetPanelToScrollValue();
        }

        public void SetPanelToScrollValue()
        {
            panel1.Location = new Point(0, menuStrip1.Height - (int)(vScrollBar1.Value * scrollSensitivity));
            Console.WriteLine("scrollval: " + vScrollBar1.Value.ToString() + "// panel location: "+ panel1.Location.ToString());
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
            Program.mainForm.TableMainGridView.Width = ClientRectangle.Width - (vScrollBar1.Visible ? vScrollBar1.Width : 0);

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

                subDGV.Width = ClientRectangle.Width - (vScrollBar1.Visible ? vScrollBar1.Width : 0) - xOffset;

            }

            //setToLastStoredScrollValue();
            


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
