using System;
using System.Drawing;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    /// <summary>
    /// Inner "textbox" com mesmos ajustes de posição do InnerLabel e caret funcional.
    /// </summary>
    public partial class InnerTextBox : InnerControl, IAcceptsKeyboard
    {
        private string text = string.Empty;
        private Point textRelativeLocation = Point.Empty;

        private TextVerticalAlignment textVerticalAlignment = TextVerticalAlignment.Center;
        private TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left;

        private HorizontalPaddingMode horizontalPaddingMode = HorizontalPaddingMode.None;
        private VerticalPaddingMode verticalPaddingMode = VerticalPaddingMode.None;
        private TextCharFilter textCharFilter = TextCharFilter.None;
        private TextFormatFilter textFormatFilter = TextFormatFilter.None;     

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyPress;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyDown;

        /// <summary>
        /// Recebe um <see cref="char"/> como parâmetro -> 
        /// retorna true ou false a depender do tipo de filtro definido.
        /// <para>Modificado exclusivamente em <see cref="TextCharFilter"/>.</para>
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

                if (value == TextCharFilter.OnlyNumbers) 
                {
                    CharFilter = char.IsDigit;                              //Define a função CharFilter para retornar verdadeiro apenas para números.
                    
                    for (int i = 0; i < Text.Length; i++)
                    { 
                        char textChar = Text[i]; if (!CharFilter(textChar)) //Se o char Text[i] não for aceito pelo CharFilter.
                        { Text = Text.Remove(i, 1); }                       //Remova o char do texto.
                    }
                }
                else if(value == TextCharFilter.None)                       //Define a função CharFilter como nula.
                { CharFilter = null; }
            }
        }

        /// <summary>
        /// Define se o texto tem um formato específico ->
        /// Aplica esse formato no texto atual ->
        /// Define que o <see cref="InnerControl.LostFocus"/>
        /// deve chamar <see cref="ApplyTextFormat"/>.
        /// </summary>
        public TextFormatFilter TextFormatFilter
        {
            get => textFormatFilter;
            set
            {
                textFormatFilter = value;
                if (value != TextFormatFilter.None) //Se for definido algum TextFormat diferente de None : é aplicado.
                {
                    ApplyTextFormat();
                    LostFocus += (s, e) => ApplyTextFormat();
                }
                else { LostFocus -= (s, e) => ApplyTextFormat(); }
            }
        }

        /// <summary>
        /// Comprimento máximo do texto.
        /// </summary>
        public int MaxLength { get; set; } = 0;                // 0 = sem limite
        
        /// <summary>Define se deve ser usado três pontos "..." se o texto não couber no tamanho atual.</summary>
        public bool UseEllipsis { get; set; } = false;

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

        /// <summary>Define se o tamanho deve ser baseado no texto.</summary>
        public bool SizeBasedOnText { get; set; } = false;

        /// <summary>Coordenada(x,y) absoluta do texto.</summary>
        public Point TextLocation
        {
            get
            {
                int x = Location.X + textRelativeLocation.X;
                int y = Location.Y + textRelativeLocation.Y;
                return new(x, y);
            }
        }

        public Size TextSize
        {
            get 
            {
                int width  = Text.Length * NhegazSizeMethods.FontUnitSize(Font).Width;
                int height = NhegazSizeMethods.FontUnitSize(Font).Height;
                return new(width, height);
            }             
        }

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

        public TextHorizontalAlignment TextHorizontalAlignment
        {
            get => textHorizontalAlignment;
            set { textHorizontalAlignment = value; AdjustTextLocation(); }
        }

        public TextVerticalAlignment TextVerticalAlignment
        {
            get => textVerticalAlignment;
            set { textVerticalAlignment = value; AdjustTextLocation(); }
        }

        public HorizontalPaddingMode HorizontalPaddingMode
        {
            get => horizontalPaddingMode;
            set { horizontalPaddingMode = value; AdjustTextLocation(); }
        }

        public VerticalPaddingMode VerticalPaddingMode
        {
            get => verticalPaddingMode;
            set { verticalPaddingMode = value; AdjustTextLocation(); }
        }
        private int GetHorizontalPadding()
        {
            int fontWidth = NhegazSizeMethods.FontUnitSize(Font).Width;
            return HorizontalPaddingMode switch
            {
                HorizontalPaddingMode.None => 0,
                HorizontalPaddingMode.HalfFontWidth => fontWidth / 2,
                HorizontalPaddingMode.OneFourthFontWidth => fontWidth / 4,
                HorizontalPaddingMode.Absolute => TextHorizontalAlignment == TextHorizontalAlignment.Left ? Padding.Left : Padding.Right,
                _ => 0
            };
        }

        private int GetVerticalPadding()
        {
            int fontHeight = NhegazSizeMethods.FontUnitSize(Font).Height;
            return VerticalPaddingMode switch
            {
                VerticalPaddingMode.None => 0,
                VerticalPaddingMode.HalfFontHeight => fontHeight / 2,
                VerticalPaddingMode.OneFourthFontHeight => fontHeight / 4,
                VerticalPaddingMode.Absolute => TextVerticalAlignment == TextVerticalAlignment.Top ? Padding.Top : Padding.Bottom,
                _ => 0
            };
        }            
    }
}
