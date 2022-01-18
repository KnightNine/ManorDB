using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Reflection;

namespace MDB
{

    /*
    All "Data Management" functions are in these partial classes
    */

    public static partial class DatabaseFunct
    {
        //used as a entry in the column that determines it's type
        public static string RowEntryRefrence = "@RowEntries";
        public static string ColumnOrderRefrence = "@ColumnOrder";
        public static string BookmarkColorRefrence = "@BookmarkColor";
        public static string ColumnDisablerArrayExt = ".ColumnsToDisable";
        public static string RefrenceColumnKeyExt = ".Refrence";
        public static string SubtableRowRestrictionExt = ".RowRestrictionInt";
        public static string ParentSubTableRefrenceColumnKeyExt = ".ParentSTRefrence";
        //if the table contains this key then it is a "File Regex Refrence" Table that will be read-only and source entries from the file refrenced.
        public static string RegexRefrenceTableConstructorDataRefrence = "@RegexRefrenceTableConstructorData";

        public static string ScriptReceiverLinkToRefrenceColumnExt = ".RefrenceColumnLinked";

        public static string selectedTable = "";






        //add all the colors
        public static Dictionary<string, object> GetStaticPropertyBag(Type t)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            var map = new Dictionary<string, object>();
            foreach (var prop in t.GetProperties(flags))
            {
                map[prop.Name] = prop.GetValue(null, null);
            }
            return map;
        }


        //entire Loaded MDB file data
        public static SortedDictionary<string, dynamic> currentData = new SortedDictionary<string, dynamic>()
        {

        };





        //GENERIC UTILITY FUNCTIONS=============================================================================================
        //(not specific to any subcategory of database function)

        
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
            //main table name is the selectedTable
            var currentDir = currentData[selectedTable][RowEntryRefrence];

            


            Console.WriteLine("Get tableData. \n From table rows:");
            foreach (Tuple<int, string> key in keys)
            {
                Console.WriteLine("Row: " + key.Item1);
                //get row
                currentDir = currentDir[key.Item1];
                Console.WriteLine("Column: " + key.Item2);
                //get column
                currentDir = currentDir[key.Item2];
            }
            

            Console.WriteLine("Returning TableData of: " + dir);
            return currentDir;

        }



        internal static void SetDataAtDir(string tableDir, dynamic value, int rowIndex, string columnIndex)
        {
            Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            tableData[rowIndex][columnIndex] = value;
        }



        //get all open Datagridviews of table key
        internal static List<CustomDataGridView> GetAllOpenDGVsAtTableLevel(string tableKey)
        {
            List<CustomDataGridView> OpenDGVs = new List<CustomDataGridView>() { };

            if (tableKey == Program.mainForm.TableMainGridView.Name)
            {
                OpenDGVs.Add(Program.mainForm.TableMainGridView);
            }
            else
            {
                foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
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
        internal static List<Dictionary<int, Dictionary<string, dynamic>>> GetAllTableDataAtTableLevel(string tableKey, ref List<string> DGVKeysPool)
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

            void subFunct(dynamic CD, List<string> lvlKeys, List<Dictionary<int, Dictionary<string, dynamic>>> tableList, string DGVKey)
            {
                Dictionary<int, Dictionary<string, dynamic>> tableData = CD as Dictionary<int, Dictionary<string, dynamic>>;

                if (lvlKeys.Count > 0)
                {
                    List<string> nextLvlKeys = GenericFunct.Clone(lvlKeys);
                    nextLvlKeys.RemoveAt(0);


                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> entry in tableData)
                    {
                        string RowDGVKey = DGVKey + "/" + entry.Key.ToString() + "," + lvlKeys[0];

                        try
                        {
                            var subtableCD = entry.Value[lvlKeys[0]];
                            subFunct(subtableCD, nextLvlKeys, collectedTables, RowDGVKey);
                        }
                        catch
                        {
                            Console.WriteLine("subtable collector could not find " + lvlKeys[0] + " in " + tableKey);
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


        //check if a cell of a row in tabledata contains data
        internal static bool DoesDataRowCellContainData(KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            Type valType;
            if (KVRow.Value[ColumnKey] != null)
            {
                valType = KVRow.Value[ColumnKey].GetType();
            }
            else
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
                //if bool return true if bool is true
                else if (valType == typeof(bool) && KVRow.Value[ColumnKey])
                {
                    return true;
                }
            }



            return false;
        }

        

    }
}
