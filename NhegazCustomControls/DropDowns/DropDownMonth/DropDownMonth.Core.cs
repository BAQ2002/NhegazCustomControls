using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NhegazCustomControls
{
    public partial class DropDownMonth : CustomControl, IHasHeader, IHasMatrix
    {
        
        public DropDownMonth(CustomDatePicker owner) : base(owner)
        {
            parentControl = owner;
            Header ??= new HeaderFeature(this);
            MonthItems ??= new MatrixFeature(this, NumberOfRows, NumberOfColumns);
            CreateMonthLabels();
            AdjustControlSize();
        }

        protected void OnLabelClick(int row, int col)
        {
            var item = (MonthItemLabel)MonthItems.GetItem(row, col);

            if (parentControl is CustomDatePicker dp)
            {
                dp.selectedMonth.Text = item.Month.ToString("D2");
                dp.selectedYear.Text = item.Year.ToString();
                Parent?.Controls.Remove(this);
            }                        
        }

        public void CreateMonthLabels()
        {
            string[] MonthTexts = NhegazCultureMethods.GetCultureMonthAbbr3OrDefault();

            int startIndex = 0;
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int currentRow = row;
                    int currentCol = col;

                    int dow = (startIndex + i) % 12;
                    var monthItemLabel = new MonthItemLabel
                    {
                        Text = MonthTexts[dow],
                        Month = dow,
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    monthItemLabel.Click += (s, e) => OnLabelClick(currentRow, currentCol);
               
                    MonthItems.AddItem(monthItemLabel, row, col);

                    if (row * col >= 11)
                        gridIndex = 0;
                    else
                        gridIndex++;
                }
            }
        }
        
    }
}
