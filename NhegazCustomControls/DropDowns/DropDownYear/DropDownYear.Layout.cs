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

            int yearItemSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;
            int headerHeight = NhegazSizeMethods.TextExactSize("0000", Font).Height;

            int startY = yPadding + headerHeight + yPadding;


            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (yearItemSize + xPadding);
                    int y = startY + row * (yearItemSize + yPadding);

                    AdjustMatrixItemsSizes(row, col, yearItemSize, yearItemSize);
                    AdjustMatrixItemsLocations(row, col, x, y);
                }
            }

            Width = xPadding + (NumberOfColumns * (yearItemSize + xPadding));
            Height = startY + (NumberOfRows * (yearItemSize + yPadding));
            AdjustInnerSizes();
            AdjustInnerLocations();
            AdjustHeaderSize(Width - (2 * xPadding), headerHeight);
            AdjustHeaderLocation(xPadding, yPadding);

        }
        protected override void AdjustInnerSizes()
        {
            ForwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            BackwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
        }
        protected override void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int itemHeight)
        {
            var label = YearItemsLabels[row, col];
            label.Width = itemWidth;
            label.Height = itemHeight;
        }
        protected override void AdjustInnerLocations()
        {
            BackwardIcon.Location = new Point(HorizontalPadding, VerticalPadding);
            ForwardIcon.Location = new Point(Width - (ForwardIcon.Width + HorizontalPadding), VerticalPadding);
            DecadeLabel.Location = new Point((Width - DecadeLabel.Width) / 2, VerticalPadding);
        }
        protected override void AdjustMatrixItemsLocations(int row, int col, int x, int y)
        {
            var label = YearItemsLabels[row, col];
            label.Location = new Point(x, y);
        }
    }
}
