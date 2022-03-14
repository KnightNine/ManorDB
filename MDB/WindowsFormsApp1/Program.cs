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
using System.Collections.Specialized;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Configuration;

namespace MDB
{


    static partial class Program
    {
        public static Form1 mainForm;
        //<<parentDGV,row>,<column,childDGV>> (only one subtable can be open at a time per row)
        public static Dictionary<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTables = new Dictionary<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>>();

        private static int initialScriptColumnTypeDuplicates = 1;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        


        static void Main(string[] args)
        {
            
            

            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            

            mainForm = new Form1();
            

            //set initial control colors
            ColorThemes.ChangeTheme(ColorThemes.currentTheme);

            //add post-init control
            mainForm.TableMainGridView = GetGridView(false);

            



            mainForm.Resize += new EventHandler(mainForm.MainForm_Resize);

            ((System.ComponentModel.ISupportInitialize)(mainForm.TableMainGridView)).BeginInit();
            mainForm.panel1.Controls.Add(mainForm.TableMainGridView);

            


            // mainForm.hideUnhideColumnsToolStripMenuItem.DropDown.AutoClose = false;
            /*mainForm.hideUnhideColumnsToolStripMenuItem.DropDown.LostFocus += new EventHandler(mainForm.hideUnhideColumnsToolStripMenuItemLostFocus);*/
            ((System.ComponentModel.ISupportInitialize)(mainForm.TableMainGridView)).EndInit();


            

            mainForm.SuspendLayout();
            mainForm.Refresh();
            mainForm.ResumeLayout(true);

            //get settings data
            LoadConfigurationFile();

            //load file if opened from file
            if (args.Count() > 0)
            {
                string mdbFilePath = args[0];

                if (mdbFilePath != "" && File.Exists(mdbFilePath) && mdbFilePath.EndsWith(".mdb"))
                {
                    InputOutput.ImportMDBFile(mdbFilePath, true);
                }
            }


            

            



            Application.Run(mainForm);
            




        }

