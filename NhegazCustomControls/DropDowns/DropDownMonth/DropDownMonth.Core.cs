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

            if (parentControl is CustomDatePicker dp)
            {
                CurrentYear = dp.Year;

                Header.Controls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { UpdateYear(-1); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { UpdateYear(-1); Invalidate(); };

                Header.Controls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { UpdateYear(+1); Invalidate(); };
                ForwardIcon.DoubleClick += (s, e) => { UpdateYear(+1); Invalidate(); };

                Header.Controls.Add(YearLabel);
                YearLabel.Text = CurrentYear.ToString();
                YearLabel.SizeBasedOnText = false;

                // Itens de mês (com o ano atual)
                CreateMonthLabels();
                UpdateMonthsYear(CurrentYear);

                AdjustControlSize();
                Header.AdjustHeaderColors();
            }
        }
        public void CreateMonthLabels()
        {
            string[] MonthTexts = NhegazCultureMethods.GetCultureMonthAbbr3OrDefault();

            int monthIndex = 0;

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int currentRow = row;
                    int currentCol = col;
                    int dow = monthIndex % 12;

                    var monthItemLabel = new MonthItemLabel
                    {
                        Text = MonthTexts[dow],
                        Month = dow+1,
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    monthItemLabel.Click += (s, e) => OnLabelClick(currentRow, currentCol);

                    MonthItems.AddItem(monthItemLabel, row, col);

                    monthIndex++;
                }
            }
        }

        private void UpdateYear(int offset)
        {
            CurrentYear += offset;
            YearLabel.Text = CurrentYear.ToString();
            UpdateMonthsYear(CurrentYear);
            AdjustInnerLocations(); // mantém coerente com Day/Year após alterar header
        }

        private void UpdateMonthsYear(int year)
        {
            for (int row = 0; row < NumberOfRows; row++)
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    var item = (MonthItemLabel)MonthItems.GetItem(row, col);
                    int idx = row * NumberOfColumns + col; // 0..15
                    item.Year = year + (idx >= 12 ? 1 : 0); // idx 12..15 -> ano seguinte
                }
        }

        protected void OnLabelClick(int row, int col)
        {
            var item = (MonthItemLabel)MonthItems.GetItem(row, col);

            if (parentControl is CustomDatePicker dp)
            {
                dp.Month = item.Month;
                dp.Year = item.Year;
                Parent?.Controls.Remove(this);
            }                        
        }

        
        
    }
}
