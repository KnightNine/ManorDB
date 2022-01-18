/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace System.Windows.Forms
{

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleManorDBProvider : TabStyleProvider
    {
        public TabStyleManorDBProvider(CustomTabControl tabControl) : base(tabControl)
        {
            this._ImageAlign = ContentAlignment.MiddleRight;
            this._Overlap = 9;
            this._TextColor = Color.White;
            this._TextColorSelected = Color.White;
            this._TextColorDisabled = Color.WhiteSmoke;
            this._FocusColor = Color.LightGreen;
            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(14, 1);
        }

        public override void AddTabBorder(System.Drawing.Drawing2D.GraphicsPath path, System.Drawing.Rectangle tabBounds)
        {
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + tabBounds.Height - 4, tabBounds.Y + 2);
                    path.AddLine(tabBounds.X + tabBounds.Height, tabBounds.Y, tabBounds.Right - 3, tabBounds.Y);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Y, 6, 6, 270, 90);
                    path.AddLine(tabBounds.Right, tabBounds.Y + 3, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - 3);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
                    path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X + tabBounds.Height, tabBounds.Bottom);
                    path.AddLine(tabBounds.X + tabBounds.Height - 4, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 3, tabBounds.Bottom);
                    path.AddArc(tabBounds.X, tabBounds.Bottom - 6, 6, 6, 90, 90);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - 3, tabBounds.X, tabBounds.Y + tabBounds.Width);
                    path.AddLine(tabBounds.X + 2, tabBounds.Y + tabBounds.Width - 4, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + tabBounds.Width - 4);
                    path.AddLine(tabBounds.Right, tabBounds.Y + tabBounds.Width, tabBounds.Right, tabBounds.Bottom - 3);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
                    path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }

        protected override Brush GetTabBackgroundBrush(int index)
        {
            Brush fillBrush = null;

            //	Capture the colours dependant on selection state of the tab
            Color dark = Color.Black;
            Color light = Color.Gray;

            if (this._TabControl.SelectedIndex == index)
            {
                dark = Color.LightGreen;
                light = SystemColors.Window;
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = dark;
            }
            else if (this.HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                dark = Color.DimGray;
                light = dark;
            }

            fillBrush = new SolidBrush(dark);

            return fillBrush;
        }

        

        public override Brush GetPageBackgroundBrush(int index)
        {

            //	Capture the colours dependant on selection state of the tab
            Color light = Color.Transparent;

            if (this._TabControl.SelectedIndex == index)
            {
                light = Color.FromArgb(229, 195, 101);
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = Color.Transparent;
            }
            else if (this._HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = Color.Transparent;
            }

            return new SolidBrush(light);
        }

    }
}
