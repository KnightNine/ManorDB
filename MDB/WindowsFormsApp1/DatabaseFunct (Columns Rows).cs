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
        internal static void AddColumn(string colName, string colType, bool isLoad, CustomDataGridView DGV)
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

                        //if loading the table, the column added is automatically valid since it already exists in currentdata (other errors would handle the columns cells being invalid)
                        if (isLoad)
                        {
                            valid = true;
                        }
                        else // if adding a new column
                        {
                            switch (colType)
                            {

                            
                            
                                case "Auto Table Constructor Script":
                                
                                    // make sure this is the main dgv and not a subtable
                                    if (DGV == Program.mainForm.TableMainGridView)
                                    {
                                        if (!currentData[tableKey].ContainsValue("Auto Table Constructor Script"))
                                        {
                                            valid = true;
                                        }
                                        else
                                        {

                                            System.Windows.Forms.MessageBox.Show("There is already an Auto Table Constructor Script Column in this Table!");
                                        }

                                    }
                                    else
                                    {
                                        //currently there's no support for refrencing scripts from a Parent Subtable Foreign Key Refrence column
                                        System.Windows.Forms.MessageBox.Show("A Auto Table Constructor Script must be placed in the Main Table and not in a Sub Table!");
                                    }
                                    break;
                                
                                case "Auto Table Constructor Script Receiver":
                                
                                    //check if a foreign key refrence exists in the same table to link to
                                    List<string> foreignKeyRefrenceColumnsInTable = new List<string>();
                                    foreach (KeyValuePair<string, dynamic> KV in currentData[tableKey])
                                    {
                                        if (KV.Value is string && KV.Value == "Foreign Key Refrence")
                                        {
                                            //add to valid columns to link to
                                            foreignKeyRefrenceColumnsInTable.Add(KV.Key);

                                        }
                                    }

                                    string refrenceColumnLinkedTo = null;

                                    if (foreignKeyRefrenceColumnsInTable.Count > 0)
                                    {
                                        // if refrenceColumnLinkedTo doesn't already exist
                                        if (!currentData[tableKey].ContainsKey((dynamic)(colName + ScriptReceiverLinkToRefrenceColumnExt)))
                                        {
                                            string[] input = Prompt.ShowDialog("Choose a Foreign Key Refrence.", "Create Auto Table Constructor Script Receiver Column", false, true, foreignKeyRefrenceColumnsInTable.ToArray());
                                            refrenceColumnLinkedTo = input[2];

                                            //store the linked column in current data
                                            currentData[tableKey][colName + ScriptReceiverLinkToRefrenceColumnExt] = refrenceColumnLinkedTo;

                                        }
                                        else
                                        {
                                            refrenceColumnLinkedTo = currentData[tableKey][colName + ScriptReceiverLinkToRefrenceColumnExt];
                                        }

                                        string tableKeyBeingRefrenced = currentData[tableKey][refrenceColumnLinkedTo + RefrenceColumnKeyExt];

                                        //now check if the selected refrenceColumnLinkedTo is refrencing a table with a "Auto Table Constructor Script" column
                                        bool refrencedTableContainsScript = false;
                                        foreach (KeyValuePair<string, dynamic> KV in currentData[tableKeyBeingRefrenced])
                                        {
                                            if (KV.Value is string && KV.Value == "Auto Table Constructor Script")
                                            {
                                                refrencedTableContainsScript = true;
                                            }
                                        }

                                        if (refrencedTableContainsScript)
                                        {

                                            // add the column
                                            valid = true;

                                        }
                                        else
                                        {
                                            
                                            System.Windows.Forms.MessageBox.Show("the foreign key refrence column you are linking this column to does not contain a \"Auto Table Constructor Script Column\" to read from!");
                                            

                                        }


                                    }
                                    else
                                    {
                                        
                                        
                                        System.Windows.Forms.MessageBox.Show("the table you are adding this column to must also contain a \"Foreign Key Refrence Column\" to link this column to!");
                                        
                                    }





                                    break;
                                case "Foreign Key Refrence":
                                
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


                                    if (tableRefrence != null && MainTables.Contains(tableRefrence) )
                                    {
                                        if (currentData[tableRefrence].ContainsValue("Primary Key") )
                                        {

                                            valid = true;
                                            //changes made to currentdata can't happen when loading a table
                                            
                                            //add refrence kv pair

                                            currentData[tableKey][colName + RefrenceColumnKeyExt] = tableRefrence;

                                            





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





                                    break;
                                case "Parent Subtable Foreign Key Refrence":
                                
                                    string parentSubtableRefrence = "";

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
                                        parentSubtableRefrence = input[2];
                                    }
                                    else
                                    {
                                        parentSubtableRefrence = currentData[tableKey][colName + ParentSubTableRefrenceColumnKeyExt];
                                    }


                                    if (parentSubtableRefrence != null && ParentSubTables.Contains(parentSubtableRefrence))
                                    {
                                        if (currentData[parentSubtableRefrence].ContainsValue("Primary Key"))
                                        {

                                            valid = true;
                                            //changes made to currentdata can't happen when loading a table
                                            
                                            
                                            //add refrence kv pair

                                            currentData[tableKey][colName + ParentSubTableRefrenceColumnKeyExt] = parentSubtableRefrence;

                                            


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

                                    break;
                                case "SubTable":
                                    
                                    
                                    valid = CreateSubTable(tableKey, colName);
                                    


                                    break;
                                case "Primary Key":
                                
                                    if (!currentData[tableKey].ContainsValue("Primary Key"))
                                    {
                                        valid = true;
                                    }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("this table already has a Primary Key Column!");
                                    }
                                    break;
                                default:
                                
                                    valid = true;
                                    break;
                            }
                        }



                    



                        if (valid)
                        {
                            

                            DataGridViewColumn myDataCol = Program.GetDataGridViewColumn(colName, colType);

                           

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
                                List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);


                                //add to other open subtables if any

                                foreach (CustomDataGridView openDGV in openDGVs)
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

                                    CustomDataGridView openDGVOfTable = null;
                                    //get openDGVOfTable
                                    foreach (CustomDataGridView openDGV in openDGVs)
                                    {
                                        if (openDGV.Name == DGVKeyOfTable)
                                        {
                                            openDGVOfTable = openDGV;
                                        }
                                    }


                                    tableIndex += 1;



                                    //for all row entries add a null entry for this column
                                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> entryData in tableData)
                                    {

                                        

                                        

                                        if (new string[3] { "Bool", "SubTable", "Auto Table Constructor Script Receiver" }.Contains(colType))
                                        {
                                            
                                            //set cell tag to enabled
                                            if (openDGVOfTable != null)
                                            {
                                                openDGVOfTable.Rows[entryData.Key].Cells[colName].Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                                            }



                                        }

                                        //define the default value
                                        dynamic defaultVal = ColumnTypes.GetDefaultColumnValue(colType);
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




        internal static void RemoveColumn(string colName, CustomDataGridView DGV)
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

                    //look for Auto Table Constructor Script Receiver columns that link to this column and remove those as well
                    List<string> removalList = new List<string>();
                    foreach (KeyValuePair<string,dynamic> KV in currentData[tableKey])
                    {
                        if (KV.Value == "Auto Table Constructor Script Receiver")
                        {
                            //if linked to this column
                            if (currentData[tableKey][KV.Key + ScriptReceiverLinkToRefrenceColumnExt] == colName)
                            {
                                string linkedColName = KV.Key;
                                //remove it
                                removalList.Add(linkedColName);
                            }
                            

                        }
                    }
                    foreach(string linkedColName in removalList)
                    {
                        RemoveColumn(linkedColName, DGV);
                    }


                }
                else if (colType == "Parent Subtable Foreign Key Refrence")
                {
                    currentData[tableKey].Remove(colName + ParentSubTableRefrenceColumnKeyExt);

                }
                else if (colType == "SubTable" || colType == "Auto Table Constructor Script Receiver")
                {
                    string subTableKey = tableKey + "/" + colName;


                    List<Tuple<CustomDataGridView, int>> removalList = new List<Tuple<CustomDataGridView, int>>();

                    //close subtables of this column
                    foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
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
                    foreach (Tuple<CustomDataGridView, int> key in removalList)
                    {
                        RemoveSubtableFromOpenSubtables(key);


                    }


                    //specific functions to either column type:

                    if (colType == "SubTable")
                    {
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
                    else if (colType == "Auto Table Constructor Script Receiver")
                    {
                        //remove the adjacent refrence column link data
                        currentData[tableKey].Remove(colName + ScriptReceiverLinkToRefrenceColumnExt);
                    }


                }
                



                currentData[tableKey].Remove(colName);


                //remove from ColumnOrderRefrence
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
                List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(ConvertDirToTableKey(DGV.Name));

                foreach (CustomDataGridView openDGV in openDGVs)
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

                    //also update Add Row button if this is a Subtable
                    var ParentObj = DGV.Parent;
                    if (ParentObj is CustomDataGridView)
                    {
                        DatabaseFunct.UpdateSubTableAddRowButton(DGV);
                    }

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

                            //if this column has any Auto Table Constructor Script Receiver columns that link to it
                            //rename the colName used in the ScriptReceiverLinkToRefrenceColumnExt
                            
                            foreach (KeyValuePair<string, dynamic> KV in currentData[tableKey])
                            {
                                if (KV.Value == "Auto Table Constructor Script Receiver")
                                {
                                    //if linked to this column
                                    if (currentData[tableKey][KV.Key + ScriptReceiverLinkToRefrenceColumnExt] == colName)
                                    {
                                        //change the linked column name
                                        currentData[tableKey][KV.Key + ScriptReceiverLinkToRefrenceColumnExt] = newColName;
                                    }


                                }
                            }
                            

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
                        else if (colType == "SubTable" || colType == "Auto Table Constructor Script Receiver")
                        {

                            string oldStart = tableKey + "/" + colName + "/";
                            string newStart = tableKey + "/" + newColName + "/";

                            //table structures in current data are only relevant to subtables
                            //and parentSubtableRefrenceEntries cannot be made to an adjacent Auto Table Constructor Script Receiver column since Auto Table Constructor Scripts don't support primary keys
                            if (colType == "SubTable")
                            {
                                List<string> keysToRemove = new List<string>();
                                SortedDictionary<string, dynamic> renamedEntries = new SortedDictionary<string, dynamic>();

                                List<dynamic[]> parentSubtableRefrenceEntries = new List<dynamic[]>();




                                

                                //rename the direct table

                                renamedEntries[tableKey + "/" + newColName] = currentData[tableKey + "/" + colName];



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
                            
                            

                            
                            
                            //get "/" index of old colName in directory string
                            char ch = '/';
                            //the old column name should always be after the number of "/" equivalent to oldDirIndex within the directories.
                            // "xxx/yyy/colname" tablekey is "xxx/yyy" so add 1 
                            int oldDirIndex = tableKey.Count(f => (f == ch)) + 1;



                            List<dynamic[]> replacer = new List<dynamic[]>();
                            //change all open DGV names that start with or are subtable directory
                            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
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
                        List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);

                        //change column within open dgvs

                        foreach (CustomDataGridView openDGV in openDGVs)
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



        }


        

        //---
        internal static void HideUnhideColumn(string colName, CustomDataGridView DGV)
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

        internal static void ShiftColumn(string colName, CustomDataGridView DGV, bool isLeft)
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
                //duplicate orderList
                List<string> oldOrderList = new List<string>(orderList);


                orderList.Remove(colName);
                orderList.Insert(newIndex, colName);
                

                

                //for all columns which have shifted position due to this change
                
                //if shifted right, a single column to the right has been shifted before the new index
                int colI = newIndex - 1;
                if (isLeft)
                {
                    //if shifted to the left, only the new index and indexes after it have been shifted
                    colI = newIndex;
                }

                //shift column DisplayIndexes
                while ( colI < orderList.Count)
                {
                    string thisColName = orderList[colI];
                    int thisOldIndex = oldOrderList.IndexOf(thisColName);
                    int thisNewIndex = colI;
                    DGV.Columns[thisColName].DisplayIndex = thisNewIndex;


                    colI++;
                }

                //shift cell outlines
                //DGV.ShiftColumnsOfInvalidCellIndexes(oldOrderList, orderList);

                /*
                //for all column indexes after the new shifted column index
                for (int colI = newIndex + 1; colI < orderList.Count; colI++)
                {
                    //adjust the display index
                    Console.WriteLine(orderList[colI]);
                    DGV.Columns[orderList[colI]].DisplayIndex = colI;

                }
                */




            }

        }






        //ROWS============================================================================================================








        internal static void AddRow(CustomDataGridView DGV, bool isInsert, int index)
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
                Console.WriteLine("Table Row Index: " + tableData.Count);
                index = tableData.Count;
                DGV.Rows.Add();

            }

            tableData[index] = new Dictionary<string, dynamic>();





            //add row header
            DGV.Rows[index].HeaderCell.Value = String.Format("{0}", index);


            //check if table is constructed
            bool isConstructed = false;
            string tableConstructorScript = null;
            Dictionary<string, dynamic> DGVTag = DGV.Tag as Dictionary<string, dynamic>;
            if (DGVTag.ContainsKey("tableConstructorScript"))
            {
                isConstructed = true;
                tableConstructorScript = DGVTag["tableConstructorScript"];
            }



            //add entity to row entry refrences
            foreach (DataGridViewColumn column in columns)
            {
                

                string colType = null;

                if (isConstructed)
                {
                    //fetch the column type from colTag is constructed:
                    Dictionary<string, dynamic> colTag = column.Tag as Dictionary<string, dynamic>;
                    colType = colTag["columnType"];
                }
                else
                {
                    colType = currentData[tableKey][column.Name];
                }

                


                //set cell tag to be enabled if it's one of these column types
                if (new string[3] { "Bool", "SubTable", "Auto Table Constructor Script Receiver" }.Contains(colType))
                {
                    DGV.Rows[index].Cells[column.Index].Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                }

                //define default value
                dynamic defaultVal = ColumnTypes.GetDefaultColumnValue(colType);
                //add default value to tabledata
                tableData[index].Add(column.Name, defaultVal);


            }

            // there's no need to check for column disabler conditions since a row must already exist for isInsert to be called, so at no point will the last row of a table be removed or the first row added for the subtable to switch between having data and not having data.

            //if this is a subtable of a column with disabler conditions, check those conditions.
            //i need to get the column and row index from the DGV's parent
            var ParentObj = DGV.Parent;
            if (ParentObj is CustomDataGridView && !isInsert)
            {
                CustomDataGridView ParentDGV = (CustomDataGridView)ParentObj;
                //how do i get the row index in which this subtable resides
                //i could use regex and stuff but this is easier

                int RowIndex = 0;
                string ColName = "";
                foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
                {
                    if (openSubTable.Value.Item2 == DGV)
                    {
                        RowIndex = openSubTable.Key.Item2;
                        ColName = openSubTable.Value.Item1;
                    }
                }

                KeyValuePair<int, Dictionary<string, dynamic>> KVRow = new KeyValuePair<int, Dictionary<string, dynamic>>(RowIndex, GetTableDataFromDir(ParentDGV.Name)[RowIndex]);

                //get if parent dgv is constructed
                bool isParentConstructed = false;
                
                Dictionary<string, dynamic> ParentDGVTag = ParentObj.Tag as Dictionary<string, dynamic>;
                if (ParentDGVTag.ContainsKey("tableConstructorScript"))
                {
                    isParentConstructed = true;
                    
                }

                //constructed tables don't have disabler array functionality.
                if (!isParentConstructed)
                {
                    UpdateStatusOfAllRowCellsInDisablerArrayOfCell(ParentDGV, ConvertDirToTableKey(ParentDGV.Name), KVRow, ColName);
                }
                
            }
            
            
            // if this is a Subtable
            if (ParentObj is CustomDataGridView)
            {
                DatabaseFunct.UpdateSubTableAddRowButton(DGV);
            }

        }


        internal static void DuplicateRow(CustomDataGridView DGV, int rowIndex)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            //collect duplicate data
            //pass the source dictionary into the destination's constructor:
            Dictionary<string, dynamic> dupedRowData = GenericFunct.Clone(tableData[rowIndex]);

            
            //insert a new row above the subject row
            AddRow(DGV, true, rowIndex);
            

            List<string> invalidColNames = new List<string>();

            //If the row has a primary key column, erase its data
            foreach (string colName in dupedRowData.Keys)
            {
                var colType = DatabaseFunct.currentData[tableKey][colName];
                if (colType == "Primary Key")
                {
                    invalidColNames.Add(colName);
                    
                }
            }

            foreach (string colName in invalidColNames)
            {
                dupedRowData[colName] = null;
            }

            //then write all data to that new row
            tableData[rowIndex] = dupedRowData;

            //set loadingTable to true
            DatabaseFunct.loadingTable = true;
            //reload the row
            List<string> columnOrderRefrence = currentData[tableKey][ColumnOrderRefrence] as List<string>;
            LoadRow(new KeyValuePair<int, Dictionary<string, dynamic>>(rowIndex, tableData[rowIndex]), DGV, false);
            //set it back to false
            DatabaseFunct.loadingTable = false;
            


        }


        internal static void LoadRow(KeyValuePair<int, Dictionary<string, dynamic>> entryData, CustomDataGridView DGV, bool isConstructed)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            var columns = DGV.Columns;


            var rowIndex = entryData.Key;


            if (DGV.Rows.Count - 1 < rowIndex)
            {
                DGV.Rows.Add();
            }


            //if constructed, remove all row data that is from columns that don't exist
            //and fetch the tableConstructorScript
            string tableConstructorScript = null;
            if (isConstructed)
            {
                List<string> removalList = new List<string>();
                foreach (string colKey in tableData[rowIndex].Keys)
                {
                    //key doesn't exist within columns and must be old data
                    if (!columns.Contains(colKey))
                    {
                        removalList.Add(colKey);
                    }
                }
                //erase from entryData.Value/tableData[index]
                foreach (string colKey in removalList)
                {
                    tableData[rowIndex].Remove(colKey);
                }


                //fetch script
                Dictionary<string, dynamic> DGVTag = DGV.Tag as Dictionary<string, dynamic>;
                tableConstructorScript = DGVTag["tableConstructorScript"];
            }



            //add row header
            DGV.Rows[rowIndex].HeaderCell.Value = String.Format("{0}", rowIndex);

            foreach (DataGridViewColumn column in columns)
            {
                int colIndex = columns.IndexOf(column);

                //get column type
                string colType = null;
                if (isConstructed)
                {

                    //fetch the constructed column type:
                    Dictionary<string, dynamic> colTag = column.Tag as Dictionary<string, dynamic>;
                    colType = colTag["columnType"];
                }
                else
                {
                    colType = currentData[tableKey][column.Name];
                }

                //if row data contains column
                if (tableData[rowIndex].ContainsKey(column.Name))
                {
                    var value = tableData[rowIndex][column.Name];
                    var displayValue = value;
                    Console.WriteLine("loaded row value: " + value);


                    

                    
                    
                        

                    //if the column type is an Auto Table Constructor Script, confirm that the script is valid
                    if (colType == "Auto Table Constructor Script" && value != null)
                    {
                        

                        string scriptError = AutoTableConstructorScriptFunct.ValidateScript(value);
                        if (scriptError != "")
                        {
                            //give the cell a red outline
                            DGV.AddInvalidCellIndexes(colIndex, rowIndex);
                        }
                        else
                        {
                            //remove the cell's red outline
                            DGV.RemoveInvalidCellIndexes(colIndex, rowIndex);
                        }
                    }
                    //if it's a comboboxcolumn add the item to the cell
                    else if (DGV.Columns[column.Name] is DataGridViewComboBoxColumn)
                    {
                        if (value != null)
                        {
                            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell() { Items = { value } };
                            DataGridViewComboBoxCell x = DGV.Rows[rowIndex].Cells[column.Name] as DataGridViewComboBoxCell;
                            cell.FlatStyle = x.FlatStyle;
                            DGV.Rows[rowIndex].Cells[column.Name] = cell;
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

                        DGV.Rows[rowIndex].Cells[column.Name] = cell;
                    }
                    else if (DGV.Columns[column.Name] is DataGridViewButtonColumn)
                    {

                        DataGridViewButtonCell bcell = (DataGridViewButtonCell)DGV.Rows[rowIndex].Cells[column.Name];
                        bcell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                        bcell.Style.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableCellFore"];
                        bcell.Style.SelectionForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableSelectedCellFore"];

                        //get abreviated sub table data and set text to that
                        Dictionary<int, Dictionary<string, dynamic>> subtableData = tableData[rowIndex][column.Name] as Dictionary<int, Dictionary<string, dynamic>>;
                        Console.WriteLine(subtableData);
                        Console.WriteLine(tableData[rowIndex]);

                        //this is left null if isConstructed is false so that the subtable structure would be pulled from currentData instead.
                        string subtableConstructorScript = null;
                        
                        if (colType == "Auto Table Constructor Script Receiver") //get receiver script 
                        {
                            
                            subtableConstructorScript = AutoTableConstructorScriptFunct.FetchTableConstructorScriptForReceiverColumn(tableKey, column.Name, tableData, rowIndex,DGV);

                            // if the primary key being refrenced is changed to a key that doesn't exist in the table being refrenced, set the subtableConstructorScript to be empty instead of null
                            // since giving a null subtableConstructorScript to GetSubTableCellDisplay() will cause issues
                            if (subtableConstructorScript == null)
                            {
                                subtableConstructorScript = "";
                                // and clear the constructed subtable's data
                                tableData[rowIndex][column.Name].Clear();
                            }
                        }
                        else if (isConstructed)
                        {
                            Dictionary<string, dynamic> columnTag = column.Tag as Dictionary<string, dynamic>;
                            subtableConstructorScript = columnTag["subtableConstructorScript"];
                        }

                        //get the display string that will show a few rows of the subtable's internal structure
                        displayValue = ColumnTypes.GetSubTableCellDisplay(subtableData,column.Name, tableKey, subtableConstructorScript);

                    }

                    //assign data to column entry
                    DGV.Rows[rowIndex].Cells[column.Name].Value = displayValue;

                    //constructed tables don't have an adjacentcolumndisabler
                    if (!isConstructed)
                    {
                        
                        //check if cell is enabled
                        bool isDisabled = IsDataRowCellStillDisabled(tableKey, new KeyValuePair<int, Dictionary<string, dynamic>>(rowIndex, tableData[rowIndex]), column.Name);
                        if (isDisabled)
                        {
                            DisableCellAtColAndRow(DGV, column.Name, rowIndex);
                        }
                    }
                    

                }
                else // if it doesn't already exist in row data
                {
                    //set to default 
                    dynamic defaultVal = ColumnTypes.GetDefaultColumnValue(colType);
                    
                    Console.WriteLine("load row create null value");
                    //just add the column to the entry as null and set entry data to such
                    tableData[rowIndex].Add(column.Name, defaultVal);
                    DGV.Rows[rowIndex].Cells[column.Name].Value = defaultVal;
                }




            }

        }


        //removes row from data
        internal static void RemoveRow(CustomDataGridView DGV, int rowIndex)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;



            //remove open subtables at index
            if (Program.openSubTables.ContainsKey(new Tuple<CustomDataGridView, int>(DGV, rowIndex)))
            {

                RemoveSubtableFromOpenSubtables(new Tuple<CustomDataGridView, int>(DGV, rowIndex));
            }

            DGV.Rows.RemoveAt(rowIndex);

            //shift larger indexes down by 1
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
            if (ParentObj is CustomDataGridView && tableData.Count() == 0)
            {
                CustomDataGridView ParentDGV = (CustomDataGridView)ParentObj;
                //how do i get the row index in which this subtable resides
                //i could use regex and stuff but this is easier

                int RowIndex = 0;
                string ColName = "";
                foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
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

            // if this is a Subtable
            if (ParentObj is CustomDataGridView)
            {
                DatabaseFunct.UpdateSubTableAddRowButton(DGV);
            }
            


        }

        internal static void ShiftRow(CustomDataGridView DGV, int rowIndex, bool isUp)
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
