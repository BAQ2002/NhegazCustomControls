using System;
using System.Drawing;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    /// <summary>
    /// Inner "textbox" com mesmos ajustes de posição do InnerLabel e caret funcional.
    /// </summary>
    public class InnerTextBox : InnerControl, IAcceptsKeyboard
    {
        private Point textLocation = Point.Empty;
        private string text = string.Empty;

        private TextVerticalAlignment textVerticalAlignment = TextVerticalAlignment.Center;
        private TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left;

        private HorizontalPaddingMode horizontalPaddingMode = HorizontalPaddingMode.None;
        private VerticalPaddingMode verticalPaddingMode = VerticalPaddingMode.None;

        public bool ApplyHorizontalPaddingWhenCentered { get; set; } = false;
        public bool ApplyVerticalPaddingWhenCentered { get; set; } = false;

        /// <summary>Define se o tamanho acompanha o texto (igual ao InnerLabel).</summary>
        public bool SizeBasedOnText { get; set; } = false;

        /// <summary>Texto atual.</summary>
        public string TextValue
        {
            get => text;
            set
            {
                text = value ?? string.Empty;
                CaretIndex = Math.Min(CaretIndex, text.Length);
                AdjustControlSize();
            }
        }

        /// <summary>Índice do caret dentro de <see cref="TextValue"/> (0..Length).</summary>
        public int CaretIndex
        {
            get => caretIndex;
            set
            {
                int v = Math.Max(0, Math.Min(TextValue.Length, value));
                if (caretIndex != v)
                {
                    caretIndex = v;
                    RestartCaretBlink();
                }
            }
        }
        private int caretIndex = 0;
        // Método que eu referenciei e faltou implementar:
        private void RestartCaretBlink()
        {
            caretVisible = true;     // aparece imediatamente
            caretTimer.Stop();       // reseta o ciclo de blink
            caretTimer.Start();
            InvalidateParent?.Invoke(); // repinta já
        }

        // Timer de blink
        private readonly System.Windows.Forms.Timer caretTimer;
        private bool caretVisible = false;
        private const int CaretBlinkIntervalMs = 500;

        public InnerTextBox(bool autoSizeBasedOnText = false) : base()
        {
            SizeBasedOnText = autoSizeBasedOnText;

            caretTimer = new System.Windows.Forms.Timer { Interval = CaretBlinkIntervalMs };
            caretTimer.Tick += (s, e) =>
            {
                caretVisible = !caretVisible;
            };
        }
        public Action? InvalidateParent { get; set; }

        public override Font Font
        {
            get => base.Font;
            set { base.Font = value; AdjustControlSize(); }
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

        protected override void AdjustControlSize()
        {
            base.AdjustControlSize();

            if (SizeBasedOnText)
            {
                Size = NhegazSizeMethods.TextExactSize(
                    string.IsNullOrEmpty(TextValue) ? " " : TextValue, Font);
            }

            AdjustTextLocation();
        }

        protected override void SymmetricalCircleAdjust()
        {
            base.SymmetricalCircleAdjust();
            TextHorizontalAlignment = TextHorizontalAlignment.Center;
            TextVerticalAlignment = TextVerticalAlignment.Center;
        }

        public override void Update()
        {
            AdjustTextLocation();
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

        private void AdjustTextLocation()
        {
            Size textSize = NhegazSizeMethods.TextExactSize(string.IsNullOrEmpty(TextValue) ? " " : TextValue, Font);

            int textX = 0, horizontalPadding = GetHorizontalPadding();
            int textY = 0, verticalPadding = GetVerticalPadding();

            switch (TextHorizontalAlignment)
            {
                case TextHorizontalAlignment.Left:
                    textX = horizontalPadding;
                    break;
                case TextHorizontalAlignment.Center:
                    textX = (Size.Width - textSize.Width) / 2;
                    if (ApplyHorizontalPaddingWhenCentered)
                        textX += (Padding.Left - Padding.Right) / 2;
                    break;
                case TextHorizontalAlignment.Right:
                    textX = Size.Width - (textSize.Width + horizontalPadding);
                    break;
            }

            switch (TextVerticalAlignment)
            {
                case TextVerticalAlignment.Top:
                    textY = verticalPadding;
                    break;
                case TextVerticalAlignment.Center:
                    textY = (Size.Height - textSize.Height) / 2;
                    if (ApplyVerticalPaddingWhenCentered)
                        textY += (Padding.Top - Padding.Bottom) / 2;
                    break;
                case TextVerticalAlignment.Bottom:
                    textY = Size.Height - (textSize.Height + verticalPadding);
                    break;
            }
            textLocation = new Point(textX, textY);
        }

        // ======== Ciclo de foco (para caret) ========
        public override void RaiseMouseEnter() { base.RaiseMouseEnter(); }
        public override void RaiseMouseLeave() { base.RaiseMouseLeave(); }

        public void StartCaret()
        {
            caretVisible = true;
            caretTimer.Start();
        }

        public void StopCaret()
        {
            caretTimer.Stop();
            caretVisible = false;
        }

        // Esses eventos já são disparados pela infra (InnerControls.HandleGotFocus/LostFocus)
        public void OnInnerGotFocus()
        {
            // por padrão, posiciona caret no fim
            CaretIndex = TextValue.Length;
            StartCaret();
        }
        public void OnInnerLostFocus()
        {
            StopCaret();
        }

        // A infra atual chama RaiseGotFocus/RaiseLostFocus sem payload;
        // então conectamos aqui via inscrição no próprio construtor do controle pai (externo).
        // Sugestão: ao instanciar, fazer:
        // innerTextBox.GotFocus += (s,e) => innerTextBox.OnInnerGotFocus();
        // innerTextBox.LostFocus += (s,e) => innerTextBox.OnInnerLostFocus();

        // ======== Entrada de teclado (encaminhada pelo CustomControl) ========
        public void OnParentKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;

            // insere caractere na posição do caret
            TextValue = TextValue.Insert(CaretIndex, e.KeyChar.ToString());
            CaretIndex++;
            e.Handled = true;
        }

        public void OnParentKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    e.Handled = true;
                    break;
                case Keys.Right:
                    CaretIndex = Math.Min(TextValue.Length, CaretIndex + 1);
                    e.Handled = true;
                    break;
                case Keys.Home:
                    CaretIndex = 0;
                    e.Handled = true;
                    break;
                case Keys.End:
                    CaretIndex = TextValue.Length;
                    e.Handled = true;
                    break;
                case Keys.Back:
                    if (CaretIndex > 0 && TextValue.Length > 0)
                    {
                        TextValue = TextValue.Remove(CaretIndex - 1, 1);
                        CaretIndex--;
                    }
                    e.Handled = true;
                    break;
                case Keys.Delete:
                    if (CaretIndex < TextValue.Length && TextValue.Length > 0)
                    {
                        TextValue = TextValue.Remove(CaretIndex, 1);
                    }
                    e.Handled = true;
                    break;
            }
        }

        // ======== Desenho ========
        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color foreColor = IsHovering ? HoverForeColor : ForeColor;

            // 1) texto
            var textRect = new Rectangle(
                Location.X + textLocation.X,
                Location.Y + textLocation.Y,
                Width - textLocation.X,
                Height - textLocation.Y);

            TextRenderer.DrawText(
                e.Graphics,
                string.IsNullOrEmpty(TextValue) ? " " : TextValue,
                Font,
                textRect,
                foreColor,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis
            );

            // 2) caret
            if (caretVisible)
            {
                // mede largura do texto até o CaretIndex
                string leftPart = (CaretIndex > 0) ? TextValue.Substring(0, CaretIndex) : string.Empty;
                Size leftSize = NhegazSizeMethods.TextExactSize(string.IsNullOrEmpty(leftPart) ? " " : leftPart, Font);

                int caretX = Location.X + textLocation.X + leftSize.Width;
                int caretTop = Location.Y + textLocation.Y;
                int caretBottom = caretTop + NhegazSizeMethods.TextExactSize("A", Font).Height;

                using (var pen = new Pen(foreColor, 1f))
                {
                    e.Graphics.DrawLine(pen, caretX, caretTop, caretX, caretBottom);
                }
            }
        }
    }
}
