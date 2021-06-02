using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Drawing;

namespace MDB
{
    public static partial class DatabaseFunct
    {
        /*
        all functions for data editing within a table/subtable
        */










        //COLUMNS======================================================================================

        //used for both loading and adding new columns
        internal static void AddColumn(string colName, string colType, bool isLoad, DataGridView DGV)
        {




            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            string error = GenericFunct.ValidateNameInput(colName);


            if (error == "")
            {
                if (ColumnTypes.Types.ContainsKey(colType))
                {

                    if (!currentData[tableKey].ContainsKey((dynamic)colName) || isLoad)
                    {

                        bool valid = false;



                        if (colType == "Foreign Key Refrence")
                        {
                            string tableRefrence = "";

                            string[] MainTables = GetMainTableKeys();

                            //if no valid refrence already exists
                            if (!currentData[tableKey].ContainsKey((dynamic)(colName + RefrenceColumnKeyExt)))
                            {

                                string[] input = Prompt.ShowDialog("Choose a table to refrence from.", "Create Refrence Column", false, true, MainTables.ToArray());
                                tableRefrence = input[2];
                            }
                            else
                            {
                                tableRefrence = currentData[tableKey][colName + RefrenceColumnKeyExt];
                            }


                            if (tableRefrence != null && MainTables.Contains(tableRefrence))
                            {
                                if (currentData[tableRefrence].ContainsValue("Primary Key"))
                                {

                                    valid = true;
                                    //changes made to currentdata can't happen when loading a table
                                    if (!isLoad)
                                    {
                                        //add refrence kv pair

                                        currentData[tableKey][colName + RefrenceColumnKeyExt] = tableRefrence;

                                    }


                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("that table doesn't have a Primary Key column!");
                                }

                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("that table doesn't exist!");

                            }





                        }
                        else if (colType == "Parent Subtable Foreign Key Refrence")
                        {
                            string tableRefrence = "";

                            List<string> ParentSubTables = new List<string>();
                            string subjectTableKey = tableKey.Clone().ToString();

                            //look through parent table levels starting from current table level
                            while (subjectTableKey != "")
                            {
                                Console.WriteLine(subjectTableKey);
                                foreach (string key in currentData[subjectTableKey].Keys)
                                {
                                    Console.WriteLine(key);
                                    Console.WriteLine(currentData[subjectTableKey][key]);
                                    Console.WriteLine("--");
                                    if (currentData[subjectTableKey][key.ToString()] is String && currentData[subjectTableKey][key.ToString()] == "SubTable")
                                    {

                                        ParentSubTables.Add(subjectTableKey + "/" + key.ToString());
                                    }

                                }
                                Console.WriteLine("-----");

                                //remove everything after and including last '/'
                                if (subjectTableKey.Contains("/"))
                                {
                                    string regex = "/(?!.*/).*";
                                    subjectTableKey = Regex.Replace(subjectTableKey, regex, "");
                                }
                                else
                                {
                                    subjectTableKey = "";
                                }



                            }

                            //if no valid refrence already exists
                            if (!currentData[tableKey].ContainsKey((dynamic)(colName + ParentSubTableRefrenceColumnKeyExt)))
                            {

                                string[] input = Prompt.ShowDialog("Choose a subtable to refrence from.", "Create Parent SubTable Refrence Column", false, true, ParentSubTables.ToArray());
                                tableRefrence = input[2];
                            }
                            else
                            {
                                tableRefrence = currentData[tableKey][colName + ParentSubTableRefrenceColumnKeyExt];
                            }


                            if (tableRefrence != null && ParentSubTables.Contains(tableRefrence))
                            {
                                if (currentData[tableRefrence].ContainsValue("Primary Key"))
                                {

                                    valid = true;
                                    //changes made to currentdata can't happen when loading a table
                                    if (!isLoad)
                                    {
                                        //add refrence kv pair

                                        currentData[tableKey][colName + ParentSubTableRefrenceColumnKeyExt] = tableRefrence;

                                    }


                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("that subtable doesn't have a Primary Key column!");
                                }

                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("that table doesn't exist!");

                            }

                        }
                        else if (colType == "SubTable")
                        {
                            if (isLoad && currentData.ContainsKey(tableKey + "/" + colName))
                            {
                                valid = true;
                            }
                            else
                            {
                                valid = CreateSubTable(tableKey, colName);
                            }


                        }
                        else if (colType == "Primary Key")
                        {
                            if (!currentData[tableKey].ContainsValue("Primary Key") || isLoad)
                            {
                                valid = true;
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("this table already has a Primary Key Column!");
                            }
                        }
                        else
                        {
                            valid = true;
                        }




                        if (valid)
                        {

                            DataGridViewColumn myDataCol = (DataGridViewColumn)Activator.CreateInstance(ColumnTypes.Types[colType]);

                            myDataCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            myDataCol.MinimumWidth = 5;
                            myDataCol.HeaderText = colName;
                            myDataCol.Name = colName;
                            myDataCol.SortMode = DataGridViewColumnSortMode.NotSortable;

                            //set column flatstyle to pupup by default so that colors apear over buttons and comboboxes that are created
                            if (myDataCol.GetType() == typeof(DataGridViewButtonColumn))
                            {
                                DataGridViewButtonColumn x = myDataCol as DataGridViewButtonColumn;
                                x.FlatStyle = FlatStyle.Popup;
                                myDataCol = x;
                            }
                            else if (myDataCol.GetType() == typeof(DataGridViewComboBoxColumn))
                            {
                                DataGridViewComboBoxColumn x = myDataCol as DataGridViewComboBoxColumn;
                                x.FlatStyle = FlatStyle.Popup;
                                myDataCol = x;
                            }

                            //add first
                            DGV.Columns.Add(myDataCol);

                            //add new empty column rows to table data at level
                            if (!isLoad)
                            {




                                currentData[tableKey].Add(colName, colType);
                                currentData[tableKey][ColumnOrderRefrence].Add(colName);

                                List<string> DGVKeysList = new List<string>();
                                //get all subtable data of same column
                                List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysList);
                                List<DataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);


                                //add to other open subtables if any

                                foreach (DataGridView openDGV in openDGVs)
                                {

                                    if (openDGV != DGV)
                                    {
                                        DataGridViewColumn adjDataCol = (DataGridViewColumn)Activator.CreateInstance(ColumnTypes.Types[colType]);

                                        adjDataCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                        adjDataCol.HeaderText = colName;
                                        adjDataCol.Name = colName;
                                        adjDataCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                                        openDGV.Columns.Add(adjDataCol);
                                    }


                                }

                                int tableIndex = 0;
                                foreach (Dictionary<int, Dictionary<string, dynamic>> tableData in TableDataRowsAtSameLevel)
                                {
                                    //get open tables with column to set their cells to enabled

                                    string DGVKeyOfTable = DGVKeysList[tableIndex];

                                    DataGridView openDGVOfTable = null;
                                    //get openDGVOfTable
                                    foreach (DataGridView openDGV in openDGVs)
                                    {
                                        if (openDGV.Name == DGVKeyOfTable)
                                        {
                                            openDGVOfTable = openDGV;
                                        }
                                    }


                                    tableIndex += 1;



                                    //for all row entries add a null row to this column
                                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> entryData in tableData)
                                    {
                                        //define the default value
                                        dynamic defaultVal = null;
                                        if (myDataCol is DataGridViewCheckBoxColumn)
                                        {
                                            defaultVal = false;
                                            //set cell tag to enabled
                                            if (openDGVOfTable != null)
                                            {
                                                openDGVOfTable.Rows[entryData.Key].Cells[colName].Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                                            }



                                        }
                                        else if (currentData[tableKey][colName] == "SubTable")
                                        {
                                            defaultVal = new Dictionary<int, Dictionary<string, dynamic>>();
                                            //set cell tag to enabled
                                            if (openDGVOfTable != null)
                                            {
                                                openDGVOfTable.Rows[entryData.Key].Cells[colName].Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                                            }
                                        }
                                        //add it to directory
                                        entryData.Value.Add(colName, defaultVal);
                                        //Program.mainForm.TableMainGridView.Rows[entryData.Key].Cells[colName].Value = null;
                                    }
                                }




                            }


                            Console.WriteLine("valid column: " + colName);









                        }



                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("that column already exists!");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("column type entered is invalid");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("column name entered " + error);
            }


        }




