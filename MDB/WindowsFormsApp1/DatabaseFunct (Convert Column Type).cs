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



        //functions to help integrate the New Integer column type to existing databases:  
        internal static void ConvertNumericalColumnToIntegerColumn(string colName, CustomDataGridView DGV)
        {
            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);
            currentData[tableKey][colName] = "Integer";
            //change all entries to Ints
            List<string> DGVKeysPool = new List<string>();
            List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysPool);
            List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(tableKey);


            //change all Numeric entries to integers in open table(s) with this column

            //enable loadingTable to stop value changed triggers since i'm changing the data in TableDataRowsAtSameLevel as well
            loadingTable = true;
            int columnIndex = DGV.Columns.IndexOf(DGV.Columns[colName]);
            foreach (CustomDataGridView openDGV in openDGVs)
            {
                
                foreach (DataGridViewRow row in openDGV.Rows)
                {
                    if (row.Cells[columnIndex].Value != null)
                    {
                        //convert display val to int
                        double displayValue = (double)row.Cells[columnIndex].Value;
                        row.Cells[columnIndex].Value = (int)displayValue;
                    }

                }



            }
            loadingTable = false;

            //change all data to int 

            foreach (Dictionary<int, Dictionary<string, dynamic>> tableData in TableDataRowsAtSameLevel)
            {


                foreach (KeyValuePair<int, Dictionary<string, dynamic>> rowEntryData in tableData)
                {
                    if (rowEntryData.Value != null)
                    {
                        double val = rowEntryData.Value[colName];
                        rowEntryData.Value[colName] = (int)val;
                    }


                }
            }
        }

        internal static void ConvertIntegerColumnToNumericalColumn(string colName, DataGridView DGV)
        {

            string tableDir = DGV.Name;
            string tableKey = ConvertDirToTableKey(tableDir);
            currentData[tableKey][colName] = "Numerical";
            //change all entries to Ints
            List<string> DGVKeysPool = new List<string>();
            List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(tableKey, ref DGVKeysPool);

            //change all data to double 
            foreach (Dictionary<int, Dictionary<string, dynamic>> tableData in TableDataRowsAtSameLevel)
            {


                foreach (KeyValuePair<int, Dictionary<string, dynamic>> rowEntryData in tableData)
                {
                    int val = rowEntryData.Value[colName];
                    rowEntryData.Value[colName] = (double)val;

                }
            }
        }
    }
}