using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        // Estado do "piscar" do caret (controlado pelo timer).
        // A regra de quando o caret pode aparecer (foco/seleção) fica no Draw.
        private bool caretBlinkState = true;

        /// <summary>
        /// Define se o Caret PODE ser visível -> apenas se 
        /// <para><see cref="HasSelection"/> for "false" e
        /// </para><see cref="InnerControl.IsFocused"/> for "true".
        /// </summary>
        private bool CanShowCaret => IsFocused && !HasSelection;

        /// <summary>
        /// Define se o Caret está visível ->
        /// Valor utilizado apenas em <see cref="OnPaint"/>
        /// 
        /// é obrigatoriamente "false" se
        /// pelo menos uma das variáveis tiver o valor ->
        /// <para><see cref="CanShowCaret"/> for "true",</para>
        /// <see cref="caretBlinkState"/> for "true".
        /// </summary>
        public bool CaretVisible => CanShowCaret && caretBlinkState;


        /// <summary>
        /// Índice do caret dentro de <see cref="Text"/>
        /// -> Valor limitado no intervalo numérico entre 0 e Text.Length.
        /// </summary>
        public int CaretIndex
        {
            get => caretIndex;
            set
            {
                int limitedValue = Nhegaz.MathMethods.Clamp(value, 0, Text.Length); //Valor limitado entre 0 e Text.Length.
                if (caretIndex != limitedValue)                                    //Se o valor atual for diferente do novo.
                { caretIndex = limitedValue; RestartCaretBlink(); }                //Atualiza o valor e reinicia o Blink.
            }
        }

        /// <summary>Localização calculada do Caret -> 
        /// Valor totalmente dependente de ->
        /// <see cref="TextLocation"/> e 
        /// <see cref="CaretIndex"/>.
        /// </summary>
        public Point CaretLocation
        {
            get
            {
                Point caretLocation = NhegazSizeMethods.
                LocationByIndex(TextLocation, Text, CaretIndex, Font);          //Incremento de deslocamento por tamanho dos caracteres.
                //if (caretLocation.X + CaretSize.Width >= Width) caretLocation -= CaretSize.Width
                return caretLocation;
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
