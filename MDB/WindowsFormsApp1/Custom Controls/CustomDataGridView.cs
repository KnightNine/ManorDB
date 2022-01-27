using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Specialized;
using System.Drawing;

namespace MDB
{

    // This fixes an issue with arrow navigation not working in controls that are nested within the datagridview


    // This is a response on StackOverflow: https://stackoverflow.com/questions/43071129/handle-navigation-keys-in-textbox-inside-datagridview

    /*
    Apparently the problem is in DataGridView. It's because DataGridView overrides the Control.ProcessKeyPreview method:
    
    
    
    TextBox works just fine if not inside DataGridView (no problem at all when using the same method adding it into TreeView for example)

Apparently the problem is in DataGridView. It's because DataGridView overrides the Control.ProcessKeyPreview method:

This method is called by a child control when the child control receives a keyboard message. The child control calls this method before generating any keyboard events for the message. If this method returns true, the child control considers the message processed and does not generate any keyboard events.

The DataGridView implementation does just that - it maintains zero or one child controls internally (EditingControl), and when there is no such control active, it handles many keys (navigation, tab, enter, escape, etc.) by returning true, thus preventing the child TextBox keyboard events generation. The return value is controlled by the ProcessDataGridViewKey method.

Since the method is virtual, you can replace the DataGridView with a custom derived class which overrides the aforementioned method and prevents the undesired behavior when neither the view nor the view active editor (if any) has the keyboard focus.
    


    
    DataGridView intercepts another key message preprocessing infrastructure method - Control.ProcessDialogKey and handles Tab, Esc, Return, etc. keys there. So in order to prevent that, the method has to be overridden as well and redirected to the parent of the data grid view. The later needs a little reflection trickery to call a protected method, but using one time compiled delegate at least avoids the performance hit.

With that addition, the final custom class would be like this:
    
    
    */
    public class CustomDataGridView : DataGridView
    {
        bool SuppressDataGridViewKeyProcessing => ContainsFocus && !Focused &&
            (EditingControl == null || !EditingControl.ContainsFocus);

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (SuppressDataGridViewKeyProcessing) return false;
            return base.ProcessDataGridViewKey(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (SuppressDataGridViewKeyProcessing)
            {
                if (Parent != null) return DefaultProcessDialogKey(Parent, keyData);
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        static readonly Func<Control, Keys, bool> DefaultProcessDialogKey =
            (Func<Control, Keys, bool>)Delegate.CreateDelegate(typeof(Func<Control, Keys, bool>),
            typeof(Control).GetMethod(nameof(ProcessDialogKey), BindingFlags.NonPublic | BindingFlags.Instance));

        // <columnIndex,List<rowIndex>>
        private Dictionary<int,List<int>> invalidCellIndexes = new Dictionary<int,List<int>>();

        public void ClearInvalidCellIndexes()
        {
            invalidCellIndexes.Clear();
        }


        public void AddInvalidCellIndexes(int colIndex, int rowIndex)
        {
            if (!invalidCellIndexes.ContainsKey(colIndex))
            {
                invalidCellIndexes[colIndex] = new List<int>();
            }
            if (!invalidCellIndexes[colIndex].Contains(rowIndex))
            {
                invalidCellIndexes[colIndex].Add(rowIndex);
            }

            //force redraw cell
            //this.InvalidateCell(this.Rows[rowIndex].Cells[colIndex]);

        }
        public void RemoveInvalidCellIndexes(int colIndex, int rowIndex)
        {
            if (invalidCellIndexes.ContainsKey(colIndex))
            {
                invalidCellIndexes[colIndex].Remove(rowIndex);
                if (invalidCellIndexes[colIndex].Count == 0)
                {
                    invalidCellIndexes.Remove(colIndex);
                }
            }


            //force redraw cell
            //this.InvalidateCell(this.Rows[rowIndex].Cells[colIndex]);

        }

        //takes old ColumnOrder and new ColumnOrder and translates the invalidCellIndexes accordingly
        /*public void ShiftColumnsOfInvalidCellIndexes(List<string> oldColumnOrder, List<string> newColumnOrder)
        {
            


            Dictionary<int, List<int>> oldInvalidCellIndexes = GenericFunct.Clone(invalidCellIndexes);
            Dictionary<int, List<int>> newInvalidCellIndexes = new Dictionary<int, List<int>>();

            for (int i = 0; i < oldColumnOrder.Count; i++)
            {
                string colName = oldColumnOrder[i];
                int oldIndex = i;
                int newIndex = newColumnOrder.IndexOf(colName);
                if (oldInvalidCellIndexes.ContainsKey(oldIndex))
                {
                    Console.WriteLine("shifting outline at index "+ oldIndex.ToString() + " TO " + newIndex.ToString());
                    
                    newInvalidCellIndexes[newIndex] = oldInvalidCellIndexes[oldIndex];

                    
                    
                }

            }


            DelayedInvalidCellIndexUpdate(newInvalidCellIndexes);

            

            


        }*/

        public void ShiftRowsOfInvalidCellIndexes(List<int> oldRowOrder, List<string> newRowOrder)
        {

        }


        private void invalidateCells()
        {
            //force redraw
            foreach (DataGridViewRow row in this.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    this.InvalidateCell(cell);

                }
                
            }
        }









        
        private void CustomDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {


            CustomDataGridView senderDGV = (CustomDataGridView)sender;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (invalidCellIndexes.ContainsKey(e.ColumnIndex) && invalidCellIndexes[e.ColumnIndex].Contains(e.RowIndex))
                {
                    DataGridViewCell cell = senderDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);
                    using (Pen p = new Pen(ColorThemes.Themes[ColorThemes.currentTheme]["InvalidCellOutline"], 1))
                    {
                        Rectangle rect = e.CellBounds;
                        rect.Width -= 2;
                        rect.Height -= 2;
                        e.Graphics.DrawRectangle(p, rect);
                    }
                    e.Handled = true;
                }
            }

            


        }


        



        public CustomDataGridView()
        {
            this.CellPainting += CustomDataGridView_CellPainting;
            
            base.DoubleBuffered = true;

        }

        
    }
}