        internal static void RemoveColumn(string colName, DataGridView DGV)
        {


            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);



            if (currentData[tableKey].ContainsKey((dynamic)colName))
            {
                //remove all disabler array connections to this column
                if (currentData[tableKey].ContainsKey(colName + ColumnDisablerArrayExt))
                {
                    List<string> disablerColumnNames = new List<string>();
                    foreach (string disablerColName in currentData[tableKey][colName + ColumnDisablerArrayExt])
                    {



                        disablerColumnNames.Add(disablerColName);

                    }
                    foreach (string selectedColKey2 in disablerColumnNames)
                    {
                        string selectedColKey1 = colName;
                        addColToDisablerArr(tableKey, selectedColKey1, selectedColKey2);
                    }

                }
                //-----


                string colType = currentData[tableKey][colName];



                if (colType == "Foreign Key Refrence")
                {
                    currentData[tableKey].Remove(colName + RefrenceColumnKeyExt);

                }
                else if (colType == "Parent Subtable Foreign Key Refrence")
                {
                    currentData[tableKey].Remove(colName + ParentSubTableRefrenceColumnKeyExt);

                }
                else if (colType == "SubTable")
                {
                    string subTableKey = tableKey + "/" + colName;


                    List<Tuple<DataGridView, int>> removalList = new List<Tuple<DataGridView, int>>();

                    //close subtables of this column
                    foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                    {
                        string openTableKey = ConvertDirToTableKey(openSubTable.Value.Item2.Name);
                        //subtable exists at same table level
                        if (subTableKey == openTableKey)
                        {
                            openSubTable.Key.Item1.Controls.Remove(openSubTable.Value.Item2);
                            removalList.Add(openSubTable.Key);
                            //close row
                            openSubTable.Key.Item1.Rows[openSubTable.Key.Item2].Height = openSubTable.Key.Item1.RowTemplate.Height;
                            openSubTable.Key.Item1.Rows[openSubTable.Key.Item2].DividerHeight = 0;

                        }
                    }
                    //remove from opensubtables
                    foreach (Tuple<DataGridView, int> key in removalList)
                    {
                        RemoveSubtableFromOpenSubtables(key);


                    }


                    //remove subtable data associated with column including it's children
                    string[] allKeys = GenericFunct.Clone(currentData.Keys.ToArray<string>());
                    foreach (string tableEntry in allKeys)
                    {
                        if (tableEntry.StartsWith(subTableKey + "/") || tableEntry == subTableKey)
                        {
                            currentData.Remove(tableEntry);
                        }
                    }


                }





                currentData[tableKey].Remove(colName);
                int index = 0;


                foreach (string orderColName in currentData[tableKey][ColumnOrderRefrence])
                {
                    if (orderColName == colName)
                    {
                        break;
                    }

                    index += 1;
                }
                currentData[tableKey][ColumnOrderRefrence].RemoveAt(index);




                List<string> throwawayDGVKeyList = new List<string>();
                //get all subtable data of same column
                List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(tableKey, ref throwawayDGVKeyList);

                foreach (Dictionary<int, Dictionary<string, dynamic>> tableData in TableDataRowsAtSameLevel)
                {
                    //for all row entries at table level remove this column's value
                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> entryData in tableData)
                    {

                        entryData.Value.Remove(colName);
                    }
                }

