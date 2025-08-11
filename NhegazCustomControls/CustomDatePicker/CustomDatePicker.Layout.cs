using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
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
            selectedDay.Location = new Point(HorizontalPadding, VerticalPadding);
            dayDropDownIcon.Location = new Point(selectedDay.Location.X + selectedDay.Width, VerticalPadding);

            daySlashMonth.Location = new Point(dayDropDownIcon.Location.X + dayDropDownIcon.Width, VerticalPadding);

            selectedMonth.Location = new Point(daySlashMonth.Location.X + daySlashMonth.Width, VerticalPadding);
            monthDropDownIcon.Location = new Point(selectedMonth.Location.X + selectedMonth.Width, VerticalPadding);

            monthSlashYear.Location = new Point(monthDropDownIcon.Location.X + monthDropDownIcon.Width, VerticalPadding);

            selectedYear.Location = new Point(monthSlashYear.Location.X + monthSlashYear.Width, VerticalPadding);
            yearDropDownIcon.Location = new Point(selectedYear.Location.X + selectedYear.Width, VerticalPadding);
        }


        protected override void SetMinimumSize()
        {
            base.SetMinimumSize();
            int X = yearDropDownIcon.Location.X + yearDropDownIcon.Width + VerticalPadding;
            int Y = (VerticalPadding * 2) + yearDropDownIcon.Height;
            MinimumSize = new Size(X, Y);
        }


    }
}
