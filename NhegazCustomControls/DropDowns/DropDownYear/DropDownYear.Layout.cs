using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownYear
    {
        public override void AdjustControlSize()
        {

            if (YearItems == null || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            int xPadding = InnerHorizontalPadding;
            int yPadding = InnerVerticalPadding;


            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;
            int headerHeight = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Height;

            int startY = yPadding + headerHeight + yPadding;
          
            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = startY + (NumberOfRows * (itemUniformSize + yPadding));
            
            Header.SetSize(Width - BorderHorizontalBoundsSum ,headerHeight);
            Header.SetLocation(RelativeLeftX(), RelativeTopY());

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);
                    int y = startY + row * (itemUniformSize + yPadding);

                    YearItems.SetItemSize(row, col, itemUniformSize, itemUniformSize);
                    YearItems.SetItemLocation(row, col, x, y);
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

        protected override void AdjustInnerLocations()
        {
            BackwardIcon.SetLocation(Header.RelativeLeftX(), Header.RelativeCenterY(BackwardIcon));

            ForwardIcon.SetLocation(Header.RelativeRightX(ForwardIcon), Header.RelativeCenterY(ForwardIcon));

            DecadeLabel.SetLocation(Header.RelativeCenterX(DecadeLabel), Header.RelativeCenterY(DecadeLabel));

        }
    }
}
