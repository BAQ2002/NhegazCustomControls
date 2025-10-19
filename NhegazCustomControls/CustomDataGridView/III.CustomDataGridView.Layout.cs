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
            int NumberOfCloumnsLines = DataLabels.GetColsLenght -1;

            int rowHeight = FontUnitSize.Height + InnerVerticalPadding;

            Size[] columnsSizes = new Size[cols];

            for (int col = 0; col < cols; col++)
            {
                columnsSizes[col] = new(ColumnWidth(HeaderLabels.GetItem(col).Width), rowHeight);

                if (col == 0) { columnsSizes[col].Width += BorderLeftPadding; }
                if (col == cols-1) { columnsSizes[col].Width += BorderRightPadding; }

                HeaderLabels.SetItemSize(col, columnsSizes[col]);
            }

            int headerTotalWidth = HeaderLabels.ItemsWidthSum + (cols-1)* lineBetweenCol;

            Header.SetSize(headerTotalWidth, rowHeight);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemSize(row, col, columnsSizes[col]);
                }
            }
        }

        protected override void SetInnerLocations()
        {
            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;

            int itemHeight = FontUnitSize.Height + InnerVerticalPadding;

            int headerItemX = BorderWidth;
            Header.SetLocation(BorderWidth, BorderWidth);

            for (int col = 0; col < cols; col++)
            {
                HeaderLabels.SetItemLocation(col, headerItemX, BorderWidth);
                headerItemX += HeaderLabels.GetItem(col).Width + lineBetweenCol;
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
