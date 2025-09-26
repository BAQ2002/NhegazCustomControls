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
    public partial class DropDownDay : CustomControl, IHasHeader, IHasMatrix
    {
        public DropDownDay(CustomDatePicker parent) : base(parent)
        {
            parentControl = parent;

            NumberOfRows = 6;
            NumberOfColumns = 7;

            WeekDayLabels = new InnerLabel[NumberOfColumns];
            DayItemsLabels = new DayItemLabel[NumberOfRows, NumberOfColumns];

            Header ??= new HeaderFeature(this);
            Matrix ??= new MatrixFeature(this, DayItemsLabels);

            if (parentControl is CustomDatePicker dp)
            {              
                CurrentMonth = int.Parse(dp.selectedMonth.Text);
                CurrentYear = int.Parse(dp.selectedYear.Text);

                Header.Controls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { ChangeMonth(-1); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { ChangeMonth(-1); Invalidate(); };
     
                Header.Controls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { ChangeMonth(1); Invalidate(); };
                ForwardIcon.DoubleClick += (s, e) => { ChangeMonth(1); Invalidate(); };

                Header.Controls.Add(MonthLabel);
                MonthLabel.Text = MonthTexts[CurrentMonth];

                SecondaryForeColor = Color.FromArgb((ForeColor.R + 255) / 2, (ForeColor.G + 255) / 2, (ForeColor.B + 255) / 2);

                CreateWeekDayLabels();
                CreateDayItemLabels();  
                
                AdjustControlSize();
                AdjustHoverColors();
                Header.AdjustHeaderColors();
            }
        }

        /// <summary>
        /// Cria/atualiza os WeekDayLabels conforme o NumberOfColumns e StartOfWeek.
        /// </summary>
        private void CreateWeekDayLabels()
        {           
            if (WeekDayLabels != null) //Remove labels anteriores
            {
                foreach (var lbl in WeekDayLabels)
                    InnerControls.Remove(lbl);
            }
            
            var weekDayLetters = NhegazCultureMethods.GetCultureWeekdayLettersOrDefault(); // len = 7
            int startIndex = (int)StartOfWeek; // 0..6

            // 4) Cria N labels, indexando ciclicamente sobre as 7 letras
            for (int i = 0; i < NumberOfColumns; i++)
            {
                // exemplo: com 7 colunas, comportamento atual; com N≠7, repete/secciona ciclicamente
                int dow = (startIndex + i) % 7;

                var lbl = new InnerLabel
                {
                    Text = weekDayLetters[dow],
                    Font = new Font(Font, FontStyle.Bold),
                    BackgroundColor = BackgroundColor,
                    ForeColor = ForeColor,
                    SizeBasedOnText = false,
                    TextHorizontalAlignment = TextHorizontalAlignment.Center,
                    TextVerticalAlignment = TextVerticalAlignment.Center,
                };
                WeekDayLabels[i] = lbl;
                InnerControls.Add(lbl);
            }
        }


        /// <summary>
        /// Método responsavel por criar os items de DayItemsLabels. 
        /// </summary>
        protected void CreateDayItemLabels()
        {
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

                    // Lógica direta para eventos
                    dayItemLabel.MouseEnter += (s, e) =>
                    {
                        dayItemLabel.ForeColor = BackgroundColor;
                        dayItemLabel.BackgroundColor = HoverColor;
                        Invalidate();
                    };

                    dayItemLabel.MouseLeave += (s, e) =>
                    {
                        dayItemLabel.ForeColor = dayItemLabel.IsCurrentMonth ? ForeColor : SecondaryForeColor;
                        dayItemLabel.BackgroundColor = BackgroundColor;
                        Invalidate();
                    };

                    dayItemLabel.Click += (s, e) => OnDayItemLabelClick(row, col);

                    InnerControls.Add(dayItemLabel);
                    DayItemsLabels[row, col] = dayItemLabel;
                }
            }

            UpdateDayItemLabels(CurrentYear, CurrentMonth);
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
            UpdateDayItemLabels(CurrentYear, CurrentMonth);
            AdjustInnerLocations();
        }

        /// <summary>
        /// Metodo responsavel por atualizar as propriedades dos DayItemsLabels
        /// </summary>
        private void UpdateDayItemLabels(int currentYear, int currentMonth)
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
                    DayItemLabel dayItemLabel = DayItemsLabels[row, col];

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
        protected void OnDayItemLabelClick(int rowIndex, int colIndex)
        {
            var item = DayItemsLabels[rowIndex, colIndex];

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
                BackwardIcon.ForeColor = Header.HoverForeColor;
                BackwardIcon.BackgroundColor = Header.HoverBackgroundColor;
            };
            BackwardIcon.MouseLeave += (s, e) =>
            {
                BackwardIcon.ForeColor = Header.ForeColor;
                BackwardIcon.BackgroundColor = Header.BackgroundColor; 
            };

            ForwardIcon.MouseEnter += (s, e) =>
            {
                ForwardIcon.ForeColor = Header.HoverForeColor;
                ForwardIcon.BackgroundColor = Header.HoverBackgroundColor;
            };
            ForwardIcon.MouseLeave += (s, e) =>
            {
                ForwardIcon.ForeColor = Header.ForeColor;
                ForwardIcon.BackgroundColor = Header.BackgroundColor;
            };
        }
    }
}
