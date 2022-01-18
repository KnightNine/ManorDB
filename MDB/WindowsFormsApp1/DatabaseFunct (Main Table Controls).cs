
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;



namespace MDB
{



   


    



    public static partial class DatabaseFunct
    {
        //TableMainGridView starts with events enabled
        private static bool clickEventsRemovedFromTableMainGridView = false;

        private static List<MainTableToolStripMenuItem> mainTableMenuItems = new List<MainTableToolStripMenuItem>();
        internal static void UpdateMainTableControls()
        {
            /*if this is in a File Regex Refrence Table
            - disable the "Add Column" and "Add Row" buttons (or remove them entirely somehow)
            - add a "Edit Regex Refrence Table Constructor Data" button that opens the RegexRefrenceTableConstructorPrompt
            - Make the Main Data Grid View read-only
            */

            //first remove all table specific controls:
            foreach (MainTableToolStripMenuItem mainTableToolStripMenuItem in mainTableMenuItems)
            {
                Program.mainForm.menuStrip1.Items.Remove(mainTableToolStripMenuItem);
            }

            //now add the controls asocciated with each type of table

            //subfunction for adding the mainTableMenuItem
            void addItem(string MainTableFunctionsKey)
            {
                MainTableToolStripMenuItem tableToolStripMenuItem = new MainTableToolStripMenuItem(MainTableFunctionsKey);
                Program.mainForm.menuStrip1.Items.Add(tableToolStripMenuItem);
                mainTableMenuItems.Add(tableToolStripMenuItem);
            }

            string tableKey = selectedTable;
            //if table is selected
            if (tableKey != "")
            {
                CustomDataGridView TableMainGridView = Program.mainForm.TableMainGridView;
                //if a File Regex Refrence Table
                if (currentData[tableKey].ContainsKey(RegexRefrenceTableConstructorDataRefrence))
                {
                    //add the "Edit Regex Refrence Table Constructor" menu strip item
                    addItem("Edit Regex Refrence Table Constructor");



                    //disable the maintable editing functionality
                    TableMainGridView.ReadOnly = true;

                    //set column headers to be the color of constructed table headers (since this table is constructed and read-only)
                    TableMainGridView.ColumnHeadersDefaultCellStyle = ColorThemes.Themes[ColorThemes.currentTheme]["ConstructedTableColumnHeader"];
                    //set column row header cells to a different color to indicate that they are read-only
                    TableMainGridView.RowHeadersDefaultCellStyle = ColorThemes.Themes[ColorThemes.currentTheme]["ConstructedTableRowHeader"];

                    //remove all click events from mainDGV
                    if (!clickEventsRemovedFromTableMainGridView)
                    {
                        TableMainGridView.CellMouseDown -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(Program.mainForm.TableMainGridView_CellMouseDown);
                        TableMainGridView.CellContentClick -= new System.Windows.Forms.DataGridViewCellEventHandler(Program.mainForm.TableMainGridView_CellContentClick);
                        clickEventsRemovedFromTableMainGridView = true;
                    }


                }
                else //if normal table
                {
                    addItem("Add Column");

                    addItem("Add Row");

                    //enable the maintable editing functionality
                    Program.mainForm.TableMainGridView.ReadOnly = false;

                    //set column and row headers to be the default color
                    TableMainGridView.ColumnHeadersDefaultCellStyle = ColorThemes.Themes[ColorThemes.currentTheme]["ColumnHeader"];
                    TableMainGridView.RowHeadersDefaultCellStyle = ColorThemes.Themes[ColorThemes.currentTheme]["RowHeader"];

                    //add all click events
                    if (clickEventsRemovedFromTableMainGridView)
                    {
                        TableMainGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(Program.mainForm.TableMainGridView_CellMouseDown);
                        TableMainGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(Program.mainForm.TableMainGridView_CellContentClick);
                        clickEventsRemovedFromTableMainGridView = false;
                    }
                }
            }






        }


    }
}