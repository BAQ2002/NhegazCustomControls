using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerLabel
    {
        protected override void UpdateLayout()
        {
            base.UpdateLayout();

            if (SizeBasedOnText == true)
                Size = NhegazSizeMethods.TextExactSize(Text, Font);

            TextFeatures.AdjustLocation();
        }


        protected override void SymmetricalCircleAdjust()
        {
            base.SymmetricalCircleAdjust();

            TextFeatures.TextHorizontalAlignment = TextHorizontalAlignment.Center;
            TextFeatures.TextVerticalAlignment = TextVerticalAlignment.Center;
        }
    }
}
