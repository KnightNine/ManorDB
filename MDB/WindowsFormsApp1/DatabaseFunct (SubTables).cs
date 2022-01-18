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
    public static partial class DatabaseFunct
    {
        internal static bool CreateSubTable(string parentTabName, string colName)
        {
            string subtableName = parentTabName + "/" + colName;
            if (!currentData.Keys.Contains(subtableName))
            {
                currentData.Add(subtableName, new Dictionary<string, dynamic>() { { ColumnOrderRefrence, new List<string>() } });
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("that subtable already exists! this shouldn't happen.");
                return false;
            }
            return true;
        }










        //this exists to remove the need to reload all subtables when a row is shifted
        //it shifts the indexes within the names of subtable DGVs
        internal static void SwapSubtableNames(CustomDataGridView DGV, int index1, int index2)
        {





            List<CustomDataGridView> storedDGVs1 = GetSubtablesOfRow(index1);
            List<CustomDataGridView> storedDGVs2 = GetSubtablesOfRow(index2);

            SwapOpenSubTableKeys(new Tuple<CustomDataGridView, int>(DGV, index1), new Tuple<CustomDataGridView, int>(DGV, index2));

            ReplaceNamesOfDGVList(storedDGVs1, index1, index2);
            ReplaceNamesOfDGVList(storedDGVs2, index2, index1);



            //store subtables of oldRowIndex
            List<CustomDataGridView> GetSubtablesOfRow(int index)
            {
                List<CustomDataGridView> storedDGVs = new List<CustomDataGridView>();
                Tuple<CustomDataGridView, int> openSubTableKey = new Tuple<CustomDataGridView, int>(DGV, index);
                if (Program.openSubTables.ContainsKey(openSubTableKey))
                {
                    CustomDataGridView subTable = Program.openSubTables[openSubTableKey].Item2;
                    storedDGVs.Add(subTable);
                    subFunct(subTable);
                    //collect tables within subtable recursively
                    void subFunct(CustomDataGridView subDGV)
                    {
                        foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> openSubTable in Program.openSubTables)
                        {
                            if (openSubTable.Key.Item1 == subDGV)
                            {

                                storedDGVs.Add(openSubTable.Value.Item2);
                            }


                        }
                    }

                }


                return storedDGVs;
            }

            void SwapOpenSubTableKeys(Tuple<CustomDataGridView, int> key1, Tuple<CustomDataGridView, int> key2)
            {
                //replace key of first subtable with the second and vice versa
                Console.WriteLine("table Keys are being swapped: " + key1.ToString() + " is swapping with " + key2.ToString());
                Tuple<string, CustomDataGridView> storedOpenSubTableVal = new Tuple<string, CustomDataGridView>(null, null);
                if (Program.openSubTables.ContainsKey(key1))
                {
                    storedOpenSubTableVal = Program.openSubTables[key1];
                    Console.WriteLine(storedOpenSubTableVal.ToString() + " is taken from " + key1.ToString());
                    Program.openSubTables.Remove(key1);
                }

                if (Program.openSubTables.ContainsKey(key2))
                {

                    Program.openSubTables[key1] = Program.openSubTables[key2];
                }

                if (storedOpenSubTableVal.Item1 != null)
                {
                    Program.openSubTables[key2] = storedOpenSubTableVal;
                    Console.WriteLine(" and placed into " + key2.ToString() + " as " + storedOpenSubTableVal.ToString());
                }



            }

            void ReplaceNamesOfDGVList(List<CustomDataGridView> gridViews, int oldIndex, int newIndex)
            {
                //change to new name
                string oldHalfName = DGV.Name + "/" + oldIndex;
                Console.WriteLine("oldHalfName:" + oldHalfName);
                //will replace the old half name
                string newHalfName = DGV.Name + "/" + newIndex;
                Console.WriteLine("newHalfName:" + newHalfName);
                Console.WriteLine("");
                foreach (CustomDataGridView subDGV in gridViews)
                {
                    Console.WriteLine("From: " + subDGV.Name);
                    subDGV.Name = newHalfName + subDGV.Name.Remove(0, oldHalfName.Length);
                    Console.WriteLine("To: " + subDGV.Name);
                }
                Console.WriteLine("---");
            }








        }






        internal static Tuple<CustomDataGridView, int> FindSubtableParentAndRowFromDGV(CustomDataGridView childSubTable)
        {
            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> subTable in Program.openSubTables)
            {

                //remove subtables that derive from subtable
                if (subTable.Value.Item2 == childSubTable)
                {
                    //return parent DGV and row index
                    return subTable.Key;
                }


            }
            System.Windows.Forms.MessageBox.Show("subtable child not found!");
            return null;
        }

        //removes subtable and children from parent table & open table array
        //<parent table, row index>
        //this does not remove the subtable from the CustomDataGridView, that is done before calling this (for some reason, i can't say why exactly, there must be a case in which this is called seperately from the removal from the dgv object, i'm writing this 5 months after i built this )
        internal static void RemoveSubtableFromOpenSubtables(Tuple<CustomDataGridView, int> subTableKey)
        {



            List<KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>>> removalList = new List<KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>>>();

            Console.WriteLine("trying to remove subtable from " + subTableKey.Item1.Name + " at row " + subTableKey.Item2.ToString());


            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> subTable in Program.openSubTables)
            {
                Console.WriteLine(subTable.Key.Item1.Name);
                //remove subtables that derive from subtable from opensubtable array
                if (subTable.Key.Item1.Name.StartsWith(Program.openSubTables[subTableKey].Item2.Name + "/") || subTable.Key.Item1.Name == Program.openSubTables[subTableKey].Item2.Name)
                {
                    removalList.Add(subTable);


                }
            }
            foreach (KeyValuePair<Tuple<CustomDataGridView, int>, Tuple<string, CustomDataGridView>> subTable in removalList)
            {
                Program.openSubTables.Remove(subTable.Key);
                Console.WriteLine(subTable.Key.ToString() + " removed from open subtables.");
            }

            //remove control
            subTableKey.Item1.Controls.Remove(Program.openSubTables[subTableKey].Item2);
            //remove from open subtables
            Program.openSubTables.Remove(subTableKey);
        }

        //the loading of a table's column and rows for the main table and subtables

        private static bool tableLoading = false;

        public static bool loadingTable
        {
            get
            {
                return tableLoading;
            }
            set
            {
                if (value)
                {
                    Console.WriteLine("Loading Table Data");
                }
                else
                {
                    Console.WriteLine("Done Loading Table Data");
                }

                tableLoading = value;
            }
        }




        //convert a datagridview name to a table key from currentdata where table data regarding columns is stored
        internal static string ConvertDirToTableKey(string dir)
        {
            //removes row indexes from dir (i.e. "abc/9,uanme/0,lol" > "abc/uanme/lol")
            string regex = "(\\/[^\\,]*\\,)";
            string output = Regex.Replace(dir, regex, "/");
            return output;

        }
    }
}
