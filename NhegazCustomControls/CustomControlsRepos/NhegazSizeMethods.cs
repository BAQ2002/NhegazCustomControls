using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public static class NhegazSizeMethods
    {

        /// <summary>
        /// Retorna o Tamanho(Width, Height) exato a partir de um texto e uma Font.
        /// </summary>
        public static Size TextExactSize(string text, Font font)
        {
            Size size = TextRenderer.MeasureText(
                text,
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );
            return size;
        }
        public static Size TextProportionalSize(string text, Font font, float? proportion = 1f)
        {
            Size size = TextRenderer.MeasureText(
                text,
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );
            float scale = proportion ?? 1f;
            size.Width = (int)(size.Width * scale);
            size.Height = (int)(size.Height * scale);
            return size;
        }

        public static Size FontUnitSize(Font font)
        {
            Size size = TextRenderer.MeasureText(
                "0",
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );
            return size;
        }

        public static SizeF textExactSizeTrue(string text, Font font)
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                StringFormat format = StringFormat.GenericTypographic;
                format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

                return g.MeasureString(text, font, int.MaxValue, format);
            }
        }
    }
}
