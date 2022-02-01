using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Drawing;

namespace MDB
{
    public static class ColumnTypes
    {

        public static Dictionary<string, Type> Types = new Dictionary<string, Type>()
        {
            //has single column per table/subtable restriction
            //provides a unique identifier to each row
            {"Primary Key", typeof(DataGridViewTextBoxColumn)},
            //stores string values
            {"Text",typeof(DataGridViewTextBoxColumn) },
            //stores double values
            {"Numerical",typeof( DataGridViewTextBoxColumn) },
            //stores integer values
            {"Integer",typeof( DataGridViewTextBoxColumn) },
            //opens to a table structure that is unique to the column 
            //(the same structure exists across rows in the same column)
            {"SubTable",typeof(DataGridViewButtonColumn) },
            //set to another maintable upon creation.(or the current maintable)
            //it stores links to this maintable's primary keys.
            {"Foreign Key Refrence",typeof(DataGridViewComboBoxColumn) },
            //stores true or false values
            {"Bool",typeof(DataGridViewCheckBoxColumn) },
            //this column exists in a subtable and is set to a parent directory layer in the same table upon creation
            //it stores links to this parent subtable's primary key's
            {"Parent Subtable Foreign Key Refrence",typeof(DataGridViewComboBoxColumn) },
            
            //has single column per table restriction , requires a primary key column in the same table
            {"Auto Table Constructor Script",typeof(DataGridViewTextBoxColumn) },
            //this column links to an existing foreign key refrence column in the same table upon creation.
            //it takes the data from the "Auto Table Constructor Script Source" column cell at the same row as the primary key being refrenced to generate a subtable of a specified structure.
            {"Auto Table Constructor Script Receiver",typeof(DataGridViewButtonColumn) }, 

        };


        internal static dynamic GetDefaultColumnValue(string colType)
        {
            dynamic defaultVal = null;
            if (colType == "Bool")
            {
                defaultVal = false;


            }
            else if (colType == "SubTable" || colType == "Auto Table Constructor Script Receiver")
            {
                defaultVal = new Dictionary<int, Dictionary<string, dynamic>>();


            }
            
            return defaultVal;
        }


        internal static DataGridViewCheckBoxCell setBoolCellTFVals(DataGridViewCheckBoxCell cell)
        {
            cell.FalseValue = false;
            cell.TrueValue = true;

            return cell;
        }

