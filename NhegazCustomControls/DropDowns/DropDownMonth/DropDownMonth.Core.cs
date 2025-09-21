using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownMonth : CustomControlWithHeader
    {
        
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
        
    }
}
