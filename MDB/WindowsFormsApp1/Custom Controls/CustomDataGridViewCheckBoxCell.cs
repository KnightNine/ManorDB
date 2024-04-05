using System.Drawing;
using System;
using System.Windows.Forms;

namespace MDB
{
    public class CustomDataGridViewCheckBoxCell : DataGridViewCheckBoxCell
    {
        private Rectangle curCellBounds;
        private Rectangle checkBoxRect;

        public CustomDataGridViewCheckBoxCell() : base() { }

        protected override void Paint(
            Graphics g,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates elementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // Paint default except the check box parts.
            var parts = paintParts & ~(DataGridViewPaintParts.ContentForeground
                | DataGridViewPaintParts.ContentBackground);

            base.Paint(g,
                clipBounds,
                cellBounds,
                rowIndex,
                elementState,
                value,
                formattedValue,
                errorText,
                cellStyle,
                advancedBorderStyle,
                parts);

            if (curCellBounds != cellBounds)
            {
                // To get the box size...
                var col = OwningColumn as CustomDataGridViewCheckBoxColumn;

                curCellBounds = cellBounds;
                // ToDo: Use col.DefaultCellStyle.Alignment or
                // DataGridView.ColumnHeadersDefaultCellStyle.Alignment
                // to position the box. MiddleCenter here...
                checkBoxRect = new Rectangle(
                    (cellBounds.Width - col.CheckBoxSize.Width) / 2 + cellBounds.X,
                    (cellBounds.Height - col.CheckBoxSize.Height) / 2 + cellBounds.Y,
                    col.CheckBoxSize.Width,
                    col.CheckBoxSize.Height);
            }

            ControlPaint.DrawCheckBox(g, checkBoxRect, (bool)formattedValue
                ? ButtonState.Checked | ButtonState.Flat
                : ButtonState.Flat);


        }

        // In case you don't use the `Alignment` property to position the 
        // box. This is to disallow toggling the state if you click on the
        // original content area outside the drawn box.
        protected override void OnContentClick(DataGridViewCellEventArgs e)
        {
            if (!ReadOnly &&
                checkBoxRect.Contains(DataGridView.PointToClient(Cursor.Position)))
                base.OnContentClick(e);
        }

        protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
        {
            if (!ReadOnly &&
                checkBoxRect.Contains(DataGridView.PointToClient(Cursor.Position)))
                base.OnContentDoubleClick(e);
        }

        // Toggle the checked state by mouse clicks...
        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (!ReadOnly && e.Button == MouseButtons.Left &&
                checkBoxRect.Contains(DataGridView.PointToClient(Cursor.Position)))
            {
                Value = Value == null || !Convert.ToBoolean(Value);
                DataGridView.RefreshEdit();
                DataGridView.NotifyCurrentCellDirty(true);
            }
        }

        // ... and Space key...
        protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
        {
            base.OnKeyDown(e, rowIndex);
            if (!ReadOnly && e.KeyCode == Keys.Space)
            {
                Value = Value == null || !Convert.ToBoolean(Value.ToString());
                DataGridView.RefreshEdit();
                DataGridView.NotifyCurrentCellDirty(true);
            }
        }
    }
}