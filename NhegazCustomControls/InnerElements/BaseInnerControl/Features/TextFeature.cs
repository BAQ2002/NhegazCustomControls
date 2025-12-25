using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace NhegazCustomControls
{
    public class TextFeature
    {
        private readonly InnerControl ownerControl;
        private readonly Func<string> GetText;

        /// <summary>
        /// Define qual posição vertical o texto deve usar como âncora para ser alinhado ->
        /// <para>Top,</para>
        /// <para>Center,</para>
        /// <para>Bottom.</para>
        /// </summary>
        private TextVerticalAlignment textVerticalAlignment = TextVerticalAlignment.Center;

        /// <summary>
        /// Define qual posição vertical o texto 
        /// deve usar como âncora para ser alinhado.
        /// <para>Left,</para>
        /// <para>Center,</para>
        /// <para>Right.</para>
        /// </summary>
        private TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left;

        /// <summary>
        /// Define qual a escala de padding é utilizada 
        /// em relação a posição horizontal do texto.
        /// <para>None (fixa o padding no valor 0),</para> 
        /// HalfFontWidth (fixa o padding no valor 1/2 da Font),
        /// <para>OneFourthFontWidth (fixa o padding no valor 1/4 da Font),</para> 
        /// Absolute(não fixa o padding em nenhum valor).
        /// </summary>
        private HorizontalPaddingMode horizontalPaddingMode = HorizontalPaddingMode.None;

        /// <summary>
        /// Define qual a escala de padding é utilizada 
        /// em relação a posição vertical do texto.
        /// <para>None (fixa o padding no valor 0),</para> 
        /// HalfFontHeight (fixa o padding no valor 1/2 da Font),
        /// <para>OneFourthFontHeight (fixa o padding no valor 1/4 da Font),</para> 
        /// Absolute(não fixa o padding em nenhum valor).
        /// </summary>
        private VerticalPaddingMode verticalPaddingMode = VerticalPaddingMode.None;

        /// <summary>
        /// Define se o padding horizontal deve ser aplicado
        /// quando a posição de alinhamento horizontal for 
        /// <see cref="TextHorizontalAlignment.Center"/>.
        /// </summary>
        public bool ApplyHorizontalPaddingWhenCentered { get; set; } = false;

        /// <summary>
        /// Define se o padding vertical deve ser aplicado
        /// quando a posição de alinhamento vertical for 
        /// <see cref="TextVerticalAlignment.Center"/>.
        /// </summary>
        public bool ApplyVerticalPaddingWhenCentered { get; set; } = false;

        public Point TextLocation { get; private set; }

        public TextHorizontalAlignment TextHorizontalAlignment
        {
            get => textHorizontalAlignment;
            set { textHorizontalAlignment = value; AdjustLocation(); }
        }

        public TextVerticalAlignment TextVerticalAlignment
        {
            get => textVerticalAlignment;
            set { textVerticalAlignment = value; AdjustLocation(); }
        }

        public HorizontalPaddingMode HorizontalPaddingMode
        {
            get => horizontalPaddingMode;
            set { horizontalPaddingMode = value; AdjustLocation(); }
        }

        public VerticalPaddingMode VerticalPaddingMode
        {
            get => verticalPaddingMode;
            set { verticalPaddingMode = value; AdjustLocation(); }
        }                    
    
        public TextFeature(InnerControl owner, Func<string> getText)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            GetText = getText ?? throw new ArgumentNullException(nameof(getText));
        }

        private int GetHorizontalPadding()
        {
            int fontWidth = NhegazSizeMethods.FontUnitSize(ownerControl.Font).Width;
            return HorizontalPaddingMode switch
            {
                HorizontalPaddingMode.None => 0,                           //Se HorizontalPaddingMode for None -> retorna 0
                HorizontalPaddingMode.HalfFontWidth => fontWidth / 2,      //Se HorizontalPaddingMode for None -> retorna metade da largura da Font.
                HorizontalPaddingMode.OneFourthFontWidth => fontWidth / 4, //Se HorizontalPaddingMode for None -> retorna 1/4 da largura da Font.
                HorizontalPaddingMode.Absolute =>                          //Se HorizontalPaddingMode for None -> retorna 0
                TextHorizontalAlignment == TextHorizontalAlignment.Left ?  //Verifica se TextVerticalAlignment é Left ->
                ownerControl.Padding.Left : ownerControl.Padding.Right,    //Retorna o valor de Padding.Left ou Padding.Right.
                _ => 0
            };
        }

        private int GetVerticalPadding()
        {
            int fontHeight = NhegazSizeMethods.FontUnitSize(ownerControl.Font).Height;
            return VerticalPaddingMode switch
            {
                VerticalPaddingMode.None => 0,                             //Se VerticalPaddingMode for None -> retorna 0
                VerticalPaddingMode.HalfFontHeight => fontHeight / 2,      //Se VerticalPaddingMode for None -> retorna metade da largura da Font.
                VerticalPaddingMode.OneFourthFontHeight => fontHeight / 4, //Se VerticalPaddingMode for None -> retorna 1/4 da largura da Font.
                VerticalPaddingMode.Absolute =>                            //Se VerticalPaddingMode for None -> retorna 0 ->
                TextVerticalAlignment == TextVerticalAlignment.Top ?       //Verifica se TextVerticalAlignment é Top ->
                ownerControl.Padding.Top : ownerControl.Padding.Bottom,    //Retorna o valor de Padding.Top ou Padding.Bottom.
                _ => 0
            };
        }

        public void AdjustLocation()
        {
            string text = GetText?.Invoke() ?? string.Empty;

            Size textSize = NhegazSizeMethods.TextExactSize(
                string.IsNullOrEmpty(text) ? " " : text,
                ownerControl.Font
            );


            int textX = 0, horizontalPadding = GetHorizontalPadding();
            int textY = 0, verticalPadding = GetVerticalPadding();

            switch (TextHorizontalAlignment)
            {
                case TextHorizontalAlignment.Left:
                    textX = horizontalPadding;
                    break;
                case TextHorizontalAlignment.Center:
                    textX = (ownerControl.Size.Width - textSize.Width) / 2;
                    if (ApplyHorizontalPaddingWhenCentered)
                        textX += (ownerControl.Padding.Left - ownerControl.Padding.Right) / 2;
                    break;
                case TextHorizontalAlignment.Right:
                    textX = ownerControl.Size.Width - (textSize.Width + horizontalPadding);
                    break;
            }

            switch (TextVerticalAlignment)
            {
                case TextVerticalAlignment.Top:
                    textY = verticalPadding;
                    break;
                case TextVerticalAlignment.Center:
                    textY = (ownerControl.Size.Height - textSize.Height) / 2;
                    if (ApplyVerticalPaddingWhenCentered)
                        textY += (ownerControl.Padding.Top - ownerControl.Padding.Bottom) / 2;
                    break;
                case TextVerticalAlignment.Bottom:
                    textY = ownerControl.Size.Height - (textSize.Height + verticalPadding);
                    break;
            }
            TextLocation = new Point(textX, textY);
        }
    }
}
