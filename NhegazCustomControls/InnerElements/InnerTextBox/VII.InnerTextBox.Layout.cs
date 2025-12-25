using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {     
        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateLayout()
        {
            base.UpdateLayout();

            if (SizeBasedOnText) //Se SizeBasedOnText for verdadeiro -> define o tamanho exatamente igual o texto.
            {
                Size = NhegazSizeMethods.TextExactSize(
                    string.IsNullOrEmpty(Text) ? " " : Text, Font);
            }

            TextFeatures.AdjustLocation();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SymmetricalCircleAdjust()
        {
            base.SymmetricalCircleAdjust();
            TextFeatures.TextHorizontalAlignment = TextHorizontalAlignment.Center;
            TextFeatures.TextVerticalAlignment = TextVerticalAlignment.Center;
        } 
    }
}
