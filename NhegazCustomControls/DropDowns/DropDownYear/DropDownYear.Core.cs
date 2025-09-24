
namespace NhegazCustomControls
{
    public partial class DropDownYear : CustomControl, IHasHeader
    {      
        public DropDownYear(CustomDatePicker owner) : base(owner)
        {
            parentControl = owner;
            Header ??= new HeaderFeature(this);

            if (parentControl is CustomDatePicker dp)
            {
                CurrentDecade = (DateTime.Now.Year / 10) * 10;
                DecadeLastYear = CurrentDecade + 9;

                //ForwardIcon.Text = "▶";
                DecadeLabel.Text = $"{CurrentDecade} - {DecadeLastYear}";
                DecadeLabel.SizeBasedOnText = false;

                BackwardIcon.Click += (s, e) => { ChangeDecade(-10); Invalidate(); };
                ForwardIcon.Click += (s, e) => { ChangeDecade(10); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { ChangeDecade(-20); Invalidate(); };
                ForwardIcon.DoubleClick += (s, e) => { ChangeDecade(20); Invalidate(); };

                Header.Controls.Add(BackwardIcon);
                Header.Controls.Add(ForwardIcon);
                Header.Controls.Add(DecadeLabel);

                CreateYearLabels();
                AdjustControlSize();
            }           
        }

        private void CreateYearLabels()
        {
            YearItemsLabels = new YearItemLabel[NumberOfRows, NumberOfColumns];

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    var yearItemLabel = new YearItemLabel()
                    {
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    int capturedRow = row;
                    int capturedCol = col;

                    yearItemLabel.MouseEnter += (s, e) =>
                    {
                        yearItemLabel.ForeColor = BackgroundColor;
                        yearItemLabel.BackgroundColor = OnFocusBorderColor;
                        Invalidate();
                    };
                    yearItemLabel.MouseLeave += (s, e) =>
                    {
                        yearItemLabel.ForeColor = ForeColor;
                        yearItemLabel.BackgroundColor = BackgroundColor;
                        Invalidate();
                    };
                    yearItemLabel.Click += (s, e) => OnYearLabelClick(capturedRow, capturedCol);

                    YearItemsLabels[row, col] = yearItemLabel;
                    InnerControls.Add(yearItemLabel);
                }
            }
            UpdateYearLabels(CurrentDecade);
        }

        private void ChangeDecade(int offset)
        {
            CurrentDecade += offset;
            DecadeLastYear = CurrentDecade + 9;
            DecadeLabel.Text = $"{CurrentDecade} - {DecadeLastYear}";
            UpdateYearLabels(CurrentDecade);
        }
        private void UpdateYearLabels(int currentDecade)
        {
            int index = 0;

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int year = currentDecade + index;
                    YearItemLabel yearItemLabel = YearItemsLabels[row, col];                                    
                    yearItemLabel.Year = year;
                    index++;
                }
            }
        }
        protected void OnYearLabelClick(int row, int col)
        {
            if (parentControl is CustomDatePicker dp)
            {
                dp.selectedYear.Text = YearItemsLabels[row, col].Year.ToString();
                Parent?.Controls.Remove(this);
            }
        }        
    }
}
