using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomControl
    {
        private int borderRadius = 5;
        private int borderWidth = 1;
        private int onFocusBorderExtraWidth = 1;
        private int horizontalPadding = 1;
        private int verticalPadding = 1;

        private float paddingRelativePercent = 0.6f; // 60% por padrão

        private bool onFocusBool = false;
        private bool layoutPending = false;
        private Color secondaryForeColor = SystemColors.GrayText; //Cor de textos secundarios

        private Color backgroundColor = SystemColors.Window; //Cor do fundo
        private Color secondaryBackgroundColor = SystemColors.ControlLightLight; //Cor do fundo secundaria

        private Color borderColor = SystemColors.WindowFrame;
        //private Color dropdownBorderColor = Color.Green;

        private Color onFocusBorderColor = SystemColors.Highlight; //Cor da borda
        private Color hoverColor = SystemColors.Highlight;

        private PaddingMode paddingMode = PaddingMode.Absolute;

        [Browsable(false)]
        public InnerControls InnerControls { get; }

        [Browsable(false)]
        public bool OnFocusBool
        {
            get => onFocusBool;
            set { onFocusBool = value; Invalidate(); }
        }

        [Category("Padding")]
        public PaddingMode PaddingMode
        {
            get => paddingMode;
            set { paddingMode = value; Invalidate(); }
        }

        [Category("Padding")]
        public int HorizontalPadding
        {
            get => horizontalPadding;
            set { horizontalPadding = value; Invalidate(); }
        }

        [Category("Padding")]
        public int VerticalPadding
        {
            get => verticalPadding;
            set { verticalPadding = value; Invalidate(); }
        }

        [Category("Borda")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; Invalidate(); }
        }

        [Category("Borda")]
        public int BorderWidth
        {
            get => borderWidth;
            set { borderWidth = value; Invalidate(); }
        }

        [Category("Borda")]
        public int OnFocusBorderExtraWidth
        {
            get => onFocusBorderExtraWidth;
            set { onFocusBorderExtraWidth = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color SecondaryBackgroundColor
        {
            get => secondaryBackgroundColor;
            set { secondaryBackgroundColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color SecondaryForeColor
        {
            get => secondaryForeColor;
            set { secondaryForeColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color OnFocusBorderColor
        {
            get => onFocusBorderColor;
            set { onFocusBorderColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color HoverColor
        {
            get => hoverColor;
            set { hoverColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public virtual Color BackgroundColor
        {
            get => backgroundColor;
            set { backgroundColor = value; Invalidate(); }
        }

        [Category("Padding")]
        public float PaddingRelativePercent
        {
            get => paddingRelativePercent;
            set
            {
                // Garante que esteja entre 0 e 2
                paddingRelativePercent = Math.Max(0f, Math.Min(2f, value));
                if (PaddingMode == PaddingMode.RelativeToFont)
                {
                    Invalidate();
                }
            }
        }

    }
}
