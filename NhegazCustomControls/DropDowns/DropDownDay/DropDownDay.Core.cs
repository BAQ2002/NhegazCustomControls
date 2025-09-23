using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace NhegazCustomControls
{
    public partial class DropDownDay : CustomControl, IHasHeader
    {

        public DropDownDay(CustomDatePicker parent) : base(parent)
        {
            parentControl = parent;
            Header ??= new HeaderFeature(this);
            //Header.BackgroundColor = owner.DropDownsHeaderColor;

            if (parentControl is CustomDatePicker dp)
            {
                NumberOfRows = 6;
                NumberOfColumns = 7;

                CurrentMonth = int.Parse(dp.selectedMonth.Text);
                CurrentYear = int.Parse(dp.selectedYear.Text);

                Header.Controls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { ChangeMonth(-1); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { ChangeMonth(-1); Invalidate(); };
                BackwardIcon.BackgroundColor = Header.BackgroundColor;
                Header.Controls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { ChangeMonth(1); Invalidate(); };
                ForwardIcon.DoubleClick += (s, e) => { ChangeMonth(1); Invalidate(); };

                Header.Controls.Add(MonthLabel);
                MonthLabel.Text = MonthTexts[CurrentMonth];

                SecondaryForeColor = Color.FromArgb((ForeColor.R + 255) / 2, (ForeColor.G + 255) / 2, (ForeColor.B + 255) / 2);

                CreateHeaderLabels();
                CreateDayItems();  
                
                AdjustControlSize();
                AdjustHoverColors();
            }
        }

        /// <summary>
        /// Método responsavel por criar os WeekDayLabels.
        /// </summary>
        protected void CreateHeaderLabels()
        {
            string[] weekDays = { "D", "S", "T", "Q", "Q", "S", "S" };
            WeekDayLabels = new InnerLabel[weekDays.Length];

            for (int i = 0; i < weekDays.Length; i++)
            {
                var headerLabel = new InnerLabel()
                {
                    Text = weekDays[i],
                    Font = new Font(Font, FontStyle.Bold),
                    BackgroundColor = BackgroundColor,
                    ForeColor = Color.FromArgb((ForeColor.R + 255) / 2, (ForeColor.G + 255) / 2, (ForeColor.B + 255) / 2),
                    TextHorizontalAlignment = TextHorizontalAlignment.Center,
                    SizeBasedOnText = false,
                };
                InnerControls.Add(headerLabel);
                WeekDayLabels[i] = headerLabel;
            }
        }

        /// <summary>
        /// Método responsavel por criar os DayItemsLabels. 
        /// </summary>
        protected void CreateDayItems()
        {
            DayItemLabels = new DayItemLabel[NumberOfRows, NumberOfColumns];

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    var dayItemLabel = new DayItemLabel()
                    {
                        Font = Font,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,
                    };

                    int capturedRow = row;
                    int capturedCol = col;

                    dayItemLabel.MouseEnter += (s, e) =>
                    {
                        dayItemLabel.ForeColor = BackgroundColor;
                        dayItemLabel.BackgroundColor = OnFocusBorderColor;
                        Invalidate();
                    };
                    dayItemLabel.MouseLeave += (s, e) =>
                    {
                        dayItemLabel.ForeColor = DayItemLabels[capturedRow, capturedCol].IsCurrentMonth ? ForeColor : SecondaryForeColor;
                        dayItemLabel.BackgroundColor = BackgroundColor;
                        Invalidate();
                    };
                    dayItemLabel.Click += (s, e) => OnDayLabelClick(capturedRow, capturedCol);

                    InnerControls.Add(dayItemLabel);
                    DayItemLabels[row, col] = dayItemLabel;
                }
                
            }
            UpdateDayLabels(CurrentYear, CurrentMonth);
        }

        /// <summary>
        /// Método que é invocado no Click/DoubleClick de Backward/Forward.
        /// Invoke => UpdateDayLabels; AdjustControlSize.
        /// </summary>
        /// <param name="offset"></param>
        private void ChangeMonth(int offset)
        {
            if (CurrentMonth + offset > 12)
            {
                CurrentMonth = 1;
                CurrentYear += 1;
            }
            else if (CurrentMonth + offset <= 0)
            {
                CurrentMonth = 12;
                CurrentYear -= 1;
            }
            else
            {
                CurrentMonth += offset;
            }
            MonthLabel.Text = MonthTexts[CurrentMonth];
            UpdateDayLabels(CurrentYear, CurrentMonth);
            AdjustInnerLocations();
        }

        /// <summary>
        /// Metodo responsavel por atualizar os DayItemsLabels
        /// </summary>
        private void UpdateDayLabels(int currentYear, int currentMonth)
        {
            DateTime firstDay = new DateTime(currentYear, currentMonth, 1);
            int firstDayOfWeek = (int)firstDay.DayOfWeek;

            int daysInCurrentMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            int previousMonth = (currentMonth == 1) ? 12 : currentMonth - 1;
            int previousYear = (currentMonth == 1) ? currentYear - 1 : currentYear;
            int daysInPrevMonth = DateTime.DaysInMonth(previousYear, previousMonth);

            int nextMonth = (currentMonth == 12) ? 1 : currentMonth + 1;
            int nextYear = (currentMonth == 12) ? currentYear + 1 : currentYear;

            int gridIndex = 0;

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    DayItemLabel dayItemLabel = DayItemLabels[row, col];

                    if (gridIndex < firstDayOfWeek)
                    {
                        // Dias do mês anterior
                        int day = daysInPrevMonth - firstDayOfWeek + gridIndex + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = previousMonth;
                        dayItemLabel.Year = previousYear;
                        dayItemLabel.IsCurrentMonth = false;
                        dayItemLabel.ForeColor = SecondaryForeColor;
                    }
                    else if (gridIndex < firstDayOfWeek + daysInCurrentMonth)
                    {
                        // Dias do mês atual
                        int day = gridIndex - firstDayOfWeek + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = currentMonth;
                        dayItemLabel.Year = currentYear;
                        dayItemLabel.IsCurrentMonth = true;
                        dayItemLabel.ForeColor = ForeColor;
                    }
                    else
                    {
                        // Dias do próximo mês
                        int day = gridIndex - (firstDayOfWeek + daysInCurrentMonth) + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = nextMonth;
                        dayItemLabel.Year = nextYear;
                        dayItemLabel.IsCurrentMonth = false;
                        dayItemLabel.ForeColor = SecondaryForeColor;
                    }
                    gridIndex++;
                }
            }
        }

        /// <summary>
        /// Método que é invocado no Click de DayItemLabel.
        /// </summary>
        protected void OnDayLabelClick(int rowIndex, int colIndex)
        {
            var item = DayItemLabels[rowIndex, colIndex];

            if (parentControl is CustomDatePicker dp)
            {
                dp.selectedDay.Text = item.Day.ToString("D2");
                dp.selectedMonth.Text = item.Month.ToString("D2");
                dp.selectedYear.Text = item.Year.ToString();
            }

            Parent?.Controls.Remove(this);
        }
        protected override void AdjustHoverColors()
        {
            BackwardIcon.MouseEnter += (s, e) =>
            {
                BackwardIcon.ForeColor = Header.BackgroundColor; // era HeaderBackgroundColor
                BackwardIcon.BackgroundColor = OnFocusBorderColor;
            };
            BackwardIcon.MouseLeave += (s, e) =>
            {
                BackwardIcon.ForeColor = this.ForeColor;
                BackwardIcon.BackgroundColor = Header.BackgroundColor; // era HeaderBackgroundColor
            };

            ForwardIcon.MouseEnter += (s, e) =>
            {
                ForwardIcon.ForeColor = Header.BackgroundColor;
                ForwardIcon.BackgroundColor = OnFocusBorderColor;
            };
            ForwardIcon.MouseLeave += (s, e) =>
            {
                ForwardIcon.ForeColor = this.ForeColor;
                ForwardIcon.BackgroundColor = Header.BackgroundColor;
            };

        }


    }
}
