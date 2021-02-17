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

            {"Primary Key", typeof(DataGridViewTextBoxColumn)},
            {"Text",typeof(DataGridViewTextBoxColumn) },
            {"Numerical",typeof( DataGridViewTextBoxColumn) },
            {"SubTable",typeof(DataGridViewButtonColumn) },
            {"Foreign Key Refrence",typeof(DataGridViewComboBoxColumn) },
            {"Bool",typeof(DataGridViewCheckBoxColumn) },
            {"Parent Subtable Foreign Key Refrence",typeof(DataGridViewComboBoxColumn) }

        };

        internal static DataGridViewCheckBoxCell setBoolCellTFVals(DataGridViewCheckBoxCell cell)
        {
            cell.FalseValue = false;
            cell.TrueValue = true;

            return cell;
        }

        internal static string GetSubTableCellDisplay(Dictionary<int, Dictionary<string, dynamic>> subtableData,string columnName, string tableKey)
        {
            var buttonText = columnName + "{\n";

            int i = 0;
            int displayMax = 5;
            while (i < subtableData.Count() && i < displayMax)
            {
                string rowText = "";
                string subTableKey = tableKey + "/" + columnName;

                foreach (string K in DatabaseFunct.currentData[subTableKey][DatabaseFunct.ColumnOrderRefrence])
                {

                    dynamic V = subtableData[i][K];
                    Type Vt = typeof(int);
                    if (V != null)
                    {
                        Vt = V.GetType();
                    }

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

        internal static dynamic ValidateCellInput(DataGridViewCellEventArgs e, DataGridView DGV)
        {

            string tableDir = DGV.Name;
            string tableKey = DatabaseFunct.ConvertDirToTableKey(tableDir);

            Dictionary<int, Dictionary<string, dynamic>> tableData = DatabaseFunct.GetTableDataFromDir(tableDir) as Dictionary<int, Dictionary<string, dynamic>>;

            dynamic value = DGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            string colName = DGV.Columns[e.ColumnIndex].Name;
            var colType = DatabaseFunct.currentData[tableKey][colName];
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

            return value;
        }
    }
}
