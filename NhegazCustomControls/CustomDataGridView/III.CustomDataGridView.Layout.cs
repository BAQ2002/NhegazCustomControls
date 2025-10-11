using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        public override void AdjustControlSize()
        {
            AdjustInnerSizes(); AdjustInnerLocations();
        }

        protected override void AdjustInnerSizes()
        {
            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;

            int columnsTotalWidth = 0;

            int rowItemHeight = NhegazSizeMethods.FontUnitSize(Font).Height + InnerVerticalPadding;

            for (int col = 0; col < cols; col++)
            {
                Size headerItemSize = new(ColumnWidth
                                         (ColumnWidthMode, HeaderLabels.GetItem(col).Width, InnerHorizontalPadding),
                                          NhegazSizeMethods.FontUnitSize(Font).Height + InnerVerticalPadding);

                if (col == 0) { headerItemSize.Width += ContentLeftBound; }
                if (col == cols) { headerItemSize.Width += ContentRightBound; }

                columnsTotalWidth += headerItemSize.Width;

                HeaderLabels.SetItemSize(col, headerItemSize);
            }

            Header.SetSize(columnsTotalWidth, rowItemHeight);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemSize(row, col, HeaderLabels.GetItem(col).Size);
                }
            }
        }

        protected override void AdjustInnerLocations()
        {
            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;

            int itemHeight = NhegazSizeMethods.FontUnitSize(Font).Height + InnerVerticalPadding;

            int headerX = BorderWidth;

            for (int col = 0; col < cols; col++)
            {
                HeaderLabels.SetItemLocation(col, headerX, BorderWidth);
                headerX += HeaderLabels.GetItem(col).Width + col * lineBetweenCol;
            }

            Header.SetLocation(BorderWidth, BorderWidth);

            for (int row = 0; row < rows; row++)
            {
                int x = BorderWidth;
                int y = BorderWidth + (row + 1) * (itemHeight + lineBetweenRow);

                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemLocation(row, col, x, y);

                    x += lineBetweenCol + HeaderLabels.GetItem(col).Width;


                }
            }
        }

        /// <summary>
        /// Com 
        /// </summary>
        public int ColumnWidth(ColumnWidthMode columnWidthMode, int headerWidth, int xPadding)
        {
            int columnWidth = 0;

            if (columnWidthMode == ColumnWidthMode.HeaderWidth)
                columnWidth = headerWidth + xPadding;

            if (columnWidthMode == ColumnWidthMode.FixedCharWidth)
            {
                string sample = new string('0', FixedCharCount);
                columnWidth = NhegazSizeMethods.TextExactSize(sample, Font).Width + xPadding;
            }

            return columnWidth;
        }
    }
}
