using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public partial class InnerLabel : InnerControl
    {        
        /// <summary>
        /// Construtor opcional para definir <see cref="SizeBasedOnText"/>.
        /// </summary>
        /// <param name="autoSizeBasedOnText"></param>
        public InnerLabel(bool autoSizeBasedOnText = true) : base()
        {
            SizeBasedOnText = autoSizeBasedOnText;
        }
  
        /// <summary>
        /// Retorna o valor do ControlPadding Left|Right a partir de HorizontalPaddingMode.
        /// </summary>
        /// <returns></returns>
        private int GetHorizontalPadding()
        {
            int fontWidth = NhegazSizeMethods.FontUnitSize(Font).Width;
            return HorizontalPaddingMode switch
            {
                HorizontalPaddingMode.None => 0,
                HorizontalPaddingMode.HalfFontWidth => fontWidth / 2,
                HorizontalPaddingMode.OneFourthFontWidth => fontWidth / 4,
                HorizontalPaddingMode.Absolute => TextHorizontalAlignment == TextHorizontalAlignment.Left ? Padding.Left : Padding.Right,
                _ => 0
            };
        }

        /// <summary>
        /// Retorna o valor do ControlPadding borderTop|Bottom a partir de VerticalPaddingMode.
        /// </summary>
        /// <returns></returns>
        private int GetVerticalPadding()
        {
            int fontHeight = NhegazSizeMethods.FontUnitSize(Font).Height;
            return VerticalPaddingMode switch
            {
                VerticalPaddingMode.None => 0,
                VerticalPaddingMode.HalfFontHeight => fontHeight / 2,
                VerticalPaddingMode.OneFourthFontHeight => fontHeight / 4,
                VerticalPaddingMode.Absolute => TextVerticalAlignment == TextVerticalAlignment.Top ? Padding.Top : Padding.Bottom,
                _ => 0
            };
        }

        
    }
}
