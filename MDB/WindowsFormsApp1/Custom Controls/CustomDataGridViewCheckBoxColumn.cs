using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDB
{
    public class CustomDataGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public CustomDataGridViewCheckBoxColumn() : base() =>
        CellTemplate = new CustomDataGridViewCheckBoxCell();

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CustomDataGridViewCheckBoxCell)))
                    throw new InvalidCastException("CustomDataGridViewCheckBoxCell.");

                base.CellTemplate = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Size), "32, 32")]
        [Description("The size of the check box.")]
        public Size CheckBoxSize { get; set; } = new Size(32, 32);

        // We should copy the new properties.
        public override object Clone()
        {
            var c = base.Clone() as CustomDataGridViewCheckBoxColumn;
            c.CheckBoxSize = CheckBoxSize;
            return c;
        }
    }
}
