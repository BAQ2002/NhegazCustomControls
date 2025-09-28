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
            public int Year { get; set; }
            public int Month { get; set; }
            public DayItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            { }
        }
        MatrixFeature IHasMatrix.Matrix => DayItems;
        public MatrixFeature DayItems { get; private set; }
        VectorFeature IHasVector.Vector => WeekDays;
        public VectorFeature WeekDays { get; private set; }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Cabeçalho")]
        public HeaderFeature Header { get; set; }

        protected CustomControl parentControl;

        private int NumberOfRows;
        private int NumberOfColumns;

        private int CurrentMonth;
        private int CurrentYear;

        private InnerLabel MonthLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);

        //private DayItemLabel[,] DayItems;//Matriz composta pelos Labels de dias.
        public DayOfWeek StartOfWeek { get; set; } = DayOfWeek.Sunday; //Início da semana configurável; padrão Brasil: Domingo

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
