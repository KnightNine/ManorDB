﻿using System;
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
    public static class ColorThemes
    {


        const string defaultTheme = "Dark";

        public static string currentTheme = defaultTheme;
        
        //Dark---------

        static DataGridViewCellStyle darkdataGridViewColumnHeadersDefaultCellStyle = new DataGridViewCellStyle() {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = Color.Black,
            ForeColor = System.Drawing.Color.White,
            Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = Color.Black,
            SelectionForeColor = System.Drawing.Color.White,
            WrapMode = System.Windows.Forms.DataGridViewTriState.True,
            
        };

        static DataGridViewHeaderBorderStyle darkColumnHeadersBorder = DataGridViewHeaderBorderStyle.Raised;

        static DataGridViewCellStyle darkdataGridViewRowHeadersDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.DimGray,
            ForeColor = System.Drawing.Color.Black,
            Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.DimGray,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };

        static DataGridViewHeaderBorderStyle darkRowHeadersBorder = DataGridViewHeaderBorderStyle.Single;

        static DataGridViewCellStyle darkdataGridViewRowsDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.ColorTranslator.FromHtml("#232323"),
            ForeColor = System.Drawing.Color.White,
            Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.LightCyan,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };

        static DataGridViewCellStyle darkdataGridViewAlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.ColorTranslator.FromHtml("#383838"),
            ForeColor = System.Drawing.Color.White,
            Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.LightCyan,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };

        static Color darkSubTableCellText = Color.Aqua;
        static Color darkSubTableSelectedCellText = Color.Blue;



        static Color darkPanelBackColor = Color.Black;
        static Color darkPanelForeColor = Color.Black;
        static Color darkScrollBackColor = System.Drawing.ColorTranslator.FromHtml("#1c1c1c");
        static Color darkScrollForeColor = Color.LightGray;
        static Color darkElseBackColor = Color.Silver;
        static Color darkElseForeColor = Color.Black;
        static Color darkFormBackColor = Color.Black;
        //autocolors labels over form
        static Color darkFormForeColor = Color.White;
        static Color darkLabelColor = Color.White;
        //color of grid lines
        static Color darkGridColor = System.Drawing.ColorTranslator.FromHtml("#1c1c1c");


        static Color darkDisabledCell = Color.Maroon;
        static Color darkSelectedDisabledCell = Color.Crimson;


        static Brush darkTabBack = new System.Drawing.SolidBrush(System.Drawing.ColorTranslator.FromHtml("#282828"));
        static Brush darkTabSelectedBack = Brushes.LightGreen;
        static Color darkTabText = Color.White;
        static Color darkTabSelectedText = Color.Black;
        //tab bar color from leftmost tab
        static Brush darkTabControlBack = Brushes.Black;


        //Light---------

        static DataGridViewCellStyle lightdataGridViewColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.SystemColors.Control,
            ForeColor = System.Drawing.Color.Black,
            Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.SystemColors.Control,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.True,

        };
        static DataGridViewHeaderBorderStyle lightColumnHeadersBorder = DataGridViewHeaderBorderStyle.Single;

        static DataGridViewCellStyle lightdataGridViewRowHeadersDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.SystemColors.ControlLight,
            ForeColor = System.Drawing.Color.Black,
            Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.Gray,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };

        static DataGridViewCellStyle lightdataGridViewRowsDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.Silver,
            ForeColor = System.Drawing.Color.Black,
            Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.LightCyan,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };

        static DataGridViewHeaderBorderStyle lightRowHeadersBorder = DataGridViewHeaderBorderStyle.Single;

        static DataGridViewCellStyle lightdataGridViewAlternatingRowsDefaultCellStyle = new DataGridViewCellStyle()
        {
            Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,
            BackColor = System.Drawing.Color.LightGray,
            ForeColor = System.Drawing.Color.Black,
            Font = new System.Drawing.Font("Consolas", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            SelectionBackColor = System.Drawing.Color.LightCyan,
            SelectionForeColor = System.Drawing.Color.Black,
            WrapMode = System.Windows.Forms.DataGridViewTriState.False,

        };


        static Color lightSubTableCellText = Color.Blue;
        static Color lightSubTableSelectedCellText = Color.Blue;


        static Color lightPanelBackColor = SystemColors.AppWorkspace;
        static Color lightPanelForeColor = SystemColors.ControlText;
        static Color lightScrollBackColor = SystemColors.ControlDark;
        static Color lightScrollForeColor = Color.LightGray;
        static Color lightElseBackColor = SystemColors.ControlLight;
        static Color lightElseForeColor = SystemColors.ControlText;
        static Color lightFormBackColor = SystemColors.AppWorkspace;
        static Color lightFormForeColor = SystemColors.ControlText;
        static Color lightLabelColor = SystemColors.ControlText;
        //color of grid lines
        static Color lightGridColor = Color.Silver;


        static Color lightDisabledCell = Color.Black;
        static Color lightSelectedDisabledCell = Color.Crimson;

        static Brush lightTabBack = new System.Drawing.SolidBrush(SystemColors.Control);
        static Brush lightTabSelectedBack = Brushes.White;
        static Color lightTabText = Color.Black;
        static Color lightTabSelectedText = Color.Black;
        //tab bar color from leftmost tab
        static Brush lightTabControlBack = new System.Drawing.SolidBrush(SystemColors.ControlLight);




        public static Dictionary<string, Dictionary<string, dynamic>> Themes = new Dictionary<string, Dictionary<string, dynamic>>()
        {

            {"Dark", 
                new Dictionary<string, dynamic>() 
                {
                    { "ColumnHeader",darkdataGridViewColumnHeadersDefaultCellStyle },
                    {"ColumnHeaderBorder", darkColumnHeadersBorder },
                    { "Cell",darkdataGridViewRowsDefaultCellStyle },
                    {"AltCell",darkdataGridViewAlternatingRowsDefaultCellStyle },
                    {"RowHeader",darkdataGridViewRowHeadersDefaultCellStyle },
                    {"RowHeaderBorder", darkRowHeadersBorder },
                    {"PanelBack",darkPanelBackColor },
                    {"PanelFore",darkPanelForeColor },
                    {"ScrollBack",darkScrollBackColor },
                    {"ScrollFore",darkScrollForeColor },
                    {"ElseBack",darkElseBackColor },
                    {"ElseFore",darkElseForeColor },
                    {"FormBack",darkFormBackColor },
                    {"FormFore",darkFormForeColor },
                    {"LabelFore",darkLabelColor },
                    {"GridColor",darkGridColor },
                    {"Disabled",darkDisabledCell },
                    {"DisabledSelected",darkSelectedDisabledCell },
                    {"TabBack",darkTabBack },
                    {"TabSelectedBack",darkTabSelectedBack },
                    {"TabFore",darkTabText },
                    {"TabSelectedFore",darkTabSelectedText },
                    {"TabControlBack",darkTabControlBack },

                    {"SubTableCellFore",darkSubTableCellText },
                    {"SubTableSelectedCellFore",darkSubTableSelectedCellText },
                    

                } 
            },
            {"Light",
                new Dictionary<string, dynamic>()
                {
                    { "ColumnHeader",lightdataGridViewColumnHeadersDefaultCellStyle },
                    {"ColumnHeaderBorder", lightColumnHeadersBorder },
                    { "Cell",lightdataGridViewRowsDefaultCellStyle },
                    {"AltCell",lightdataGridViewAlternatingRowsDefaultCellStyle },
                    {"RowHeader",lightdataGridViewRowHeadersDefaultCellStyle },
                    {"RowHeaderBorder", lightRowHeadersBorder },
                    {"PanelBack",lightPanelBackColor },
                    {"PanelFore",lightPanelForeColor },
                    {"ScrollBack",lightScrollBackColor },
                    {"ScrollFore",lightScrollForeColor },
                    {"ElseBack",lightElseBackColor },
                    {"ElseFore",lightElseForeColor },
                    {"FormBack",lightFormBackColor },
                    {"FormFore",lightFormForeColor },
                    {"LabelFore",lightLabelColor },
                    {"GridColor",lightGridColor },
                    {"Disabled",lightDisabledCell },
                    {"DisabledSelected",lightSelectedDisabledCell },
                    {"TabBack",lightTabBack },
                    {"TabSelectedBack",lightTabSelectedBack },
                    {"TabFore",lightTabText },
                    {"TabSelectedFore",lightTabSelectedText },
                    {"TabControlBack",lightTabControlBack },

                    {"SubTableCellFore",lightSubTableCellText },
                    {"SubTableSelectedCellFore",lightSubTableSelectedCellText },
                }
            }

        };



        public static void ChangeTheme(string theme)
        {
            currentTheme = theme;
            Program.mainForm.vScrollBar1.ForeColor = Themes[currentTheme]["ScrollFore"];
            Program.mainForm.vScrollBar1.BackColor = Themes[currentTheme]["ScrollBack"];
            Program.mainForm.panel1.BackColor = Themes[currentTheme]["PanelBack"];
            Program.mainForm.panel1.ForeColor = Themes[currentTheme]["PanelFore"];
            Program.mainForm.panel1.BorderStyle = BorderStyle.None;
            Program.mainForm.tabControl1.BackColor = Themes[currentTheme]["ElseBack"];
            Program.mainForm.tabControl1.ForeColor = Themes[currentTheme]["ElseFore"];
            Program.mainForm.menuStrip1.BackColor = Themes[currentTheme]["ElseBack"];
            Program.mainForm.menuStrip1.ForeColor = Themes[currentTheme]["ElseFore"];
            Program.mainForm.BackColor = Themes[currentTheme]["FormBack"];
            Program.mainForm.label1.ForeColor = Themes[currentTheme]["LabelFore"];

        }

    }
}
