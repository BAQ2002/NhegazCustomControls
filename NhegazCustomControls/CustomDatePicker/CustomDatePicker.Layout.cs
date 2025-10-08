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
            Size = GetSize(); AdjustInnerSizes();  AdjustInnerLocations(); SetMinimumSize();
        }
        public Size GetSize()
        {
            Size contentSize = GetContentSize();
            Size paddingSize = GetPaddingSize();

            int sizeWidth = contentSize.Width
                          + paddingSize.Width;

            int sizeHeight = contentSize.Height
                           + paddingSize.Height;

            return new Size(sizeWidth, sizeHeight);
        }
        public Size GetPaddingSize()
        {
            int paddingWidth = BorderHorizontalBoundsSum;
            int paddingHeight = BorderVerticalBoundsSum;  

            return new Size(paddingWidth, paddingHeight);
        }
        public Size GetContentSize()
        {
            Size dayTextBoxSize = NhegazSizeMethods.TextExactSize("00", Font);
            Size dayDropDownIconSize = NhegazSizeMethods.TextExactSize("00", Font);

            Size monthTextBoxSize = NhegazSizeMethods.TextExactSize("00", Font);
            Size monthDropDownIconSize = NhegazSizeMethods.TextExactSize("00", Font);

            Size yearTextBoxSize = NhegazSizeMethods.TextExactSize("0000", Font);
            Size yearDropDownIconSize = NhegazSizeMethods.TextExactSize("00", Font);

            Size slashsSize = NhegazSizeMethods.TextExactSize("/", Font);

            int contentWidth = dayTextBox.Width
                             + dayDropDownIcon.Width
                             + monthTextBox.Width
                             + monthDropDownIcon.Width
                             + yearTextBox.Width
                             + yearDropDownIcon.Width
                             + 2 * slashsSize.Width;

            int contentHeight = NhegazSizeMethods.TextExactSize("00", Font).Height;

            return new Size(contentWidth, contentHeight);
        }
        protected override void AdjustInnerSizes()
        {
            dayTextBox.Width = NhegazSizeMethods.TextExactSize("00", Font).Width;
            dayTextBox.Height = Font.Height;

            dayDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));

            monthTextBox.Width = NhegazSizeMethods.TextExactSize("00", Font).Width;
            monthTextBox.Height = Font.Height;

            monthDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));

            yearTextBox.Width = NhegazSizeMethods.TextExactSize("0000", Font).Width;
            yearTextBox.Height = Font.Height;

            yearDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));
        }
        protected override void AdjustInnerLocations()
        {
            dayTextBox.Location = new Point(RelativeLeftX(), RelativeCenterY(dayTextBox.Height));
            dayDropDownIcon.SetLocation(dayTextBox.Location.X + dayTextBox.Width, RelativeCenterY(dayDropDownIcon));

            daySlashMonth.SetLocation(dayDropDownIcon.Right, RelativeCenterY(daySlashMonth));

            monthTextBox.Location = new Point(daySlashMonth.Right, RelativeCenterY(monthTextBox.Height));
            monthDropDownIcon.SetLocation(monthTextBox.Location.X + monthTextBox.Width, RelativeCenterY(monthDropDownIcon));

            monthSlashYear.SetLocation(monthDropDownIcon.Right, RelativeCenterY(monthDropDownIcon));

            yearTextBox.Location = new Point(monthSlashYear.Right, RelativeCenterY(yearTextBox.Height));
            yearDropDownIcon.SetLocation(yearTextBox.Location.X + yearTextBox.Width, RelativeCenterY(yearDropDownIcon));
        }

        protected override void SetMinimumSize()
        {
            int minimumWidth = GetSize().Width;
            int minimumHeight = GetSize().Height;

            MinimumSize = new Size(minimumWidth, minimumHeight);
        }

    }
}
