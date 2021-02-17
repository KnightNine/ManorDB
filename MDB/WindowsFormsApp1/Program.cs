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

namespace MDB
{


    static partial class Program
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

            //set initial control colors
            ColorThemes.ChangeTheme(ColorThemes.currentTheme);

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


       


        public static DataGridView GetGridView()//(int tableDepth)
        {


            DataGridView TableMainGridView = new DataGridView();

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
            TableMainGridView.ColumnHeadersDefaultCellStyle = ColorThemes.Themes[currentTheme]["ColumnHeader"];

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
            TableMainGridView.CellMouseEnter += new DataGridViewCellEventHandler(mainForm.TableMainGridView_Focus);
            TableMainGridView.GotFocus += new EventHandler(mainForm.TableMainGridView_Got_Focus);
            TableMainGridView.Click += new EventHandler(mainForm.TableMainGridView_Click);
            TableMainGridView.CellEnter += new DataGridViewCellEventHandler(mainForm.TableMainGridView_Focus);

            DoubleBuffered(TableMainGridView, true);


            return TableMainGridView;

        }

        public static Button[] GetSubTableButtons() 
        {
            Button newColumnButton = new Button();
            // 
            // newColumnButton
            // 
            newColumnButton.Location = new System.Drawing.Point(0, 0);
            newColumnButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
            newColumnButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];
            newColumnButton.Name = "newColumnButton";
            newColumnButton.Text = "New Column";
            newColumnButton.Size = new System.Drawing.Size(100, 20);
            newColumnButton.Click += new System.EventHandler(mainForm.subTableNewColumnButton_Click);


            Button newRowButton = new Button();
            // 
            // newRowButton
            //

            newRowButton.Location = new System.Drawing.Point(0, 20);
            newRowButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
            newRowButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];
            newRowButton.Name = "newRowButton";
            newRowButton.Text = "New Row";
            newRowButton.Size = new System.Drawing.Size(100, 20);
            newRowButton.Click += new System.EventHandler(mainForm.subTableNewRowButton_Click);


            

            


            return new Button[2] { newColumnButton, newRowButton };
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
