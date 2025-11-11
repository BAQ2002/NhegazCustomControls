using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        private int caretIndex = 0;
        private const int CaretBlinkIntervalMs = 500;
        private readonly System.Windows.Forms.Timer caretTimer;
        private bool caretVisible = true;

        /// <summary>Índice do caret dentro de <see cref="Text"/> (0..Length).</summary>
        public int CaretIndex
        {
            get => caretIndex;
            set
            {
                int maxValue = Math.Max(0, Math.Min(Text.Length, value));
                if (caretIndex != maxValue)
                {
                    caretIndex = maxValue;
                }
            }
        }

        /// <summary>
        /// Localização do Caret dependente do CaretIndex.
        /// </summary>
        public Point CaretLocation
        {
            get
            {
                int caretX = TextLocation.X                                           //Localização X(0) do Texto.
                           + CaretIndex * NhegazSizeMethods.FontUnitSize(Font).Width; //Incremento do deslocamento por tamanho de carácter.
                if(CaretIndex >= Text.Length - 1)                                     //Se o CaretIndex estiver na posição após o último carácter Escrito:
                    caretX -= CaretSize.Width;                                        //Subtrai a largura do caret para caber na "caixa" do texto.
                                                                                      
                int caretY = TextLocation.Y;                                          //Localização Y(0) do Texto.
                return new(caretX, caretY);
            }
        }

        public Size CaretSize
        {
            get => new(1, NhegazSizeMethods.FontUnitSize(Font).Height); //Size = (1, Font.Height).          
        }

        public Rectangle CaretRectangle
        {
            get => new(CaretLocation, CaretSize);
        }

        public Color CaretColor{ get; set; }

        public Color CaretHoverColor { get; set; }
    }
}
