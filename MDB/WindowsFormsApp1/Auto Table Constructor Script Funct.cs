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
            -Parent Subtable Foreign Key Refrence: " <column_name>:PF:<outward_directory> "
                to define the "outward_directory" you can assume that the table is a file directory and the current directory/base directory is where the Parent Subtable Foreign Key Refrence column is located.
                you must travel outwards in the directory to reach the desired subtable, to escape the current directory by one table level use '-'
                
                example: if the AutoTableConstructorScriptReceiver column subtable containing this column is located in: < "SCol Name0"/"SCol Name1"/"AutoTableConstructorScriptReceiver Column Name" > (quotations included)
                and you want to reach an adjacent subtable in <"SCol Name0"/"SCol Name1"/...>  called "Adjacent SCol Name1", you can enter the directory as < -/"Adjacent SCol Name1" >.
                    - with '-' you are escaping once to the previous table level ("SCol Name1") 
                    - then you are entering a different subtable column called "Adjacent SCol Name1"
                
                Note1: you don't necessarily need to enter an adjacent subtable, <-/-> is a valid outward_directory.
                Note2: you cannot escape to nor beyond the main table directory and must reference a subtable column.
                Note3: only the immediate table level of Adjacent subtable columns to the base directory referenced is used, so <-/"Adj Sub"/"Sub2"> is invalid.
                Note4: spacing outside of quotes is trimmed.
                Note5: you cannot reference a subtable column within a constructed table (this is mostly irrelevant though as there are is no primary key support within TableConstructorScript).
                
                This "outward directory method" is designed to consider that, if any columnnames in the directory are changed, it won't affect the directory within the script unless it's the name of an adjacent subtable columnname that is being referenced.
                


            -Auto Table Constructor Script Receiver: " <column_name>:A:<linked_fKey_column_name> "

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
            {"PF", "Parent Subtable Foreign Key Refrence" },
            {"S", "SubTable" },
            {"A","Auto Table Constructor Script Receiver" }
        };
        public static Dictionary<string, Type> columnCellValueValidType = new Dictionary<string, Type>()
        {
            {"Text",typeof(string) },
            {"Numerical", typeof(double) },
            {"Integer",typeof(int) },
            {"Bool",typeof(bool) },
            {"Foreign Key Refrence",typeof(string) },
            {"Parent Subtable Foreign Key Refrence",typeof(string) },
            {"SubTable", typeof(Dictionary<int, Dictionary<string, dynamic>>)},
            {"Auto Table Constructor Script Receiver", typeof(Dictionary<int, Dictionary<string, dynamic>>) }
        };




        public static string FetchDirectoryFromOutwardDirectory(string outwardDirectory, string currentDirectory)
        {
            string selectedDirectory = currentDirectory + "";
            

            //remove from selectedDirectory while outwarDirectory uses -
            string[] splitOutwardDirectory = outwardDirectory.Split('/');

            bool selectedDirectoryComplete = false;

            foreach (string command in splitOutwardDirectory)
            {
                string c = command.Trim();
                if (c == "-")
                {

                    if (!selectedDirectory.Contains(","))
                    {
                        MessageBox.Show("outward directory escapes into or beyond main-table! you must reference a subtable column.");
                        return null;
                    }

                    //remove from selectedDirectory
                    // match everything after and including last ','
                    string regex = @",[^,]*$";
                    selectedDirectory = Regex.Replace(selectedDirectory, regex, "");

                    




                }
                else// c is subtable column name in quotes
                {



                    //match within quotes ""
                    //without escaping quotes: "(?<=")[^"]*(?= ")"
                    string regex = "(?<=\")[^\"]*(?=\")";

                    string subColName = Regex.Match(c, regex).Value;
                    //re-add the removed ','
                    selectedDirectory += "," + subColName;

                    selectedDirectoryComplete = true;

                    break;
                    

                }

            }

            //clip the row index of the remaining directory
            if (!selectedDirectoryComplete)
            {
                // match everything after and including last '/'
                string regex = @"/[^/]*$";

                selectedDirectory = Regex.Replace(selectedDirectory, regex,"");

                if (!selectedDirectory.Contains(","))
                {
                    MessageBox.Show("outward directory escapes into main-table! you must reference a subtable column.");
                    return null;
                }

            }

            


            return selectedDirectory;
        }



        public static void LoadConstructedColumn(string colName, string colType, CustomDataGridView DGV, Dictionary<int, Dictionary<string, dynamic>> tableData, Tuple<string,dynamic> additionalColumnData)
        {




            //create column object
            DataGridViewColumn myDataCol = Program.GetDataGridViewColumn(colName, colType);

            Dictionary<string, dynamic>  colTag = new Dictionary<string, dynamic>();

            //store additional data specific to certain column types
            if (additionalColumnData != null)
            {


                colTag.Add(additionalColumnData.Item1,additionalColumnData.Item2);
                
            }

            //store the column type within the tag in order to refrence the column types within constructed tables
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

        
        public static Tuple<MatchCollection,bool, string[]> FetchTopLevelScriptData(string tableConstructorScript)
        {
            //collect subscripts within "subtable column" curly brackets and includes nested curly brackets
            MatchCollection subtableScripts = Regex.Matches(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})");

            //ignore these subscripts
            string scriptAtCurrentLevel = Regex.Replace(tableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})", "");
            //get if single row restriction:
            bool singleRowRestriction = scriptAtCurrentLevel.StartsWith("#");

            string[] columnScripts = scriptAtCurrentLevel.Split(',');

            return new Tuple<MatchCollection,bool, string[]>(subtableScripts, singleRowRestriction, columnScripts);
        }

        //whenever a AutoTableConstructorScriptReader button (or a subtable button within the AutoTableConstructorScriptReader subtable) is pressed, instead of referring to the currentData table structure, refer to the script that the button is refrencing
        public static void ConstructSubTableStructureFromScript(CustomDataGridView DGV)
        {
            Dictionary<string,dynamic> DGVTag = DGV.Tag as Dictionary<string,dynamic>;
            string tableConstructorScript = DGVTag["tableConstructorScript"];



            //find data source
            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(DGV.Name);




            DatabaseFunct.loadingTable = true;


            Tuple<MatchCollection,bool, string[]>  dat = FetchTopLevelScriptData(tableConstructorScript);
            MatchCollection subtableScripts = dat.Item1;
            bool singleRowRestriction = dat.Item2;
            string[] columnScripts = dat.Item3;


            

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
                else if (columnType == "Auto Table Constructor Script Receiver")
                {
                    //get string within <>
                    string linkedFKeyColumn = Regex.Match(columnDat[2].Trim(), @"(?<=\<)[^}]*(?=\>)").Value;
                    additionalColumnData = new Tuple<string, dynamic>(DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt, linkedFKeyColumn);

                }
                else if (columnType == "Foreign Key Refrence")
                {
                    //get string within <>
                    string tableBeingRefrenced = Regex.Match(columnDat[2].Trim(), @"(?<=\<)[^}]*(?=\>)").Value;

                    additionalColumnData = new Tuple<string, dynamic>(DatabaseFunct.RefrenceColumnKeyExt, tableBeingRefrenced);
                }
                else if (columnType == "Parent Subtable Foreign Key Refrence")
                {
                    //get string within <>
                    string outwardDirectory = Regex.Match(columnDat[2].Trim(), @"(?<=\<)[^}]*(?=\>)").Value;

                    additionalColumnData = new Tuple<string, dynamic>(DatabaseFunct.ParentSubTableRefrenceColumnKeyExt, outwardDirectory);
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

                Tuple<MatchCollection, bool, string[]> dat = FetchTopLevelScriptData(scriptToValidate);
                MatchCollection subtableScripts = dat.Item1;
                bool singleRowRestriction = dat.Item2;
                string[] columnScripts = dat.Item3;

                

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
                    else if (columnType == "Auto Table Constructor Script Receiver")
                    {
                        //get string within <>
                        
                        string linkedFKeyColumn = columnDat[2].Trim();
                        //get string within <>
                        MatchCollection x = Regex.Matches(linkedFKeyColumn, @"(?<=\<)[^}]*(?=\>)");
                        if (x.Count == 0)
                        {
                            columnError += "    linkedForeignKeyColumn name is missing <> brackets \n";
                        }
                        else
                        {
                            //check if adjacent column of Fkey type exists
                            linkedFKeyColumn = x[0].Value;

                            bool linkedFKeyColumnIsFound = false;

                            foreach (string columnScriptAtCurrentLevel in columnScripts)
                            {
                                try
                                {
                                    string[] _columnDat = columnScriptAtCurrentLevel.Split(':');
                                    string _columnName = Regex.Match(_columnDat[0], @"(?<=\<)[^}]*(?=\>)").Value;
                                    string _columnTypeShorthand = _columnDat[1].Trim();

                                    Console.WriteLine(_columnName +"=="+ linkedFKeyColumn+" ?");

                                    if (_columnName == linkedFKeyColumn)
                                    {
                                        if (columnTypeShorthandDict.ContainsKey(_columnTypeShorthand))
                                        {

                                            string _columnType = columnTypeShorthandDict[_columnTypeShorthand];
                                            if (_columnType != "Foreign Key Refrence")
                                            {
                                                columnError += "    linked column is not a Foreign Key Refrence column \n";
                                                break;
                                            }
                                            else
                                            {
                                                linkedFKeyColumnIsFound = true;
                                            }

                                        }
                                        else
                                        {
                                            columnError += "    could not read a column type of linkedFKeyColumn \n";
                                            break;
                                        }
                                    }

                                }
                                catch
                                {
                                    columnError += "    could not read a column name or type when checking for the linked Foreign key refrence column \n";
                                }
                               
                            }

                            if (!linkedFKeyColumnIsFound)
                            {
                                columnError += "    could not find linkedForeignKeyColumn \n";
                            }


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
                    else if (columnType == "Parent Subtable Foreign Key Refrence")
                    {
                        string outwardDirectory = columnDat[2].Trim();
                        //get string within <>
                        MatchCollection x = Regex.Matches(outwardDirectory, @"(?<=\<)[^}]*(?=\>)");
                        if (x.Count == 0)
                        {
                            columnError += "    outwardDirectory is missing <> brackets \n";
                        }
                        else
                        {
                            //check that all directory syntax is valid:
                            string[] splitOutwardDirectory = x[0].Value.Split('/');
                            foreach (string s in splitOutwardDirectory)
                            {
                                string ts = s.Trim();
                                if (ts != "-" )
                                {
                                    MatchCollection y = Regex.Matches(outwardDirectory, "(?<=\")[^\"]*(?=\")");
                                    if (y.Count == 0)
                                    {
                                        columnError += "    outwardDirectory entry \""+ ts + "\" is missing \"\" quotes \n";
                                    }
                                   
                                }
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
                            Match match = subtableScripts[subtableScriptIndex];
                            string subtableScriptToValidate = match.Value;
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
        public static string FetchTableConstructorScriptForReceiverColumn(string tableKey, string colName, Dictionary<int, Dictionary<string, dynamic>> tableData, int rowIndex, CustomDataGridView senderDGV)
        {
            string tableConstructorScript = null;

            //get if the table is constructed by script
            Dictionary<string, dynamic> DGVTag = senderDGV.Tag as Dictionary<string, dynamic>;
            bool isConstructed = false;
            if (DGVTag.ContainsKey("tableConstructorScript"))
            {
                isConstructed = true;
            }


            //get foreign key refrence column this column is linked to 
            string linkedRefrenceColumnName = "";
            //get the table being refrenced by that foreign key refrence column
            string tableBeingRefrenced = "";

            if (isConstructed)
            {
                Dictionary<string, dynamic> colTag = senderDGV.Columns[colName].Tag as Dictionary<string, dynamic>;
                linkedRefrenceColumnName = colTag[DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];
                Dictionary<string, dynamic> refColTag = senderDGV.Columns[linkedRefrenceColumnName].Tag as Dictionary<string, dynamic>;
                tableBeingRefrenced = refColTag[DatabaseFunct.RefrenceColumnKeyExt];

            }
            else
            {
                linkedRefrenceColumnName = DatabaseFunct.currentData[tableKey][colName + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];
                tableBeingRefrenced = DatabaseFunct.currentData[tableKey][linkedRefrenceColumnName + DatabaseFunct.RefrenceColumnKeyExt];
            }

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