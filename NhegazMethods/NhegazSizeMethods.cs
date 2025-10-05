using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public enum ReferenceDimension
    {
        None,
        Width,
        Height
    }

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
        public static Size TextProportionalSize(string text, Font font, float proportion = 1f)
        {
            Size size = TextRenderer.MeasureText(
                text,
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );

            float scale = proportion;

            size.Width = (int)(size.Width * scale);
            size.Height = (int)(size.Height * scale);
            return size;
        }

     
        public static Size TextSquareSizeByReference(string text, Font font, float proportion = 1f, ReferenceDimension referenceDimension = ReferenceDimension.None)
        {
            Size size = TextRenderer.MeasureText(
                text,
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );

            float scale = proportion;

            if (referenceDimension == ReferenceDimension.Height)
            {
                return new((int)(size.Height * scale), (int)(size.Height * scale));
            }
            else if (referenceDimension == ReferenceDimension.Width)
            {
                return new((int)(size.Width * scale), (int)(size.Width * scale));
            }

            return size;
        }
        /// <summary>
        /// Retorna um Tamanho com base na text, font, widthProportion e heightProportion
        /// </summary>
        public static Size TextProportionalSize(string text, Font font, float widthProportion = 1f, float heightProportion = 1f)
        {
            Size size = TextRenderer.MeasureText(
                text,
                font,
                new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine
            );

            float widthScale = widthProportion;
            float heightScale = heightProportion;

            size.Width = (int)(size.Width * widthScale);
            size.Height = (int)(size.Height * heightScale);
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
    }
}
