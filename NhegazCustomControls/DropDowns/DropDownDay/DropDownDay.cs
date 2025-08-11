using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Buffers.Text;
using System.Drawing.Drawing2D;


namespace NhegazCustomControls
{
    public class DropDownDay : CustomControlWithHeader
    {
        class DayItemLabel : InnerLabel
        {
            private int day;
            public int Day
            {
                get => day;
                set{ day = value; Text = day.ToString(); }
            }
            public bool IsCurrentMonth { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public DayItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            {}
        }
        protected CustomControl parentControl;
        public int HeaderVerticalPadding {  get; set; }
        public int HeaderhorizontalPadding { get; set; }

        private int NumberOfRows;
        private int NumberOfColumns;

        private int CurrentMonth;
        private int CurrentYear;

        private InnerLabel MonthLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);


        private DayItemLabel[,] DayItemLabels; //Matriz composta pelos Labels de dias.
        private InnerLabel[] WeekDayLabels; //Matriz composta pelos Labels do cabecalho de dias da semana.

        private string[] MonthTexts =  {"null", "Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho",
                                         "Julho", "Agosto", "Setembro", "Outubro", "Novembro","Dezembro" };

        public override Font Font
        {
            get => base.Font;
            set 
            { 
                base.Font = value; 
                MonthLabel.Font = new Font(value, FontStyle.Bold); ForwardIcon.Font = value; 
                BackwardIcon.Font = value; 
                AdjustControlSize(); 
            }
        }

        public override Color ForeColor 
        {
            get => base.ForeColor;
            set 
            { 
                base.ForeColor = value; 
                MonthLabel.ForeColor = value; ForwardIcon.ForeColor = value; BackwardIcon.ForeColor = value; 
                Invalidate(); 
            }
        }
        public override Color HeaderBackgroundColor
        {
            get => base.HeaderBackgroundColor;
            set { base.HeaderBackgroundColor = value; MonthLabel.BackgroundColor = Color.Transparent; ForwardIcon.BackgroundColor = value; BackwardIcon.BackgroundColor = value; Invalidate(); }
        }

        public DropDownDay(CustomDatePicker owner) : base(owner)
        {
            parentControl = owner;
            HeaderBackgroundColor = owner.DropDownsHeaderColor;
    

            if (parentControl is CustomDatePicker dp)
            {
                NumberOfRows = 6;
                NumberOfColumns = 7;

                CurrentMonth = int.Parse(dp.selectedMonth.Text);
                CurrentYear = int.Parse(dp.selectedYear.Text);
                
                HeaderControls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { ChangeMonth(-1); Invalidate(); };
                BackwardIcon.DoubleClick += (s, e) => { ChangeMonth(-1); Invalidate(); };

                HeaderControls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { ChangeMonth(1); Invalidate(); };
                ForwardIcon.DoubleClick += (s, e) => { ChangeMonth(1); Invalidate(); };

                HeaderControls.Add(MonthLabel);
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
                BackwardIcon.ForeColor = HeaderBackgroundColor;
                BackwardIcon.BackgroundColor = OnFocusBorderColor;

            };
            BackwardIcon.MouseLeave += (s, e) =>
            {
                BackwardIcon.ForeColor = ForeColor;
                BackwardIcon.BackgroundColor = HeaderBackgroundColor;

            };
            ForwardIcon.MouseEnter += (s, e) =>
            {
                ForwardIcon.ForeColor = HeaderBackgroundColor;
                ForwardIcon.BackgroundColor = OnFocusBorderColor;

            };
            ForwardIcon.MouseLeave += (s, e) =>
            {
                ForwardIcon.ForeColor = ForeColor;
                ForwardIcon.BackgroundColor = HeaderBackgroundColor;

            };
        }

        protected override void AdjustControlSize()
        {           
            if (DayItemLabels == null || DayItemLabels.Length == 0 || NumberOfColumns <= 0 || NumberOfRows <= 0)
                return;

            AdjustPadding();
        
            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;
            
            int itemUniformSize = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            Width = xPadding + (NumberOfColumns * (itemUniformSize + xPadding));
            Height = yPadding + ((NumberOfRows + 2) * (itemUniformSize + yPadding));

            AdjustHeaderSize(Width - (2 * xPadding), itemUniformSize);
            AdjustHeaderLocation(xPadding, yPadding);

            AdjustInnerSizes(); AdjustInnerLocations(); 

            int weekDayY = (2 * yPadding) + ForwardIcon.Height;
            int baseGridY = weekDayY + itemUniformSize + yPadding;

            for (int row = 0; row < NumberOfRows; row++)
            {
                int y = baseGridY + row * (itemUniformSize + yPadding);

                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int x = xPadding + col * (itemUniformSize + xPadding);

                    if (row == 0)
                    {
                        AdjustInnerSizes(col, itemUniformSize, itemUniformSize);
                        AdjustInnerLocations(col, x, weekDayY);
                    }

                    AdjustInnerSizes(row, col, itemUniformSize, itemUniformSize);
                    AdjustInnerLocations(row, col, x, y);
                }
            }

        }
        protected override void AdjustInnerSizes()
        {
            BackwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            BackwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

            ForwardIcon.Width = NhegazSizeMethods.TextExactSize("00", Font).Height;
            ForwardIcon.Height = NhegazSizeMethods.TextProportionalSize("00", Font, 1.3f).Height;

        }
        protected override void AdjustInnerSizes(int index, int itemWidth, int ItemHeight)
        {
            var label = WeekDayLabels[index];
            label.Width = itemWidth;
            label.Height = ItemHeight;
        }
        protected override void AdjustInnerSizes(int row, int col, int itemWidth, int ItemHeight)
        {
            var label = DayItemLabels[row, col];
            label.Width = itemWidth;
            label.Height = ItemHeight;
        }
        
        protected override void AdjustInnerLocations()
        {
            BackwardIcon.SetLocation(HorizontalPadding, VerticalPadding);
            ForwardIcon.SetLocation(Width - (ForwardIcon.Width + HorizontalPadding), VerticalPadding);
            MonthLabel.SetLocation((Width - MonthLabel.Width) / 2, VerticalPadding);
        }
        protected override void AdjustInnerLocations(int index, int x, int y)
        {
            var label = WeekDayLabels[index];
            label.SetLocation(x, y);
        }
        protected override void AdjustInnerLocations(int row, int col, int x, int y)
        {
            var label = DayItemLabels[row, col];
            label.SetLocation(x, y);
        }      
      

    }
}
