using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
          
        public static int TextCharWidth(string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;           //Texto nulo ou vazio.
            if (index < 0 || index >= text.Length) return 0;     //Índice inválido.

            string charOnly = text.Substring(index, 1);          //Texto do índice[index].
            int charWidth = TextExactSize(charOnly, font).Width; //Largura do texto do índice[index].
           
            return charWidth;
        }

        /// <summary>
        /// Retorna a localização X de um <see cref="char"/> 
        /// contido na <see cref="string"/> <paramref name="text"/> 
        /// -> selecionado por um índice <see cref="int"/> 
        /// <paramref name="index"/> em relação à localização X = 0 do texto. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static int TextCharLocX(string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text)) return 0;             //0.a)Texto nulo ou vazio.
            if (index < 0 || index > text.Length) return 0;       //0.b)Índice inválido.


            int charWidth = TextCharWidth(text, index, font); //I_.a)Largura do texto do índice[index].
            if (index == text.Length) index -= 1;
            int IncludedWidht = TextExactSize                     //II.a)Largura do texto do ->
            (text.Substring(0, index + 1), font).Width;           //II.b)índice[0] até o índice[index].

            int charLocX = IncludedWidht - charWidth;  //Diferença entre a largura I.a) e II.b).

            return charLocX;
        }

        public static Size TextCharSize(string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text)) return Size.Empty;        //Texto nulo ou vazio.
            if (index < 0 || index >= text.Length) return Size.Empty; //Índice inválido.

            int charWidth  = TextCharWidth(text, index, font);        //Largura do texto do índice[index].
            int charHeight = TextExactSize(text, font).Height;        //Altura do texto(padrão).

            return new(charWidth, charHeight);
        }

        

        public static Rectangle TextCharRect(string text, int index, Font font, int startX, int startY)
        {
            if (string.IsNullOrEmpty(text))        return Rectangle.Empty; //0.a)Texto nulo ou vazio.
            if (index < 0 || index >= text.Length) return Rectangle.Empty; //0.b)Índice inválido.

            Size charSize      = TextCharSize(text, index, font);          //II.a)Tamanho do carácter ->   Em relação ao X = 0 do texto.
            Point charLocation = new(TextCharLocX(text, index, font) + startX, startY);  //IV.b)Posição X do carácter -> Em relação ao X = 0 do texto.

            return new(charLocation, charSize);
        }
        public static Size TextProportionalSize(string text, Font font, float proportion = 1f)
        {
            Size size = TextExactSize(text, font);

            size.Width = (int)(size.Width * proportion);
            size.Height = (int)(size.Height * proportion);
            return size;
        }

        public static Size TextSquareSizeByReference(string text, Font font, float proportion = 1f, ReferenceDimension referenceDimension = ReferenceDimension.None)
        {
            Size size = TextExactSize(text, font);

            if (referenceDimension == ReferenceDimension.Height)
            {
                return new((int)(size.Height * proportion), (int)(size.Height * proportion));
            }
            else if (referenceDimension == ReferenceDimension.Width)
            {
                return new((int)(size.Width * proportion), (int)(size.Width * proportion));
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
            Size size = TextExactSize( "0", font);
            return size;
        }
    }
}
