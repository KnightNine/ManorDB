using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Reflection;
using Newtonsoft.Json;
using System.Drawing;

namespace MDB
{
    public static class InputOutput
    {
        //the path that was previously selected
        public static string selectedPath = "";

        //stores the path the user has previously navigated to.
        public static string defaultPath = "C:\\";

        //static string fileName = "Database";
        internal static void ExportMDBFile()
        {
            if (DatabaseFunct.currentData.Count > 0)
            {
                //convert currentData to json
                string js = Newtonsoft.Json.JsonConvert.SerializeObject(DatabaseFunct.currentData);

                Stream myStream;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "mdb files (*.mdb)|*.mdb|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = ".mdb";
                //saveFileDialog1.FileName = fileName;
                saveFileDialog1.InitialDirectory = defaultPath;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    if (saveFileDialog1.FileName != "")
                    {

                        defaultPath = saveFileDialog1.FileName;
                        selectedPath = Path.GetDirectoryName(saveFileDialog1.FileName);
                        File.WriteAllText(saveFileDialog1.FileName, js);



                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("filename field is empty!");
                    }




                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("cannot save an empty file");
            }



        }

        //open file with OpenFileDialog
        //isReplace is false if appending data from another table
        internal static void ImportMDBFile(bool isReplace)
        {

            

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "mdb files (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            //openFileDialog1.FileName = fileName;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                

                if (openFileDialog1.FileName != "" && openFileDialog1.FileName.EndsWith(".mdb"))
                {





                    ImportMDBFile(openFileDialog1.FileName, isReplace);





                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("no valid file selected!");
                }


            }
            

        }

