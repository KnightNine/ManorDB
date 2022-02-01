

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

    - this one is to match the name of the function itself to function as the table's primary key:
    A function looks like this in the script file: "func function_name():" and this will match "function_name" if the previous line doesn't contain "#-hide-" nor will it match if the line is commented out with '#' at its start:
    "(?m)(?<=(?<!#-hide-\s*[\r\n]+|#)func\s+).+?(?=\s*\()"

    - and this one is to match the Auto Table Contructor Script that will be used in setting the static Params (parameters that are function specific)
    The description looks like this in the script file: "static_param_table_constructor_script: {data}" and the matched text is "data". (if the line isn't commented out with '#')
    "(?<=(?<!#+\s*)static_param_table_constructor_script:\s*\{).*(?=\})"

    

    ------------------other regex i'll be using-----------------------

    "(?<=(?<!#+\s*)required_inputs:\s*)\[.*\]"

    "(?<=(?<!#+\s*)optional_inputs:\s*)\[.*\]"


    "(?<=(?<!#+\s*)is_selection_dependent:\s*\[).*(?=\])"
    is_selection_dependent


    "(?<=(?<!#+\s*)dynamic_requirement_description_format:\s*\").*(?=\")"
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

            string fileText = null;


            if (String.IsNullOrWhiteSpace(RegexRefrenceTableConstructorData[FileDirectoryRefrence]))
            {
                MessageBox.Show("Referenced File/Folder directory is empty, a directory to a file or folder must be defined first to read from.");

            }
            else if (Directory.Exists(fileDirectory)) // if directory read all files within directory
            {


                fileText = "";
                //add text from all files within directory
                foreach (string fileFullName in Directory.GetFiles(fileDirectory))
                {
                    fileText += File.ReadAllText(fileFullName) + "/n";
                }
            }
            else if (File.Exists(fileDirectory))
            {
                fileText = File.ReadAllText(fileDirectory);
                
            }
            else
            {
                MessageBox.Show("File/Folder does not exist at referenced directory (" + fileDirectory + ") to construct table from.");

            }

            //construct table
            if (fileText != null)
            {
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


        }


        internal static void UpdateRegexRefrenceTableData(string mainTableKey)
        {

            


            // get the table contructor data
            Dictionary<string, dynamic> RegexRefrenceTableConstructorData = DatabaseFunct.currentData[mainTableKey][DatabaseFunct.RegexRefrenceTableConstructorDataRefrence];
            //the stored directory is the relative directory that is combined with the absolute directory
            string fileDirectory = Path.Combine(InputOutput.selectedPath, RegexRefrenceTableConstructorData[FileDirectoryRefrence]);

            string fileText = null;

            

            string error_header = "Error encountered while updating row data for File Regex Reference Table \"" + mainTableKey + "\": ";

            if (String.IsNullOrWhiteSpace(RegexRefrenceTableConstructorData[FileDirectoryRefrence]))
            {
                MessageBox.Show(error_header+ "Referenced directory is empty, a directory to a file or folder isn't defined for this table to reference data from.");

            }
            else if (Directory.Exists(fileDirectory)) // if directory read all files within directory
            {


                fileText = "";
                //add text from all files within directory
                foreach (string fileFullName in Directory.GetFiles(fileDirectory))
                {
                    fileText += File.ReadAllText(fileFullName) + "/n";
                }
            }
            else if (File.Exists(fileDirectory))
            {
                fileText = File.ReadAllText(fileDirectory);

            }
            else
            {
                MessageBox.Show(error_header + "File or folder does not exist at directory (" + fileDirectory + ") to construct table from. The directory for the referenced file or folder must have changed.");

            }

            //update table row data
            if (fileText != null)
            {
                
                //wipe table row data
                Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.currentData[mainTableKey][DatabaseFunct.RowEntryRefrence];
                tableData.Clear();

                
                List<string> ColList = RegexRefrenceTableConstructorData[ColumnOrderRefrence];

                List<string> ColOrderList = DatabaseFunct.currentData[mainTableKey][DatabaseFunct.ColumnOrderRefrence];

                //wipe existing columns from the table:
                foreach(string colKey in ColOrderList)
                {
                    DatabaseFunct.currentData[mainTableKey].Remove(colKey);
                }
                ColOrderList.Clear();


                //add in order of column order list
                foreach (string colKey in ColList)
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

                    //add column if it doesn't exist yet
                    if (!DatabaseFunct.currentData[mainTableKey].ContainsKey(colKey))
                    {
                        ColOrderList.Add(colKey);
                        DatabaseFunct.currentData[mainTableKey][colKey] = columnType;
                        //none of the column types used here require metadata
                    }


                    


                    

                    //get all the regex matches for this column
                    string regex = columnConstructorData["regex"];
                    MatchCollection matches = Regex.Matches(fileText, regex);

                    //write to the table
                    int rowIndex = 0;
                    foreach (Match match in matches)
                    {
                        string value = match.Value;

                        //add rows when matches exceeds the number of rows in the table
                        if (tableData.Count - 1 < rowIndex)
                        {
                            //create new blank row data for new rows
                            Dictionary<string, dynamic> rowData = new Dictionary<string, dynamic>();
                            foreach (string _colKey in ColList)
                            {
                                rowData[_colKey] = null;
                            }


                            tableData.Add(rowIndex, rowData);
                            
                        }
                        
                        //write to cell data at row index
                        tableData[rowIndex][colKey] = value;



                        rowIndex += 1;
                    }



                }
            }


        }




    }
}

