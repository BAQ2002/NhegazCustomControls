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

            AdjustTextLocation();
        }

        private void AdjustTextLocation()
        {
            Size textSize = NhegazSizeMethods.TextExactSize(Text, Font);

            int textX = 0, horizontalPadding = GetHorizontalPadding();
            int textY = 0, verticalPadding = GetVerticalPadding();

            switch (TextHorizontalAlignment)
            {
                case TextHorizontalAlignment.Left:
                    textX = horizontalPadding;
                    break;
                case TextHorizontalAlignment.Center:
                    textX = (Size.Width - textSize.Width) / 2;
                    if (ApplyHorizontalPaddingWhenCentered)
                        textX += (Padding.Left - Padding.Right) / 2;
                    break;
                case TextHorizontalAlignment.Right:
                    textX = Size.Width - (textSize.Width + horizontalPadding);
                    break;
            }

            switch (TextVerticalAlignment)
            {
                case TextVerticalAlignment.Top:
                    textY = verticalPadding;
                    break;
                case TextVerticalAlignment.Center:
                    textY = (Size.Height - textSize.Height) / 2;
                    if (ApplyVerticalPaddingWhenCentered)
                        textY += (Padding.Top - Padding.Bottom) / 2;
                    break;
                case TextVerticalAlignment.Bottom:
                    textY = Size.Height - (textSize.Height + verticalPadding);
                    break;
            }
            textRelativeLocation = new Point(textX, textY);
        }

        protected override void SymmetricalCircleAdjust()
        {
            base.SymmetricalCircleAdjust();

            TextHorizontalAlignment = TextHorizontalAlignment.Center;
            TextVerticalAlignment = TextVerticalAlignment.Center;
        }
    }
}
