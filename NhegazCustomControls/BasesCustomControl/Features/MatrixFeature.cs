using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace NhegazCustomControls
{
    public class MatrixFeature
    {
        private readonly CustomControl ownerControl;

        private readonly InnerControl[,] Matrix;
        public MatrixFeature(CustomControl owner, InnerControl[,] matrix)
        {
            Matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public void OnItemMouseEnter(object? sender, EventArgs e)
        {
            var item = (InnerControl)sender!;
            item.ForeColor = ownerControl.BackgroundColor;         // texto "inverso"
            item.BackgroundColor = ownerControl.HoverColor; // highlight de fundo
            ownerControl.Invalidate();
        }

        public void OnItemMouseLeave(object? sender, EventArgs e, Color? foreColor = null)
        {
            var item = (InnerControl)sender!;
            item.ForeColor = foreColor ?? ownerControl.ForeColor;
            item.BackgroundColor = ownerControl.BackgroundColor;
            ownerControl.Invalidate();
        }

        public void AdjustItemSize(int row, int col, int itemWidth, int ItemHeight)
        {
            var item = Matrix[row, col];
            item.Width = itemWidth;
            item.Height = ItemHeight;
        }

        public void AdjustItemLocation(int row, int col, int x, int y)
        {
            var item = Matrix[row, col];
            item.SetLocation(x, y);
        }
    }
}
