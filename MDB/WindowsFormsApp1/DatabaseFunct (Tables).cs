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
        internal static void AddTable(string tabName, string tableType)
        {
            string error = GenericFunct.ValidateNameInput(tabName);

            if (error == "")
            {
                if (!currentData.Keys.Contains(tabName))
                {
                    //add dictionary with empty rowEntry database
                    currentData.Add(tabName, new Dictionary<string, dynamic>() { { RowEntryRefrence, new Dictionary<int, Dictionary<string, dynamic>>() }, { ColumnOrderRefrence, new List<string>() } });

                    if (tableType == "File Regex Refrence Table")
                    {
                        currentData[tabName][RegexRefrenceTableConstructorDataRefrence] = RegexRefrenceTableConstructorPromptHandler.CreateNewRegexRefrenceTableConstructorData();
                    }



                    Program.mainForm.customTabControl1.TabPages.Add(tabName, tabName);

                    Program.mainForm.label1.Visible = false;

                    //change color of tab page to not be visible (this absolutely breaks rendering so it's commented out)
                    //Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName)].BackColor = Color.Transparent;
                    //Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName)].ForeColor = Color.Transparent;


                    


                    //select new tab
                    Program.mainForm.customTabControl1.SelectedTab = Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName)];
                    ChangeMainTable(tabName);

                    

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("that table already exists!");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("table name entered " + error);
            }
        }


        //adds a colored bookmark on top of the tab
        internal static void BookmarkTable(string tabName, Color color)
        {

            

            Dictionary<string,Color> TabBookmarkColorsByName = Program.mainForm.customTabControl1.DisplayStyleProvider.TabBookmarkColorsByName;
            
            TabBookmarkColorsByName[tabName] = color;

            currentData[tabName][BookmarkColorRefrence] = color;

        }
        internal static void RemoveBookmarkOrTransferBookmarkToNewTableName(string tabName, string newTabName)
        {
            Dictionary<string, Color> TabBookmarkColorsByName = Program.mainForm.customTabControl1.DisplayStyleProvider.TabBookmarkColorsByName;
            if (TabBookmarkColorsByName.ContainsKey(tabName))
            {
                Color oldTabNameColor = TabBookmarkColorsByName[tabName];
                TabBookmarkColorsByName.Remove(tabName);

                //change if newTabName not null:
                if (newTabName != null)
                {
                    TabBookmarkColorsByName.Add(newTabName, oldTabNameColor);

                }
                else
                {
                    //remove BookmarkColorRefrence from table data
                    if (currentData.ContainsKey(tabName))
                    {
                        currentData[tabName].Remove(BookmarkColorRefrence);
                    }
                }

            }
            


        }


        internal static void RemoveTable(string tabName)
        {

            if (currentData.Keys.Contains(tabName))
            {
                if (selectedTable == tabName)
                {

                    //go to a different tab
                    if (Program.mainForm.customTabControl1.TabPages.Count > 1)
                    {
                        //do nothing, this happens automatically

                        /*if (Program.mainForm.customTabControl1.SelectedIndex > 0)
                        {
                            Program.mainForm.customTabControl1.SelectedTab = Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName) - 1];
                        }
                        else
                        {
                            Program.mainForm.customTabControl1.SelectedTab = Program.mainForm.customTabControl1.SelectedTab = Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName) + 1];
                        }*/

                    }
                    else
                    {
                        loadingTable = true;
                        ClearMainTable();
                        //update main table controls to remove them
                        UpdateMainTableControls();

                        Program.mainForm.panel1.Visible = false;
                        Program.mainForm.vScrollBar1.Visible = false;
                        Program.mainForm.label1.Visible = true;
                    }
                }

                Program.mainForm.customTabControl1.TabPages.RemoveAt(Program.mainForm.customTabControl1.TabPages.IndexOfKey(tabName));
                //remove from bookmarks
                RemoveBookmarkOrTransferBookmarkToNewTableName(tabName, null);

                string[] allKeys = currentData.Keys.ToArray<string>();
                //remove table and subtables within table from data
                foreach (string tabKey in allKeys)
                {
                    if (tabKey.StartsWith(tabName + "/") || tabKey == tabName)
                    {
                        currentData.Remove(tabKey);
                    }
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("that table doesn't exist!");
            }
            loadingTable = false;
        }


        internal static void ChangeTableName(string tabName, string newTabName)
        {
            if (currentData.Keys.Contains(tabName))
            {
                string error = GenericFunct.ValidateNameInput(newTabName);


                if (error == "")
                {
                    if (!currentData.Keys.Contains(newTabName))
                    {

                        List<string> keysToRemove = new List<string>();
                        SortedDictionary<string, dynamic> renamedEntries = new SortedDictionary<string, dynamic>();
                        List<dynamic[]> refrenceEntries = new List<dynamic[]>();

                        foreach (string key in currentData.Keys)
                        {

                            //for all table data
                            if (key.StartsWith(tabName + "/") || key == tabName)
                            {
                                string newKey = newTabName + key.Remove(0, tabName.Length);
                                //old keys to be removed
                                keysToRemove.Add(key);
                                //re-implement under new name
                                renamedEntries[newKey] = currentData[key];

                            }

                            //change refrences to renamed table
                            foreach (string subKey in currentData[key].Keys)
                            {
                                //foreign key refrence to this table
                                if (subKey.EndsWith(RefrenceColumnKeyExt) && currentData[key][subKey] == tabName)
                                {
                                    refrenceEntries.Add(new dynamic[] { key, subKey, newTabName });


                                }
                                //change subtable refrences with this as the main table
                                if (subKey.EndsWith(ParentSubTableRefrenceColumnKeyExt) && currentData[key][subKey].StartsWith(tabName + "/"))
                                {
                                    string refAlt = newTabName + currentData[key][subKey].Remove(0, tabName.Length);
                                    refrenceEntries.Add(new dynamic[] { key, subKey, refAlt });
                                }
                            }

                        }

                        //----------------------------------------------------------------------
                        //make changes to currentData

                        //apply altered refrence entries (this should also change entries within renamedEntries)
                        foreach (dynamic[] refEnt in refrenceEntries)
                        {
                            string key = refEnt[0];
                            string subKey = refEnt[1];
                            string refAlt = refEnt[2];

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

                        if (Program.mainForm.customTabControl1.TabPages.ContainsKey(tabName))
                        {
                            //change tabcontrol data
                            Program.mainForm.customTabControl1.TabPages[tabName].Text = newTabName;
                            Program.mainForm.customTabControl1.TabPages[tabName].Name = newTabName;
                        }

                        //refresh table if it is the currently selected table (so that DGVs are renamed)
                        if (Program.mainForm.customTabControl1.SelectedTab.Name == newTabName)
                        {
                            ChangeMainTable(newTabName);
                        }

                        //transfer over bookmark to newTabName
                        RemoveBookmarkOrTransferBookmarkToNewTableName(tabName,newTabName);

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
            }

        }




        internal static string[] GetMainTableKeys()
        {
            var MainTables = currentData.Keys.ToList<string>();
            foreach (string tab in currentData.Keys)
            {
                //remove subtables and config settings from list
                if (tab.Contains("/") || tab.Contains("@"))
                {
                    MainTables.Remove(tab);
                }
            }
            return MainTables.ToArray();
        }

        
        

        internal static void LoadTable(CustomDataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            loadingTable = true;

            //add columns of table in order of ColumnOrderRefrence
            foreach (string K in currentData[tableKey][ColumnOrderRefrence])
            {
                dynamic V = currentData[tableKey][K];
                //confirm that this column is a valid column type
                if (V is string && ColumnTypes.Types.ContainsKey(V))
                {

                    AddColumn(K, V, true, DGV);
                }

            }
            //find data source
            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
            {
                Console.WriteLine("loaded row " + entry.Key.ToString() + ": " + entry.Value);

                LoadRow(entry, DGV, false);
            }
            Console.WriteLine("---");



            loadingTable = false;
        }



        internal static void ChangeMainTable(string tabName)
        {


            if (currentData.Keys.ToArray<string>().Contains(tabName))
            {
                Program.mainForm.panel1.Visible = true;
                Program.mainForm.SetPanelToScrollValue();

                ClearMainTable();
                selectedTable = tabName;
                Program.mainForm.TableMainGridView.Name = tabName;
                //load the main table
                LoadTable(Program.mainForm.TableMainGridView);
                //update table controls
                UpdateMainTableControls();

                Program.mainForm.UpdateScrollBar();
                Program.mainForm.RecenterSubTables();


            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Table does not exist");
            }

        }
        internal static void ClearMainTable()
        {
            //clear subtables
            Console.WriteLine("removing the controls:");
            //Yes this needs to be done this way to work properly
            int i = Program.mainForm.TableMainGridView.Controls.Count - 1;
            while (i >= 0)
			{
                dynamic control = Program.mainForm.TableMainGridView.Controls[i];
				Console.WriteLine("     -" + control.Name + "," + control.ToString());
                //if control is a table:
				if (control is CustomDataGridView)
                {
					Program.mainForm.TableMainGridView.Controls.Remove(control);
                    Console.WriteLine("      removed");
				}
                i--;
				
			}
            //This should've replaced the code above but fails to remove all the controls when there are more than 3
			//Program.mainForm.TableMainGridView.Controls.Clear();
			

			Program.mainForm.TableMainGridView.ClearInvalidCellIndexes();
            Program.mainForm.TableMainGridView.Rows.Clear();
            Program.mainForm.TableMainGridView.Columns.Clear();
            //this should clear all the children of TableMainGridView
            
			Program.openSubTables.Clear();

		}

    }
}
