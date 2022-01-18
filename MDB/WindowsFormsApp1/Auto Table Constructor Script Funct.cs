using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace MDB
{

    public static class AutoTableConstructorScriptFunct
    {
        /*
        how a table structure is defined is as follows:
        the script can contain a number of columns seperated by commas, this is how to define each type of column (ignore quotations):
            -Text: " <column_name>:T "
            -Numerical: " <column_name>:N "
            -Integer: " <column_name>:I "
            -Subtable: " <column_name>:S:{...internal_column_structure...}" "
            -Bool: " <column_name>:B "
            -Foreign Key Refrence: " <column_name>:F:<table_name> "

        -if you want to add a single row restriction to a constructed table or sub-table, add a hashtag "#" at the beginning of the script (there cannot be spaces before this hashtag)

        - any spaces before or after the column names and type shorthand are trimmed so there's no need to worry about spacing,
          so "  <Bool Column>  : B  , ..." will correct to "<Bool Column>:B,..."

        


        Example:
        "# <Bool Column>:B, <Subtable Column>:S:{<Sub Bool Column>:B,<Sub Text Column>:T }, <Text Column>:T, <Foreign Key Refrence Column>:F:<My Table Name>"



        */

        public static Dictionary<string, string> columnTypeShorthandDict = new Dictionary<string, string>()
        {
            {"T","Text" },
            {"N", "Numerical" },
            {"I", "Integer" },
            {"B", "Bool" },
            {"F", "Foreign Key Refrence" },
            {"S", "SubTable" },
        };
        public static Dictionary<string, Type> columnCellValueValidType = new Dictionary<string, Type>()
        {
            {"Text",typeof(string) },
            {"Numerical", typeof(double) },
            {"Integer",typeof(int) },
            {"Bool",typeof(bool) },
            {"Foreign Key Refrence",typeof(string) },
            {"SubTable", typeof(Dictionary<int, Dictionary<string, dynamic>>)}
        };
        


        public static void LoadConstructedColumn(string colName, string colType, CustomDataGridView DGV, Dictionary<int, Dictionary<string, dynamic>> tableData, Tuple<string,dynamic> additionalColumnData)
        {




            //create column object
            DataGridViewColumn myDataCol = Program.GetDataGridViewColumn(colName, colType);

            Dictionary<string, dynamic>  colTag = new Dictionary<string, dynamic>();

            //store the subtableConstructorScript in the column tag if this is a subtable column
            if (additionalColumnData != null)
            {


                colTag.Add(additionalColumnData.Item1,additionalColumnData.Item2);
                
            }


            colTag.Add("columnType", colType);



            myDataCol.Tag = colTag;

            //add to DGV
            DGV.Columns.Add(myDataCol);


            //add to tableData of this table directory's rows.
            foreach (Dictionary<string,dynamic> rowData in tableData.Values)
            {

                
                //check if existing value type is valid for column:
                bool isDataTypeValid = true;
                Type validType = columnCellValueValidType[colType];
                Type existingDataType = validType;
                //cannot get type if  rowData[colName] is null
                if (rowData[colName] != null)
                {
                    existingDataType = rowData[colName].GetType();
                }
                else// if null
                {
                    
                    //cannot be null if type is bool or subtable
                    if (colType == "Bool" || colType == "SubTable")
                    {
                        isDataTypeValid = false;
                    }

                }

                if (existingDataType != validType)
                {
                    //if existing data type can be converted , convert it instead of removing it entirely
                    if (existingDataType == typeof(int) && validType == typeof(double))
                    {
                        rowData[colName] = (double)rowData[colName];
                    }
                    else if (existingDataType == typeof(double) && validType == typeof(int))
                    {
                        rowData[colName] = (int)rowData[colName];
                    }
                    else
                    {
                        isDataTypeValid = false;
                    }
                }

                //if the column doesn't doesn't exist yet
                //or if the column type was changed and must be wiped to the default value
                if (!rowData.ContainsKey(colName) || !isDataTypeValid)
                {
                    //set to default value of new column
                    //define the default value
                    dynamic defaultVal = ColumnTypes.GetDefaultColumnValue(colType);
                    

                    rowData[colName] = defaultVal;
                }

                

            }






        }


        public static string GetTypeOfColumnInScript(string colName, string tableConstructorScript)
        {


            //collect subscripts within "subtable column" curly brackets and includes nested curly brackets
            MatchCollection subtableScripts = Regex.Matches(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})");

            //ignore these subscripts
            string scriptAtCurrentLevel = Regex.Replace(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})", "");

            bool singleRowRestriction = scriptAtCurrentLevel.StartsWith("#");


            //then split script into it's columns by commas
            string[] columnScripts = scriptAtCurrentLevel.Split(',');

            
            //load columns at current level from script
            foreach (string columnScript in columnScripts)
            {
                string[] columnDat = columnScript.Split(':');
                //get name within <>
                string columnName = Regex.Match(columnDat[0], @"(?<=\<)[^}]*(?=\>)").Value;

                if (columnName == colName)
                {
                    //get type from shorthand
                    string columnType = columnTypeShorthandDict[columnDat[1].Trim()];
                    return columnType;
                }
                
                



            }

            MessageBox.Show("column type not found for column: " + colName + " within constructed table");
            return null;

        }


            //whenever a AutoTableConstructorScriptReader button (or a subtable button within the AutoTableConstructorScriptReader subtable) is pressed, instead of referring to the currentData table structure, refer to the script that the button is refrencing
        public static void ConstructSubTableStructureFromScript(CustomDataGridView DGV)
        {
            Dictionary<string,dynamic> DGVTag = DGV.Tag as Dictionary<string,dynamic>;
            string tableConstructorScript = DGVTag["tableConstructorScript"];



            //find data source
            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(DGV.Name);




            DatabaseFunct.loadingTable = true;

            //collect subscripts within "subtable column" curly brackets and includes nested curly brackets
            MatchCollection subtableScripts = Regex.Matches(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})");
            
            //ignore these subscripts
            string scriptAtCurrentLevel = Regex.Replace(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})", "");

            bool singleRowRestriction = scriptAtCurrentLevel.StartsWith("#");
            

            //then split script into it's columns by commas
            string[] columnScripts = scriptAtCurrentLevel.Split(',');

            List<string> columnNames = new List<string>();

            int subtableScriptIndex = 0;
            //load columns at current level from script
            foreach (string columnScript in columnScripts)
            {
                string[] columnDat = columnScript.Split(':');
                //get name within <>
                string columnName = Regex.Match(columnDat[0], @"(?<=\<)[^}]*(?=\>)").Value;

                columnNames.Add(columnName);
                //get type from shorthand
                string columnType = columnTypeShorthandDict[columnDat[1].Trim()];


                //add subtableConstructorScript to tag if type is SubTable
                Tuple<string, dynamic> additionalColumnData = null;
                
                if (columnType == "SubTable")
                {
                    string subtableConstructorScript = subtableScripts[subtableScriptIndex].Value;

                    additionalColumnData = new Tuple<string, dynamic>("subtableConstructorScript", subtableConstructorScript);
                    subtableScriptIndex += 1;
                }
                else if (columnType == "Foreign Key Refrence")
                {
                    string tableBeingRefrenced = Regex.Match(columnDat[2].Trim(), @"(?<=\<)[^}]*(?=\>)").Value;
                    //get string within <>
                    additionalColumnData = new Tuple<string, dynamic>(DatabaseFunct.RefrenceColumnKeyExt, tableBeingRefrenced);
                }

                LoadConstructedColumn(columnName, columnType,DGV,tableData, additionalColumnData);
            }



            
            
            //load rows from table data
            foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
            {
                Console.WriteLine("loaded row " + entry.Key.ToString() + ": " + entry.Value);

                DatabaseFunct.LoadRow(entry, DGV, true);
            }
            Console.WriteLine("---");



            DatabaseFunct.loadingTable = false;
        }


        //returns a string if there are errors in the script
        public static string ValidateScript(string script)
        {
            string error = "";

            string[] parentColumns;


            void recursiveScriptValidator(string scriptToValidate, List<string> parentColumnDirectory)
            {
                


                //collect subscripts within "subtable column" curly brackets and includes nested curly brackets
                MatchCollection subtableScripts = Regex.Matches(scriptToValidate, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})");
                //ignore these subscripts
                string scriptAtCurrentLevel = Regex.Replace(scriptToValidate, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})", "");

                //then split script into it's columns by commas
                string[] columnScripts = scriptAtCurrentLevel.Split(',');

                int subtableScriptIndex = 0;

                List<string> columnNamesAtCurrentLevel = new List<string>();

                foreach (string columnScript in columnScripts)
                {
                    string[] columnDat = columnScript.Split(':');
                    string columnName = "";
                    string columnTypeShorthand = "";



                    string columnError = "";
                    try
                    {
                        columnName = columnDat[0].Trim();
                        //get string within <>
                        MatchCollection x = Regex.Matches(columnName, @"(?<=\<)[^}]*(?=\>)");
                        if (x.Count == 0)
                        {
                            columnError += "    column name is missing <> brackets \n";
                        }
                        else
                        {
                            columnName = x[0].Value;
                            if (columnNamesAtCurrentLevel.Contains(columnName))
                            {
                                columnError += "    column name is duplicate to existing column name at table level. \n";
                            }
                            else
                            {
                                columnNamesAtCurrentLevel.Add(columnName);
                            }



                        }


                        columnTypeShorthand = columnDat[1].Trim();
                    }
                    catch
                    {
                        columnError += "    column script formatting is invalid, and must be: \" <column_name> : column_type_shorthand \" \n"; 
                    }
                    

                    //make sure the column name is valid
                    string nameError = GenericFunct.ValidateNameInput(columnName);
                    if (nameError != "")
                    {
                        columnError += nameError;
                        
                    }
                    // then confirm the type is valid
                    string columnType = "";
                    if (!columnTypeShorthandDict.ContainsKey(columnTypeShorthand))
                    {
                        columnError += "    Column Type isn't valid, please use refer to the columnTypeShorthand info \n";
                    }
                    else
                    {
                        columnType = columnTypeShorthandDict[columnTypeShorthand];
                        //sanity check
                        if (!ColumnTypes.Types.ContainsKey(columnType))
                        {
                            columnError += "    INTERNAL ERROR: Column Type string in columnTypeShorthandDict doesn't refrence an existing key within ColumnTypes.Types \n";
                        }
                    }

                    //if the column type is a subtable or foreign key refrence, get the 3rd entry in columnDat

                   
                    bool breakloop = false;

                    if (columnType == "SubTable")
                    {
                        string subTableScript = columnDat[2].Trim();
                        //this should only contain the brackets since i've already removed all subtable data from the scriptToValidate
                        if (subTableScript != "{}")
                        {
                            columnError += "    subtable data formatting is invalid and must be contained within curly brackets \n";
                            //break the error loop if subtable formatting is invalid because the error loop will start considering the data within the subtable to be at the current table level when it isn't.
                            breakloop = true;
                        }
                        


                    }
                    else if (columnType == "Foreign Key Refrence")
                    {
                        string tableBeingRefrenced = columnDat[2].Trim();
                        //get string within <>
                        MatchCollection x = Regex.Matches(tableBeingRefrenced, @"(?<=\<)[^}]*(?=\>)");
                        if (x.Count == 0)
                        {
                            columnError += "    tableBeingRefrenced name is missing <> brackets \n";
                        }
                        else
                        {
                            tableBeingRefrenced = x[0].Value;

                            //confirm that the table being refrenced exists in current data
                            if (!DatabaseFunct.currentData.ContainsKey(tableBeingRefrenced))
                            {
                                columnError += "    The table being refrenced by the column doesn't exist within currentData \n";
                            }

                        }

                        
                    }





                    if (columnError != "")
                    {
                        error += "In ";
                        foreach (string parentColumnName in parentColumnDirectory)
                        {
                            error += parentColumnName + ">";

                        }
                        error += "\n the column with the name \"" + columnName + "\" is invalid:";
                        error += "{\n" + columnError + "}\n";
                    }
                    else if (columnType == "SubTable")
                    {
                        if (subtableScripts.Count > subtableScriptIndex)
                        {

                            //relevant match to subtable column should line up with subtableScriptIndex
                            Match dat = subtableScripts[subtableScriptIndex];
                            string subtableScriptToValidate = dat.Value;
                            subtableScriptIndex += 1;
                            //duplicate parentColumnDirectory
                            List<string> subtableParentColumnDirectory = new List<string>(parentColumnDirectory);

                            subtableParentColumnDirectory.Add(columnName);
                            //run recursive validation
                            recursiveScriptValidator(subtableScriptToValidate, subtableParentColumnDirectory);
                        }
                        else
                        {
                            columnError += "    the ammount of subtable data doesn't match up with the number of subtable columns, maybe there is a subtable left blank\n";
                            breakloop = true;
                        }
                    }

                }
            }


            //script cannot be blank if constructing the table
            if (String.IsNullOrWhiteSpace(script))
            {
                return "Script is blank";
            }

            recursiveScriptValidator(script, new List<string>() );


            return error;

        }

        //get tableConstructorScript from Auto Table Constructor Script that the Receiver column fetches
        public static string FetchTableConstructorScriptForReceiverColumn(string tableKey, string colName, Dictionary<int, Dictionary<string, dynamic>> tableData, int rowIndex)
        {
            string tableConstructorScript = null;


            //get foreign key refrence column this column is linked to 
            string linkedRefrenceColumnName = DatabaseFunct.currentData[tableKey][colName + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];
            //get the table being refrenced by that foreign key refrence column
            string tableBeingRefrenced = DatabaseFunct.currentData[tableKey][linkedRefrenceColumnName + DatabaseFunct.RefrenceColumnKeyExt];
            //get the value of the cell of the foreign key refrence column at the same row
            string tablePKSelected = tableData[rowIndex][linkedRefrenceColumnName];


            // check if anything in the linkedRefrenceColumn is even selected
            if (tablePKSelected == null)
            {
                if (!DatabaseFunct.loadingTable)
                {
                    MessageBox.Show("there is no Primary Key selected by the \"" + linkedRefrenceColumnName + "\" Column that this Auto Table Constructor Script Receiver Column is linked to.");
                }
                
                return null;
            }

            //now go to the refrenced table and look through the columns for the Primary Key Column and Auto Table Constructor Script Column
            string autoTableConstructorScriptColumnName = null;
            string primaryKeyColumnName = null;
            foreach (KeyValuePair<string, dynamic> KV in DatabaseFunct.currentData[tableBeingRefrenced])
            {
                if (KV.Value is string)
                {
                    if (KV.Value == "Primary Key")
                    {
                        primaryKeyColumnName = KV.Key;
                    }
                    else if (KV.Value == "Auto Table Constructor Script")
                    {
                        autoTableConstructorScriptColumnName = KV.Key;
                    }
                }
                
            }
           


            //check if Auto Table Constructor Script column Exists:
            if (autoTableConstructorScriptColumnName == null)
            {
                if (!DatabaseFunct.loadingTable)
                {
                    MessageBox.Show("There is no Auto Table Constructor Script column in the main table being refrenced by the \"" + linkedRefrenceColumnName + "\" Column that this Auto Table Constructor Script Receiver Column is linked to.");
                }
                return null;
            }
            else if (primaryKeyColumnName == null)//check if Primary Key column Exists:
            {
                if (!DatabaseFunct.loadingTable)
                {
                    MessageBox.Show(" there is no Primary Key column in the main table being refrenced by the \"" + linkedRefrenceColumnName + "\" Column that this Auto Table Constructor Script Receiver Column is linked to.");
                }
                    
                return null;
            }
            else
            {
                
                
                Dictionary<int, Dictionary<string, dynamic>> tableBeingRefrencedData = DatabaseFunct.currentData[tableBeingRefrenced][DatabaseFunct.RowEntryRefrence];
                

                bool PKFound = false;
                //look for the row that contains the primary key selected in the tableBeingRefrencedData
                for (int i = 0; i < tableBeingRefrencedData.Keys.Count; i++)
                {
                    Dictionary<string, dynamic> rowData = tableBeingRefrencedData[i];
                    if (rowData[primaryKeyColumnName] == tablePKSelected)
                    {
                        tableConstructorScript = rowData[autoTableConstructorScriptColumnName];
                        PKFound = true;
                        break;
                    }
                }
                // has a tableConstructorScript been found?
                if (!PKFound)
                {
                    if (!DatabaseFunct.loadingTable)
                    {
                        MessageBox.Show(" the Primary Key selected within the \"" + linkedRefrenceColumnName + "\" Column, that this Auto Table Constructor Script Receiver Column is linked to, doesn't exist within the main table (" + tableBeingRefrenced + ") the column is refrencing.");
                    }
                    return null;
                }
                else
                {
                    //validate the script
                    string scriptError = AutoTableConstructorScriptFunct.ValidateScript(tableConstructorScript);
                    if (scriptError != "")
                    {
                        if (!DatabaseFunct.loadingTable)
                        {
                            MessageBox.Show("the Auto Table Constructor Script in Table: " + tableBeingRefrenced + ", at Column: " + autoTableConstructorScriptColumnName + ", with Primary Key: " + tablePKSelected + ", being refrenced has error(s): \n" + scriptError);
                        }
                        return null;
                    }
                    else // valid
                    {
                        return tableConstructorScript;
                    }


                }


            }

            
        }


    }


}