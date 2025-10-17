using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        public override void UpdateLayout()
        {
            if (DataIsSourced == false) return;

            base.UpdateLayout();
        }

        public override Size GetContentSize()
        {
            return new(0, 0);
            //throw new NotImplementedException();
        }
        public override Size GetPaddingSize()
        {
            return new(0, 0);
            //throw new NotImplementedException();
        }

        protected override void SetInnerSizes()
        {
            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;

            int columnsTotalWidth = 0;
            int rowHeight = NhegazSizeMethods.FontUnitSize(Font).Height + InnerVerticalPadding;

            for (int col = 0; col < cols; col++)
            {
                Size headerItemSize = new(ColumnWidth(HeaderLabels.GetItem(col).Width), rowHeight);

                if (col == 0) { headerItemSize.Width += BorderLeftPadding; }
                if (col == cols-1) { headerItemSize.Width += BorderRightPadding; }

                columnsTotalWidth += headerItemSize.Width + lineBetweenCol;

                HeaderLabels.SetItemSize(col, headerItemSize);
            }

            Header.SetSize(columnsTotalWidth, rowHeight);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemSize(row, col, HeaderLabels.GetItem(col).Size);
                }
            }
        }

        protected override void SetInnerLocations()
        {
            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;

            int itemHeight = NhegazSizeMethods.FontUnitSize(Font).Height + InnerVerticalPadding;

            int headerX = BorderWidth;
            Header.SetLocation(BorderWidth, BorderWidth);
            for (int col = 0; col < cols; col++)
            {
                HeaderLabels.SetItemLocation(col, headerX, BorderWidth);
                headerX += HeaderLabels.GetItem(col).Width + lineBetweenCol;
            }

            

            for (int row = 0; row < rows; row++)
            {
                int x = BorderWidth;
                int y = Header.Bottom + row * (itemHeight + lineBetweenRow);

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
        public int ColumnWidth(int headerWidth)
        {
            int columnWidth = 0;

            if (ColumnWidthMode == ColumnWidthMode.HeaderWidth)
                columnWidth = headerWidth + InnerHorizontalPadding;

            if (ColumnWidthMode == ColumnWidthMode.FixedCharWidth)
            {
                columnWidth = NhegazSizeMethods.FontUnitSize(Font).Width 
                            + InnerHorizontalPadding;
            }

            return columnWidth;
        }
    }
}
