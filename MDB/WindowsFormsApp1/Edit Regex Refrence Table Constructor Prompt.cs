using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace MDB
{
    public partial class EditRegexRefrenceTableConstructorPrompt : Form
    {

        



        public EditRegexRefrenceTableConstructorPrompt()
        {
            InitializeComponent();
        }

        private void addColumnButton_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;


            string error = "";

            //PrimaryKey and AutoTableConstructor checkboxes can't be both checked
            if (isPrimaryKeyCheckBox.Checked && isAutoTableConstructorScriptCheckBox.Checked)
            {
                error += "The PrimaryKey Column Can't also be the AutoTableConstructorScript Column.\n";
            }
            if (RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnDataRefrence].ContainsKey(columnNameTextBox.Text))
            {
                error += "The column name \"" + columnNameTextBox.Text + "\" already exists.\n";
            }

            //validate columnNameTextBox.Text
            string colNameError = GenericFunct.ValidateNameInput(columnNameTextBox.Text);
            if (colNameError != "")
            {
                error += "Column name " + colNameError;
            }
             

            
            

            //if valid
            if (error == "")
            {

                //add row using input control data
                addRowToDataGridViewControl(columnNameTextBox.Text, isPrimaryKeyCheckBox.Checked, isAutoTableConstructorScriptCheckBox.Checked, regexTextBox.Text);
                //add to column order
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnOrderRefrence].Add(columnNameTextBox.Text);

                //add to RegexRefrenceTableConstructorData
                
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnDataRefrence].Add( columnNameTextBox.Text , new Dictionary<string, dynamic>
                {
                    { "isPK", isPrimaryKeyCheckBox.Checked },
                    { "isATCS", isAutoTableConstructorScriptCheckBox.Checked },
                    { "regex", regexTextBox.Text }
                }
                );
                //empty all input controls
                columnNameTextBox.Text = "";
                isPrimaryKeyCheckBox.Checked = false;
                isAutoTableConstructorScriptCheckBox.Checked = false;
                regexTextBox.Text = "";

            }
            else
            {
                MessageBox.Show(error);
            }
        }

        private string GetPathRelativeToSelectedPath(string path)
        {
            //get the path relative to the application
            Uri mainDir = new Uri(InputOutput.selectedPath + "\\");

            Uri absoluteFileDirectoryPath = new Uri(path);

            Uri diff = mainDir.MakeRelativeUri(absoluteFileDirectoryPath);

            Console.WriteLine("Getting Path: \"" + path + "\" relative to path: \"" + InputOutput.selectedPath + "\\\"");

            //converting to Uri replaces characters like spaces with character codes so UrlDecode() is needed to undo this change
            string relativePath = HttpUtility.UrlDecode(diff.OriginalString);
            // flip slashes to backslashes 
            relativePath = relativePath.Replace("/", "\\");

            return relativePath;
        }


        private void fileDirectoryButton_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrWhiteSpace(InputOutput.selectedPath))
            {
                MessageBox.Show("selected filepath is undefined, load or save a file first");
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All Files (*.*)|*.*"; 
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = InputOutput.selectedPath;


            

            //openFileDialog1.FileName = fileName;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                string relativePath = GetPathRelativeToSelectedPath(openFileDialog1.FileName);

                //show path in the textbox
                fileDirectoryTextBox.Text = relativePath;

                 


                //store path
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.FileDirectoryRefrence] = relativePath;


            }
        }

        private void folderDirectoryButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(InputOutput.selectedPath))
            {
                MessageBox.Show("selected filepath is undefined, load or save a file first");
                return;
            }


            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = InputOutput.selectedPath;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string relativePath = GetPathRelativeToSelectedPath(folderBrowserDialog1.SelectedPath);

                //show path in the textbox
                fileDirectoryTextBox.Text = relativePath;




                //store path
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.FileDirectoryRefrence] = relativePath;
            }
        }








        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            //delete the row if its header is clicked
            CustomDataGridView senderDGV = sender as CustomDataGridView;
            if (e.Button == MouseButtons.Left)
            {
                
                Console.WriteLine("removing row: " + e.RowIndex.ToString());

                DataGridViewRow deletedRow = dataGridView.Rows[e.RowIndex];
                string colName = deletedRow.Cells[dataGridView.Columns.IndexOf(dataGridView.Columns["columnNameColumn"])].Value as string;

                //re enable row input control checkboxes of the restricted column types
                if (RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnDataRefrence][colName]["isPK"])
                {
                    isPrimaryKeyCheckBox.Enabled = true;
                }
                

                //delete the column from RegexRefrenceTableConstructorData
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnDataRefrence].Remove(colName);
                //remove from column order
                RegexRefrenceTableConstructorPromptHandler.RegexRefrenceTableConstructorData[RegexRefrenceTableConstructorPromptHandler.ColumnOrderRefrence].RemoveAt(e.RowIndex);


                dataGridView.Rows.RemoveAt(e.RowIndex);



                
            }
        }

        private void dataGridView_RowPostPaint
        (object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Set row headers to indicate that they delete the row
            //give row headers red text that says delete, and a grey background
            


            //Convert the image to icon, in order to load it in the row header column
            /*
            Bitmap myBitmap = new Bitmap(imageList1.Images[0]);
            Icon myIcon = Icon.FromHandle(myBitmap.GetHicon());

            Graphics graphics = e.Graphics;

            //Set Image dimension - User's choice
            int iconHeight = 14;
            int iconWidth = 14;

            //Set x/y position - As the center of the RowHeaderCell
            int xPosition = e.RowBounds.X + (dataGridView1.RowHeadersWidth / 2);
            int yPosition = e.RowBounds.Y +
            ((dataGridView1.Rows[e.RowIndex].Height - iconHeight) / 2);

            Rectangle rectangle = new Rectangle(xPosition, yPosition, iconWidth, iconHeight);
            graphics.DrawIcon(myIcon, rectangle); */
        }





        private void constructTableButton_Click(object sender, EventArgs e)
        {
            RegexRefrenceTableConstructorPromptHandler.ConstructRegexRefrenceTable();
        }















        public void addRowToDataGridViewControl(string colName, bool isPK, bool isATCS, string regex)
        {

            //disable the checkbox of rows of columns that are restricted to one entry
            if (isPK)
            {
                isPrimaryKeyCheckBox.Enabled = false;
            }
            


            //add the row to the DGV
            dataGridView.Rows.Add();


            int rowIndex = dataGridView.Rows.Count - 1;

            //assign new data to the columnRow (row that represents a column of the constructed table)
            DataGridViewRow columnRow = dataGridView.Rows[dataGridView.Rows.Count - 1];



            //Set row headers to indicate that they delete the row
            DataGridViewRowHeaderCell RHCell = dataGridView.Rows[rowIndex].HeaderCell;
            RHCell.Value = "Delete";


            //column name
            DataGridViewTextBoxCell columnNameCell = columnRow.Cells[dataGridView.Columns.IndexOf(columnNameColumn)] as DataGridViewTextBoxCell;
            columnNameCell.Value = colName;

            //is Primary Key column
            DataGridViewCheckBoxCell isPKCell = columnRow.Cells[dataGridView.Columns.IndexOf(isPrimaryKeyColumn)] as DataGridViewCheckBoxCell;
            ColumnTypes.setBoolCellTFVals(isPKCell);


            //set checkbox state
            var cellVal = isPKCell.FalseValue;
            if (isPK)
            {
                cellVal = isPKCell.TrueValue;
            }
            isPKCell.Value = cellVal;
            


            //is Auto Table Constructor Script column
            DataGridViewCheckBoxCell isATCSCell = columnRow.Cells[dataGridView.Columns.IndexOf(isAutoTableConstructorScriptColumn)] as DataGridViewCheckBoxCell;
            ColumnTypes.setBoolCellTFVals(isATCSCell);

            //set checkbox state
            cellVal = isATCSCell.FalseValue;
            if (isATCS)
            {
                cellVal = isATCSCell.TrueValue;
            }

            isATCSCell.Value = cellVal;

            //Regex 
            DataGridViewTextBoxCell regexCell = columnRow.Cells[dataGridView.Columns.IndexOf(regexColumn)] as DataGridViewTextBoxCell;
            regexCell.Value = regex;
        }

        
    }
}
