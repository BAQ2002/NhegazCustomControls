using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownMonth
    {
        protected override void AdjustControlSize()
        {
            Controls.Clear();

            if (MonthList == null || MonthList.Length == 0 || NumberOfColumns <= 0)
                return;

            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;

            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;

            Header.AdjustHeaderSize(Width - (2 * xPadding), itemUniformSize);
            Header.AdjustHeaderLocation(xPadding, yPadding);

            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = yPadding + (NumberOfRows * (itemUniformSize + yPadding));
            AdjustInnerLocations();
        }
        protected override void AdjustInnerLocations()
        {
            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;
            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;

            for (int row = 0; row < NumberOfRows; row++)
            {
                int y = yPadding + row * (itemUniformSize + yPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);

                    var label = MonthList[row, col];
                    label.Location = new Point(x, y);
                    label.Width = itemUniformSize;
                    label.Height = itemUniformSize;
                }
            }
        }

    }
}