        public static void DoubleBuffered(this CustomDataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static void SaveConfigurationFile()
        {
            try
            {
                //convert dictionary to json string so that it can be stored as string
                Properties.Settings.Default.scriptPrefabDict = Newtonsoft.Json.JsonConvert.SerializeObject(AutoTableConstructorScriptFunct.scriptPrefabDict);
                //set initial script column type dupes
                Properties.Settings.Default.scriptColumnTypeDuplicates = ColumnTypes.scriptColumnTypeDuplicates;

                Properties.Settings.Default.Save();
            }
            catch
            {
                MessageBox.Show("could not save config file");
            }
        }

        static void LoadConfigurationFile()
        {
            try
            {
                //convert dictionary from json string
                AutoTableConstructorScriptFunct.scriptPrefabDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(Properties.Settings.Default.scriptPrefabDict);
                //set initial script column type dupes
                ColumnTypes.SetScriptColumnTypeDuplicates(Properties.Settings.Default.scriptColumnTypeDuplicates);
            }
            catch
            {
                MessageBox.Show("Script Prefab Data not found.");
                SaveConfigurationFile();
            }
                
            
            
        }


        //isConstructed is true if the DataGridView Table is constructed from an Auto Table Constructor script
        public static CustomDataGridView GetGridView(bool isConstructed)//(int tableDepth)
        {


            CustomDataGridView TableMainGridView = new CustomDataGridView();

            TableMainGridView.AllowUserToAddRows = false;
            TableMainGridView.AllowUserToDeleteRows = false;
            TableMainGridView.AllowUserToResizeColumns = false;
            TableMainGridView.AllowUserToResizeRows = false;
            
            TableMainGridView.BackgroundColor = System.Drawing.Color.LightGray;
            TableMainGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //this needs to be false for some reason
            TableMainGridView.EnableHeadersVisualStyles = false;


            string currentTheme = ColorThemes.currentTheme;

            //header
            if (isConstructed)
            {
                //indicate that the headers are read-only on constructed tables

                TableMainGridView.ColumnHeadersDefaultCellStyle = ColorThemes.Themes[currentTheme]["ConstructedTableColumnHeader"];
            }
            else
            {
                TableMainGridView.ColumnHeadersDefaultCellStyle = ColorThemes.Themes[currentTheme]["ColumnHeader"];
            }
            

            TableMainGridView.ColumnHeadersBorderStyle = ColorThemes.Themes[currentTheme]["ColumnHeaderBorder"];
            TableMainGridView.ColumnHeadersHeight = 58;
            TableMainGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            //row header
            TableMainGridView.RowHeadersDefaultCellStyle = ColorThemes.Themes[currentTheme]["RowHeader"];

            TableMainGridView.RowHeadersWidth = 102;
            TableMainGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            TableMainGridView.RowHeadersBorderStyle = ColorThemes.Themes[currentTheme]["RowHeaderBorder"];

            //cell
            //TableMainGridView.DefaultCellStyle = ColorThemes.Themes[currentTheme]["Cell"];
            TableMainGridView.RowsDefaultCellStyle = ColorThemes.Themes[currentTheme]["Cell"];

            TableMainGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;


            //alt cell
            TableMainGridView.AlternatingRowsDefaultCellStyle = ColorThemes.Themes[currentTheme]["AltCell"];


            //grid color
            //(only works with certain borderstyles)
            TableMainGridView.GridColor = ColorThemes.Themes[currentTheme]["GridColor"];


            //n/a
            TableMainGridView.Dock = System.Windows.Forms.DockStyle.None;
            TableMainGridView.Location = new System.Drawing.Point(0, 0);
            TableMainGridView.MultiSelect = false;
            TableMainGridView.Name = "TableMainGridView";
           
            

            
            TableMainGridView.RowTemplate.Height = 40;
            TableMainGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            TableMainGridView.Size = new System.Drawing.Size(2517, 942);
            TableMainGridView.TabIndex = 2;
            TableMainGridView.Visible = true;
            TableMainGridView.ScrollBars = ScrollBars.None;


            //DGV
            TableMainGridView.BackgroundColor = ColorThemes.Themes[currentTheme]["PanelBack"];
            TableMainGridView.ForeColor = ColorThemes.Themes[currentTheme]["PanelBack"];


            //events

            TableMainGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellClick);
            TableMainGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(mainForm.TableMainGridView_CellMouseDown);
            TableMainGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellContentClick);
            TableMainGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(mainForm.TableMainGridView_CellValueChanged);
            TableMainGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(mainForm.TableMainGridView_DataError);
            TableMainGridView.RowsRemoved += new DataGridViewRowsRemovedEventHandler(mainForm.RowsRemoved);
            TableMainGridView.RowsAdded += new DataGridViewRowsAddedEventHandler(mainForm.RowsAdded);
            TableMainGridView.CellPainting += new DataGridViewCellPaintingEventHandler(mainForm.TableMainGridView_CellPainting);
            TableMainGridView.CellEndEdit += new DataGridViewCellEventHandler(mainForm.TableMainGridView_CellEndEdit);
            TableMainGridView.CellMouseEnter += new DataGridViewCellEventHandler(mainForm.TableMainGridView_CellMouseEnter);
            TableMainGridView.GotFocus += new EventHandler(mainForm.TableMainGridView_Got_Focus);
            TableMainGridView.Click += new EventHandler(mainForm.TableMainGridView_Click);
            

            DoubleBuffered(TableMainGridView, true);

            //the Tag is used to define different table types based on what is within this dictionary
            TableMainGridView.Tag = new Dictionary<string, dynamic>();


            return TableMainGridView;

        }

        public static DataGridViewColumn GetDataGridViewColumn(string colName, string colType)
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

            return myDataCol;
        }

        public static Button[] GetSubTableButtons(bool isConstructed) 
        {
            List<Button> buttons = new List<Button>();

            //you cannot add columns to constructed tables
            if (!isConstructed)
            {
                Button newColumnButton = new Button();
                // 
                // newColumnButton
                // 
                newColumnButton.Location = new System.Drawing.Point(0, 0);
                newColumnButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                newColumnButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
                newColumnButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];
                newColumnButton.Name = "newColumnButton";
                newColumnButton.Text = "New Column";
                newColumnButton.Size = new System.Drawing.Size(100, 20);
                newColumnButton.Click += new System.EventHandler(mainForm.subTableNewColumnButton_Click);

                buttons.Add(newColumnButton);
            }
            


            Button newRowButton = new Button();
            // 
            // newRowButton
            //

            newRowButton.Location = new System.Drawing.Point(0, 20);
            newRowButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            newRowButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
            newRowButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];
            newRowButton.Name = "newRowButton";
            newRowButton.Text = "New Row";
            newRowButton.Size = new System.Drawing.Size(100, 20);
            newRowButton.Click += new System.EventHandler(mainForm.subTableNewRowButton_Click);
            //the tag is used for setting the cell click event to be enabled or disabled.
            newRowButton.Tag = new Dictionary<string, dynamic>() { { "Enabled", true } };

            buttons.Add((newRowButton));






            return buttons.ToArray();
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
    


    
}
