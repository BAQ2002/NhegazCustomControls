using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerLabel
    {
        private string text = "";
        private Point textRelativeLocation = Point.Empty;
        
        private TextVerticalAlignment textVerticalAlignment = TextVerticalAlignment.Center;
        private TextHorizontalAlignment textHorizontalAlignment = TextHorizontalAlignment.Left;

        private HorizontalPaddingMode horizontalPaddingMode = HorizontalPaddingMode.None;
        private VerticalPaddingMode verticalPaddingMode = VerticalPaddingMode.None;

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
        public bool SizeBasedOnText { get; set; } = true;

        public string Text
        {
            get => text;
            set { text = value; UpdateLayout(); }
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

        /// <summary>Define a posição de alinhamento horizontal do texto.</summary>
        public TextHorizontalAlignment TextHorizontalAlignment
        {
            get => textHorizontalAlignment;
            set { textHorizontalAlignment = value; AdjustTextLocation(); }
        }

        /// <summary>Define a posição de alinhamento vertical do texto.</summary>
        public TextVerticalAlignment TextVerticalAlignment
        {
            get => textVerticalAlignment;
            set { textVerticalAlignment = value; AdjustTextLocation(); }
        }

        public Point TextLocation
        {
            get
            {
                int x = Location.X + textRelativeLocation.X;
                int y = Location.Y + textRelativeLocation.Y;
                return new(x, y);
            }
        }
        /// <summary>Define qual o tamanho do padding horizontal do texto em relação à posição de alinhamento.</summary>
        public HorizontalPaddingMode HorizontalPaddingMode
        {
            get => horizontalPaddingMode;
            set { horizontalPaddingMode = value; AdjustTextLocation(); }
        }

        /// <summary>Define qual o tamanho do padding vertical do texto em relação à posição de alinhamento.</summary>
        public VerticalPaddingMode VerticalPaddingMode
        {
            get => verticalPaddingMode;
            set { verticalPaddingMode = value; AdjustTextLocation(); }
        }
    }
}
