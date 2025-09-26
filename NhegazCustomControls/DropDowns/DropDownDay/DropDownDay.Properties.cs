using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownDay
    {
        class DayItemLabel : InnerLabel
        {
            private int day;
            public int Day
            {
                get => day;
                set { day = value; Text = day.ToString(); }
            }
            public bool IsCurrentMonth { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public DayItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            { }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Cabeçalho")]
        public HeaderFeature Header { get; set; }
        public MatrixFeature Matrix { get; set; }

        protected CustomControl parentControl;

        private int NumberOfRows;
        private int NumberOfColumns;

        private int CurrentMonth;
        private int CurrentYear;

        private InnerLabel MonthLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);

        private DayItemLabel[,] DayItemsLabels;//Matriz composta pelos Labels de dias.
        public DayOfWeek StartOfWeek { get; set; } = DayOfWeek.Sunday; //Início da semana configurável; padrão Brasil: Domingo

        private InnerLabel[] WeekDayLabels; //Vetor que Armazena os Labels de dias da semana.

        private string[] MonthTexts =  {"null", "Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho",
                                         "Julho", "Agosto", "Setembro", "Outubro", "Novembro","Dezembro" };

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                MonthLabel.Font = new Font(value, FontStyle.Bold); 
                ForwardIcon.Font = value;
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
                MonthLabel.ForeColor = value; 
                ForwardIcon.ForeColor = value;
                BackwardIcon.ForeColor = value;
                Invalidate();
            }
        }
                        
    }
}
