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
using System.Collections.Specialized;
using System.Drawing;

namespace MDB
{


    static class Program
    {
        public static Form1 mainForm;
        //<<parentDGV,row>,<column,childDGV>> (only one subtable can be open at a time per row)
        public static Dictionary<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTables = new Dictionary<Tuple<DataGridView, int>, Tuple<string, DataGridView>>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new Form1();
            //add post-init control
            mainForm.TableMainGridView = GetGridView();


            
            

            mainForm.Resize += new EventHandler(mainForm.MainForm_Resize);

            ((System.ComponentModel.ISupportInitialize)(mainForm.TableMainGridView)).BeginInit();
            mainForm.panel1.Controls.Add(mainForm.TableMainGridView);
           // mainForm.hideUnhideColumnsToolStripMenuItem.DropDown.AutoClose = false;
            /*mainForm.hideUnhideColumnsToolStripMenuItem.DropDown.LostFocus += new EventHandler(mainForm.hideUnhideColumnsToolStripMenuItemLostFocus);*/
            ((System.ComponentModel.ISupportInitialize)(mainForm.TableMainGridView)).EndInit();

            

            mainForm.SuspendLayout();
            mainForm.Refresh();
            mainForm.ResumeLayout(true);

            Application.Run(mainForm);
            


        }

        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }


       


        public static DataGridView GetGridView()
        {


            DataGridView TableMainGridView = new DataGridView();

            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            TableMainGridView.AllowUserToAddRows = false;
            TableMainGridView.AllowUserToDeleteRows = false;
            TableMainGridView.AllowUserToResizeColumns = false;
            TableMainGridView.AllowUserToResizeRows = false;
            
            TableMainGridView.BackgroundColor = System.Drawing.Color.LightGray;
            TableMainGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;


            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            TableMainGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;

            TableMainGridView.ColumnHeadersHeight = 58;
            TableMainGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.LightGray;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            TableMainGridView.DefaultCellStyle = dataGridViewCellStyle3;
            TableMainGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;

            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.DarkGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightCyan;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            TableMainGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            TableMainGridView.Dock = System.Windows.Forms.DockStyle.None;
            TableMainGridView.Location = new System.Drawing.Point(0, 0);
            TableMainGridView.MultiSelect = false;
            TableMainGridView.Name = "TableMainGridView";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            TableMainGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;

            TableMainGridView.RowHeadersWidth = 102;
            TableMainGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            

            TableMainGridView.RowTemplate.Height = 40;
            TableMainGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            TableMainGridView.Size = new System.Drawing.Size(2517, 942);
            TableMainGridView.TabIndex = 2;
            TableMainGridView.Visible = true;
            TableMainGridView.ScrollBars = ScrollBars.None;
            

            TableMainGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellClick);
            TableMainGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(mainForm.TableMainGridView_CellMouseDown);
            TableMainGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellContentClick);
            TableMainGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellValueChanged);
            TableMainGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(mainForm.TableMainGridView_DataError);
            TableMainGridView.RowsRemoved += new DataGridViewRowsRemovedEventHandler(mainForm.RowsRemoved);
            TableMainGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(mainForm.RowsAdded);
            TableMainGridView.CellPainting += new DataGridViewCellPaintingEventHandler(mainForm.TableMainGridView_CellPainting);

            DoubleBuffered(TableMainGridView, true);

            return TableMainGridView;

        }

        public static MenuStrip GetSubMenuStrip() 
        {
            MenuStrip newMenuStrip = new MenuStrip();
            ToolStripMenuItem newColumnToolStripMenuItem = new ToolStripMenuItem();
            ToolStripMenuItem newRowToolStripMenuItem = new ToolStripMenuItem();
            // 
            // menuStrip1
            // 
            newMenuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            newMenuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            newMenuStrip.Items.AddRange(new ToolStripItem[] {
            newColumnToolStripMenuItem,
            newRowToolStripMenuItem
            });
            newMenuStrip.Location = new System.Drawing.Point(0, 0);
            newMenuStrip.Name = "menuStrip";
            newMenuStrip.Size = new System.Drawing.Size(2517, 49);
            newMenuStrip.TabIndex = 0;
            newMenuStrip.Text = "menuStrip";

            //items
            newColumnToolStripMenuItem.Name = "newColumnToolStripMenuItem";
            newColumnToolStripMenuItem.Size = new System.Drawing.Size(214, 45);
            newColumnToolStripMenuItem.Text = "New Column";
            newColumnToolStripMenuItem.Click += new System.EventHandler(mainForm.subTableNewColumnToolStripMenuItem_Click);
            // 
            // newRowToolStripMenuItem
            // 
            newRowToolStripMenuItem.Name = "newRowToolStripMenuItem";
            newRowToolStripMenuItem.Size = new System.Drawing.Size(164, 45);
            newRowToolStripMenuItem.Text = "New Row";
            newRowToolStripMenuItem.Click += new System.EventHandler(mainForm.subTableNewRowToolStripMenuItem_Click);

            newMenuStrip.Dock = DockStyle.None;


            return newMenuStrip;
        }

    }
    /*class DictConverter : CustomCreationConverter<IDictionary<string, object>>
    {
        public override IDictionary<string, object> Create(Type objectType)
        {
            return new Dictionary<string, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            // in addition to handling IDictionary<string, object>
            // we want to handle the deserialization of dict value
            // which is of type object
            return objectType == typeof(object) || base.CanConvert(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject
                || reader.TokenType == JsonToken.Null)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            // if the next token is not an object
            // then fall back on standard deserializer (strings, numbers etc.)
            return serializer.Deserialize(reader);
        }
    }*/
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
        internal static void ImportMDBFile( bool isReplace)
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
                        Console.WriteLine("current level being converted: "+tableLevel);

                        if (currentTable is SortedDictionary<string, dynamic> || currentTable is Dictionary<string, dynamic>)
                        {
                            

                            ConvertStringKeyDict(currentTable);
                        }
                        else if(currentTable is Dictionary<int, Dictionary<string, dynamic>>)
                        {
                            int i = 0;
                            foreach(Dictionary<string, dynamic> value in currentTable.Values)
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
                                else if (ct[key] is Newtonsoft.Json.Linq.JObject )
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
                                if (!(KV.Value is ValueType) && KV.Value !=null)
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

    public static class DatabaseFunct
    {
        //used as a entry in the column that determines it's type
        public static string RowEntryRefrence = "@RowEntries";
        public static string ColumnOrderRefrence = "@ColumnOrder";
        public static string ColumnDisablerArrayExt = ".ColumnsToDisable";
        public static string RefrenceColumnKeyExt = ".Refrence";
        public static string selectedTable = "";

        public static SortedDictionary<string, dynamic> emptyData = new SortedDictionary<string, dynamic>()
        {

        };

        public static SortedDictionary<string, dynamic> currentData = new SortedDictionary<string, dynamic>()
        {

        };

        internal static void AddTable(string tabName)
        {
            string error = ColumnTypes.ValidateInput(tabName);
            
            if (error == "")
            {
                if (!currentData.Keys.Contains(tabName))
                {
                    //add dictionary with empty rowEntry database
                    currentData.Add(tabName, new Dictionary<string, dynamic>() { { RowEntryRefrence, new Dictionary<int, Dictionary<string, dynamic>>() }, { ColumnOrderRefrence, new List<string>() } });

                    Program.mainForm.tabControl1.TabPages.Add(tabName, tabName);
                    Program.mainForm.label1.Visible = false;

                    Program.mainForm.tabControl1.SelectedTab = Program.mainForm.tabControl1.TabPages[Program.mainForm.tabControl1.TabPages.IndexOfKey(tabName)];
                    ChangeMainTable(tabName);


                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("that table already exists!");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("table name entered " + error );
            }
        }

        internal static bool CreateSubTable(string parentTabName, string colName)
        {
            string subtableName = parentTabName + "/" + colName;
            if (!currentData.Keys.Contains(subtableName))
            {
                currentData.Add(subtableName, new Dictionary<string, dynamic>( ) { { ColumnOrderRefrence, new List<string>() } });
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("that subtable already exists! this shouldn't happen.");
                return false;
            }
            return true;
        }

        internal static void RemoveTable(string tabName)
        {
            
            if (currentData.Keys.Contains(tabName))
            {
                if (selectedTable == tabName)
                {

                    //go to a different tab
                    if (Program.mainForm.tabControl1.TabPages.Count > 1)
                    {
                        //do nothing
                        /*if (Program.mainForm.tabControl1.SelectedIndex > 0)
                        {
                            Program.mainForm.tabControl1.SelectedTab = Program.mainForm.tabControl1.TabPages[Program.mainForm.tabControl1.TabPages.IndexOfKey(tabName) - 1];
                        }
                        else
                        {
                            Program.mainForm.tabControl1.SelectedTab = Program.mainForm.tabControl1.SelectedTab = Program.mainForm.tabControl1.TabPages[Program.mainForm.tabControl1.TabPages.IndexOfKey(tabName) + 1];
                        }*/ 

                    }
                    else
                    {
                        loadingTable = true;
                        ClearMainTable();
                        Program.mainForm.panel1.Visible = false;
                        Program.mainForm.label1.Visible = true;
                    }
                }
                Program.mainForm.tabControl1.TabPages.RemoveAt(Program.mainForm.tabControl1.TabPages.IndexOfKey(tabName));


                string[] allKeys = currentData.Keys.ToArray<string>();
                //remove table and subtables within table from data
                foreach (string tabKey in allKeys)
                {
                    if (tabKey.StartsWith(tabName+"/") || tabKey == tabName)
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

        //not implemented yet
        internal static void ChangeTableName(string tabName, string newTabName)
        {
            if (currentData.Keys.Contains(tabName))
            {
                string error = ColumnTypes.ValidateInput(newTabName);


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
                                if (subKey.EndsWith(RefrenceColumnKeyExt) && currentData[key][subKey] == tabName)
                                {
                                    currentData[key][subKey] = newTabName;
                                }
                            }

                            if (key.StartsWith(tabName + "/") || key == tabName)
                            {
                                string newKey = newTabName + key.Remove(0, tabName.Length);
                                var data = currentData[key];
                                keysToRemove.Add(key);
                                currentData[newKey] = data;

                            }


                        }
                        foreach (string key in keysToRemove)
                        {
                            currentData.Remove(key);
                        }

                        if (Program.mainForm.tabControl1.TabPages.ContainsKey(tabName))
                        {
                            Program.mainForm.tabControl1.TabPages[tabName].Name = newTabName;
                            Program.mainForm.tabControl1.TabPages[tabName].Text = newTabName;
                        }

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("that table name already exists");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("table entered "+error);
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
                //remove subtables from list
                if (tab.Contains("/"))
                {
                    MainTables.Remove(tab);
                }
            }
            return MainTables.ToArray();
        }

        internal static void AddColumn(string colName, string colType, bool isLoad, DataGridView DGV)
        {




            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            string error = ColumnTypes.ValidateInput(colName);


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
                                tableRefrence = input[1];
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
                                            //set cell to enabled
                                            if (openDGVOfTable != null)
                                            {
                                                openDGVOfTable.Rows[entryData.Key].Cells[colName].Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                                            }



                                        }
                                        else if (currentData[tableKey][colName] == "SubTable")
                                        {
                                            defaultVal = new Dictionary<int, Dictionary<string, dynamic>>();
                                            //set cell to enabled
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

                                //add to other open subtables if any

                                foreach (DataGridView openDGV in openDGVs)
                                {

                                    if (openDGV!= DGV)
                                    {
                                        DataGridViewColumn adjDataCol = (DataGridViewColumn)Activator.CreateInstance(ColumnTypes.Types[colType]);

                                        adjDataCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                        adjDataCol.HeaderText = colName;
                                        adjDataCol.Name = colName;
                                        adjDataCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                                        openDGV.Columns.Add(adjDataCol);
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
                System.Windows.Forms.MessageBox.Show("column name entered "+ error);
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
                    string[] allKeys = ColumnTypes.Clone(currentData.Keys.ToArray<string>());
                    foreach (string tableEntry in allKeys)
                    {
                        if (tableEntry.StartsWith(subTableKey+"/") || tableEntry == subTableKey)
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
                
                for (int colI = newIndex+1; colI<orderList.Count; colI++)
                {
                    Console.WriteLine(orderList[colI]);
                    DGV.Columns[orderList[colI]].DisplayIndex = colI;
                }

            }

            


        }


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
                foreach (KeyValuePair < Tuple<DataGridView, int>, Tuple<string, DataGridView> > openSubTable in Program.openSubTables)
                {
                    if (openSubTable.Value.Item2 == DGV)
                    {
                        RowIndex = openSubTable.Key.Item2;
                        ColName = openSubTable.Value.Item1;
                    }
                }

                KeyValuePair<int, Dictionary<string, dynamic>> KVRow = new KeyValuePair<int, Dictionary<string, dynamic>>(RowIndex, GetTableDataFromDir(ParentDGV.Name)[RowIndex] );




                UpdateStatusOfAllRowCellsInDisablerArrayOfCell(ParentDGV, ConvertDirToTableKey(ParentDGV.Name), KVRow, ColName);
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
            while (index < tableData.Count() )
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

            if ( newIndex >= 0 && newIndex <= DGV.Rows.Count - 1)
            {
                Dictionary<string,dynamic> storedRowData = ColumnTypes.Clone(tableData[newIndex]);

                //swap row data
                tableData[newIndex] = tableData[rowIndex];
                

                tableData[rowIndex] = storedRowData;

                SwapSubtableNames(DGV, rowIndex,newIndex );

                //swap rows
                
                DataGridViewRow selectedRow = DGV.Rows[rowIndex];
                DGV.Rows.Remove(selectedRow);
                DGV.Rows.Insert(newIndex, selectedRow);
                DGV.Rows[rowIndex].HeaderCell.Value = String.Format("{0}",rowIndex);
                DGV.Rows[newIndex].HeaderCell.Value = String.Format("{0}", newIndex);

                //minimize row divider heights as they will be set later
                DGV.Rows[rowIndex].Height = DGV.RowTemplate.Height;
                DGV.Rows[rowIndex].DividerHeight = 0;
                DGV.Rows[newIndex].Height = DGV.RowTemplate.Height;
                DGV.Rows[newIndex].DividerHeight = 0;


            }
            

            
        }


        //this exists to remove the need to reload all subtables when a row is shifted
        //it shifts the indexes within the names of subtable DGVs
        internal static void SwapSubtableNames(DataGridView DGV, int index1, int index2)
        {
            


            

            List<DataGridView> storedDGVs1 = GetSubtablesOfRow(index1);
            List<DataGridView> storedDGVs2 = GetSubtablesOfRow(index2);

            SwapOpenSubTableKeys(new Tuple<DataGridView, int>(DGV, index1), new Tuple<DataGridView, int>(DGV, index2));

            ReplaceNamesOfDGVList(storedDGVs1, index1, index2);
            ReplaceNamesOfDGVList(storedDGVs2, index2, index1);



            //store subtables of oldRowIndex
            List < DataGridView > GetSubtablesOfRow(int index)
            {
                List<DataGridView> storedDGVs = new List<DataGridView>();
                Tuple<DataGridView, int> openSubTableKey = new Tuple<DataGridView, int>(DGV, index);
                if (Program.openSubTables.ContainsKey(openSubTableKey))
                {
                    DataGridView subTable = Program.openSubTables[openSubTableKey].Item2;
                    storedDGVs.Add(subTable);
                    subFunct(subTable);
                    //collect tables within subtable recursively
                    void subFunct(DataGridView subDGV)
                    {
                        foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                        {
                            if (openSubTable.Key.Item1 == subDGV)
                            {

                                storedDGVs.Add(openSubTable.Value.Item2);
                            }


                        }
                    }

                }


                return storedDGVs;
            }

            void SwapOpenSubTableKeys(Tuple<DataGridView, int> key1, Tuple<DataGridView, int> key2)
            {
                //replace key of first subtable with the second and vice versa
                Console.WriteLine("table Keys are being swapped: " + key1.ToString() + " is swapping with " + key2.ToString());
                Tuple<string, DataGridView> storedOpenSubTableVal = new Tuple<string, DataGridView>(null,null);
                if (Program.openSubTables.ContainsKey(key1))
                {
                    storedOpenSubTableVal = Program.openSubTables[key1];
                    Console.WriteLine(storedOpenSubTableVal.ToString() + " is taken from " + key1.ToString() );
                    Program.openSubTables.Remove(key1);
                }

                if (Program.openSubTables.ContainsKey(key2))
                {

                    Program.openSubTables[key1] = Program.openSubTables[key2];
                }

                if (storedOpenSubTableVal.Item1 != null)
                {
                    Program.openSubTables[key2] = storedOpenSubTableVal;
                    Console.WriteLine(" and placed into "+ key2.ToString() + " as " + storedOpenSubTableVal.ToString());
                }
                


            }

            void ReplaceNamesOfDGVList(List<DataGridView> gridViews,int oldIndex, int newIndex)
            {
                //change to new name
                string oldHalfName = DGV.Name + "/" + oldIndex;
                Console.WriteLine("oldHalfName:" + oldHalfName);
                //will replace the old half name
                string newHalfName = DGV.Name + "/" + newIndex;
                Console.WriteLine("newHalfName:" + newHalfName);
                Console.WriteLine("");
                foreach (DataGridView subDGV in gridViews)
                {
                    Console.WriteLine("From: " + subDGV.Name);
                    subDGV.Name = newHalfName + subDGV.Name.Remove(0, oldHalfName.Length);
                    Console.WriteLine("To: " + subDGV.Name);
                }
                Console.WriteLine("---");
            }

            
            





        }



        internal static void LoadRow(KeyValuePair<int, Dictionary<string, dynamic>> entryData, DataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            var columns = DGV.Columns;


            var index = entryData.Key;


            if (DGV.Rows.Count -1 < index)
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
                    Console.WriteLine("loaded row value: " + value);



                    //if it's a comboboxcolumn add the item to the cell
                    if (DGV.Columns[column.Name] is DataGridViewComboBoxColumn)
                    {
                        if (value != null)
                        {
                            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell() { Items = { value } };
                            DGV.Rows[index].Cells[column.Name] = cell;
                        }



                    }
                    else if (DGV.Columns[column.Name] is DataGridViewCheckBoxColumn)
                    {
                        DataGridViewCheckBoxCell cell = ColumnTypes.setBoolCellTFVals(new DataGridViewCheckBoxCell());

                        cell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };

                        //set checkbox state
                        var cellVal = cell.TrueValue;
                        if (value)
                        {
                            cellVal = cell.TrueValue;
                        }

                        cell.Value = cellVal;

                        DGV.Rows[index].Cells[column.Name] = cell;
                    }
                    else if (DGV.Columns[column.Name] is DataGridViewButtonColumn)
                    {
                        DataGridViewButtonCell bcell = (DataGridViewButtonCell)DGV.Rows[index].Cells[column.Name];
                        bcell.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };
                    }

                    //assign data to column entry
                    DGV.Rows[index].Cells[column.Name].Value = value;

                    //check if cell is enabled
                    bool isDisabled = IsDataRowCellStillDisabled(tableKey, new KeyValuePair<int, Dictionary<string, dynamic>>(index,tableData[index]), column.Name);
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


        internal static void ChangeMainTable(string tabName)
        {


            if (currentData.Keys.ToArray<string>().Contains(tabName))
            {
                Program.mainForm.panel1.Visible = true;
                ClearMainTable();
                selectedTable = tabName;
                Program.mainForm.TableMainGridView.Name = tabName;
                LoadTable(Program.mainForm.TableMainGridView);
                Program.mainForm.RecenterSubTables();

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Table does not exist");
            }

        }
        internal static void ClearMainTable()
        {
            Program.mainForm.TableMainGridView.Rows.Clear();
            Program.mainForm.TableMainGridView.Columns.Clear();
            Program.mainForm.TableMainGridView.Controls.Clear();
            Program.openSubTables.Clear();
        }


        internal static Tuple<DataGridView, int> FindSubtableParentAndRowFromDGV(DataGridView childSubTable)
        {
            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> subTable in Program.openSubTables)
            {

                //remove subtables that derive from subtable
                if (subTable.Value.Item2 == childSubTable)
                {
                    //return parent DGV and row index
                    return subTable.Key;
                }


            }
            System.Windows.Forms.MessageBox.Show("subtable child not found!");
            return null;
        }

        //removes subtable and children from parent table & open table array
        //<parent table, row index>
        //this does not remove the subtable from the DataGridView, that is done before calling this (for some reason, i can't say why exactly, there must be a case in which this is called seperately from the removal from the dgv object, i'm writing this 5 months after i built this )
        internal static void RemoveSubtableFromOpenSubtables(Tuple<DataGridView, int> subTableKey)
        {



            List < KeyValuePair <Tuple<DataGridView, int>, Tuple<string, DataGridView>>> removalList = new List<KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>>>();

            Console.WriteLine("trying to remove subtable from " + subTableKey.Item1.Name +" at row " + subTableKey.Item2.ToString());


            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> subTable in Program.openSubTables)
            {
                Console.WriteLine(subTable.Key.Item1.Name);
                //remove subtables that derive from subtable from opensubtable array
                if (subTable.Key.Item1.Name.StartsWith(Program.openSubTables[subTableKey].Item2.Name + "/") || subTable.Key.Item1.Name == Program.openSubTables[subTableKey].Item2.Name)
                {
                    removalList.Add(subTable);
                    

                }
            }
            foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> subTable in removalList)
            {
                Program.openSubTables.Remove(subTable.Key);
                Console.WriteLine(subTable.Key.ToString() + " removed from open subtables.");
            }

            //remove control
            subTableKey.Item1.Controls.Remove(Program.openSubTables[subTableKey].Item2);
            //remove from open subtables
            Program.openSubTables.Remove(subTableKey);
        }

        //the loading of a table's column and rows for the main table and subtables
        public static bool loadingTable = false;
        internal static void LoadTable(DataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);

            loadingTable = true;

            //add columns of table in order of ColumnOrderRefrence
            foreach (string K in currentData[tableKey][ColumnOrderRefrence] )
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

                LoadRow(entry, DGV);
            }
            Console.WriteLine("---");



            loadingTable = false;
        }

        //convert a datagridview name to a table key from currentdata where table data regarding columns is stored
        internal static string ConvertDirToTableKey(string dir)
        {
            //removes row indexes from dir (i.e. "abc/9,uanme/0,lol" > "abc/uanme/lol")
            string regex = "(\\/[^\\,]*\\,)";
            string output = Regex.Replace(dir, regex, "/");
            return output;

        }


        internal static Dictionary<int, Dictionary<string, dynamic>> GetTableDataFromDir(string dir)
        {
            List<string> tupleKeys = dir.Split('/').ToList<string>();
            //ignore main table name:
            tupleKeys.RemoveAt(0);

            //create array of rows and columns
            List<Tuple<int, string>> keys = new List<Tuple<int, string>>();
            foreach (string tupleKey in tupleKeys)
            {
                string[] strings = tupleKey.Split(',');
                keys.Add(new Tuple<int, string>(Convert.ToInt32(strings[0]), strings[1]));
            }

            var currentDir = currentData[selectedTable][RowEntryRefrence];
            Console.WriteLine("Get tableData. \n From main table rows:");
            foreach (Tuple<int, string> key in keys)
            {
                Console.WriteLine("Row: " + key.Item1);
                //get row
                currentDir = currentDir[key.Item1];
                Console.WriteLine("Column: " + key.Item2);
                //get column
                currentDir = currentDir[key.Item2];
            }

            return currentDir;

        }


        //======================================================================================================================
        // Disable and Enable Cells in a DataGridView



        internal static void DisableCellAtColAndRow(DataGridView DGV, string ColKey, int RowIndex)
        {


            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);
            
            DataGridViewCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex];

            
            cell.Value = null;

            //ignore if already disabled
            if (!cell.ReadOnly)
            {

                cell.ReadOnly = true;

                //make button not pressable
                if (cell.GetType() == typeof(DataGridViewButtonCell))
                {

                    DataGridViewButtonCell bcell = (DataGridViewButtonCell)cell;
                    ((Dictionary<string, dynamic>)bcell.Tag)["Enabled"] = false;
                    bcell.FlatStyle = FlatStyle.Popup;
                }
                if (cell.GetType() == typeof(DataGridViewComboBoxCell))
                {
                    DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Popup;
                }
                if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Popup;
                }

                //gray out the cell
                cell.Style.BackColor = Color.Black;
                cell.Style.ForeColor = Color.Black;
                cell.Style.SelectionBackColor = Color.Crimson;
            }

        }

        internal static void EnableCellAtColAndRow(DataGridView DGV, string ColKey, int RowIndex)
        {
            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);
            
            DataGridViewCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex];
            //ignore if already enabled
            if (cell.ReadOnly)
            {
                cell.ReadOnly = false;

                //make button pressable
                if (cell.GetType() == typeof(DataGridViewButtonCell))
                {

                    DataGridViewButtonCell bcell = (DataGridViewButtonCell)cell;
                    ((Dictionary<string, dynamic>)bcell.Tag)["Enabled"] = true;

                    bcell.FlatStyle = FlatStyle.Standard;
                }
                if (cell.GetType() == typeof(DataGridViewComboBoxCell))
                {
                    DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Standard;
                }
                if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Standard;
                }


                //restore cell style to the default value
                //default style
                if (RowIndex % 2 == 0)
                {
                    cell.Style.BackColor = DGV.DefaultCellStyle.BackColor;
                    cell.Style.ForeColor = DGV.DefaultCellStyle.ForeColor;
                    cell.Style.SelectionBackColor = DGV.DefaultCellStyle.SelectionBackColor;
                }
                else
                {
                    cell.Style.BackColor = DGV.AlternatingRowsDefaultCellStyle.BackColor;
                    cell.Style.ForeColor = DGV.AlternatingRowsDefaultCellStyle.ForeColor;
                    cell.Style.SelectionBackColor = DGV.AlternatingRowsDefaultCellStyle.SelectionBackColor;
                }
            }
            
        }

        //check if a cell of a row in tabledata contains data
        internal static bool DoesDataRowCellContainData(KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            Type valType;
            try
            {
                valType = KVRow.Value[ColumnKey].GetType();
                
            }
            catch
            {
                valType = null;
            }

            if (valType != null)
            {
                //doesn't count bool column data
                if ((KVRow.Value[ColumnKey] != null && valType != typeof(Dictionary<int, Dictionary<string, dynamic>>) && valType != typeof(bool)) || (valType == typeof(Dictionary<int, Dictionary<string, dynamic>>) && KVRow.Value[ColumnKey].Count != 0))
                {
                    return true;
                }
            }
            


            return false;
        }

        //I'd need to account for other potential disabler conditions that may still be disabling either of the two cells
        internal static bool IsDataRowCellStillDisabled(string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            if (DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
            {
                foreach (string disablerColumnKey in currentData[tableKey][ColumnKey + ColumnDisablerArrayExt])
                {

                    if (DoesDataRowCellContainData(KVRow, disablerColumnKey))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //when a change is made to a cell of a column with a disabler array, update all cells in the same row of columns within the disabler array
        internal static void UpdateStatusOfAllRowCellsInDisablerArrayOfCell(DataGridView DGV, string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            Console.WriteLine("in " + DGV.Name + " at row index " + KVRow.Key.ToString());
            Console.WriteLine("Checking Row Cells if Disabled:");

            if (DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
            {
                foreach (string ColumnKeyToUpdate in currentData[tableKey][ColumnKey + ColumnDisablerArrayExt])
                {
                    Console.WriteLine(ColumnKeyToUpdate);
                    bool isDisabled = IsDataRowCellStillDisabled(tableKey, KVRow, ColumnKeyToUpdate);
                    Console.WriteLine("     IsDisabled = " + isDisabled.ToString());
                    if (isDisabled)
                    {
                        DisableCellAtColAndRow(DGV, ColumnKeyToUpdate, KVRow.Key);
                    }
                    else
                    {
                        EnableCellAtColAndRow(DGV, ColumnKeyToUpdate, KVRow.Key);
                    }
                }
            }
        }

        //add new disabler array connection between two columns
        internal static void addColToDisablerArr(string tableKey, string selectedColKey1, string selectedColKey2)
        {
            //is being removed
            bool isRemoveDisablerCondition = false;

            void addOrRemoveFromDisablerArray(string disablerColKey, string selectedColKey)
            {
                //remove if already added
                if (!DatabaseFunct.currentData[tableKey].ContainsKey(disablerColKey +ColumnDisablerArrayExt))
                {
                    currentData[tableKey][disablerColKey + ColumnDisablerArrayExt] = new List<string> { selectedColKey };
                }
                else
                {
                    if (currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Contains(selectedColKey))
                    {
                        currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Remove(selectedColKey);
                        if (currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Count == 0)
                        {
                            currentData[tableKey].Remove(disablerColKey + ColumnDisablerArrayExt);
                        }
                        isRemoveDisablerCondition = true;
                    }
                    else
                    {
                        currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Add(selectedColKey);
                    }

                    //Console.WriteLine(DatabaseFunct.currentData[tableKey][disablerColKey + DatabaseFunct.ColumnDisablerArrayExt].ToString());


                }

            }

            addOrRemoveFromDisablerArray(selectedColKey1, selectedColKey2);
            // do the reverse where selected col key disables the disabler column key
            addOrRemoveFromDisablerArray(selectedColKey2, selectedColKey1);


            //--------------------------------------------------------------------------------------------------

            //i need to reconstruct the DGV names from the TableDataWithRow
            //parallel list with TableDataWithRow that contains DGV key names of TableData if they appear in openDGVs
            List<string> DGVKeysList = new List<string>();

            //Check along all rows at this table depth for this column for conflicting data and delete it
            List<Dictionary<int, Dictionary<string, dynamic>>> TableDataWithRow = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysList);

            //how can i tell if a cell is in an open table
            List<DataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);



            if (!isRemoveDisablerCondition)
            {
                //then disable the cells where there is conflict








                int tableIndex = 0;

                foreach (Dictionary<int, Dictionary<string, dynamic>> Table in TableDataWithRow)
                {
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



                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> KVRow in Table)
                    {
                        //if the selected cell isn't void of data then disable the other column cell and vice versa
                        void disableCol2IfCol1HasData(string Col1, string Col2)
                        {

                            if (DatabaseFunct.DoesDataRowCellContainData(KVRow, Col1))
                            {
                                Type valType;
                                try
                                {
                                    valType = KVRow.Value[Col2].GetType();
                                }
                                catch
                                {
                                    valType = null;
                                }

                                //erase value in data

                                //if subtable
                                if (valType == typeof(Dictionary<int, Dictionary<string, dynamic>>))
                                {

                                    KVRow.Value[Col2].Clear();
                                    //close the subtable if subtable open
                                    if (openDGVOfTable != null)
                                    {
                                        Tuple<DataGridView, int> openSubTableKey = new Tuple<DataGridView, int>(openDGVOfTable, KVRow.Key);
                                        //open subtable exists
                                        if (Program.openSubTables.ContainsKey(openSubTableKey))
                                        {
                                            //and that open subtable is of selectedColKey
                                            if (Program.openSubTables[openSubTableKey].Item2.Name.EndsWith("," + Col2))
                                            {
                                                openSubTableKey.Item1.Controls.Remove(Program.openSubTables[openSubTableKey].Item2);

                                                //close subtable
                                                RemoveSubtableFromOpenSubtables(openSubTableKey);
                                                //close row
                                                openSubTableKey.Item1.Rows[openSubTableKey.Item2].Height = openSubTableKey.Item1.RowTemplate.Height;
                                                openSubTableKey.Item1.Rows[openSubTableKey.Item2].DividerHeight = 0;
                                            }
                                        }
                                    }
                                }
                                //if bool
                                if (valType == typeof(bool))
                                {
                                    KVRow.Value[Col2] = false;
                                }
                                //if not subtable
                                else
                                {
                                    KVRow.Value[Col2] = null;
                                }

                                //disable if dgv table is open

                                if (openDGVOfTable != null)
                                {
                                    DisableCellAtColAndRow(openDGVOfTable, Col2, KVRow.Key);
                                }






                            }
                        }



                        disableCol2IfCol1HasData(selectedColKey1, selectedColKey2);
                        disableCol2IfCol1HasData(selectedColKey2, selectedColKey1);

                    }
                    tableIndex += 1;
                }
            }
            else //check what cells need to be re-enabled upon this condition being lifted
            {


                int tableIndex = 0;

                foreach (Dictionary<int, Dictionary<string, dynamic>> Table in TableDataWithRow)
                {
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

                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> KVRow in Table)
                    {

                        if (openDGVOfTable != null)
                        {
                            bool isDisabled1 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey1);
                            bool isDisabled2 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey2);
                            if (!isDisabled1)
                            {
                                EnableCellAtColAndRow(openDGVOfTable, selectedColKey1, KVRow.Key);
                            }
                            if (!isDisabled2)
                            {
                                EnableCellAtColAndRow(openDGVOfTable, selectedColKey2, KVRow.Key);
                            }
                        }

                    }

                    tableIndex += 1;
                }
            }






        }


        //======================================================================================================================
        internal static void SetDataAtDir(string tableDir, dynamic value, int rowIndex, string columnIndex)
        {
            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            tableData[rowIndex][columnIndex] = value;
        }

        //get all open Datagridviews of table key
        internal static List<DataGridView> GetAllOpenDGVsAtTableLevel(string tableKey)
        {
            List<DataGridView> OpenDGVs = new List<DataGridView>() { };

            if (tableKey == Program.mainForm.TableMainGridView.Name)
            {
                OpenDGVs.Add(Program.mainForm.TableMainGridView);
            }
            else
            {
                foreach (KeyValuePair<Tuple<DataGridView, int>, Tuple<string, DataGridView>> openSubTable in Program.openSubTables)
                {
                    string openTableKey = ConvertDirToTableKey(openSubTable.Value.Item2.Name);
                    if (tableKey == openTableKey)
                    {
                        OpenDGVs.Add(openSubTable.Value.Item2);

                    }


                }
            }

            
            return OpenDGVs;

        }


        //get all data from certain level of table.
        //DGV keys get added into DGVKeysPool if you need to collect them
        internal static List<Dictionary<int, Dictionary<string, dynamic>>> GetAllTableDataAtTableLevel(string tableKey, ref List<string>  DGVKeysPool)
        {
            List<string> levelKeys = tableKey.Split('/').ToList<string>();




            //List<string> collectedDirectories = new List<string>();

            dynamic currentDir = currentData[levelKeys[0]][RowEntryRefrence];

            //create the start of DGVKey before removing first level key
            string DGVKeyStart = levelKeys[0];
            levelKeys.RemoveAt(0);
            List<Dictionary<int, Dictionary<string, dynamic>>> collectedTables = new List<Dictionary<int, Dictionary<string, dynamic>>>();

            //keys/names of the tables if they were to appear in open subtables
            List<string> DGVKeysPool2 = new List<string>();

            subFunct(currentDir, levelKeys, collectedTables, DGVKeyStart);


            //recursive subfunction that calls itself through subtable columns until lvlkeys is empty, wherein the table data is collected
            
            void subFunct(dynamic CD, List<string>  lvlKeys ,List<Dictionary<int, Dictionary<string, dynamic>>> tableList, string DGVKey)
            {
                Dictionary<int, Dictionary<string, dynamic>> tableData = CD as Dictionary<int, Dictionary<string, dynamic>>;
                
                if (lvlKeys.Count > 0)
                {
                    List<string> nextLvlKeys = ColumnTypes.Clone(lvlKeys);
                    nextLvlKeys.RemoveAt(0);


                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
                    {
                        string RowDGVKey = DGVKey + "/" + entry.Key.ToString() + "," + lvlKeys[0] ;
                        
                        try
                        {
                            var subtableCD = entry.Value[lvlKeys[0]];
                            subFunct(subtableCD, nextLvlKeys, collectedTables, RowDGVKey);
                        }
                        catch
                        {
                            Console.WriteLine("subtable collecter could not find " + lvlKeys[0] + " in " + tableKey);
                        }
                        


                    }
                }
                else
                {
                    DGVKeysPool2.Add(DGVKey);
                    tableList.Add(tableData);
                }

            }

            DGVKeysPool = DGVKeysPool2;

            return collectedTables;

            
            
            

           
        }
    }
    public static class ColumnTypes
    {
        public static Dictionary<string, Type> Types = new Dictionary<string, Type>()
        {
            
            {"Primary Key", typeof(DataGridViewTextBoxColumn)},
            {"Text",typeof(DataGridViewTextBoxColumn) },
            {"Numerical",typeof( DataGridViewTextBoxColumn) },
            {"SubTable",typeof(DataGridViewButtonColumn) },
            {"Foreign Key Refrence",typeof(DataGridViewComboBoxColumn) },
            {"Bool",typeof(DataGridViewCheckBoxColumn) },

        };

        public static string ValidateInput(string str)
        {
            string error = "";
            int indexOf = str.IndexOfAny(SpecialChars);

            if (indexOf != -1)
            {
                error += "cannot contain \" !@#$%^*()/\\.) \"";
            }
            if (str == "")
            {
                error += "is blank";
            }
            

            if (error != "")
            {
                error += "!";
            }

            return error;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        static readonly char[] SpecialChars = "!@#$%^*()/\\.".ToCharArray();

        internal static DataGridViewCheckBoxCell setBoolCellTFVals(DataGridViewCheckBoxCell cell)
        {
            cell.FalseValue = false;
            cell.TrueValue = true;

            return cell;
        }

        internal static dynamic ValidateInput(DataGridViewCellEventArgs e, DataGridView DGV)
        {

            string tableDir = DGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            dynamic value = DGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string colName = DGV.Columns[e.ColumnIndex].Name;
            var colType = DatabaseFunct.currentData[tableKey][colName];
            if (colType == "Primary Key")
            {
                int index = 0;
                foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
                {
                    if (entry.Value.ContainsKey(colName) && entry.Value[colName] == value)
                    {
                        System.Windows.Forms.MessageBox.Show("Duplicate primary key \"" + value + "\" exists at row index " + index);
                        return null;
                    }
                    index++;
                }
            }
            else if (colType == "Numerical")
            {
                double a;
                if (!double.TryParse(value, out a))
                {
                    return null;
                }
                else
                {
                    value = a;
                }
            }
            /*else if (colType == "Bool")
            {
                
                if ()
                {

                    return true;
                }
                else
                {
                    return false;
                }
                
                
            }*/

            return value;
        }

    }
}
