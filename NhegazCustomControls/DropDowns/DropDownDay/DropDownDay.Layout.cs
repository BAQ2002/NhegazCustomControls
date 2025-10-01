using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    public partial class DropDownDay
    {
        public override void AdjustControlSize()
        {
            if (DayItems == null || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            Size itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f);

            int DayItemsOcupationWidth = NumberOfColumns * (itemUniformSize.Height + InnerHorizontalPadding) - InnerHorizontalPadding;

            int DayItemsOcupationHeight = (NumberOfRows+1) * (itemUniformSize.Height + InnerVerticalPadding) - InnerVerticalPadding;

            Width = BorderHorizontalBoundsSum + DayItemsOcupationWidth;
            Height = BorderVerticalBoundsSum + DayItemsOcupationHeight;

            AdjustInnerSizes(); AdjustInnerLocations();

            
        }
      
        protected override void AdjustInnerSizes()
        {
            Size itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f);

            Header.SetSize(Width - BorderHorizontalBoundsSum, itemUniformSize.Height);

            BackwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            BackwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            ForwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            ForwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;
          
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    if (row == 0)
                    {
                        WeekDays.SetItemSize(col, itemUniformSize.Height, itemUniformSize.Height);
                    }

                    DayItems.SetItemSize(row, col, itemUniformSize.Height, itemUniformSize.Height);
                }
            }
        }

        protected override void AdjustInnerLocations()
        {
            Header.SetLocation(RelativeLeftX(), RelativeTopY());
            
            MonthLabel.SetLocation(Header.RelativeCenterX(MonthLabel), Header.RelativeCenterY(MonthLabel));
            ForwardIcon.SetLocation(Header.RelativeRightX(ForwardIcon), Header.RelativeCenterY(ForwardIcon));
            BackwardIcon.SetLocation(Header.RelativeLeftX(), Header.RelativeCenterY(BackwardIcon));

            int weekDaysY = Header.Bottom + InnerVerticalPadding;
            Size itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f);

            for (int row = 0; row < NumberOfRows; row++)
            {
                int dayItemsY = weekDaysY + row * (itemUniformSize.Height + InnerVerticalPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = ContentLeftBound + col * (itemUniformSize.Height + InnerHorizontalPadding);

                    if (row == 0)
                    {
                        WeekDays.SetItemLocation(col, x, weekDaysY);
                    }

                    DayItems.SetItemLocation(row, col, x, dayItemsY);
                }
            }
        }
    }
}
