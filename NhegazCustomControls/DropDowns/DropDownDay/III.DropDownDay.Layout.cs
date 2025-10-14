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
        public override void UpdateLayout()
        {
            if (DayItems == null || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            base.UpdateLayout();
        }

        public override Size GetContentSize()
        {
            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference
                            ("00", Font, 1.5f, ReferenceDimension.Width);

            int headerHeight = itemSize.Height;                   //Altura do cabeçalho
            int weekDaysHeight = itemSize.Height;                 //Altura do WeekDays

            int NumberOfHorizontalPaddings = NumberOfColumns - 1; //Quantidade de Gaps Horizontais
            int NumberOfVerticalPaddings = NumberOfRows + 1;      //Quantidade de Gaps Verticais

            int contentWidth = NumberOfColumns * itemSize.Height; //Largura do YearItems

            int contentHeight = headerHeight                      //Altura do cabeçalho
                              + weekDaysHeight                    //Altura do WeekDays
                              + NumberOfRows * itemSize.Width;    //Altura do YearItems

            return new Size(contentWidth, contentHeight);
        }

        public override Size GetPaddingSize()
        {
            int NumberOfHorizontalPaddings = NumberOfColumns - 1;                   //Quantidade de Gaps Horizontais
            int NumberOfVerticalPaddings = NumberOfRows + 1;                        //Quantidade de Gaps Verticais

            int paddingWidth = BorderHorizontalBoundsSum                            //Padding horizontal das bordas 
                             + NumberOfHorizontalPaddings * InnerHorizontalPadding; //Paddings entre todos

            int paddingHeight = BorderVerticalBoundsSum                             //Padding vertical das bordas 
                              + NumberOfVerticalPaddings * InnerVerticalPadding;    //Paddings entre todos

            return new Size(paddingWidth, paddingHeight);
        }
 
        protected override void SetInnerSizes()
        {
            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference("00", Font, 1.5f, ReferenceDimension.Width);

            Header.SetSize(Width - BorderHorizontalBoundsSum, itemSize.Height);
            BackwardIcon.SetSize(itemSize);
            ForwardIcon.SetSize(itemSize);

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    if (row == 0)
                    {
                        WeekDays.SetItemSize(col, itemSize);
                    }

                    DayItems.SetItemSize(row, col, itemSize);
                }
            }
        }

        protected override void SetInnerLocations()
        {
            Header.SetLocation(RelativeLeftX(), RelativeTopY());
            
            MonthLabel.SetLocation(Header.RelativeCenterX(MonthLabel), Header.RelativeCenterY(MonthLabel));
            ForwardIcon.SetLocation(Header.RelativeRightX(ForwardIcon), Header.RelativeCenterY(ForwardIcon));
            BackwardIcon.SetLocation(Header.RelativeLeftX(), Header.RelativeCenterY(BackwardIcon));

            int weekDaysY = Header.Bottom + InnerVerticalPadding;

            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference("00", Font, 1.5f, ReferenceDimension.Width);

            for (int row = 0; row < NumberOfRows; row++)
            {
                int dayItemsY = weekDaysY + (row + 1) * (itemSize.Height + InnerVerticalPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = ContentLeftBound + col * (itemSize.Height + InnerHorizontalPadding);

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
