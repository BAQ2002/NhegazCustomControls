using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        private int caretIndex = 0; 
        private const int CaretBlinkIntervalMs = 500; 
        private readonly System.Windows.Forms.Timer caretTimer;
        private bool caretVisible = true;

        /// <summary>
        /// Índice do caret dentro de <see cref="Text"/>
        /// -> Valor limitado no intervalo numérico entre 0 e Text.Length.
        /// </summary>
        public int CaretIndex
        {
            get => caretIndex;
            set
            {
                int maxValue = Math.Max(0, Math.Min(Text.Length, value)); //Valor limitado entre 0 e Text.Length.
                if (caretIndex != maxValue)
                {
                    caretIndex = maxValue;
                }
            }
        }

        /// <summary>Localização calculada do Caret -> Pode ser modificada apenas por <see cref="CaretIndex"/>.</summary>
        public Point CaretLocation
        {
            get
            {
                
                int caretX = TextLocation.X                                           //Localização X(0) do Texto.
                           + CaretIndex * NhegazSizeMethods.FontUnitSize(Font).Width; //Incremento de deslocamento por tamanho dos caracteres.
                if (Text.Length > 1 && CaretIndex >= Text.Length - 1)                 //Se o CaretIndex estiver na posição após o último carácter Escrito:
                    caretX -= CaretSize.Width;                                        //Subtrai a largura do caret para caber na "caixa" do texto.
                                                                                      
                int caretY = TextLocation.Y;                                          //Localização Y(0) do Texto.
                return new(caretX, caretY);
            }
        }

        /// <summary>Tamanho do Caret -> Fixado em Largura = 1 e Altura = <see cref="Font.Height"/>.</summary>
        public Size CaretSize
        {
            get => new(1, NhegazSizeMethods.FontUnitSize(Font).Height); //Size = (1, Font.Height).          
        }

        /// <summary>
        /// Retângulo correspondente a <see cref="CaretLocation"/>
        /// e <see cref="CaretSize"/> -> utilizado exclusivamente em <see cref="DrawCaret"/>.
        /// </summary>
        public Rectangle CaretRectangle
        {
            get => new(CaretLocation, CaretSize);
        }

        /// <summary>Cor padrão do Caret.</summary>
        public Color CaretColor { get; set; } = SystemColors.ControlText;

        /// <summary>Cor do Caret quando o cursor do mouse está sobre o <see cref="InnerTextBox"/>.</summary>
        public Color CaretHoverColor { get; set; } = SystemColors.ControlText;
    }
}
