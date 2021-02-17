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

namespace MDB
{
    public static class InputOutput
    {
        static string defaultPath = "C:\\";
        //static string fileName = "Database";
        internal static void ExportMDBFile()
        {
            if (DatabaseFunct.currentData.Count > 0)
            {
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
        internal static void ImportMDBFile(bool isReplace)
        {

            DatabaseFunct.loadingTable = true;
            string js = "";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "mdb files (*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;
            //openFileDialog1.FileName = fileName;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (isReplace)
                {
                    DatabaseFunct.currentData = new SortedDictionary<string, dynamic>() { };
                    DatabaseFunct.ClearMainTable();
                    Program.mainForm.tabControl1.TabPages.Clear();
                }

                if (openFileDialog1.FileName != "" && openFileDialog1.FileName.EndsWith(".mdb"))
                {
                    defaultPath = openFileDialog1.FileName;

                    js = File.ReadAllText(openFileDialog1.FileName);
                    Console.WriteLine(js);
                    SortedDictionary<string, dynamic> cd;


                    cd = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, dynamic>>(js);
                    subFunct(0, cd);

                    bool valid = true;
                    //deserialize all dicts and values
                    void subFunct(int tableLevel, dynamic currentTable)
                    {
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
                        else
                        {
                            Console.WriteLine("unrecognized table type: " + currentTable.GetType().ToString());
                            valid = false;
                        }

                        void ConvertStringKeyDict(dynamic ct)
                        {

                            Dictionary<string, dynamic> tableLevelKVs = new Dictionary<string, dynamic>();

                            foreach (dynamic key in ct.Keys)
                            {
                                Console.WriteLine("key is: " + key);
                                //Console.WriteLine("Value Type is: " + ct[key].GetType().ToString());

                                //remove old table if duplicate:
                                if (tableLevel == 0 && DatabaseFunct.currentData.ContainsKey(key))
                                {
                                    DatabaseFunct.currentData.Remove(key);
                                    Console.WriteLine("removing old table: " + key);

                                }





                                if (ct[key] is Newtonsoft.Json.Linq.JArray)
                                {
                                    if (key == DatabaseFunct.ColumnOrderRefrence || key.EndsWith(DatabaseFunct.ColumnDisablerArrayExt))
                                    {
                                        //column order list
                                        tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(ct[key].ToString());

                                    }
                                }
                                else if (ct[key] is Newtonsoft.Json.Linq.JObject)
                                {

                                    if (tableLevel < 1)
                                    {
                                        //table
                                        tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ct[key].ToString());


                                    }
                                    else
                                    {
                                        //entryTable

                                        tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, dynamic>>>(ct[key].ToString());

                                    }
                                }
                                else if (ct[key] is Newtonsoft.Json.Linq.JValue)
                                {

                                    tableLevelKVs[key] = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(ct[key].ToString());
                                }
                                else if (ct[key] == null)
                                {
                                    tableLevelKVs[key] = null;
                                }
                                else
                                {
                                    valid = false;
                                }

                            }
                            //apply to ct
                            foreach (KeyValuePair<string, dynamic> KV in tableLevelKVs)
                            {

                                ct[KV.Key] = KV.Value;
                                if (!(KV.Value is ValueType) && KV.Value != null)
                                {
                                    subFunct(tableLevel + 1, ct[KV.Key]);
                                }
                            }
                        }


                    }



                    if (valid)
                    {
                        DatabaseFunct.currentData = cd;

                        Program.mainForm.tabControl1.TabPages.Clear();

                        //load tables
                        string[] mainTableKeys = DatabaseFunct.GetMainTableKeys();
                        foreach (string mainTableKey in mainTableKeys)
                        {
                            Program.mainForm.tabControl1.TabPages.Add(mainTableKey, mainTableKey);
                            //change color of tab (doesn't work)
                            Program.mainForm.tabControl1.TabPages[Program.mainForm.tabControl1.TabPages.IndexOfKey(mainTableKey)].BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
                            Program.mainForm.tabControl1.TabPages[Program.mainForm.tabControl1.TabPages.IndexOfKey(mainTableKey)].ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];

                            


                        }




                        //change table

                        DatabaseFunct.ChangeMainTable(Program.mainForm.tabControl1.TabPages[0].Name);

                        Program.mainForm.label1.Visible = false;

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("ReadError");
                    }










                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("no valid file selected!");
                }


            }
            DatabaseFunct.loadingTable = false;

        }

    }
}