        //isReplace is false if appending data from another table
        internal static void ImportMDBFile(string fileName, bool isReplace)
        {
            DatabaseFunct.loadingTable = true;
            string js = "";

            bool valid = true;

            int LargestATCSDuplicateIndex = 0;

            //deserialize all dicts and values
            void subFunct(int tableLevel, dynamic currentTable)
            {

                Type tabType = currentTable.GetType();

                Console.WriteLine("current level being converted: " + tableLevel);

                if (currentTable is SortedDictionary<string, dynamic> || currentTable is Dictionary<string, dynamic>)
                {


                    ConvertStringKeyDict(currentTable);
                }
                else if (currentTable is Dictionary<int, Dictionary<string, dynamic>>)
                {
                    int i = 0;
                    foreach (Dictionary<string, dynamic> value in currentTable.Values)
                    {
                        Console.WriteLine("at rowIndex: " + i);
                        ConvertStringKeyDict(value);
                        i += 1;
                    }

                }
                else if (tabType.IsGenericType && tabType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // do nothing

                }
                else if (currentTable is Dictionary<string, Dictionary<string, dynamic>>) // is construtor column data
                {
                    //do nothing
                }
                else
                {
                    Console.WriteLine("unhandled table/object type: " + currentTable.GetType().ToString());
                    MessageBox.Show("unhandled table/object type: " + currentTable.GetType().ToString());
                    valid = false;
                }


                void ConvertStringKeyDict(dynamic ct)
                {

                    Dictionary<string, dynamic> tableLevelKVs = new Dictionary<string, dynamic>();

                    foreach (dynamic key in ct.Keys)
                    {
                        Console.WriteLine("key is: " + key);
                        //Console.WriteLine("Value Type is: " + ct[key].GetType().ToString());





                        if (ct[key] is Newtonsoft.Json.Linq.JArray)
                        {
                            if (key == DatabaseFunct.ColumnOrderRefrence || key.EndsWith(DatabaseFunct.ColumnDisablerArrayExt) || key == RegexRefrenceTableConstructorPromptHandler.ColumnOrderRefrence || key.EndsWith(DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt))
                            {
                                //column order list OR disabler array
                                // and column order list within regex constructor data
                                tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ct[key].ToString());

                            }
                            else
                            {

                                Console.WriteLine("\"" + key + "\" key Array value not handled.");
                                valid = false;
                            }
                        }
                        else if (ct[key] is Newtonsoft.Json.Linq.JObject)
                        {

                            if (tableLevel < 1)
                            {
                                //tableData (the base table structure data)
                                tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ct[key].ToString());


                            }
                            else if (key == DatabaseFunct.RegexRefrenceTableConstructorDataRefrence)
                            {
                                //regex constructor table data
                                tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ct[key].ToString());
                            }
                            else if (key == RegexRefrenceTableConstructorPromptHandler.ColumnDataRefrence)
                            {
                                //the column order refrence inside regex constructor data
                                tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(ct[key].ToString());

                            }
                            else
                            {
                                //entryData (@RowEntries) and Subtable Data

                                tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, dynamic>>>(ct[key].ToString());

                            }
                        }
                        else if (ct[key] is Newtonsoft.Json.Linq.JValue) // (this is very likely redundant but is here anyways, just in case)
                        {


                            //any value 
                            tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(ct[key].ToString());

                            


                        }
                        else if (ct[key] is dynamic)//when converting dicts from json, keys are automatically converted to dynamic so "if (ct[key] is Newtonsoft.Json.Linq.JValue)" never gets used
                        {

                            if (key == DatabaseFunct.BookmarkColorRefrence)
                            {

                                //color is stored as a string within the database and must be converted back into a color object
                                tableLevelKVs[key] = Color.FromName(ct[key] as string);


                            }
                            else if (ct[key].GetType() == typeof(long))
                            {
                                //int64s must be int32s to register properly
                                tableLevelKVs[key] = (int)ct[key];
                            }
                            else if (tableLevel == 1 && ct[key].GetType() == typeof(string))
                            {
                                //see if duplicate column types exist
                                string x = (string)ct[key];
                                if (Regex.IsMatch(x,@"Auto Table Constructor Script\d+$")|| Regex.IsMatch(x, @"Auto Table Constructor Script Receiver\d+$"))
                                {
                                    int duplicateVal = Int32.Parse(Regex.Match(x, @"\d+$").Value);
                                    if (duplicateVal > LargestATCSDuplicateIndex)
                                    {
                                        LargestATCSDuplicateIndex = duplicateVal;
                                    }
                                }
                            }
                            else
                            {
                                //do nothing
                            }



                        }
                        else if (ct[key] == null)
                        {
                            tableLevelKVs[key] = null;
                        }
                        else
                        {
                            Console.WriteLine("\"" + key + "\" key value not handled.");
                            valid = false;
                        }

                    }
                    //apply to ct
                    foreach (KeyValuePair<string, dynamic> KV in tableLevelKVs)
                    {

                        ct[KV.Key] = KV.Value;
                        //if not a value type
                        if (!(KV.Value is ValueType) && KV.Value != null)
                        {
                            subFunct(tableLevel + 1, ct[KV.Key]);
                        }
                    }
                }


            }





            js = File.ReadAllText(fileName);
            Console.WriteLine(js);
            SortedDictionary<string, dynamic> cd;

            //appended/opened table data
            cd = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, dynamic>>(js);

            subFunct(0, cd);






            if (valid)
            {

                if (LargestATCSDuplicateIndex > ColumnTypes.scriptColumnTypeDuplicates)
                {
                    MessageBox.Show("Increasing the number of supported Script Column Type Duplicates to accomodate for the Script Column Type Duplicates defined in this .mdb file.");
                    ColumnTypes.SetScriptColumnTypeDuplicates(LargestATCSDuplicateIndex);
                }



                DatabaseFunct.ClearMainTable();

                if (isReplace)
                {
                    defaultPath = fileName;
                    selectedPath = Path.GetDirectoryName(fileName);

                    DatabaseFunct.currentData = cd;
                }
                else
                {
                    if (String.IsNullOrEmpty(selectedPath))
                    {
                        defaultPath = fileName;
                        selectedPath = Path.GetDirectoryName(fileName);
                    }

                    SortedDictionary<string, dynamic> fetchAllDataAssociatedWithMainTableInCD(string mainTabName)
                    {
                        SortedDictionary<string, dynamic> dat = new SortedDictionary<string, dynamic>();
                        foreach (KeyValuePair<string, dynamic> tabDat in cd)
                        {
                            if (tabDat.Key.StartsWith(mainTabName + "/") || tabDat.Key == mainTabName)
                            {
                                dat.Add(tabDat.Key, tabDat.Value);
                            }

                        }
                        return dat;
                    }
                    //if duplicate maintables exist overwrite them and indicate that they were overwritten
                    foreach (string tabKey in cd.Keys)
                    {
                        //if is maintable
                        if (!tabKey.Contains("/"))
                        {

                            bool add = false;
                            if (DatabaseFunct.currentData.ContainsKey(tabKey))
                            {

                                DialogResult dr = MessageBox.Show("Duplicate Maintable \"" + tabKey + "\" exists in appended mdb file. Overwrite current maintable data?", "Alert!", MessageBoxButtons.YesNo);

                                switch (dr)
                                {
                                    case DialogResult.Yes:

                                        //remove currentData table data and replace it with the appended file's table data
                                        DatabaseFunct.RemoveTable(tabKey);
                                        add = true;


                                        break;
                                    case DialogResult.No:
                                        //do nothing
                                        break;
                                }
                            }
                            else
                            {
                                add = true;
                            }

                            if (add)
                            {
                                Console.WriteLine("Added new table from appended file: \"" + tabKey + "\"");
                                // add the table data from the appended file:
                                SortedDictionary<string, dynamic> dat = fetchAllDataAssociatedWithMainTableInCD(tabKey);
                                DatabaseFunct.currentData = new SortedDictionary<string, dynamic>(DatabaseFunct.currentData.Concat(dat).ToDictionary(x => x.Key, x => x.Value));
                            }

                        }
                    }
                }
                /*string z = "";
                foreach (string tabKey in DatabaseFunct.currentData.Keys.ToArray())
                {
                    z += "\n" + tabKey;
                }

                MessageBox.Show(z);
                */
                // clear tabs
                Program.mainForm.customTabControl1.TabPages.Clear();
                //clear bookmarks
                Dictionary<string, Color> TabBookmarkColorsByName = Program.mainForm.customTabControl1.DisplayStyleProvider.TabBookmarkColorsByName;
                TabBookmarkColorsByName.Clear();


                //load tables
                string[] mainTableKeys = DatabaseFunct.GetMainTableKeys();
                foreach (string mainTableKey in mainTableKeys)
                {



                    Program.mainForm.customTabControl1.TabPages.Add(mainTableKey, mainTableKey);

                    //change color of tab page to not be visible (this absolutely breaks rendering)
                    //Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(mainTableKey)].BackColor = Color.Transparent;
                    //Program.mainForm.customTabControl1.TabPages[Program.mainForm.customTabControl1.TabPages.IndexOfKey(mainTableKey)].ForeColor = Color.Transparent;

                    //add tab bookmark if it exists
                    if (cd[mainTableKey].ContainsKey(DatabaseFunct.BookmarkColorRefrence))
                    {


                        DatabaseFunct.BookmarkTable(mainTableKey, cd[mainTableKey][DatabaseFunct.BookmarkColorRefrence]);

                    }


                    //if this is a File Regex Reference Table update its referenced data  
                    if (cd[mainTableKey].ContainsKey(DatabaseFunct.RegexRefrenceTableConstructorDataRefrence))
                    {
                        RegexRefrenceTableConstructorPromptHandler.UpdateRegexRefrenceTableData(mainTableKey);
                    }



                }




                //change table to first table

                DatabaseFunct.ChangeMainTable(Program.mainForm.customTabControl1.TabPages[0].Name);

                Program.mainForm.label1.Visible = false;

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("ReadError");
            }

            DatabaseFunct.loadingTable = false;

        }



    }



    


}



