using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownDay
    {
        protected override void AdjustControlSize()
        {
            if (DayItemLabels == null || DayItemLabels.Length == 0 || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            AdjustPadding();

            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;

            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = yPadding + ((NumberOfRows + 2) * (itemUniformSize + yPadding));

            Header.AdjustHeaderSize(Width - (2 * xPadding), itemUniformSize);
            Header.AdjustHeaderLocation(xPadding, yPadding);
            
            AdjustInnerSizes(); AdjustInnerLocations();

            int weekDayY = (2 * yPadding) + ForwardIcon.Height;
            int baseGridY = weekDayY + itemUniformSize + yPadding;

            for (int row = 0; row < NumberOfRows; row++)
            {
                int y = baseGridY + row * (itemUniformSize + yPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);

                    if (row == 0)
                    {
                        AdjustVectorItemsSizes(col, itemUniformSize, itemUniformSize);
                        AdjustVectorItemsLocations(col, x, weekDayY);
                    }

                    AdjustMatrixItemsSizes(row, col, itemUniformSize, itemUniformSize);
                    AdjustMatrixItemsLocations(row, col, x, y);
                }
            }

        }
        protected override void AdjustInnerSizes()
        {
            BackwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            BackwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            ForwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            ForwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

        }
        protected override void AdjustVectorItemsSizes(int index, int itemWidth, int ItemHeight)
        {
            var label = WeekDayLabels[index];
            label.Width = itemWidth;
            label.Height = ItemHeight;
        }
        protected override void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int ItemHeight)
        {
            var label = DayItemLabels[row, col];
            label.Width = itemWidth;
            label.Height = ItemHeight;
        }

        protected override void AdjustInnerLocations()
        {
            int pos = Header.HeaderBounds.Y + (Header.HeaderBounds.Height - NhegazSizeMethods.FontUnitSize(Font).Height)/2;
            BackwardIcon.SetLocation(HorizontalPadding, VerticalPadding);
            ForwardIcon.SetLocation(Width - (ForwardIcon.Width + HorizontalPadding), VerticalPadding);
            MonthLabel.SetLocation((Width - MonthLabel.Width) / 2, pos);
        }
        protected override void AdjustVectorItemsLocations(int index, int x, int y)
        {
            var label = WeekDayLabels[index];
            label.SetLocation(x, y);
        }
        protected override void AdjustMatrixItemsLocations(int row, int col, int x, int y)
        {
            var label = DayItemLabels[row, col];
            label.SetLocation(x, y);
        }
    }
}
