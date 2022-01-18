﻿

using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace MDB
{

    /*File Regex Refrence Table": 
    What if you wanted to refrence functions of a script? 
    this feature allows the database manager scan through that script file and update its data automatically.
    For each column in this table you'll need to define layers of regex (regular expressions) on what you want to search for
    within the script file and the table would create rows and fill in the extracted data for you.


    The columns of this table are filled in order. 
    So whether you get a match for column1 before or after you get a match in column2 doesn't matter.

    */

    /*
    EXAMPLES:
    In my project i'll be using these regular expressions:

    - this one is to match the string that comes after my static paramters description (parameters that are unique to the function)
    The description looks like this in the script file: "#static-parameters: [data]" and the matched text is "[data]".
    "(?<=#static-parameters:\s*)\[.*\]"

    - this one is to match the name of the function itself:
    A function looks like this in the script file: "func function_name():" and this will match "function_name" if the previous line doesn't contain "#-hide-":
    "(?m)(?<=(?<!^#-hide-[\r\n]+)func\s+).+?(?=\s*\()"

    - and this one is to match the Auto Table Contructor Script that will be used in setting the static Params (parameters that are function specific):
    "(?<=#auto-table-construction-script-for-static-parameters:\s*)\".*\""


    */


    public static class RegexRefrenceTableConstructorPromptHandler
    {
        // when calling to create a "File Regex Refrence Table"
        /*this class will be used to create a prompt that allows the user to define a File the regex matches will be sourced from
        and how the table will be structured.*/

        //all columns in this table must be the type of String and there must be at least one Primary Key Column

        // the columns of the input DataGridView are [Column Name, Is PrimaryKey (only one), Is AutoTableConstructorScript (only one), Regex Match]


        //RegexRefrenceTableConstructorData is structured as:

        /*
        {

        {"fileDirectory", string},
        {"columnData", {columnName { {"isPK", bool },{"isATCS", bool },{"regex", string }}}, {...}}}}
        {"columnOrder", list<string>}

        }
        */
        public static Dictionary<string,dynamic> RegexRefrenceTableConstructorData;

        public static string FileDirectoryRefrence = "@constructorDataFileDirectory";
        public static string ColumnDataRefrence = "@constructorDataColumnData";
        public static string ColumnOrderRefrence = "@constructorDataColumnOrder";


        public static Dictionary<string,dynamic> CreateNewRegexRefrenceTableConstructorData()
        {
            return new Dictionary<string, dynamic>
            {
                {FileDirectoryRefrence,""},
                {ColumnDataRefrence, new Dictionary<string,Dictionary<string,dynamic>>() },
                {ColumnOrderRefrence, new List<string>() },


            };
        }


        public static Dictionary<string, dynamic> ShowDialog(Dictionary<string, dynamic> currentRegexRefrenceTableConstructorData)
        {


            EditRegexRefrenceTableConstructorPrompt prompt = new EditRegexRefrenceTableConstructorPrompt()
            {
                
                StartPosition = FormStartPosition.CenterScreen,
                
                //BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormBack"],
                //ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["FormFore"],
            };

            //fix bugged controls that resize on their own:
            prompt.dataGridView.ColumnHeadersHeight = 100;






            //set to existing currentRegexRefrenceTableConstructorData
            if (currentRegexRefrenceTableConstructorData != null)
            {
                RegexRefrenceTableConstructorData = currentRegexRefrenceTableConstructorData;
                //set to the stored file directory:
                prompt.fileDirectoryTextBox.Text = RegexRefrenceTableConstructorData[FileDirectoryRefrence];


                //load all existing entries to the DataGridView
                //add in order of column order list
                foreach (string colKey in RegexRefrenceTableConstructorData[ColumnOrderRefrence])
                {
                    Dictionary<string, dynamic> columnData = RegexRefrenceTableConstructorData[ColumnDataRefrence][colKey];
                    

                    string colName = colKey;
                    bool isPK = columnData["isPK"];
                    bool isATCS = columnData["isATCS"];
                    string regex = columnData["regex"];
                    prompt.addRowToDataGridViewControl(colName, isPK, isATCS, regex);
                }
            }
            else
            {
                // RegexRefrenceTableConstructorData will already exist to define this table as being a File Regex Refrence Table so this is redundant
                RegexRefrenceTableConstructorData = CreateNewRegexRefrenceTableConstructorData();
            }




            //connect DialogResult.OK to constructTableButton being pressed
            prompt.Controls.Find("constructTableButton", true)[0].Click += (sender, e) => { prompt.Close(); };




            //return null if closed
            return prompt.ShowDialog() == DialogResult.OK ? RegexRefrenceTableConstructorData : null;
        }



        internal static void ConstructRegexRefrenceTable()
        {
            CustomDataGridView TableMainGridView = Program.mainForm.TableMainGridView;

            // erase all columns in the table
            while (TableMainGridView.Columns.Count > 0)
            {
                string colName = TableMainGridView.Columns[0].Name;
                DatabaseFunct.RemoveColumn(colName, TableMainGridView);
            }

            //then get the table contructor data
            Dictionary<string, dynamic> RegexRefrenceTableConstructorData = DatabaseFunct.currentData[DatabaseFunct.selectedTable][DatabaseFunct.RegexRefrenceTableConstructorDataRefrence];
            //the stored directory is the relative directory that is combined with the absolute directory
            string fileDirectory = Path.Combine(InputOutput.selectedPath, RegexRefrenceTableConstructorData[FileDirectoryRefrence]);
            


            if (String.IsNullOrWhiteSpace(RegexRefrenceTableConstructorData[FileDirectoryRefrence]))
            {
                MessageBox.Show("File directory is empty, a directory to a file must be defined first to read from.");

            }
            else if (File.Exists(fileDirectory))
            {
                string fileText = File.ReadAllText(fileDirectory);
                
            

                //add in order of column order list
                foreach (string colKey in RegexRefrenceTableConstructorData[ColumnOrderRefrence])
                {
                    Dictionary<string, dynamic> columnConstructorData = RegexRefrenceTableConstructorData[ColumnDataRefrence][colKey];


                    //get column type
                    string columnType = "Text";
                    if (columnConstructorData["isPK"])
                    {
                        columnType = "Primary Key";
                    }
                    else if (columnConstructorData["isATCS"])
                    {
                        columnType = "Auto Table Constructor Script";
                    }

                    string colName = colKey;

                    DatabaseFunct.AddColumn(colName, columnType, false, TableMainGridView);

                    // now get all the regex matches for this column
                    string regex = columnConstructorData["regex"];
                    MatchCollection matches = Regex.Matches(fileText, regex);

                    //write to the table
                    int rowIndex = 0;
                    foreach (Match match in matches)
                    {
                        string value = match.Value;

                        //add rows when matches exceeds the number of rows in the table
                        if (TableMainGridView.Rows.Count - 1 < rowIndex)
                        {
                            DatabaseFunct.AddRow(TableMainGridView, false, rowIndex);
                        }

                        //write to cell at row index
                        DataGridViewCell cell = TableMainGridView.Rows[rowIndex].Cells[TableMainGridView.Columns.IndexOf(TableMainGridView.Columns[colName])];

                        
                        


                        //this should trigger the TableMainGridView_CellValueChanged event so that it is added to currentData and any errors will be triggered
                        cell.Value = value;


                        rowIndex += 1;
                    }



                }

            }
            else
            {
                MessageBox.Show("File does not exist at file directory (" + fileDirectory + ") to construct table from.");

            }

        }







    }
}
