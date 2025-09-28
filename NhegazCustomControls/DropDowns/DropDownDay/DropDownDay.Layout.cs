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
            if (DayItems == null || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            AdjustPadding();

            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;

            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = yPadding + ((NumberOfRows + 2) * (itemUniformSize + yPadding));

            Header.SetSize(Width - (2 * xPadding), itemUniformSize);
            Header.SetLocation(xPadding, yPadding);
            
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
                        WeekDays.SetItemSize(col, itemUniformSize, itemUniformSize);
                        WeekDays.SetItemLocation(col, x, weekDayY);
                    }

                    DayItems.SetItemSize(row, col, itemUniformSize, itemUniformSize);
                    DayItems.SetItemLocation(row, col, x, y);
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
        protected override void AdjustInnerLocations()
        {
            int pos = Header.Y + (Header.Height - NhegazSizeMethods.FontUnitSize(Font).Height)/2;
            BackwardIcon.SetLocation(HorizontalPadding, VerticalPadding);
            ForwardIcon.SetLocation(Width - (ForwardIcon.Width + HorizontalPadding), VerticalPadding);
            MonthLabel.SetLocation((Width - MonthLabel.Width) / 2, pos);
        }
    }
}
