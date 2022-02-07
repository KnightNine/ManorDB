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

        private CustomDataGridView lastFocusedDGV = null;

        //focus on cell hover:
        public void TableMainGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            CustomDataGridView senderDGV = sender as CustomDataGridView;

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
            
            CustomDataGridView senderDGV = sender as CustomDataGridView;
            
        }

        public void TableMainGridView_Click(object sender, EventArgs e)
        {
            CustomDataGridView senderDGV = sender as CustomDataGridView;
            
        }

        

        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[] ListBoxArr = new string[] { "User Input Table", "File Regex Refrence Table" };
            string[] input = Prompt.ShowDialog("Enter table Name and Type:", "New Table", true,"", true, ListBoxArr,false,null);

            if (input[0] == "T")
            {
                DatabaseFunct.AddTable(input[1], input[2]);
            }
            
        }

        private void removeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string[] input = Prompt.ShowDialog("Table Name:", "Remove Table", true, "", false, null, false, null);

            if (input[0] == "T")
            {
                DatabaseFunct.RemoveTable(input[1]);
            }
            
        }


        public void newColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //store scroll value for later
            //scrollValue = panel1.AutoScrollPosition;

            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, "", true, ColumnTypes.Types.Keys.ToArray<string>(), false, null);
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
        public void newRowToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (DatabaseFunct.selectedTable != "")
            {
                if (Program.mainForm.TableMainGridView.Columns.Count > 0)
                {
                    DatabaseFunct.AddRow(Program.mainForm.TableMainGridView, false, 0);
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


        public void editRegexRefrenceTableConstructorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (DatabaseFunct.selectedTable != "")
            {
                //check for existing Regex Refrence Table Data

                CustomDataGridView TableMainGridView = Program.mainForm.TableMainGridView;
                string tableKey = DatabaseFunct.ConvertDirToTableKey(TableMainGridView.Name);
                if (DatabaseFunct.currentData[tableKey].ContainsKey(DatabaseFunct.RegexRefrenceTableConstructorDataRefrence))
                {
                    var RegexRefrenceTableConstructorData = DatabaseFunct.currentData[tableKey][DatabaseFunct.RegexRefrenceTableConstructorDataRefrence];
                    RegexRefrenceTableConstructorData = RegexRefrenceTableConstructorPromptHandler.ShowDialog( RegexRefrenceTableConstructorData);
                    if (RegexRefrenceTableConstructorData != null)
                    {
                        //set to new data:
                        DatabaseFunct.currentData[tableKey][DatabaseFunct.RegexRefrenceTableConstructorDataRefrence] = RegexRefrenceTableConstructorData;
                        //construct the table:
                        RegexRefrenceTableConstructorPromptHandler.ConstructRegexRefrenceTable();
                    }

                }
                    
                //there is no else case since editRegexRefrenceTableConstructorToolStripMenuItem isn't accesible if there's no RegexRefrenceTableConstructorDataRefrence in the table data


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
                CustomDataGridView senderDGV = sender as CustomDataGridView;


                if (senderDGV.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {
                    e.PaintBackground(e.CellBounds, true);
                    ControlPaint.DrawCheckBox(e.Graphics, 11, 88, 44, 44, (bool)e.FormattedValue ? ButtonState.Checked : ButtonState.Normal);
                    e.Handled = true;
                }
            }
            
            */

        }

        

        public void subTableNewColumnButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            CustomDataGridView senderDGV = senderButton.Parent as CustomDataGridView;

            

            if (DatabaseFunct.selectedTable != "")
            {

                string[] input = Prompt.ShowDialog("Name Column And Select Type", "Create Column", true, "", true, ColumnTypes.Types.Keys.ToArray<string>(),false,null);

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
            Console.WriteLine("New Row Pressed");

            Button senderButton = sender as Button;
            CustomDataGridView senderDGV = senderButton.Parent as CustomDataGridView;

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

        private void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DatabaseFunct.loadingTable)
            {
                DatabaseFunct.ChangeMainTable(Program.mainForm.customTabControl1.SelectedTab.Name);
            }

        }

        private void customTabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("tabcontrol mouse down");
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < customTabControl1.TabCount; ++i)
                {
                    if (customTabControl1.GetTabRect(i).Contains(e.Location))
                    {
                        //tabs.Controls[i]; // this is your tab
                        var tableName = customTabControl1.Controls[i].Text;
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



            CustomDataGridView senderDGV = sender as CustomDataGridView;

            string tableDir = senderDGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);
            

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            if (e.RowIndex > -1 && e.ColumnIndex > -1 && !DatabaseFunct.loadingTable)
            {

                //validate input of column type and return dynamic type var:
                dynamic val = ColumnTypes.ValidateCellInput(e, senderDGV);

                string displayVal = Convert.ToString(val);

                //get if this table is constructed by script
                Dictionary<string,dynamic> DGVTag = senderDGV.Tag as Dictionary<string,dynamic>;
                bool isConstructed = false;
                if (DGVTag.ContainsKey("tableConstructorScript"))
                {
                    isConstructed = true;
                }

                


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

                //get colType
                string colType = null;
                if (isConstructed)
                {
                    //fetch the column type from colTag is constructed:
                    Dictionary<string, dynamic> colTag = senderDGV.Columns[e.ColumnIndex].Tag as Dictionary<string, dynamic>;
                    colType = colTag["columnType"];
                }
                else
                {
                    colType = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name];
                }


                //if the column type is a foreign key refrence

                if (colType == "Foreign Key Refrence")
                {
                    //if this row has an open subtable
                    //get the columnname that subtable is being sourced from
                    string sourceColNameOfOpenSubTableAtRow = null;
                    Tuple<CustomDataGridView, int> subTableKey = new Tuple<CustomDataGridView, int>(senderDGV, rowIndex);
                    if (Program.openSubTables.ContainsKey(subTableKey))
                    {
                        Tuple<string, CustomDataGridView> openSubTableAtRow = Program.openSubTables[subTableKey];
                        sourceColNameOfOpenSubTableAtRow = openSubTableAtRow.Item1;
                    }


                    //look for Auto Table Constructor Script Receiver columns that link to this column and close their tables
                    List<string> removalList = new List<string>();



                    DatabaseFunct.loadingTable = true;

                    Dictionary<string, dynamic> relevantTableColumnData;
                    if (isConstructed)
                    {
                        relevantTableColumnData = new Dictionary<string, dynamic>();
                        //fetch column names and types from the table constructor scripts
                        string tableConstructorScript = DGVTag["tableConstructorScript"];

                        //get column scripts
                        Tuple<MatchCollection, bool, string[]> dat = AutoTableConstructorScriptFunct.FetchTopLevelScriptData(tableConstructorScript);
                        string[] columnScripts = dat.Item3;

                        //store name and type within relevantTableColumnData
                        foreach (string columnScript in columnScripts)
                        {
                            string[] columnDat = columnScript.Split(':');
                            //get name within <>
                            string columnName = Regex.Match(columnDat[0], @"(?<=\<)[^<>]*(?=\>)").Value;
                            //get type from shorthand
                            string columnType = AutoTableConstructorScriptFunct.columnTypeShorthandDict[columnDat[1].Trim()];

                            relevantTableColumnData.Add(columnName, columnType);
                        }
                    }
                    else
                    {
                        //use currentData if not constructed
                        relevantTableColumnData = DatabaseFunct.currentData[tableKey];
                    }

                    foreach (KeyValuePair<string, dynamic> KV in relevantTableColumnData)
                    {
                        if (KV.Value is string && KV.Value == "Auto Table Constructor Script Receiver")
                        {
                            Console.WriteLine(KV.Key);

                            //if linked to this fkey column
                            bool isLinked = false;

                            if (isConstructed)
                            {
                                //check if linked column from ATCSR tag
                                Dictionary<string, dynamic> ATCSRColumnTag = senderDGV.Columns[KV.Key].Tag as Dictionary<string, dynamic>;
                                isLinked = ATCSRColumnTag[DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt].Contains(colName);
                               
                            }
                            else
                            {
                                
                                dynamic linkedFKeyRefrenceColumnNameData = relevantTableColumnData[KV.Key + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];
                                //convert to list from string for backwards compatablity
                                if (linkedFKeyRefrenceColumnNameData is string)
                                {
                                    linkedFKeyRefrenceColumnNameData = new List<string>() { linkedFKeyRefrenceColumnNameData };
                                    relevantTableColumnData[KV.Key + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt] = linkedFKeyRefrenceColumnNameData;
                                }

                                isLinked = linkedFKeyRefrenceColumnNameData.Contains(colName);
                            }


                            if (isLinked)
                            {
                                string linkedColName = KV.Key;
                                int linkedColIndex = senderDGV.Columns.IndexOf(senderDGV.Columns[linkedColName]);



                                //if the open subTable of this row is sourcing it's data from the linked column
                                if (sourceColNameOfOpenSubTableAtRow == linkedColName)
                                {




                                    //remove the subtable at this row due to its table construction script being changed. 
                                    CloseSubTable(subTableKey, senderDGV, e.RowIndex, linkedColIndex);

                                    //
                                    RecenterSubTables();

                                }

                                string subtableConstructorScript = AutoTableConstructorScriptFunct.FetchTableConstructorScriptForReceiverColumn(tableKey, linkedColName, tableData, e.RowIndex, senderDGV);
                                //subtableConstructorScript shouldn't be null here unless there's an issue with the new foreign key refrence value being linked.
                                if (subtableConstructorScript == null)
                                {
                                    subtableConstructorScript = "";
                                }


                                //clear the linked column's data
                                ((Dictionary<int, Dictionary<string, dynamic>>)tableData[e.RowIndex][linkedColName]).Clear();

                                //get data within linked subtable
                                Dictionary<int, Dictionary<string, dynamic>> subtableData = tableData[e.RowIndex][linkedColName];
                                //update displayValue of button cell (to empty since there's no rows)
                                senderDGV.Rows[e.RowIndex].Cells[linkedColIndex].Value = ColumnTypes.GetSubTableCellDisplay(subtableData, senderDGV.Columns[e.ColumnIndex].Name, tableKey, subtableConstructorScript);




                            }


                        }
                    }
                    DatabaseFunct.loadingTable = false;
                }







                //if it isn't constructed
                if (!isConstructed)
                {
                    //update disabled cells
                    DatabaseFunct.UpdateStatusOfAllRowCellsInDisablerArrayOfCell(senderDGV, tableKey, KVRow, colName);
                }
                
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

            CustomDataGridView senderDGV = sender as CustomDataGridView;
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1)
                {
                    ContextMenuPrompt.ShowRowContextMenu(senderDGV, e.RowIndex);
                }

                else if (e.RowIndex == -1 && e.ColumnIndex > -1)
                {
                    Dictionary<string,dynamic> DGVTag = senderDGV.Tag as Dictionary<string,dynamic>;
                    //if this is a constructed table, there is no column related options
                    if (DGVTag.ContainsKey("tableConstructorScript"))
                    {
                        MessageBox.Show("You cannot use column altering functionality within a constructed table.");
                    }
                    else
                    {
                        ContextMenuPrompt.ShowColumnContextMenu(senderDGV, senderDGV.Columns[e.ColumnIndex].Name);
                    }
                    
                }
            }


            else if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;


                string tableDir = senderDGV.Name;
                string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

                Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

                //is this a contructed table
                bool isConstructed = false;
                

                //if this is a subtable column within an auto contructed table,then this is a constructed table:
                Dictionary<string, dynamic> senderDGVTag = senderDGV.Tag as Dictionary<string, dynamic>;
                if (senderDGVTag.ContainsKey("tableConstructorScript"))
                {
                    //this means that this is part of a constructor script
                    isConstructed = true;
                    

                }
                string colName = senderDGV.Columns[e.ColumnIndex].Name;

                string colType = null;
                Dictionary<string, dynamic> colTag = null;
                
                if (isConstructed)
                {
                    //fetch the column type from colTag is constructed:
                    colTag = senderDGV.Columns[e.ColumnIndex].Tag as Dictionary<string, dynamic>;
                    colType = colTag["columnType"];
                }
                else
                {
                    colType = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name];
                }

                
                if (colType == "Foreign Key Refrence")
                {
                    string error = "";

                    string refrencedTableKey = null;

                    if (isConstructed)
                    {

                        refrencedTableKey = colTag[DatabaseFunct.RefrenceColumnKeyExt];
                    }
                    else
                    {
                        refrencedTableKey = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name + DatabaseFunct.RefrenceColumnKeyExt];
                    }

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
                        var refrencedTable = DatabaseFunct.currentData[refrencedTableKey];
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

                    //get the subtable key refrenced
                    string refrencedSubTableKey = null;
                    string subjectDirectory = "";
                    string tableKeyContainingRefrencedSubTable = "";
                    string refrencedSubTableCol = "";

                    if (isConstructed)
                    {
                        string outwardDirectory = colTag[DatabaseFunct.ParentSubTableRefrenceColumnKeyExt];
                        subjectDirectory = AutoTableConstructorScriptFunct.FetchDirectoryFromOutwardDirectory(outwardDirectory, tableDir);


                        if (subjectDirectory == null)//end execution if error is thrown
                        {
                            
                            return;
                        }

                        refrencedSubTableKey = DatabaseFunct.ConvertDirToTableKey(subjectDirectory);

                        //match all after last '/'
                        string regex = @"[^/]*$";
                        //get the subtable's column name
                        refrencedSubTableCol = Regex.Match(refrencedSubTableKey, regex).Value;

                        // match everything after and including last '/'
                        regex = @"/[^/]*$";
                        //get the table/subtable that contains the refrenced subtable's column
                        tableKeyContainingRefrencedSubTable = Regex.Replace(refrencedSubTableKey, regex, "");


                    }
                    else
                    {

                        refrencedSubTableKey = DatabaseFunct.currentData[tableKey][senderDGV.Columns[e.ColumnIndex].Name + DatabaseFunct.ParentSubTableRefrenceColumnKeyExt];

                        //match everything after and including last '/'
                        string regex = @"/[^/]*$";     // note:  this does the same thing> "/(?!.*/).*";
                        //get the table/subtable that contains the refrenced subtable's column

                        tableKeyContainingRefrencedSubTable = Regex.Replace(refrencedSubTableKey, regex, "");

                        //match all after last '/'
                        regex = @"[^/]*$";
                        //get the subtable's column name
                        refrencedSubTableCol = Regex.Match(refrencedSubTableKey, regex).Value;


                        string rowIndexOfRefrencedSubTable = "";
                        subjectDirectory = tableDir.Clone().ToString();

                        //remove from subject until dir is equivalent to tableKeyContainingRefrencedSubTable

                        while (DatabaseFunct.ConvertDirToTableKey(subjectDirectory) != tableKeyContainingRefrencedSubTable)
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


                        subjectDirectory = subjectDirectory + "/" + rowIndexOfRefrencedSubTable + "," + refrencedSubTableCol;
                    }







                    if (!DatabaseFunct.currentData.ContainsKey(tableKeyContainingRefrencedSubTable))//check if tableKeyContainingRefrencedSubTable exists in currentData
                    {
                        //cannot refrence to a constructed table
                        error += "Table Structure Data for \"" + tableKeyContainingRefrencedSubTable + "\" does not exist, the refrenced subtable cannot be a constructed table!";

                    }
                    //make sure refrenced subtable column still exists 
                    else if (!DatabaseFunct.currentData[tableKeyContainingRefrencedSubTable].ContainsKey(refrencedSubTableCol))
                    {

                        error += "The subtable column being refrenced, \"" + refrencedSubTableCol + "\" within \"" + tableKeyContainingRefrencedSubTable + "\" doesn't exist.";
                    }
                    //check if refrenced column is a subtable column
                    else if (DatabaseFunct.currentData[tableKeyContainingRefrencedSubTable][refrencedSubTableCol] != "SubTable")
                    {

                        error += "The column being refrenced, \"" + refrencedSubTableCol + "\" within \"" + tableKeyContainingRefrencedSubTable + "\" is not a subtable column.";
                    }
                    //check if the table still has a primary key col
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
                            Dictionary<int, Dictionary<string, dynamic>> refrencedTableData = DatabaseFunct.GetTableDataFromDir(subjectDirectory) as Dictionary<int, Dictionary<string, dynamic>>;

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


        private CustomDataGridView lastDGVClicked = null;

        public void TableMainGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomDataGridView senderDGV = sender as CustomDataGridView;
            //deselect cells from last selected table (so that their selection doesn't intervene with copy and paste)
            if (lastDGVClicked != null && lastDGVClicked != senderDGV)
            {
                Console.WriteLine("clearing last DGV selection");
                lastDGVClicked.ClearSelection();
            }
            lastDGVClicked = senderDGV;

        }

        public void TableMainGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustomDataGridView senderDGV = sender as CustomDataGridView;
            string tableDir = senderDGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            // Ignore clicks that are on an empty table or a num column
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;

            //if it is a button cell
            string colName = senderDGV.Columns[e.ColumnIndex].Name;

            //is this a contructed table
            bool isConstructed = false;
            // the script used by the table constructed from this cell
            string subtableConstructorScript = null;

            //if this is a subtable column within an auto contructed table,then this is a constructed table:
            Dictionary<string, dynamic> senderDGVTag = senderDGV.Tag as Dictionary<string, dynamic>;
            if (senderDGVTag.ContainsKey("tableConstructorScript"))
            {
                //this means that this is part of a constructor script
                isConstructed = true;

                //try to find the constructor script for this column within the column's tag if it exists:
                if (senderDGV.Columns[colName].Tag != null)
                {
                    
                    Dictionary<string, dynamic> columnTag = senderDGV.Columns[colName].Tag as Dictionary<string, dynamic>;
                    if (columnTag.ContainsKey("subtableConstructorScript"))
                    {
                        subtableConstructorScript = columnTag["subtableConstructorScript"];
                    }
                    
                }
                
                
                

            }

            //get column type
            string colType = null;

            if (isConstructed)
            {
                
                //fetch the column type from colTag if constructed:
                Dictionary<string, dynamic> colTag = senderDGV.Columns[e.ColumnIndex].Tag as Dictionary<string, dynamic>;
                colType = colTag["columnType"];
            }
            else
            {
                //fetch from currentData
                colType = DatabaseFunct.currentData[tableKey][colName];
            }


            
            
            
                
                
            //fetch script for Auto Table Constructor Script Receiver column
            if (colType == "Auto Table Constructor Script Receiver")
            {

                subtableConstructorScript = AutoTableConstructorScriptFunct.FetchTableConstructorScriptForReceiverColumn(tableKey, colName, tableData, e.RowIndex,senderDGV);
                //if tableConstructorScript is null then an error was shown and cancel function here
                if (subtableConstructorScript == null)
                {
                    return;
                }
            }



            

           

            

            bool isEnabled = false;

            //selected cell
            var selcell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //check if enabled
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
                

                
                if (colType == "SubTable" || colType == "Auto Table Constructor Script Receiver")
                {
                    


                    DatabaseFunct.loadingTable = true;

                    //store scroll value for later
                    //scrollValue = panel1.AutoScrollPosition;
                    

                    Tuple<CustomDataGridView, int> subTableKey = new Tuple<CustomDataGridView, int>(senderDGV, e.RowIndex);

                    //add or replace subtable below row
                    if (!Program.openSubTables.ContainsKey(subTableKey) || Program.openSubTables[subTableKey].Item1 != senderDGV.Columns[e.ColumnIndex].Name)
                    {


                        if (Program.openSubTables.ContainsKey(subTableKey))
                        {
                            //remove old open table
                            DatabaseFunct.RemoveSubtableFromOpenSubtables(subTableKey);

                        }


                        //get the DGV setup for new subtable
                        //make columns read only if new subtable is constructed via subtableConstructorScript
                        CustomDataGridView newDGV = Program.GetGridView(subtableConstructorScript != null);
                        //-------------------------------------------------setting edits-----
                        newDGV.Dock = DockStyle.None;
                        

                        newDGV.Name = senderDGV.Name + "/" + e.RowIndex.ToString() + "," + senderDGV.Columns[e.ColumnIndex].Name;



                       
                        //store the table construction data within the DGV tag to be used instead of the table structure from currentData
                        if (subtableConstructorScript != null)
                        {
                            Dictionary<string, dynamic> DGVTag = newDGV.Tag as Dictionary<string, dynamic>;
                            DGVTag["tableConstructorScript"] = subtableConstructorScript;
                        }
                         
                        

                        //-------------------------------------------------------------------
                        Button[] newButtonArr = Program.GetSubTableButtons(subtableConstructorScript != null);

                        //add newdgv to parent
                        senderDGV.Controls.Add(newDGV);

                        //if the new subtable is constructed
                        if (subtableConstructorScript != null)
                        {
                            //load table structure from tableConstructorScript
                            AutoTableConstructorScriptFunct.ConstructSubTableStructureFromScript(newDGV);
                        }
                        else
                        {
                            //load table structure from currentData table 
                            DatabaseFunct.LoadTable(newDGV);
                        }
                        

                        //add buttons to new dgv
                        newDGV.Controls.AddRange(newButtonArr);

                        

                        // update table buttons (after table is loaded)
                        DatabaseFunct.UpdateSubTableAddRowButton(newDGV);

                        //add to open subtables
                        Program.openSubTables.Add(subTableKey, new Tuple<string, CustomDataGridView>(senderDGV.Columns[e.ColumnIndex].Name, newDGV));

                        //change color
                        ColorTabOfOpenTable(e.ColumnIndex,e.RowIndex,senderDGV);


                    }
                    else // close table
                    {
                        Console.WriteLine("close subtable/ATCSR table");

                        //close the subtable of the selected cell
                        CloseSubTable(subTableKey, senderDGV, e.RowIndex, e.ColumnIndex);

                        //update displayValue of button cell
                        Dictionary<int, Dictionary<string, dynamic>> subtableData = tableData[e.RowIndex][senderDGV.Columns[e.ColumnIndex].Name];
                        //if constructed, use stored script within column tag
                        selcell.Value = ColumnTypes.GetSubTableCellDisplay(subtableData, senderDGV.Columns[e.ColumnIndex].Name, tableKey, subtableConstructorScript);

                    }
                    RecenterSubTables();
                    DatabaseFunct.loadingTable = false;

                }
                else if (colType == "Bool")
                {
                    
                    senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Convert.ToBoolean(senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                }
            }
            

        }
        //function for indicating when a cell button is pressed
        private void ColorTabOfOpenTable(int colIndex, int rowIndex, CustomDataGridView senderDGV)
        {



            foreach (DataGridViewCell cell in senderDGV.Rows[rowIndex].Cells)
            {

                if (cell is DataGridViewButtonCell)
                {
                    DataGridViewButtonCell bCell = cell as DataGridViewButtonCell;
                    if (cell.ColumnIndex == colIndex)
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
                        if ((double)rowIndex % 2 == 0)
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

        private void CloseSubTable( Tuple<CustomDataGridView, int> subTableKey, CustomDataGridView senderDGV, int rowIndex, int colIndex)
        {
            DatabaseFunct.RemoveSubtableFromOpenSubtables(subTableKey);
            //set row height to default
            senderDGV.Rows[rowIndex].Height = senderDGV.RowTemplate.Height;
            senderDGV.Rows[rowIndex].DividerHeight = 0;

            //change color of all to default
            ColorTabOfOpenTable(-1,rowIndex,senderDGV);

            

            
            
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
            int space = (windowHeight - customTabControl1.Height) - menuStrip1.Height;


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

        public double GetDataGridViewHeightAtRow(CustomDataGridView DGV, int index)
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

            void subFunctExpandParentLoop(CustomDataGridView DGV)
            {

                CustomDataGridView parentDGV = DGV.Parent as CustomDataGridView;
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
            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
            {


                CustomDataGridView subDGV = openSubTable.Value.Item2;
                
                subFunctExpandParentLoop(subDGV);

                /*CustomDataGridView parentDGV = openSubTable.Key.Item1;
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
            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
            {
                CustomDataGridView subDGV = openSubTable.Value.Item2;
                CustomDataGridView parentDGV = openSubTable.Key.Item1;
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

        private void Form1_Load(object sender, EventArgs e)
        {

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
