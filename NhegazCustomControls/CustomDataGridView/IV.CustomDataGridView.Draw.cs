using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;
            if (rows <= 0 || cols <= 0) return;

            if (LinesBetweenColumns && cols > 1)
            {
                using var pen = new Pen(BorderColor, LinesWidth);
                for (int c = 0; c < cols - 1; c++)
                {
                    var headerItem = HeaderLabels.GetItem(c);
                    int x = headerItem.Location.X + headerItem.Width;
                    e.Graphics.DrawLine(pen, new Point(x, BorderWidth), new Point(x, Bottom - BorderWidth));
                }
            }

            if (LinesBetweenRows && rows > 0)
            {
                using var pen = new Pen(BorderColor, LinesWidth);
                for (int r = 0; r < rows; r++)
                {
                    var firstCell = DataLabels.GetItem(r, 0);
                    int y = firstCell.Location.Y;
                    e.Graphics.DrawLine(pen, new Point(BorderWidth, y), new Point(Right, y));
                }
            }
        }
    }
}
