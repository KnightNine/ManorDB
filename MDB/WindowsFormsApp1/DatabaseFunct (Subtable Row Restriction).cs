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
        internal static void AddRowRestrictionToSubtableColumn(CustomDataGridView DGV, string ColKey, int restrictVal)
        {
            string tableKey = ConvertDirToTableKey(DGV.Name);
            string subTableKey = tableKey+"/"+ColKey;
            currentData[tableKey][ColKey+SubtableRowRestrictionExt] = restrictVal;

            //check if the rows in each subtable exceeds 1
            List<string> DGVKeysPool = new List<string>();
            List<Dictionary<int, Dictionary<string, dynamic>>> TableDataRowsAtSameLevel = GetAllTableDataAtTableLevel(subTableKey, ref DGVKeysPool);
            List<CustomDataGridView> openDGVs = GetAllOpenDGVsAtTableLevel(subTableKey);




            //remove all but [restrictVal] row(s) from openDGVs
            loadingTable = true;
            foreach (CustomDataGridView openDGV in openDGVs)
            {

                int i = openDGV.Rows.Count - 1;

                while (i > restrictVal-1)
                {
                    //close open subtables of rowIndex i
                    if (Program.openSubTables.ContainsKey(new Tuple<CustomDataGridView, int>(openDGV, i)))
                    {

                        RemoveSubtableFromOpenSubtables(new Tuple<CustomDataGridView, int>(openDGV, i));
                    }


                    //remove row
                    openDGV.Rows.RemoveAt(i);

                    i -= 1;

                }
                //update the addRow button for openDGV
                UpdateSubTableAddRowButton(openDGV);
                

            }
            // recenter subtables
            Program.mainForm.RecenterSubTables();

            loadingTable = false;

            //remove all but [restrictVal] row(s) from TableDataRowsAtSameLevel 

            foreach (Dictionary<int, Dictionary<string, dynamic>> tableData in TableDataRowsAtSameLevel)
            {

                int i = tableData.Count - 1;
                while (i > restrictVal-1)
                {
                    tableData.Remove(i);
                    i -= 1;
                }
            }

        }

        internal static void RemoveRowRestrictionFromSubtableColumn(CustomDataGridView DGV, string ColKey)
        {
            string tableKey = ConvertDirToTableKey(DGV.Name);
            currentData[tableKey].Remove(ColKey + SubtableRowRestrictionExt);

        }

        internal static bool DoesSubTableMeetOrExceedRowRestriction(CustomDataGridView subDGV)
        {

            int restrictVal = 0;

            //if the DGV is a constructed table, check if script starts with #
            Dictionary<string, dynamic> DGVTag = subDGV.Tag as Dictionary<string, dynamic>;
            if (DGVTag.ContainsKey("tableConstructorScript"))
            {
                string tableConstructorScript = DGVTag["tableConstructorScript"] as string;
                if (tableConstructorScript.StartsWith("#"))
                {
                    //if so, restrict to 1 row
                    restrictVal = 1;


                }
            }
            else
            {
                string tableKey = ConvertDirToTableKey(subDGV.Name);

                //need to check whether or not the subtable exceeds the limit
                //get the tableKey of the parent table (this removes everything including and after the last slash)
                string parentTableKey = Regex.Replace(tableKey, @"/[^/]*$", "");
                // this gets all text after the last slash
                string colKey = Regex.Match(tableKey, @"[^\/]+\/?$").Value;

                //if restriction exists
                if (currentData[parentTableKey].ContainsKey(colKey + SubtableRowRestrictionExt))
                {

                    restrictVal = currentData[parentTableKey][colKey + SubtableRowRestrictionExt];

                    
                }

                
            }

            //if number of rows are equal or above restrictVal
            if (restrictVal != 0 && subDGV.Rows.Count >= restrictVal) 
            {
                return true;
            }

            return false;

        }

        

        internal static void UpdateSubTableAddRowButton(CustomDataGridView subDGV)
        {
            Console.WriteLine("updating AddRowButton");
            

            if (DoesSubTableMeetOrExceedRowRestriction(subDGV)) //indicate rows are full and disable button
            {
                Button newRowButton = subDGV.Controls.Find("newRowButton", true)[0] as Button;
                    
                newRowButton.Text = "Rows Restricted";
                newRowButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["DisabledText"];
                newRowButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["Disabled"];

                // remove the event triggers since disabling the button removes the forecolor
                Dictionary<string, dynamic> buttonTag = newRowButton.Tag as Dictionary<string,dynamic>;
                bool isEnabled = buttonTag["Enabled"];

                if (isEnabled)
                {
                    newRowButton.Click -= new System.EventHandler(Program.mainForm.subTableNewRowButton_Click);
                    buttonTag["Enabled"] = false;
                }
                

                    



            }
            else // set to default
            {
                Button newRowButton = subDGV.Controls.Find("newRowButton", true)[0] as Button;
                    
                newRowButton.Text = "New Row";
                newRowButton.ForeColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseFore"];
                newRowButton.BackColor = ColorThemes.Themes[ColorThemes.currentTheme]["ElseBack"];

                Dictionary<string, dynamic> buttonTag = newRowButton.Tag as Dictionary<string, dynamic>;
                bool isEnabled = buttonTag["Enabled"];
                if (!isEnabled)
                {
                    //re-add the click event trigger
                    newRowButton.Click += new System.EventHandler(Program.mainForm.subTableNewRowButton_Click);
                    buttonTag["Enabled"] = true;
                }
               

            }

                
                



            

            
        }

    }
}