                //remove this column from adjacent open subtables at same level
                List<DataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(ConvertDirToTableKey(DGV.Name));

                foreach (DataGridView openDGV in openDGVs)
                {

                    if (openDGV != DGV)
                    {
                        DataGridViewColumn adjDataCol = (DataGridViewColumn)Activator.CreateInstance(ColumnTypes.Types[colType]);


                        openDGV.Columns.Remove(openDGV.Columns[colName]);
                    }


                }










                Console.WriteLine("removed column: " + colName);

                DGV.Columns.Remove(DGV.Columns[colName]);

                //last column in table is removed and therefore all row data is removed
                if (DGV.Columns.Count == 0)
                {
                    DGV.Rows.Clear();
                    Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;
                    tableData.Clear();
                }







            }
            else
            {
                System.Windows.Forms.MessageBox.Show("that column doesn't exist!?");
            }



        }



        internal static void ChangeColumnName(string colName, string newColName, DataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            string error = GenericFunct.ValidateNameInput(newColName);

            string colType = currentData[tableKey][colName];


            if (error == "")
            {
                if (ColumnTypes.Types.ContainsKey(colType))
                {

                    if (!currentData[tableKey].ContainsKey((dynamic)newColName))
                    {

                        //swap column within table data
                        currentData[tableKey][newColName] = colType;
                        //in col order
                        List<string> orderRef = (List<string>)currentData[tableKey][ColumnOrderRefrence];
                        int orderIndex = orderRef.IndexOf(colName);
                        orderRef.RemoveAt(orderIndex);
                        orderRef.Insert(orderIndex, newColName);
                        currentData[tableKey][ColumnOrderRefrence] = orderRef;


                       

                        //change name of disabler array starting with old colname:
                        if (currentData[tableKey].ContainsKey(colName + ColumnDisablerArrayExt))
                        {
                            currentData[tableKey][newColName + ColumnDisablerArrayExt] = currentData[tableKey][colName + ColumnDisablerArrayExt];
                            currentData[tableKey].Remove(colName + ColumnDisablerArrayExt);

                            //if a disabler array exists for this column, that means there are other disabler arrays containing this column
                            //in any adjacent disabler array
                            List<dynamic[]> replacers = new List<dynamic[]>();
                            foreach (KeyValuePair<string, dynamic> KV in currentData[tableKey])
                            {

                                int disablerIndex;
                                //is a disabler array and contains the renamed column
                                if (KV.Key.EndsWith(ColumnDisablerArrayExt) && (disablerIndex = KV.Value.IndexOf(colName)) != -1)
                                {
                                    List<string> disablerArr = (List<string>)currentData[tableKey][KV.Key];
                                    replacers.Add(new dynamic[] { KV.Key, disablerIndex });

                                }

                            }
                            //apply replacers to change colname in all disabler arrays
                            foreach (dynamic[] replacer in replacers)
                            {
                                List<string> disablerArr = (List<string>)currentData[tableKey][replacer[0]];
                                disablerArr.RemoveAt(replacer[1]);
                                disablerArr.Insert(replacer[1], newColName);
                                currentData[tableKey][replacer[0]] = disablerArr;
                            }

                        }

                        //remove it
                        currentData[tableKey].Remove(colName);

                        //rename column refrence data
                        if (colType == "Foreign Key Refrence")
                        {
                            string refDat = currentData[tableKey][colName + RefrenceColumnKeyExt];
                            //add new entry
                            currentData[tableKey][newColName + RefrenceColumnKeyExt] = refDat;
                            //remove old entry
                            currentData[tableKey].Remove(colName + RefrenceColumnKeyExt);

                        }
                        else if (colType == "Parent Subtable Foreign Key Refrence")
                        {
                            string refDat = currentData[tableKey][colName + ParentSubTableRefrenceColumnKeyExt];
                            //add new entry 
                            currentData[tableKey][newColName + ParentSubTableRefrenceColumnKeyExt] = refDat;
                            //remove old entry
                            currentData[tableKey].Remove(colName + ParentSubTableRefrenceColumnKeyExt);

                        }
                        //rename all data branching from the subtable if it is a subtable column being renamed
                        else if (colType == "SubTable")
                        {

                            List<string> keysToRemove = new List<string>();
                            SortedDictionary<string, dynamic> renamedEntries = new SortedDictionary<string, dynamic>();

                            List<dynamic[]> parentSubtableRefrenceEntries = new List<dynamic[]>();

                            //rename the direct table

                            renamedEntries[tableKey + "/" + newColName] = currentData[tableKey + "/" + colName];

                            string oldStart = tableKey + "/" + colName + "/";
                            string newStart = tableKey + "/" + newColName + "/";

                            //rename all sub tables below it
                            foreach (KeyValuePair<string, dynamic> tableKV in currentData)
                            {
                                //start with tables that begin with this subtable directory 
                                if (tableKV.Key.StartsWith(oldStart))
                                {
                                    
                                    //remove old starting directory from the old key string and replace it with the new start the get the key that will be used
                                    string newKey = newStart + tableKV.Key.Remove(0, oldStart.Length);
                                    renamedEntries[newKey] = tableKV.Value;

                                    keysToRemove.Add(tableKV.Key);
                                }
                                //look through all keys within table that refrence to this subtable via a ParentSubTableRefrenceColumn
                                if (tableKV.Key.StartsWith(tableKey + "/") || tableKV.Key == tableKey)
                                {
                                    Console.WriteLine("Searching for subtable refrence columns to alter directories in: " + tableKV.Key);
                                    foreach (KeyValuePair<string, dynamic> KV in tableKV.Value)
                                    {
                                        if (KV.Key.EndsWith(ParentSubTableRefrenceColumnKeyExt))
                                        {
                                            bool added = false;

                                            if (KV.Value.StartsWith(oldStart))
                                            {
                                                string newValue = newStart + tableKV.Value.Remove(0, oldStart.Length);
                                                parentSubtableRefrenceEntries.Add(new dynamic[] { tableKV.Key, KV.Key, newValue });
                                                added = true;
                                            }
                                            else if (KV.Value == tableKey + "/" + colName) //if refrencing this table directly
                                            {

                                                string newValue = tableKey + "/" + newColName;
                                                parentSubtableRefrenceEntries.Add(new dynamic[] { tableKV.Key, KV.Key, newValue });
                                                added = true;
                                            }
                                            Console.WriteLine(KV.Value + (added ? "- added" : ""));
                                        }

                                    }
                                    Console.WriteLine("---");
                                }



                            }

                            //get "/" index of old colName in directory string
                            char ch = '/';
                            //the old column name should always be after the number of "/" equivalent to oldDirIndex within the directories.
                            // "xxx/yyy/colname" tablekey is "xxx/yyy" so add 1 
                            int oldDirIndex = tableKey.Count(f => (f == ch)) + 1;



                            List<dynamic[]> replacer = new List<dynamic[]>();
                            //change all open DGV names that start with or are subtable directory
                            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                            {
                                string openTableDir = openSubTable.Value.Item2.Name;
                                string openTableKey = ConvertDirToTableKey(openTableDir);

                                if (openTableKey.StartsWith(oldStart))
                                {


                                    //"xxx/#,colname/#,xxx/..."
                                    //get parent name/directory
                                    //string parentDir = openSubTable.Key.Item1.Name;
                                    int charIndexBeforeOldColName = -1;
                                    for (int i = 0; i < oldDirIndex; i++)
                                    {
                                        charIndexBeforeOldColName = openTableDir.IndexOf(ch, charIndexBeforeOldColName + 1);
                                        if (charIndexBeforeOldColName == -1) break;
                                    }

                                    string parentDirBeforeColumn = openTableDir.Remove(charIndexBeforeOldColName, openTableDir.Length - (charIndexBeforeOldColName));
                                    Console.WriteLine("parent dir Before Column Name :" + parentDirBeforeColumn);
                                    

                                    //since the parent dir may be modified already first, i'll replace
                                    string clippedDir = openTableDir.Remove(0, parentDirBeforeColumn.Length);
                                    // "/#,colname/#,xxx/..." is what remains


                                    Regex rgx = new Regex(@"\,(.*?)\/");
                                    //replace first instance of ",colname/" and re insert parent dir
                                    string newDir = rgx.Replace(clippedDir, "," + newColName + "/", 1).Insert(0, parentDirBeforeColumn);


                                    Console.WriteLine("Renaming open table directory \"" + openTableDir + "\" to \"" + newDir + "\"");
                                    //change the name
                                    openSubTable.Value.Item2.Name = newDir;

                                }
                                else if (openTableKey == tableKey + "/" + colName) // is a subtable open within the column
                                {
                                    // "xxx/#,colname"
                                    // get parent name/directory
                                    string parentDir = openSubTable.Key.Item1.Name;
                                    string clippedDir = openTableDir.Remove(0, parentDir.Length);
                                    // "/#,colname" is what remains
                                    int index = clippedDir.LastIndexOf(",");

                                    string newDir = parentDir + clippedDir.Substring(0, index + 1) + newColName; // index + 1 to keep ","




                                    Console.WriteLine("Renaming open table directory " + openTableDir + " to " + newDir);
                                    //change the name
                                    openSubTable.Value.Item2.Name = newDir;
                                }


                            }
                            //---------------------------------
                            //apply changes

                            //apply altered refrence entries (this should also change entries within renamedEntries)
                            foreach (dynamic[] refEnt in parentSubtableRefrenceEntries)
                            {
                                string key = refEnt[0];
                                string subKey = refEnt[1];
                                string refAlt = refEnt[2];

                                Console.WriteLine("within \"" + key + "\" altering subtale refrence entry column \"" + subKey + "\" from \"" + currentData[key][subKey] + "\" to \"" + refAlt + "\"");
                                currentData[key][subKey] = refAlt;
                            }

                            //
                            foreach (string key in keysToRemove)
                            {
                                //remove old keys
                                currentData.Remove(key);
                            }

                            //concat current data with renamed entries
                            foreach (KeyValuePair<string, dynamic> KV in renamedEntries)
                            {
                                currentData.Add(KV.Key, KV.Value);
                            }

                            //change sub table key
                            dynamic dat = currentData[tableKey + "/" + colName];
                            currentData[tableKey + "/" + newColName] = dat;
                            currentData.Remove(tableKey + "/" + colName);

                        }

                        //change all data at table level
                        List<string> DGVKeysList = new List<string>();
                        //get all rows with the same column
                        List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysList);

                        foreach (Dictionary<int, Dictionary<string, dynamic>> TableDataBranch in TableDataRowsAtSameLevel)
                        {
                            foreach (KeyValuePair<int, Dictionary<string, dynamic>> TableDataRow in TableDataBranch)
                            {
                                TableDataRow.Value[newColName] = TableDataRow.Value[colName];
                                TableDataRow.Value.Remove(colName);
                            }
                        }



                        //change column name on open dgvs
                        List<DataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);

                        //change column within open dgvs

                        foreach (DataGridView openDGV in openDGVs)
                        {
                            openDGV.Columns[colName].HeaderText = newColName;
                            openDGV.Columns[colName].Name = newColName;


                        }








                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("that column already exists!");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("column type entered is invalid");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("column name entered " + error);
            }

            /*if (currentData.Keys.Contains(tabName))
            {
                string error = GenericFunct.ValidateNameInput(newTabName);


                if (error == "")
                {
                    if (!currentData.Keys.Contains(newTabName))
                    {
                        List<string> keysToRemove = new List<string>();
                        foreach (string key in currentData.Keys)
                        {
                            //change refrences to renamed table
                            foreach (string subKey in currentData[key].Keys)
                            {
                                //foregn key refrence to this table
                                if (subKey.EndsWith(RefrenceColumnKeyExt) && currentData[key][subKey] == tabName)
                                {
                                    currentData[key][subKey] = newTabName;
                                }
                                //change subtable refrences with this as the main table
                                if (subKey.EndsWith(ParentSubTableRefrenceColumnKeyExt) && currentData[key][subKey].StartsWith(tabName + "/"))
                                {
                                    currentData[key][subKey] = newTabName + currentData[key][subKey].Remove(0, tabName.Length);
                                }
                            }
                            //for all table data
                            if (key.StartsWith(tabName + "/") || key == tabName)
                            {
                                string newKey = newTabName + key.Remove(0, tabName.Length);
                                //old keys to be removed
                                keysToRemove.Add(key);
                                //re-implement under new name
                                currentData[newKey] = currentData[key];

                            }


                        }
                        foreach (string key in keysToRemove)
                        {
                            //remove old keys
                            currentData.Remove(key);
                        }

                        if (Program.mainForm.tabControl1.TabPages.ContainsKey(tabName))
                        {
                            //change tabcontrol data
                            Program.mainForm.tabControl1.TabPages[tabName].Text = newTabName;
                            Program.mainForm.tabControl1.TabPages[tabName].Name = newTabName;

                        }

                        //refresh table if it is the currently selected table (so that DGVs are renamed)
                        if (Program.mainForm.tabControl1.SelectedTab.Name == newTabName)
                        {
                            ChangeMainTable(newTabName);
                        }


                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("that table name already exists");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("table entered " + error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("table entered doesn't exist!");
            }*/

        }


        internal static void HideUnhideColumn(string colName, DataGridView DGV)
        {
            if (DGV.Columns[colName].Width != 2)
            {
                DGV.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                DGV.Columns[colName].MinimumWidth = 2;
                DGV.Columns[colName].Width = 2;

            }
            else
            {
                DGV.Columns[colName].MinimumWidth = 5;
                DGV.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }

        internal static void ShiftColumn(string colName, DataGridView DGV, bool isLeft)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            List<string> orderList = currentData[tableKey][ColumnOrderRefrence];

            int newIndex = orderList.IndexOf(colName);
            if (isLeft)
            {
                newIndex -= 1;
            }
            else
            {
                newIndex += 1;
            }

            if (newIndex > -1 && newIndex < orderList.Count)
            {
                orderList.Remove(colName);
                orderList.Insert(newIndex, colName);
                DGV.Columns[colName].DisplayIndex = newIndex;

                for (int colI = newIndex + 1; colI < orderList.Count; colI++)
                {
                    Console.WriteLine(orderList[colI]);
                    DGV.Columns[orderList[colI]].DisplayIndex = colI;
                }

            }




        }






        //ROWS============================================================================================================








        internal static void AddRow(DataGridView DGV, bool isInsert, int index)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            var columns = DGV.Columns;

            if (isInsert)
            {
                DGV.Rows.Insert(index);
                int i = tableData.Count() - 1;
                //iterate all tabledata from index
                while (i >= index)
                {
                    //close row beforehand since RecenterSubtables() only accounts for open rows 
                    //don't do this on the last index as that row would no longer exist though it's data does

                    DGV.Rows[i].Height = DGV.RowTemplate.Height;
                    DGV.Rows[i].DividerHeight = 0;



                    //iterate index
                    tableData[i + 1] = tableData[i];



                    DGV.Rows[i + 1].HeaderCell.Value = String.Format("{0}", i + 1);



                    //open subtable names will need to be shifted
                    SwapSubtableNames(DGV, i, i + 1);

                    i -= 1;


                }
            }
            else
            {
                index = tableData.Count;
                DGV.Rows.Add();

            }

            tableData[index] = new Dictionary<string, dynamic>();





            //add row header
            DGV.Rows[index].HeaderCell.Value = String.Format("{0}", index);


            //add entity to row entry refrences
            foreach (DataGridViewColumn column in columns)
            {
                //define default value
                dynamic defaultVal = null;

                //Default checkBox true and false values are the same and must be set to true and false
                if (DGV.Columns[column.Name] is DataGridViewCheckBoxColumn)
                {
                    defaultVal = false;
                    //check box is enabled by default
                    DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)DGV.Rows[index].Cells[column.Name];
                    //unsure if this is needed as cbcell is readonly when disabled
                    cbcell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                }
                if (currentData[tableKey][column.Name] == "SubTable")
                {
                    defaultVal = new Dictionary<int, Dictionary<string, dynamic>>();
                    //button is enabled by default
                    DataGridViewButtonCell bcell = (DataGridViewButtonCell)DGV.Rows[index].Cells[column.Name];
                    bcell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                }

                //add default value to tabledata
                tableData[index].Add(column.Name, defaultVal);


            }

            // there's no need to check for column disabler conditions since a row must already exist for isInsert to be called, so at no point will the last row of a table be removed or the first row added for the subtable to switch between having data and not having data.

            //if this is a subtable of a column with disabler conditions, check those conditions.
            //i need to get the column and row index from the DGV's parent
            var ParentObj = DGV.Parent;
            if (ParentObj is DataGridView && !isInsert)
            {
                DataGridView ParentDGV = (DataGridView)ParentObj;
                //how do i get the row index in which this subtable resides
                //i could use regex and stuff but this is easier

                int RowIndex = 0;
                string ColName = "";
                foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                {
                    if (openSubTable.Value.Item2 == DGV)
                    {
                        RowIndex = openSubTable.Key.Item2;
                        ColName = openSubTable.Value.Item1;
                    }
                }

                KeyValuePair<int, Dictionary<string, dynamic>> KVRow = new KeyValuePair<int, Dictionary<string, dynamic>>(RowIndex, GetTableDataFromDir(ParentDGV.Name)[RowIndex]);




                UpdateStatusOfAllRowCellsInDisablerArrayOfCell(ParentDGV, ConvertDirToTableKey(ParentDGV.Name), KVRow, ColName);
            }


        }


        internal static void LoadRow(KeyValuePair<int, Dictionary<string, dynamic>> entryData, DataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            var columns = DGV.Columns;


            var index = entryData.Key;


            if (DGV.Rows.Count - 1 < index)
            {
                DGV.Rows.Add();
            }



            //add row header
            DGV.Rows[index].HeaderCell.Value = String.Format("{0}", index);

            foreach (DataGridViewColumn column in columns)
            {


                if (tableData[index].ContainsKey(column.Name))
                {
                    var value = tableData[index][column.Name];
                    var displayValue = value;
                    Console.WriteLine("loaded row value: " + value);



                    //if it's a comboboxcolumn add the item to the cell
                    if (DGV.Columns[column.Name] is DataGridViewComboBoxColumn)
                    {
                        if (value != null)
                        {
                            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell() { Items = { value } };
                            DataGridViewComboBoxCell x = DGV.Rows[index].Cells[column.Name] as DataGridViewComboBoxCell;
                            cell.FlatStyle = x.FlatStyle;
                            DGV.Rows[index].Cells[column.Name] = cell;
                        }



                    }
                    else if (DGV.Columns[column.Name] is DataGridViewCheckBoxColumn)
                    {
                        DataGridViewCheckBoxCell cell = ColumnTypes.setBoolCellTFVals(new DataGridViewCheckBoxCell());

                        cell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };

                        //set checkbox state
                        var cellVal = cell.FalseValue;
                        if (value)
                        {
                            cellVal = cell.TrueValue;
                        }

                        displayValue = cellVal;

                        DGV.Rows[index].Cells[column.Name] = cell;
                    }
                    else if (DGV.Columns[column.Name] is DataGridViewButtonColumn)
                    {

                        DataGridViewButtonCell bcell = (DataGridViewButtonCell)DGV.Rows[index].Cells[column.Name];
                        bcell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                        bcell.Style.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableCellFore"];
                        bcell.Style.SelectionForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableSelectedCellFore"];

                        //get abreviated sub table data and set text to that
                        Dictionary<int, Dictionary<string, dynamic>> subtableData = tableData[index][column.Name] as Dictionary<int, Dictionary<string, dynamic>>;
                        Console.WriteLine(subtableData);
                        Console.WriteLine(tableData);

                        displayValue = ColumnTypes.GetSubTableCellDisplay(subtableData,column.Name,tableKey);

                    }

                    //assign data to column entry
                    DGV.Rows[index].Cells[column.Name].Value = displayValue;

                    //check if cell is enabled
                    bool isDisabled = IsDataRowCellStillDisabled(tableKey, new KeyValuePair<int, Dictionary<string, dynamic>>(index, tableData[index]), column.Name);
                    if (isDisabled)
                    {
                        DisableCellAtColAndRow(DGV, column.Name, index);
                    }

                }
                else
                {
                    dynamic defaultVal = null;
                    if (DGV.Columns[column.Name] is DataGridViewCheckBoxColumn)
                    {
                        defaultVal = false;
                    }
                    Console.WriteLine("load row create null value");
                    //just add the column to the entry as null and set entry data to such
                    tableData[index].Add(column.Name, null);
                    DGV.Rows[index].Cells[column.Name].Value = defaultVal;
                }




            }

        }


        //removes row from data
        internal static void RemoveRow(DataGridView DGV, int rowIndex)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;



            //remove open subtables at index
            if (Program.openSubTables.ContainsKey(new Tuple<DataGridView, int>(DGV, rowIndex)))
            {

                RemoveSubtableFromOpenSubtables(new Tuple<DataGridView, int>(DGV, rowIndex));
            }

            DGV.Rows.RemoveAt(rowIndex);

            //shift higher indexes down by 1
            int index = rowIndex + 1;
            while (index < tableData.Count())
            {
                //close row beforehand since RecenterSubtables() only accounts for open rows 
                //don't do this on the last index as that row would no longer exist though it's data does
                if (index < tableData.Count() - 1)
                {
                    DGV.Rows[index].Height = DGV.RowTemplate.Height;
                    DGV.Rows[index].DividerHeight = 0;
                }


                //paste down
                tableData[index - 1] = tableData[index];



                DGV.Rows[index - 1].HeaderCell.Value = String.Format("{0}", index - 1);



                //open subtable names will need to be shifted
                SwapSubtableNames(DGV, index, index - 1);

                index += 1;


            }
            //remove last in table
            tableData.Remove(tableData.Count() - 1);

            //if this is a subtable of a column with disabler conditions, check those conditions.
            //i need to get the column and row index from the DGV's parent
            //also only check if this is the last row to be removed
            var ParentObj = DGV.Parent;
            if (ParentObj is DataGridView && tableData.Count() == 0)
            {
                DataGridView ParentDGV = (DataGridView)ParentObj;
                //how do i get the row index in which this subtable resides
                //i could use regex and stuff but this is easier

                int RowIndex = 0;
                string ColName = "";
                foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                {
                    if (openSubTable.Value.Item2 == DGV)
                    {
                        RowIndex = openSubTable.Key.Item2;
                        ColName = openSubTable.Value.Item1;
                    }
                }

                KeyValuePair<int, Dictionary<string, dynamic>> KVRow = new KeyValuePair<int, Dictionary<string, dynamic>>(RowIndex, GetTableDataFromDir(ParentDGV.Name)[RowIndex]);


                UpdateStatusOfAllRowCellsInDisablerArrayOfCell(ParentDGV, ConvertDirToTableKey(ParentDGV.Name), KVRow, ColName);
            }


        }

        internal static void ShiftRow(DataGridView DGV, int rowIndex, bool isUp)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            int newIndex = rowIndex;
            if (isUp)
            {
                newIndex -= 1;

            }
            else//down
            {
                newIndex += 1;
            }

            if (newIndex >= 0 && newIndex <= DGV.Rows.Count - 1)
            {
                Dictionary<string, dynamic> storedRowData = GenericFunct.Clone(tableData[newIndex]);

                //swap row data
                tableData[newIndex] = tableData[rowIndex];


                tableData[rowIndex] = storedRowData;

                SwapSubtableNames(DGV, rowIndex, newIndex);

                //swap rows

                DataGridViewRow selectedRow = DGV.Rows[rowIndex];
                DGV.Rows.Remove(selectedRow);
                DGV.Rows.Insert(newIndex, selectedRow);
                DGV.Rows[rowIndex].HeaderCell.Value = String.Format("{0}", rowIndex);
                DGV.Rows[newIndex].HeaderCell.Value = String.Format("{0}", newIndex);

                //minimize row divider heights as they will be set later
                DGV.Rows[rowIndex].Height = DGV.RowTemplate.Height;
                DGV.Rows[rowIndex].DividerHeight = 0;
                DGV.Rows[newIndex].Height = DGV.RowTemplate.Height;
                DGV.Rows[newIndex].DividerHeight = 0;


            }



        }


    }
}
