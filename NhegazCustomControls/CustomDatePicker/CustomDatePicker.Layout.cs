using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
        public override void AdjustControlSize()
        {
            AdjustInnerSizes();  AdjustInnerLocations();
        }
        protected override void AdjustInnerSizes()
        {
            selectedDay.Width = NhegazSizeMethods.TextExactSize("00", Font).Width;
            selectedDay.Height = Font.Height;

            dayDropDownIcon.Height = Font.Height;

            selectedMonth.Width = NhegazSizeMethods.TextExactSize("00", Font).Width;
            selectedMonth.Height = Font.Height;

            monthDropDownIcon.Height = Font.Height;

            selectedYear.Width = NhegazSizeMethods.TextExactSize("0000", Font).Width;
            selectedYear.Height = Font.Height;

            yearDropDownIcon.Height = Font.Height;
        }
        protected override void AdjustInnerLocations()
        {
            selectedDay.Location = new Point(RelativeLeftX(), RelativeCenterY(selectedDay.Height));
            dayDropDownIcon.SetLocation(selectedDay.Location.X + selectedDay.Width, RelativeCenterY(dayDropDownIcon));

            daySlashMonth.SetLocation(dayDropDownIcon.Right, RelativeCenterY(daySlashMonth));

            selectedMonth.Location = new Point(daySlashMonth.Right, RelativeCenterY(selectedMonth.Height));
            monthDropDownIcon.SetLocation(selectedMonth.Location.X + selectedMonth.Width, RelativeCenterY(monthDropDownIcon));

            monthSlashYear.SetLocation(monthDropDownIcon.Right, RelativeCenterY(monthDropDownIcon));

            selectedYear.Location = new Point(monthSlashYear.Right, RelativeCenterY(selectedYear.Height));
            yearDropDownIcon.SetLocation(selectedYear.Location.X + selectedYear.Width, RelativeCenterY(yearDropDownIcon));
        }

        protected override void SetMinimumSize()
        {
            int innerControlsWidth = 0;
            foreach (InnerControl ic in InnerControls.GetAll) 
                innerControlsWidth += ic.Width;

            int innerControlsHeight = 0;
            foreach (InnerControl ic in InnerControls.GetAll) 
                if (innerControlsWidth < ic.Height) innerControlsWidth = ic.Height;

            int paddingWidth = BorderHorizontalBoundsSum + InnerHorizontalPadding;
            int paddingHeight = BorderVerticalBoundsSum;

            int minimumWidth = innerControlsWidth + paddingWidth;
            int minimumHeight = innerControlsHeight + paddingHeight;

            MinimumSize = new Size(minimumWidth, minimumHeight);
        }

    }
}
