using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
        public override void UpdateLayout()
        {
            SetInnerSizes(); SetInnerLocations(); SetMinimumSize();
        }
        
        public Size GetControlSize()
        {
            Size controlSize = GetContentSize() + GetPaddingSize();
            return controlSize;
        }

        /// <summary>
        /// Retorna os valores de Largura e Altura que as propriedades de Padding
        /// ocupam com base em uma política específica para cada CustomControl.
        /// </summary>
        /// <returns>Size(paddingWidth, paddingHeight)</returns>
        public Size GetPaddingSize()
        {
            int paddingWidth = BorderHorizontalBoundsSum;
            int paddingHeight = BorderVerticalBoundsSum;  

            return new Size(paddingWidth, paddingHeight);
        }

        /// <summary>
        /// Retorna os valores de Largura e Altura que os InnerControls ocupam
        /// com base em uma política específica para cada CustomControl.
        /// </summary>
        /// <returns>Size(contentWidth, contentHeight)</returns>
        public Size GetContentSize()
        {
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
         
        
        protected override void SetInnerSizes()
        {
            dayTextBox.Size = NhegazSizeMethods.TextExactSize("00", Font);
            dayDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));

            monthTextBox.Size = NhegazSizeMethods.TextExactSize("00", Font);
            monthDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));

            yearTextBox.Size = NhegazSizeMethods.TextExactSize("0000", Font);
            yearDropDownIcon.SetSize(NhegazSizeMethods.TextExactSize("00", Font));
        }

        protected override void SetInnerLocations()
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
            int minimumWidth = GetControlSize().Width;
            int minimumHeight = GetControlSize().Height;

            MinimumSize = new Size(minimumWidth, minimumHeight);
        }

    }
}
