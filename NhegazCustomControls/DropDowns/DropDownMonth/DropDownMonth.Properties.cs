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
            public int Month { get; set; }
        }

        protected CustomControl parentControl;

        public HeaderFeature Header { get; set; }

        private InnerLabel YearLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.FitRectangle); //Label&&Button para passar para a década anteriror
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.FitRectangle);

        private MonthItemLabel[,] MonthList;

        int NumberOfRows = 3;
        int NumberOfColumns = 4;
    }
}
