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
        // Grey out dependent script receiver cells that require one or more linked columns to be filled:


        //get linked columns and grey out the cell if none of the linked columns are filled
        //any cell can be put in this function if the columntype is uncertain 
        internal static void UpdateReceiverCellUnfulfilledDependencyState(string tableKey, string colName, Dictionary<int, Dictionary<string, dynamic>> tableData, int rowIndex, CustomDataGridView senderDGV)
        {
            bool hasUnfulfilledDependency = true;

            string tableConstructorScript = null;

            //get if the table is constructed by script
            Dictionary<string, dynamic> DGVTag = senderDGV.Tag as Dictionary<string, dynamic>;
            bool isConstructed = false;
            if (DGVTag.ContainsKey("tableConstructorScript"))
            {
                isConstructed = true;
            }


            //get foreign key refrence column(s) this column is linked to 
            dynamic linkedFKeyRefrenceColumnNameData;
            

            if (isConstructed)
            {

                

                Dictionary<string, dynamic> colTag = senderDGV.Columns[colName].Tag as Dictionary<string, dynamic>;

                string colType = colTag["columnType"];


                if (Regex.IsMatch(colType, @"Auto Table Constructor Script Receiver\d*$"))
                {
                    linkedFKeyRefrenceColumnNameData = colTag[DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];





                    foreach (string linkedFKeyRefrenceColumnName in linkedFKeyRefrenceColumnNameData)
                    {



                        //get the value of the cell of the foreign key refrence column at the same row
                        string tablePKSelected = tableData[rowIndex][linkedFKeyRefrenceColumnName];

                        if (tablePKSelected != null)
                        {
                            hasUnfulfilledDependency = false;
                            break;
                        }
                    }
                }
                else
                {
                    hasUnfulfilledDependency = false;
                }


            }
            else
            {

                string colType = currentData[tableKey][colName];


                if (Regex.IsMatch(colType, @"Auto Table Constructor Script Receiver\d*$"))
                {

                    linkedFKeyRefrenceColumnNameData = DatabaseFunct.currentData[tableKey][colName + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt];

                    //convert to list from string for backwards compatablity
                    if (linkedFKeyRefrenceColumnNameData is string)
                    {
                        linkedFKeyRefrenceColumnNameData = new List<string>() { linkedFKeyRefrenceColumnNameData };
                        DatabaseFunct.currentData[tableKey][colName + DatabaseFunct.ScriptReceiverLinkToRefrenceColumnExt] = linkedFKeyRefrenceColumnNameData;
                    }

                    foreach (string linkedFKeyRefrenceColumnName in linkedFKeyRefrenceColumnNameData)
                    {


                        //get the value of the cell of the foreign key refrence column at the same row
                        string tablePKSelected = tableData[rowIndex][linkedFKeyRefrenceColumnName];

                        if (tablePKSelected != null)
                        {
                            hasUnfulfilledDependency = false;
                            break;
                        }

                    }
                }
                else
                {
                    hasUnfulfilledDependency = false;
                }
            }


            if (hasUnfulfilledDependency)
            {
                ShowUnfufilledDependencyOfReceiverCellAtColAndRow(senderDGV, colName, rowIndex);
            }
            else
            {
                HideUnfufilledDependencyOfReceiverCellAtColAndRow(senderDGV, colName, rowIndex);
            }
        }


        internal static void ShowUnfufilledDependencyOfReceiverCellAtColAndRow(CustomDataGridView DGV, string ColKey, int RowIndex)
        {

            

            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);

            DataGridViewButtonCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex] as DataGridViewButtonCell;

            //if the cell isn't disabled:
            if(((Dictionary<string, dynamic>)cell.Tag)["Enabled"] != false)
            {
                //gray out the cell
                string currentTheme = ColorThemes.currentTheme;
                cell.Style.ForeColor = ColorThemes.Themes[currentTheme]["UnfulfilledDependency"];
                cell.Style.SelectionForeColor = ColorThemes.Themes[currentTheme]["UnfulfilledDependency"];

            }

            
        }

        internal static void HideUnfufilledDependencyOfReceiverCellAtColAndRow(CustomDataGridView DGV, string ColKey, int RowIndex)
        {
            int ColumnIndex = DGV.Columns.IndexOf(DGV.Columns[ColKey]);

            DataGridViewButtonCell cell = DGV.Rows[RowIndex].Cells[ColumnIndex] as DataGridViewButtonCell;


            if (((Dictionary<string, dynamic>)cell.Tag)["Enabled"] != false)
            {
                //restore cell style to the default value
                //default style
                cell.Style.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableCellFore"];
                cell.Style.SelectionForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["SubTableSelectedCellFore"];
            }


        }

        //======================================================================================================================
        // Disable and Enable Cells in a CustomDataGridView



        internal static void DisableCellAtColAndRow(CustomDataGridView DGV, string ColKey, int RowIndex)
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
                if (cell.GetType() == typeof(CustomDataGridViewCheckBoxCell))
                {
                    CustomDataGridViewCheckBoxCell cbcell = (CustomDataGridViewCheckBoxCell)cell;
                    //cbcell.FlatStyle = FlatStyle.Popup;
                    cbcell.enabled = false;

                }


                //gray out the cell
                string currentTheme = ColorThemes.currentTheme;
                cell.Style.BackColor = ColorThemes.Themes[currentTheme]["Disabled"];
                cell.Style.ForeColor = ColorThemes.Themes[currentTheme]["Disabled"];
                cell.Style.SelectionBackColor = ColorThemes.Themes[currentTheme]["DisabledSelected"];
            }

        }

        internal static void EnableCellAtColAndRow(CustomDataGridView DGV, string ColKey, int RowIndex)
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
                if (cell.GetType() == typeof(CustomDataGridViewCheckBoxCell))
                {
                    CustomDataGridViewCheckBoxCell cbcell = (CustomDataGridViewCheckBoxCell)cell;
                    //cbcell.FlatStyle = FlatStyle.Standard;

                    cbcell.enabled = true;
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


                //if this cell is of a script receiver column: 
                if (cell.GetType() == typeof(DataGridViewButtonCell))
                {
                    string tableKey = ConvertDirToTableKey(DGV.Name);
                    Dictionary<int, Dictionary<string, dynamic>> tableData = GetTableDataFromDir(DGV.Name);
                    //UpdateReceiverCellUnfulfilledDependencyState() will confirm the column type
                    //set the UnfulfilledDependencyState of the cell
                    UpdateReceiverCellUnfulfilledDependencyState(tableKey, ColKey, tableData, RowIndex, DGV);
                }

                

            }

        }

        

        //I'd need to account for other potential disabler conditions that may still be disabling either of the two cells
        internal static bool IsDataRowCellStillDisabled(string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey, string[] columnDisablerArray)
        {
            //fetch disabler array if not already defined
            if (columnDisablerArray == null)
            {
                if (DatabaseFunct.currentData.ContainsKey(tableKey) && DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
                {
                    columnDisablerArray = currentData[tableKey][ColumnKey + ColumnDisablerArrayExt].ToArray();
                }
            }

            if (columnDisablerArray != null)
            {
                foreach (string disablerColumnKey in columnDisablerArray)
                {
                    //column can't disable itself
                    if (disablerColumnKey != ColumnKey)
                    {
                        if (DoesDataRowCellContainData(KVRow, disablerColumnKey))
                        {
                            return true;
                        }
                    }

                    
                }
            }
                
            
            return false;

        }

        //when a change is made to a cell of a column with a disabler array, update all cells in the same row of columns within the disabler array
        internal static void UpdateStatusOfAllRowCellsInDisablerArrayOfCell(CustomDataGridView DGV, string tableKey, KeyValuePair<int, Dictionary<string, dynamic>> KVRow, string ColumnKey, List<string[]> constructedTableDisablerArrays)
        {
            Console.WriteLine("in " + DGV.Name + " at row index " + KVRow.Key.ToString());
            Console.WriteLine("Checking Row Cells if Disabled:");


            string[] columnDisablerArray = null;

            if (constructedTableDisablerArrays != null) // use constructedTableDisablerArrays
            {

                columnDisablerArray = AutoTableConstructorScriptFunct.GetColumnDisablerArrayFromConstructedTableDisablerArrays(ColumnKey, constructedTableDisablerArrays);
                
                
            }
            else //source disablerArray from currentData
            {
                if (DatabaseFunct.currentData[tableKey].ContainsKey(ColumnKey + ColumnDisablerArrayExt))
                {
                    columnDisablerArray = currentData[tableKey][ColumnKey + ColumnDisablerArrayExt].ToArray();

                    
                }
            }


            if (columnDisablerArray != null)
            {
                foreach (string ColumnKeyToUpdate in columnDisablerArray)
                {
                    if (ColumnKeyToUpdate != ColumnKey) // this check is only for constructedTables
                    {
                        //leave null to fetch from tabel data when passed to IsDataRowCellStillDisabled()
                        string[] columnToUpdateDisablerArray = null;

                        if (constructedTableDisablerArrays != null)
                        {
                            //get columnDisablerArray relative to the column being updated
                            columnToUpdateDisablerArray = AutoTableConstructorScriptFunct.GetColumnDisablerArrayFromConstructedTableDisablerArrays(ColumnKeyToUpdate, constructedTableDisablerArrays);
                        }
                        


                        Console.WriteLine(ColumnKeyToUpdate);
                        bool isDisabled = IsDataRowCellStillDisabled(tableKey, KVRow, ColumnKeyToUpdate, columnToUpdateDisablerArray);
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
            List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);



            if (!isRemoveDisablerCondition)
            {
                //then disable the cells where there is conflict








                int tableIndex = 0;

                foreach (Dictionary<int, Dictionary<string, dynamic>> Table in TableDataWithRow)
                {
                    string DGVKeyOfTable = DGVKeysList[tableIndex];

                    CustomDataGridView openDGVOfTable = null;
                    //get openDGVOfTable
                    foreach (CustomDataGridView openDGV in openDGVs)
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
                                        Tuple<CustomDataGridView, int> openSubTableKey = new Tuple<CustomDataGridView, int>(openDGVOfTable, KVRow.Key);
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

                    CustomDataGridView openDGVOfTable = null;
                    //get openDGVOfTable
                    foreach (CustomDataGridView openDGV in openDGVs)
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
                            bool isDisabled1 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey1,null);
                            bool isDisabled2 = IsDataRowCellStillDisabled(tableKey, KVRow, selectedColKey2,null);
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
