using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{

    [Flags]
    public enum ControlAdjustments
    {
        None,
        Padding,
        InnerLocation,
        Size,
        All = Padding | InnerLocation | Size 
    }

    public class InnerLabel : InnerControl
    {
        private Point textLocation = Point.Empty;
        private string text = "";
    
        private TextVerticalAlignment textVerticalAlignment = TextVerticalAlignment.Center;
        private TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left;

        private HorizontalPaddingMode horizontalPaddingMode = HorizontalPaddingMode.None;
        private VerticalPaddingMode verticalPaddingMode = VerticalPaddingMode.None;
        public bool ApplyHorizontalPaddingWhenCentered { get; set; } = false;
        public bool ApplyVerticalPaddingWhenCentered { get; set; } = false;
        public bool SizeBasedOnText {  get; set; } = true;
        public string Text
        {
            get => text;
            set { text = value; AdjustControlSize(); }
        }
        public override Font Font
        {
            get => base.Font;
            set { base.Font = value; AdjustControlSize(); }
        }
        public override int Height 
        { 
            get => base.Height;
            set { SizeBasedOnText = false; base.Height = value; } // } 
        }
        public override int Width 
        { 
            get => base.Width;
            set { SizeBasedOnText = false; base.Width = value; }// SizeBasedOnText = false; }
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
            set{ horizontalPaddingMode = value; AdjustTextLocation(); }
        }
        public VerticalPaddingMode VerticalPaddingMode
        {
            get => verticalPaddingMode;
            set{ verticalPaddingMode = value; AdjustTextLocation(); }
        }
      
        public InnerLabel(bool autoSizeBasedOnText = true) : base()
        {
            SizeBasedOnText = autoSizeBasedOnText;
        }

        protected override void AdjustControlSize()
        {
            base.AdjustControlSize();
         
            if (SizeBasedOnText == true)
                Size = NhegazSizeMethods.TextExactSize(Text, Font);

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
            AdjustTextLocation(); // recalcula o ponto do texto baseado na altura atual
        }

        /// <summary>
        /// Retorna o valor do ControlPadding Left|Right a partir de HorizontalPaddingMode.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Retorna o valor do ControlPadding borderTop|Bottom a partir de VerticalPaddingMode.
        /// </summary>
        /// <returns></returns>
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
            Size textSize = NhegazSizeMethods.TextExactSize(Text, Font);

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

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            

            Color foreColor = IsHovering? HoverForeColor : ForeColor;
            
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(Location.X + textLocation.X, Location.Y + textLocation.Y, Width - textLocation.X, Height - textLocation.Y),
                foreColor,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis
            );
        }    
    }
}
