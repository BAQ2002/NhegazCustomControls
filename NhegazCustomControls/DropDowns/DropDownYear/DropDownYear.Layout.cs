using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownYear
    {
        protected override void AdjustControlSize()
        {
            AdjustPadding();

            if (YearItemsLabels == null || YearItemsLabels.Length == 0 || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;

            
            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;
            int headerHeight = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Height;

            int startY = yPadding + headerHeight + yPadding;
          
            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = startY + (NumberOfRows * (itemUniformSize + yPadding));

            Header.SetSize(Width - (2 * xPadding), headerHeight);
            Header.SetLocation(xPadding, yPadding);

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);
                    int y = startY + row * (itemUniformSize + yPadding);

                    AdjustMatrixItemsSizes(row, col, itemUniformSize, itemUniformSize);
                    AdjustMatrixItemsLocations(row, col, x, y);
                }
            }
            AdjustInnerSizes();
            AdjustInnerLocations();
        }

        protected override void AdjustInnerSizes()
        {
            BackwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            BackwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            ForwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            ForwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;
        }
        protected override void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int itemHeight)
        {
            var label = YearItemsLabels[row, col];
            label.Width = itemWidth;
            label.Height = itemHeight;
        }
        protected override void AdjustInnerLocations()
        {
            int pos = Header.Y + (Header.Height - DecadeLabel.Height) / 2;
            BackwardIcon.SetLocation(HorizontalPadding, VerticalPadding);
            ForwardIcon.SetLocation(Width - (ForwardIcon.Width + HorizontalPadding), VerticalPadding);
            DecadeLabel.SetLocation((Width - DecadeLabel.Width) / 2, pos);

        }

        protected override void AdjustMatrixItemsLocations(int row, int col, int x, int y)
        {
            var label = YearItemsLabels[row, col];
            label.Location = new Point(x, y);
        }
    }
}
