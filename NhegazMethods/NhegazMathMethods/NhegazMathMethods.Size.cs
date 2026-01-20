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

        /// <summary>
        /// Retorna uma coordenada <see cref="Point"/> 
        /// a partir de um índice <see cref="int"/> 
        /// <paramref name="index"/> de uma posição existente 
        /// na <see cref="string"/> <paramref name="text"/> -> 
        /// <paramref name="startLocation"/>.X  
        /// <para>
        /// <paramref name="text"/>[0 .. (<paramref name="index"/> + 1)].Width 
        /// - <see cref="char"/>.Width
        /// </para>
        /// 
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Point LocationByIndex(Point startLocation, string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text))       return startLocation; //0.a)Se o text for nulo ou vazio.
            if (index < 0 || index > text.Length) return startLocation; //0.b)Se o index for inválido.

            int widthUpToIndex = TextExactSize                          //II.a)Largura do texto da posição
            (text.Substring(0, index), font).Width;                     //II.b)text[0] até text[index].

            int locationX = startLocation.X + widthUpToIndex;           //III.a)Localização X.
            int locationY = startLocation.Y;                            //III.b)Localização Y.

            return new(locationX, locationY);
        }

        public static Size TextCharSize(string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text)) return Size.Empty;        //0.a)Se o text for nulo ou vazio.
            if (index < 0 || index >= text.Length) return Size.Empty; //0.b)Se o index for inválido.

            Size charSize = TextExactSize            //Largura do texto composto apenas
            (text.Substring(index, 1), font);  //pelo caracter do índice text[index].

            return charSize;
        }

        

        public static Rectangle TextCharRect(Point startLocation, string text, int index, Font font)
        {
            if (string.IsNullOrEmpty(text))        return Rectangle.Empty; //0.a)Texto nulo ou vazio.
            if (index < 0 || index >= text.Length) return Rectangle.Empty; //0.b)Índice inválido.

           

            Size charSize      = TextCharSize(text, index, font);          //II.a)Tamanho do carácter ->   Em relação ao X = 0 do texto.
            Point charLocation = LocationByIndex(startLocation, text, index, font);  //IV.b)Posição X do carácter -> Em relação ao X = 0 do texto.

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
