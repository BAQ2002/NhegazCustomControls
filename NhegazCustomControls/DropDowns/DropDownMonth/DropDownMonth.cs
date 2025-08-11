using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public class DropDownMonth : CustomControlWithHeader
    {
        class MonthItemLabel : InnerLabel
        {
            public int Month { get; set; }
        }

        protected CustomControl parentControl;

        private InnerLabel YearLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);

        private MonthItemLabel[,] MonthList;

        int NumberOfRows = 3;
        int NumberOfColumns = 4;

        public DropDownMonth(CustomDatePicker owner) : base(owner)
        {
            parentControl = owner;
            HeaderBackgroundColor = owner.DropDownsHeaderColor;

            CreateMonthLabels();
            AdjustControlSize();
        }

        protected void OnLabelClick(int rowIndex, int colIndex)
        {
            var item = MonthList[rowIndex, colIndex];

            if (parentControl is CustomDatePicker dp)
                dp.selectedMonth.Text = item.Month.ToString("D2");

            Parent?.Controls.Remove(this);
        }

        public void CreateMonthLabels()
        {
            string[] MonthTexts = { "Jan", "Fev", "Mar", "Abr",
                                    "Mai", "Jun", "Jul", "Ago",
                                    "Set", "Out", "Nov", "Dez" };
            MonthList = new MonthItemLabel[NumberOfRows, NumberOfColumns];

            int gridIndex = 0;
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    

                    var monthItemLabel = new MonthItemLabel
                    {
                        Text = MonthTexts[gridIndex],
                        Month = gridIndex + 1,
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    int capturedRow = row;
                    int capturedCol = col;

                    monthItemLabel.MouseEnter += (s, e) =>
                    {
                        monthItemLabel.ForeColor = BackgroundColor;
                        monthItemLabel.BackgroundColor = OnFocusBorderColor;                        
                    };
                    monthItemLabel.MouseLeave += (s, e) =>
                    {
                        monthItemLabel.ForeColor = ForeColor;
                        monthItemLabel.BackgroundColor = BackgroundColor;
                    };
                    monthItemLabel.Click += (s, e) => OnLabelClick(capturedRow, capturedCol);
                    gridIndex++;
                    InnerControls.Add(monthItemLabel);
                    MonthList[row, col] = monthItemLabel;
                }
            }
        }
        protected override void AdjustControlSize()
        {
            Controls.Clear();

            if (MonthList == null || MonthList.Length == 0 || NumberOfColumns <= 0)
                return;

            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;

            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;

            AdjustHeaderSize(Width - (2 * xPadding), itemUniformSize);
            AdjustHeaderLocation(xPadding, yPadding);

            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = yPadding + (NumberOfRows * (itemUniformSize + yPadding));
            AdjustInnerLocations();
        }
        protected override void AdjustInnerLocations()
        {
            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;
            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("0000", Font, 1.3f).Width;

            for (int row = 0; row < NumberOfRows; row++)
            {
                int y = yPadding + row * (itemUniformSize + yPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);

                    var label = MonthList[row, col];
                    label.Location = new Point(x, y);
                    label.Width = itemUniformSize;
                    label.Height = itemUniformSize;
                }
            }
        }
        //protected override

    }
}