        internal static string GetSubTableCellDisplay(Dictionary<int, Dictionary<string, dynamic>> subtableData,string columnName, string tableKey, string subtableConstructorScript)
        {
            var buttonText = columnName + "{\n";

            int i = 0;
            int displayMax = 5;
            while (i < subtableData.Count() && i < displayMax)
            {
                string rowText = "";
                string subTableKey = tableKey + "/" + columnName;

                List<string> columnOrder = new List<string>();
                Dictionary<string,string> columnTypes = new Dictionary<string, string>();
                //if this isn't a constructed table, leave subtableConstructorScript null
                if (subtableConstructorScript == null)
                {
                    columnOrder = DatabaseFunct.currentData[subTableKey][DatabaseFunct.ColumnOrderRefrence];
                    //get types
                    foreach (string subColName in columnOrder)
                    {
                        columnTypes[subColName] = DatabaseFunct.currentData[subTableKey][subColName];
                    }
                }
                else
                {
                    //get the column order from the subtable's constructor script
                    //collect subscripts within "subtable column" curly brackets and includes nested curly brackets
                    MatchCollection subtableScripts = Regex.Matches(subtableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})");

                    //ignore these subscripts
                    string scriptAtCurrentLevel = Regex.Replace(subtableConstructorScript, @"(?<=\{)(?>\{(?<c>)|[^{}]+|\}(?<-c>))*(?(c)(?!))(?=\})", "");

                    
                    //then split script into it's columns by commas
                    string[] columnScripts = scriptAtCurrentLevel.Split(',');


                    //collect column names into columnOrder
                    
                    foreach (string columnScript in columnScripts)
                    {
                        string[] columnDat = columnScript.Split(':');
                        //get name within <>
                        string subColumnName = Regex.Match(columnDat[0], @"(?<=\<)[^<>]*(?=\>)").Value;

                        columnOrder.Add(subColumnName);
                        //get type
                        columnTypes[subColumnName] = AutoTableConstructorScriptFunct.columnTypeShorthandDict[columnDat[1].Trim()];
                    }
                }

                //in order of the subtable's columnOrder
                foreach (string K in columnOrder)
                {

                    dynamic V = null;
                    Type Vt = typeof(int);
                    if (!subtableData[i].ContainsKey(K))//if the column doesn't exist in data, get the default value and set it to that within the table data
                    {
                        string colType = columnTypes[K];
                        dynamic defaultVal = ColumnTypes.GetDefaultColumnValue(colType);
                        subtableData[i][K] = defaultVal;
                    }
                        
                    

                    V = subtableData[i][K];

                    if (V != null)
                    {
                        Vt = V.GetType();
                    }


                    //show value of the subtable row unless it's a subtable column; in which {...} would be shown instead
                    rowText += "|" + K + ":" + (V == null ? "null" : (Vt.IsGenericType && Vt.GetGenericTypeDefinition() == typeof(Dictionary<,>) ? "{...}" : V.ToString()));
                }




                buttonText += i.ToString() + "- " + rowText + "\n";
                i += 1;
            }
            if (i == 0)
            {
                buttonText = "empty";
            }

            return buttonText;
        }

        internal static dynamic ValidateCellInput(DataGridViewCellEventArgs e, CustomDataGridView DGV)
        {

            string tableDir = DGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            dynamic value = DGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string colName = DGV.Columns[e.ColumnIndex].Name;


            bool isConstructed = false;
            //if this is a subtable column within an auto contructed table,then this is a constructed table:
            Dictionary<string, dynamic> DGVTag = DGV.Tag as Dictionary<string, dynamic>;
            if (DGVTag.ContainsKey("tableConstructorScript"))
            {
                //this means that this is part of a constructor script
                isConstructed = true;


            }
            

            string colType = null;
            
            if (isConstructed)
            {
                Dictionary<string , dynamic> colTag = DGV.Columns[e.ColumnIndex].Tag as Dictionary<string , dynamic>;
                //fetch the constructed column type:
                colType = colTag["columnType"];
                
                
            }
            else
            {
                colType = DatabaseFunct.currentData[tableKey][DGV.Columns[e.ColumnIndex].Name];
            }



            if (colType == "Primary Key")
            {
                int index = 0;
                //confirm that no other primary index exists with the same key
                foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
                {
                    if (entry.Value.ContainsKey(colName) && entry.Value[colName] == value)
                    {
                        //don't display if primary key is null
                        if (value != null)
                        {
                            System.Windows.Forms.MessageBox.Show("Duplicate primary key \"" + value + "\" exists at row index " + index);
                        }
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
            else if (colType == "Integer")
            {
                double a;
                if (!double.TryParse(value, out a))
                {
                    return null;
                }
                else
                {
                    //rounds to int
                    value = (int)a;
                }
            }
            else if (colType == "Auto Table Constructor Script" && value != null)
            {

                string scriptError = AutoTableConstructorScriptFunct.ValidateScript(value);
                if (scriptError != "")
                {
                    MessageBox.Show("There were errors in the Auto Table Constructor Script entered: \n" + scriptError);
                    //give the cell a red outline
                    DGV.AddInvalidCellIndexes(e.ColumnIndex, e.RowIndex);
                }
                else
                {
                    //remove the cell's red outline
                    DGV.RemoveInvalidCellIndexes(e.ColumnIndex, e.RowIndex);
                }

            }

            return value;
        }
    }
}
