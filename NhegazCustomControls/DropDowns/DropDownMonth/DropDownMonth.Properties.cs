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
                set { month = value;Text = month.ToString(); }
            }
            public int Year { get; set; }
            public MonthItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            { }
        }

        protected CustomControl parentControl;

        public HeaderFeature Header { get; set; }
        MatrixFeature IHasMatrix.Matrix => MonthItems;
        public MatrixFeature MonthItems { get; private set; }

        private InnerLabel YearLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);
 
        int NumberOfRows = 4;
        int NumberOfColumns = 4;
    }
}
