
namespace NhegazCustomControls
{
    public partial class DropDownYear : CustomControl, IHasHeader , IHasMatrix
    {      
        public DropDownYear(CustomDatePicker owner) : base(owner)
        {
            parentControl = owner;
            Header ??= new HeaderFeature(this);
            YearItems = new MatrixFeature(this, NumberOfRows, NumberOfColumns);

            if (parentControl is CustomDatePicker dp)
            {
                CurrentDecade = (dp.Year / 10) * 10;
                DecadeLastYear = CurrentDecade + 9;

                Header.Controls.Add(DecadeLabel);
                DecadeLabel.Text = $"{CurrentDecade} - {DecadeLastYear}";
                DecadeLabel.SizeBasedOnText = false;

                Header.Controls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { UpdateDecade(-10); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { UpdateDecade(-20); Invalidate(); };

                Header.Controls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { UpdateDecade(10); Invalidate(); };          
                ForwardIcon.DoubleClick += (s, e) => { UpdateDecade(20); Invalidate(); };
               
                CreateYearLabels();
                UpdateLayout();
                Header.AdjustHeaderColors();
            }           
        }

        private void CreateYearLabels()
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int currentRow = row;
                    int currentCol = col;

                    var yearItemLabel = new YearItemLabel()
                    {
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    yearItemLabel.Click += (s, e) => OnYearLabelClick(currentRow, currentCol);

                    YearItems.AddItem(yearItemLabel, row, col);
                }
            }
            UpdateYearLabels(CurrentDecade);
        }

        private void UpdateDecade(int offset)
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
                    YearItemLabel yearItemLabel = (YearItemLabel)YearItems.GetItem(row, col);                                    
                    yearItemLabel.Year = year;
                    index++;
                }
            }
        }
        protected void OnYearLabelClick(int row, int col)
        {
            var item = (YearItemLabel)YearItems.GetItem(row, col);

            if (parentControl is CustomDatePicker dp)
            {
                dp.Year = item.Year;
                dp.CloseDropDownInstance();
            }
        }        
    }
}
