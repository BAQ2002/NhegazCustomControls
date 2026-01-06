using System;
using System.Drawing;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    public partial class InnerTextBox : InnerControl, IHasText, IAcceptsKeyboard
    {
        public TextFeature TextFeatures { get; }

        private string text = string.Empty;
      
        private TextCharFilter textCharFilter = TextCharFilter.None;

        /// <summary></summary>
        private TextFormatFilter textFormatFilter = TextFormatFilter.None;
      
        private EventHandler? applyTextFormatHandler;

        /// <summary>Pilha de estados para Ctrl+Z.</summary>
        private readonly Stack<UndoState> undoStack = new();

        /// <summary>
        /// Esttrutura de estado para desfazer (Undo): guarda texto, posição do caret e seleção.
        /// </summary>
        private struct UndoState
        {
            public string Text;
            public int CaretIndex;
            public int SelectionStartIndex;
            public int SelectionEndIndex;
        }
        

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyPress;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyDown;
        public override Color HoverBackgroundColor { get; set; } = SystemColors.Window;
        public override Color HoverForeColor { get; set; } = SystemColors.ControlText;

        /// <summary>
        /// Recebe um <see cref="char"/> como parâmetro -> 
        /// retorna true ou false a depender do tipo de filtro definido.
        /// <para>Valor modificado exclusivamente por <see cref="TextCharFilter"/>.</para>
        /// Acionado em <see cref="RaiseKeyPress"/> ->
        /// Se a função retornar falso não insere o carácter.
        /// </summary>
        private Func<char, bool>? CharFilter { get; set; } = null;

        /// <summary>
        /// Define qual o tipo de filtro de carácteres que o elemento utiliza,
        /// a partir do valor -> Define o valor de <see cref="CharFilter"/> ->
        /// executa esse filtro no texto atual.
        /// </summary>
        public TextCharFilter TextCharFilter
        {
            get => textCharFilter;
            set 
            {
                textCharFilter = value;

                if (value == TextCharFilter.OnlyNumbers)                    //Se TextCharFilter for OnlyNumbers -> 
                {
                    CharFilter = char.IsDigit;                              //Define a função CharFilter para retornar verdadeiro apenas para números.
                    
                    for (int i = Text.Length - 1; i >= 0; i--)              //Percorre do c ao primeiro carácter do texto.
                    { 
                        char textChar = Text[i]; if (!CharFilter(textChar)) //Se o char Text[i] não for aceito pelo CharFilter ->
                        { Text = Text.Remove(i, 1); }                       //Remova o char do texto.
                    }
                }
                else if(value == TextCharFilter.None)                       //Se TextCharFilter for None ->               
                { CharFilter = null; }                                      //Define a função CharFilter como nula.
            }
        }

        /// <summary>
        /// Define se o texto tem um formato específico ->
        /// Aplica esse formato no texto atual ->
        /// Define que o <see cref="InnerControl.LostFocus"/>
        /// deve chamar <see cref="applyTextFormatHandler"/> ->
        /// <see cref="ApplyTextFormat"/>.
        /// </summary>
        public TextFormatFilter TextFormatFilter
        {
            get => textFormatFilter;
            set
            {
                textFormatFilter = value;

                applyTextFormatHandler ??= (s, e) => ApplyTextFormat(); //Se o applyTextFormatHandler
                if (value != TextFormatFilter.None)
                {
                    ApplyTextFormat();
                    LostFocus += applyTextFormatHandler;
                }
                else
                { LostFocus -= applyTextFormatHandler; }
            }
        }

        /// <summary>
        /// Comprimento máximo do texto.
        /// </summary>
        public int MaxLength { get; set; } = 0;                // 0 = sem limite
        
        /// <summary>Define se deve ser usado três pontos "..." se o texto não couber no tamanho atual.</summary>
        public bool UseEllipsis { get; set; } = false;


        /// <summary>Define se o tamanho deve ser baseado no texto.</summary>
        public bool SizeBasedOnText { get; set; } = false;

        /// <summary>Coordenada(positionX,y) absoluta do texto.</summary>
        public Point TextLocation => new(Location.X + TextFeatures.TextLocation.X, Location.Y + TextFeatures.TextLocation.Y);
       

        /// <summary>Tamanho do texto atual -> totalmente dependende de <see cref="Text"/>.Length e <see cref="Font"/>.</summary>
        public Size TextSize
        {
            get 
            {
                int width  = Text.Length * NhegazSizeMethods.FontUnitSize(Font).Width;
                int height = NhegazSizeMethods.FontUnitSize(Font).Height;
                return new(width, height);
            }             
        }

        /// <summary>
        /// Retângulo correspondente a <see cref="TextLocation"/>
        /// e <see cref="TextSize"/> -> utilizado exclusivamente em <see cref="RaiseClick"/>.
        /// </summary>
        public Rectangle TextRectangle
        {
            get => new(TextLocation, TextSize);      
        }

        /// <summary>Texto atual.</summary>
        public string Text
        {
            get => text;
            set
            {
                text = value ?? string.Empty;                   //Se o novo valor for nulo.
                CaretIndex = Math.Min(CaretIndex, text.Length); //Atualiza o CaretIndex respeitando o limite do texto atual.
                UpdateLayout();   
            }
        }

        public override Font Font
        {
            get => base.Font;
            set { base.Font = value; UpdateLayout(); }
        }

        public override int Height
        {
            get => base.Height;
            set { SizeBasedOnText = false; base.Height = value; }
        }

        public override int Width
        {
            get => base.Width;
            set { SizeBasedOnText = false; base.Width = value; }
        }                  
    }
}
