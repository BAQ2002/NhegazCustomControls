using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NhegazCustomControls
{
    public class TextFeature(InnerControl owner, Func<string> getText)
    {
        private readonly InnerControl ownerInnerControl = owner ?? throw new ArgumentNullException(nameof(owner));
        private readonly Func<string> GetText = getText ?? throw new ArgumentNullException(nameof(getText));

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

        private int GetHorizontalPadding()
        {
            int fontWidth = NhegazSizeMethods.FontUnitSize(ownerInnerControl.Font).Width;
            return HorizontalPaddingMode switch
            {
                HorizontalPaddingMode.None => 0,                           //Se HorizontalPaddingMode for None -> retorna 0
                HorizontalPaddingMode.HalfFontWidth => fontWidth / 2,      //Se HorizontalPaddingMode for None -> retorna metade da largura da Font.
                HorizontalPaddingMode.OneFourthFontWidth => fontWidth / 4, //Se HorizontalPaddingMode for None -> retorna 1/4 da largura da Font.
                HorizontalPaddingMode.Absolute =>                          //Se HorizontalPaddingMode for None -> retorna 0
                TextHorizontalAlignment == TextHorizontalAlignment.Left ?  //Verifica se TextVerticalAlignment é Left ->
                ownerInnerControl.Padding.Left : 
                TextHorizontalAlignment == TextHorizontalAlignment.Right ? 
                ownerInnerControl.Padding.Right : 0,    //Retorna o valor de Padding.Left ou Padding.Right.
                _ => 0
            };
        }

        private int GetVerticalPadding()
        {
            int fontHeight = NhegazSizeMethods.FontUnitSize(ownerInnerControl.Font).Height;
            return VerticalPaddingMode switch
            {
                VerticalPaddingMode.None => 0,                             //Se VerticalPaddingMode for None -> retorna 0
                VerticalPaddingMode.HalfFontHeight => fontHeight / 2,      //Se VerticalPaddingMode for HalfFontHeight -> retorna metade da largura da Font.
                VerticalPaddingMode.OneFourthFontHeight => fontHeight / 4, //Se VerticalPaddingMode for OneFourthFontHeight -> retorna 1/4 da largura da Font.
                VerticalPaddingMode.Absolute =>                            //Se VerticalPaddingMode for Absolute -> retorna 0 ->
                TextVerticalAlignment == TextVerticalAlignment.Top ?       //Verifica se TextVerticalAlignment é Top ->
                ownerInnerControl.Padding.Top :
                TextVerticalAlignment == TextVerticalAlignment.Bottom ?
                ownerInnerControl.Padding.Bottom : 0,    //Retorna o valor de Padding.Top ou Padding.Bottom.
                _ => 0
            };
        }

        public void AdjustLocation()
        {
            string text = GetText?.Invoke() ?? string.Empty;

            Size textSize = NhegazSizeMethods.TextExactSize(
                string.IsNullOrEmpty(text) ? " " : text,
                ownerInnerControl.Font
            );


            int horizontalPadding = GetHorizontalPadding();
            int verticalPadding = GetVerticalPadding();

            int textX = TextHorizontalAlignment switch
            {
                TextHorizontalAlignment.Left => horizontalPadding,
                TextHorizontalAlignment.Center => (ownerInnerControl.Size.Width - textSize.Width) / 2,
                TextHorizontalAlignment.Right => ownerInnerControl.Size.Width - (textSize.Width + horizontalPadding),
                _ => 0
            };

            int textY = TextVerticalAlignment switch
            {
                TextVerticalAlignment.Top => verticalPadding,
                TextVerticalAlignment.Center => (ownerInnerControl.Size.Height - textSize.Height) / 2,
                TextVerticalAlignment.Bottom => ownerInnerControl.Size.Height - (textSize.Height + verticalPadding),
                _ => 0
            };
            TextLocation = new Point(textX, textY);
        }
    }
}
