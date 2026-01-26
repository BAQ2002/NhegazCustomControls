using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace NhegazCustomControls
{
    public partial class DropDownDay : CustomControl, IHasHeader, IHasMatrix, IHasVector
    {
        
        public DropDownDay(CustomDatePicker parent) : base(parent)
        {
            parentControl = parent;

            Header ??=   new HeaderFeature(this);
            WeekDays ??= new VectorFeature(this, NumberOfColumns);
            DayItems ??= new MatrixFeature(this, NumberOfRows, NumberOfColumns);
            

            if (parentControl is CustomDatePicker dp)
            {              
                CurrentMonth = dp.Month;
                CurrentYear = dp.Year;

                Header.Controls.Add(BackwardIcon);
                BackwardIcon.Click += (s, e) => { UpdateMonth(-1); Invalidate();  
                    MessageBox.Show("Icon.Location: " + BackwardIcon.Location.ToString() + " Icon.Size: " + BackwardIcon.Size.ToString()
                                   +"Header.Location:" + Header.Location.ToString() + "Header.Size:" + Header.Size.ToString()); };
                BackwardIcon.DoubleClick += (s, e) => { UpdateMonth(-1); Invalidate(); };
     
                Header.Controls.Add(ForwardIcon);
                ForwardIcon.Click += (s, e) => { UpdateMonth(1); Invalidate(); 
                    MessageBox.Show("Icon.Location: " + ForwardIcon.Location.ToString()
                                   +"Icon.Size: " + ForwardIcon.Size.ToString()); };
                ForwardIcon.DoubleClick += (s, e) => { UpdateMonth(1); Invalidate(); };
        
                Header.Controls.Add(MonthLabel);
                MonthLabel.Text = MonthTexts[CurrentMonth];

                SecondaryForeColor = Color.FromArgb((ForeColor.R + 255) / 2, (ForeColor.G + 255) / 2, (ForeColor.B + 255) / 2);

                CreateWeekDayLabels();
                CreateDayItems();  
                
                UpdateLayout();
                Header.AdjustHeaderColors();
            }
        }
        
        /// <summary>
        /// Cria/atualiza os WeekDays conforme o NumberOfColumns e StartOfWeek.
        /// </summary>
        private void CreateWeekDayLabels()
        {                     
            string[] weekDayLetters = NhegazCultureMethods.GetCultureWeekdayLettersOrDefault(); // len = 7
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
                    AbleToHover = false,
                    ForeColor = ForeColor,
                    SizeBasedOnText = false,

                };

                lbl.TextFeatures.TextHorizontalAlignment = TextHorizontalAlignment.Center;
                lbl.TextFeatures.TextVerticalAlignment = TextVerticalAlignment.Center;

                WeekDays.AddItem(lbl, i);
            }
        }


        /// <summary>
        /// Método responsavel por criar os items de DayItems. 
        /// </summary>
        protected void CreateDayItems()
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int currentRow = row;
                    int currentCol = col;

                    var dayItemLabel = new DayItemLabel()
                    {
                        Font = Font,
                        BackgroundColor = BackgroundColor,
                        BackGroundShape = BackGroundShape.SymmetricCircle,
                        SizeBasedOnText = false,

                    };

                    dayItemLabel.Click += (s, e) => OnDayItemLabelClick(currentRow, currentCol);
                   
                    DayItems.AddItem(dayItemLabel, row, col);
                }
            }

            UpdateDayItemLabels(CurrentYear, CurrentMonth);
        }

        /// <summary>
        /// Método que é invocado no Click/DoubleClick de LeftArrow/RightArrow.
        /// Invoke => UpdateDayLabels; UpdateLayout.
        /// </summary>
        /// <param name="offset"></param>
        private void UpdateMonth(int offset)
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
            SetInnerLocations();
        }

        /// <summary>
        /// Método responsável por atualizar as propriedades dos DayItems.
        /// </summary>
        private void UpdateDayItemLabels(int currentYear, int currentMonth)
        {
            DateTime firstDay = new DateTime(currentYear, currentMonth, 1);           //Data do primeiro dia do mês atual.
            int firstDayOfWeek = (int)firstDay.DayOfWeek;                             //Dia da semana do primeiro dia do mês atual. 

            int daysInCurrentMonth = DateTime.DaysInMonth(currentYear, currentMonth); //Quantidade de dias no mês atual.

            int previousMonth = (currentMonth == 1) ? 12 : currentMonth - 1;          //Mês anterior.
            int previousYear = (currentMonth == 1) ? currentYear - 1 : currentYear;   //Ano do mês anterior.
            int daysInPrevMonth = DateTime.DaysInMonth(previousYear, previousMonth);  //Quantidade de dias no mês anterior.

            int nextMonth = (currentMonth == 12) ? 1 : currentMonth + 1;              //Mês sucessor.
            int nextYear = (currentMonth == 12) ? currentYear + 1 : currentYear;      //Ano do mês sucessor.

            int gridIndex = 0;

            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    DayItemLabel dayItemLabel = (DayItemLabel)DayItems.GetItem(row, col);

                    if (gridIndex < firstDayOfWeek)
                    {
                        // Dias do mês anterior
                        int day = daysInPrevMonth - firstDayOfWeek + gridIndex + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = previousMonth;
                        dayItemLabel.Year = previousYear;
                        dayItemLabel.ForeColor = SecondaryForeColor;
                    }
                    else if (gridIndex < firstDayOfWeek + daysInCurrentMonth)
                    {
                        // Dias do mês atual
                        int day = gridIndex - firstDayOfWeek + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = currentMonth;
                        dayItemLabel.Year = currentYear;
                        dayItemLabel.ForeColor = ForeColor;
                    }
                    else
                    {
                        // Dias do próximo mês
                        int day = gridIndex - (firstDayOfWeek + daysInCurrentMonth) + 1;
                        dayItemLabel.Day = day;
                        dayItemLabel.Month = nextMonth;
                        dayItemLabel.Year = nextYear;
                        dayItemLabel.ForeColor = SecondaryForeColor;
                    }
                    gridIndex++;
                }
            }
        }

        /// <summary>
        /// Método que é invocado no Click de DayItemLabel.
        /// </summary>
        protected void OnDayItemLabelClick(int row, int col)
        {
            var item = (DayItemLabel)DayItems.GetItem(row, col);

            if (parentControl is CustomDatePicker dp)
            {
                dp.Day = item.Day;
                dp.Month = item.Month;
                dp.Year = item.Year;
                dp.CloseDropDownInstance();
            }
        }

    }
}
