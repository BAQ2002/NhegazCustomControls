using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class DropDownYear
    {
        class YearItemLabel : InnerLabel
        {
            private int year;
            public int Year
            {
                get => year;
                set
                {
                    year = value;
                    Text = year.ToString(); // sem chamar Size de novo
                }
            }
            public YearItemLabel(bool autoSizeBasedOnText = true) : base(autoSizeBasedOnText)
            { }
        }
        public HeaderFeature Header { get; set; }

        protected CustomControl parentControl;

        private InnerLabel DecadeLabel = new();
        private InnerButton BackwardIcon = new(ButtonIcon.Backward, BackGroundShape.SymmetricCircle);
        private InnerButton ForwardIcon = new(ButtonIcon.Forward, BackGroundShape.SymmetricCircle);
        private YearItemLabel[,] YearItemsLabels;
        

        private int CurrentDecade;
        private int DecadeLastYear;

        private int NumberOfRows = 2;
        private int NumberOfColumns = 5;

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                BackwardIcon.Font = value;
                ForwardIcon.Font = value;
                DecadeLabel.Font = value;
                AdjustControlSize();
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
                DecadeLabel.ForeColor = value;
                Invalidate();
            }
        }
    }
}
