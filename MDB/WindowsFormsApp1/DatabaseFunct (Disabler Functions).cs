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
        //======================================================================================================================
        // Disable and Enable Cells in a DataGridView



        internal static void DisableCellAtColAndRow(DataGridView DGV, string ColKey, int RowIndex)
        {


            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);

            DataGridViewCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex];

            //this is a cosmetic change and shouldn't trigger TableMainGridView_CellValueChanged

            //also don't want this prematurely ending the loading sequence
            bool is_already_loading = loadingTable;
            if (!is_already_loading)
            {
                loadingTable = true;
            }

            cell.Value = null;

            if (!is_already_loading)
            {
                loadingTable = false;
            }


            //ignore if already disabled
            if (!cell.ReadOnly)
            {

                cell.ReadOnly = true;

                //make button not pressable
                if (cell.GetType() == typeof(DataGridViewButtonCell))
                {

                    DataGridViewButtonCell bcell = (DataGridViewButtonCell)cell;
                    Console.WriteLine("bcell.Tag...");
                    Console.WriteLine(bcell.Tag);
                    Console.WriteLine(bcell.Tag.GetType());
                    ((Dictionary<string, dynamic>)bcell.Tag)["Enabled"] = false;
                    //bcell.FlatStyle = FlatStyle.Popup;
                }
                if (cell.GetType() == typeof(DataGridViewComboBoxCell))
                {
                    DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)cell;
                    //cbcell.FlatStyle = FlatStyle.Popup;
                }
                if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Popup;
                }


                //gray out the cell
                string currentTheme = ColorThemes.currentTheme;
                cell.Style.BackColor = ColorThemes.Themes[currentTheme]["Disabled"];
                cell.Style.ForeColor = ColorThemes.Themes[currentTheme]["Disabled"];
                cell.Style.SelectionBackColor = ColorThemes.Themes[currentTheme]["DisabledSelected"];
            }

        }

        internal static void EnableCellAtColAndRow(DataGridView DGV, string ColKey, int RowIndex)
        {
            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);

            DataGridViewCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex];
            //ignore if already enabled
            if (cell.ReadOnly)
            {
                cell.ReadOnly = false;

                //make button pressable
                if (cell.GetType() == typeof(DataGridViewButtonCell))
                {

                    DataGridViewButtonCell bcell = (DataGridViewButtonCell)cell;
                    ((Dictionary<string, dynamic>)bcell.Tag)["Enabled"] = true;

                    //bcell.FlatStyle = FlatStyle.Standard;
                }
                if (cell.GetType() == typeof(DataGridViewComboBoxCell))
                {
                    DataGridViewComboBoxCell cbcell = (DataGridViewComboBoxCell)cell;
                    //cbcell.FlatStyle = FlatStyle.Standard;
                }
                if (cell.GetType() == typeof(DataGridViewCheckBoxCell))
                {
                    DataGridViewCheckBoxCell cbcell = (DataGridViewCheckBoxCell)cell;
                    cbcell.FlatStyle = FlatStyle.Standard;
                }


                //restore cell style to the default value
                //default style
                if (RowIndex % 2 == 0)
                {
                    cell.Style.BackColor = DGV.RowsDefaultCellStyle.BackColor;
                    cell.Style.ForeColor = DGV.RowsDefaultCellStyle.ForeColor;
                    cell.Style.SelectionBackColor = DGV.RowsDefaultCellStyle.SelectionBackColor;
                }
                else
                {
                    cell.Style.BackColor = DGV.AlternatingRowsDefaultCellStyle.BackColor;
                    cell.Style.ForeColor = DGV.AlternatingRowsDefaultCellStyle.ForeColor;
                    cell.Style.SelectionBackColor = DGV.AlternatingRowsDefaultCellStyle.SelectionBackColor;
                }
            }

        }

        

        //I'd need to account for other potential disabler conditions that may still be disabling either of the two cells
        internal static bool IsDataRowCellStillDisabled(string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            if (DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
            {
                foreach (string disablerColumnKey in currentData[tableKey][ColumnKey + ColumnDisablerArrayExt])
                {

                    if (DoesDataRowCellContainData(KVRow, disablerColumnKey))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //when a change is made to a cell of a column with a disabler array, update all cells in the same row of columns within the disabler array
        internal static void UpdateStatusOfAllRowCellsInDisablerArrayOfCell(DataGridView DGV, string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey)
        {
            Console.WriteLine("in " + DGV.Name + " at row index " + KVRow.Key.ToString());
            Console.WriteLine("Checking Row Cells if Disabled:");

            if (DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
            {
                foreach (string ColumnKeyToUpdate in currentData[tableKey][ColumnKey + ColumnDisablerArrayExt])
                {
                    Console.WriteLine(ColumnKeyToUpdate);
                    bool isDisabled = IsDataRowCellStillDisabled(tableKey, KVRow, ColumnKeyToUpdate);
                    Console.WriteLine("     IsDisabled = " + isDisabled.ToString());
                    if (isDisabled)
                    {
                        DisableCellAtColAndRow(DGV, ColumnKeyToUpdate, KVRow.Key);
                    }
                    else
                    {
                        EnableCellAtColAndRow(DGV, ColumnKeyToUpdate, KVRow.Key);
                    }
                }
            }
        }

        //add new disabler array connection between two columns
        internal static void addColToDisablerArr(string tableKey, string selectedColKey1, string selectedColKey2)
        {
            //is being removed
            bool isRemoveDisablerCondition = false;

            void addOrRemoveFromDisablerArray(string disablerColKey, string selectedColKey)
            {
                //remove if already added
                if (!DatabaseFunct.currentData[tableKey].ContainsKey(disablerColKey + ColumnDisablerArrayExt))
                {
                    currentData[tableKey][disablerColKey + ColumnDisablerArrayExt] = new List<string> { selectedColKey };
                }
                else
                {
                    if (currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Contains(selectedColKey))
                    {
                        currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Remove(selectedColKey);
                        if (currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Count == 0)
                        {
                            currentData[tableKey].Remove(disablerColKey + ColumnDisablerArrayExt);
                        }
                        isRemoveDisablerCondition = true;
                    }
                    else
                    {
                        currentData[tableKey][disablerColKey + ColumnDisablerArrayExt].Add(selectedColKey);
                    }

                    //Console.WriteLine(DatabaseFunct.currentData[tableKey][disablerColKey + DatabaseFunct.ColumnDisablerArrayExt].ToString());


                }

            }

            addOrRemoveFromDisablerArray(selectedColKey1, selectedColKey2);
            // do the reverse where selected col key disables the disabler column key
            addOrRemoveFromDisablerArray(selectedColKey2, selectedColKey1);


            //--------------------------------------------------------------------------------------------------

            //i need to reconstruct the DGV names from the TableDataWithRow
            //parallel list with TableDataWithRow that contains DGV key names of TableData if they appear in openDGVs
            List<string> DGVKeysList = new List<string>();

            //Check along all rows at this table depth for this column for conflicting data and delete it
            List<Dictionary<int, Dictionary<string, dynamic>>> TableDataWithRow = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysList);

            //how can i tell if a cell is in an open table
            List<DataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);



            if (!isRemoveDisablerCondition)
            {
                //then disable the cells where there is conflict








                int tableIndex = 0;

                foreach (Dictionary<int, Dictionary<string, dynamic>> Table in TableDataWithRow)
                {
                    string DGVKeyOfTable = DGVKeysList[tableIndex];

                    DataGridView openDGVOfTable = null;
                    //get openDGVOfTable
                    foreach (DataGridView openDGV in openDGVs)
                    {
                        if (openDGV.Name == DGVKeyOfTable)
                        {
                            openDGVOfTable = openDGV;
                        }
                    }



                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> KVRow in Table)
                    {
                        //if the selected cell isn't void of data then disable the other column cell and vice versa
                        void disableCol2IfCol1HasData(string Col1, string Col2)
                        {
                            Console.WriteLine("Row Dat: ");
                            Console.WriteLine(KVRow.Value.ToString());
                            Console.WriteLine("DoesDataRowCellContainData Input: ");
                            if (KVRow.Value[Col1] != null)
                            {
                                Console.WriteLine(KVRow.Value[Col1].ToString());
                            }
                            else
                            {
                                Console.WriteLine("null");
                            }
                            Console.WriteLine("Erase/Disable Subject: ");
                            if (KVRow.Value[Col2] != null)
                            {
                                Console.WriteLine(KVRow.Value[Col2].ToString());
                            }
                            else
                            {
                                Console.WriteLine("null");
                            }



                            if (DatabaseFunct.DoesDataRowCellContainData(KVRow, Col1))
                            {

                                Type valType;
                                if (KVRow.Value[Col2] != null)
                                {
                                    valType = KVRow.Value[Col2].GetType();
                                }
                                else
                                {
                                    //val is null and null doesn't have a type so it will catch
                                    valType = null;
                                }

                                //erase value in data

                                //if subtable
                                if (valType == typeof(Dictionary<int, Dictionary<string, dynamic>>))
                                {

                                    KVRow.Value[Col2].Clear();


                                    //close the subtable if subtable open
                                    if (openDGVOfTable != null)
                                    {
                                        Tuple<DataGridView, int> openSubTableKey = new Tuple<DataGridView, int>(openDGVOfTable, KVRow.Key);
                                        //open subtable exists
                                        if (Program.openSubTables.ContainsKey(openSubTableKey))
                                        {
                                            //and that open subtable is of selectedColKey
                                            if (Program.openSubTables[openSubTableKey].Item2.Name.EndsWith("," + Col2))
                                            {


                                                openSubTableKey.Item1.Controls.Remove(Program.openSubTables[openSubTableKey].Item2);

                                                //close subtable
                                                RemoveSubtableFromOpenSubtables(openSubTableKey);
                                                //close row
                                                openSubTableKey.Item1.Rows[openSubTableKey.Item2].Height = openSubTableKey.Item1.RowTemplate.Height;
                                                openSubTableKey.Item1.Rows[openSubTableKey.Item2].DividerHeight = 0;
                                            }
                                        }

                                    }
                                }
                                //if bool
                                else if (valType == typeof(bool))
                                {
                                    KVRow.Value[Col2] = false;
                                }
                                //if not subtable or a bool
                                else
                                {
                                    KVRow.Value[Col2] = null;
                                }



                                //disable if dgv table is open

                                if (openDGVOfTable != null)
                                {
                                    DisableCellAtColAndRow(openDGVOfTable, Col2, KVRow.Key);
                                }






                            }
                        }



                        disableCol2IfCol1HasData(selectedColKey1, selectedColKey2);
                        disableCol2IfCol1HasData(selectedColKey2, selectedColKey1);

                    }
                    tableIndex += 1;
                }
            }
            else //check what cells need to be re-enabled upon this condition being lifted
            {


                int tableIndex = 0;

                foreach (Dictionary<int, Dictionary<string, dynamic>> Table in TableDataWithRow)
                {
                    string DGVKeyOfTable = DGVKeysList[tableIndex];

                    DataGridView openDGVOfTable = null;
                    //get openDGVOfTable
                    foreach (DataGridView openDGV in openDGVs)
                    {
                        if (openDGV.Name == DGVKeyOfTable)
                        {
                            openDGVOfTable = openDGV;
                        }
                    }

                    foreach (KeyValuePair<int, Dictionary<string, dynamic>> KVRow in Table)
                    {

                        if (openDGVOfTable != null)
                        {
                            bool isDisabled1 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey1);
                            bool isDisabled2 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey2);
                            if (!isDisabled1)
                            {
                                EnableCellAtColAndRow(openDGVOfTable, selectedColKey1, KVRow.Key);
                            }
                            if (!isDisabled2)
                            {
                                EnableCellAtColAndRow(openDGVOfTable, selectedColKey2, KVRow.Key);
                            }
                        }

                    }

                    tableIndex += 1;
                }
            }






        }


        //======================================================================================================================
    }
}
