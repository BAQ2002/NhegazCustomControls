using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownMonth
    {
        public override void UpdateLayout()
        {
            if (MonthItems == null || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            base.UpdateLayout();

        }

        public override Size GetContentSize()
        {
            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference
                            ("0000", Font, 1.3f, ReferenceDimension.Width);

            int headerHeight = NhegazSizeMethods.TextSquareSizeByReference
                               ("00", Font, 1.5f, ReferenceDimension.Width).Height;

            int contentWidth = NumberOfColumns * itemSize.Height; //Largura do YearItems

            int contentHeight = headerHeight                      //Altura do cabeçalho
                              + NumberOfRows * itemSize.Width;    //Altura do YearItems

            return new Size(contentWidth, contentHeight);
        }

        public override Size GetPaddingSize()
        {
            int NumberOfHorizontalPaddings = NumberOfColumns - 1;                   //Quantidade de Gaps Horizontais
            int NumberOfVerticalPaddings = NumberOfRows;                            //Quantidade de Gaps Verticais

            int paddingWidth = BorderHorizontalBoundsSum                            //Padding horizontal das bordas 
                             + NumberOfHorizontalPaddings * InnerHorizontalPadding; //Paddings entre todos

            int paddingHeight = BorderVerticalBoundsSum                             //Padding vertical das bordas 
                              + NumberOfVerticalPaddings * InnerVerticalPadding;    //Paddings entre todos

            return new Size(paddingWidth, paddingHeight);
        }

        protected override void SetInnerSizes()
        {
            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference("0000", Font, 1.3f, ReferenceDimension.Width);
            Size headerItemSize = NhegazSizeMethods.TextSquareSizeByReference("00", Font, 1.5f, ReferenceDimension.Width);

            Header.SetSize(Width - BorderHorizontalBoundsSum, headerItemSize.Height);
            BackwardIcon.SetSize(headerItemSize);
            ForwardIcon.SetSize(headerItemSize);

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    MonthItems.SetItemSize(row, col, itemSize);
                }
            }
        }
        protected override void SetInnerLocations()
        {
            Header.SetLocation(RelativeLeftX(), RelativeTopY());

            YearLabel.SetLocation(Header.RelativeCenterX(YearLabel), Header.RelativeCenterY(YearLabel));
            ForwardIcon.SetLocation(Header.RelativeRightX(ForwardIcon), Header.RelativeCenterY(ForwardIcon));
            BackwardIcon.SetLocation(Header.RelativeLeftX(), Header.RelativeCenterY(BackwardIcon));

            int startY = Header.Bottom + InnerVerticalPadding;

            Size itemSize = NhegazSizeMethods.TextSquareSizeByReference("0000", Font, 1.3f, ReferenceDimension.Width);

            for (int row = 0; row < NumberOfRows; row++)
            {
                int y = startY + row * (itemSize.Height + InnerVerticalPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = ContentLeftBound + col * (itemSize.Height + InnerHorizontalPadding);

                    MonthItems.SetItemLocation(row, col, x, y);
                }
            }
        }
    }
}
