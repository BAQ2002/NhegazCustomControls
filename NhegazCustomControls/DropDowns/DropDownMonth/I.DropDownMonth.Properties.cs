using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownMonth
    {
        class MonthItemLabel : InnerLabel
        {
            private int month;
            public int Month
            {
                get => month;
                set { month = value; }
            }
            public int Year { get; set; }
            public MonthItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            { }
        }

        protected CustomControl parentControl;

        private int NumberOfRows = 4;
        private int NumberOfColumns = 4;

        private int CurrentYear;

        public HeaderFeature Header { get; set; }
        MatrixFeature IHasMatrix.Matrix => MonthItems;
        public MatrixFeature MonthItems { get; private set; }

        private InnerLabel YearLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);
       
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                BackwardIcon.Font = value;
                ForwardIcon.Font = value;
                YearLabel.Font = new Font(value, FontStyle.Bold);
                UpdateLayout();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                BackwardIcon.ForeColor = value;
                ForwardIcon.ForeColor = value;
                YearLabel.ForeColor = value;
                Invalidate();
            }
        }

    }
}
