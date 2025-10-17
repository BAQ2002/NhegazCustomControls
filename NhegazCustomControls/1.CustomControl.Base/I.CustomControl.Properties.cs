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
        /// <summary>Raio do arrendondamento das quinas da borda do Controle.</summary>
        private int borderRadius = 5;

        /// <summary>Espessura das bordas do Controle.</summary>
        private int borderWidth = 1;

        /// <summary>Espessura adicional para as bordas quando o Controle esta em foco.</summary>
        private int onFocusBorderExtraWidth = 1;

        /// <summary>Indica se o Controle esta em foco.</summary>
        private bool onFocus = false;

        /// <summary>Indica se a borda deve expandir quando o Controle esta em foco.</summary>
        private bool inflateBorderOnFocus = false;

        /// <summary></summary>
        private bool layoutPending = false;

        /// <summary>Indica se o Controle possui visualmente uma borda.</summary>
        private bool HasBorder => borderWidth >= 1;

        /// <summary>
        /// Valor do deslocamento do GraphicsPath utilizado em DrawBackground baseado em
        /// <para>(BorderWidth = 0 : BackgroundOffset = 0); </para>
        /// <para>(BorderWidth = 1 : BackgroundOffset = 1); </para>
        /// <para>(BorderWidth > 1 : BackgroundOffset = BorderWidth-1); </para>
        /// </summary>
        private int BackgroundOffset=>
            BorderWidth <= 0 ? 0 :
            BorderWidth == 1 ? 1 :
            BorderWidth - 1;

        /// <summary>
        /// Raio do arrendondamento das quinas do fundo do Controle baseado em
        /// <para>(BorderWidth  = 0 : BackgroundCornerRaidius = BorderRadius); </para>
        /// <para>(BorderWidth >= 1 : BackgroundCornerRaidius = BorderRadius - 1); </para>
        /// </summary>
        private int BackgroundCornerRaidius => HasBorder ? BorderRadius - 1 : BorderRadius;

        /// <summary>Cor de textos secundários.</summary>
        private Color secondaryForeColor = SystemColors.ControlText;

        /// <summary>Cor de fundo do Controle. </summary>
        private Color backgroundColor = SystemColors.Window;

        /// <summary>Cor de fundo secundária do Controle.</summary>
        private Color secondaryBackgroundColor = SystemColors.ControlLightLight;

        /// <summary>Cor de texto quando o cursor está sobre.</summary>
        private Color hoverBackgroundColor = SystemColors.Highlight;

        /// <summary>Cor de texto quando o cursor está sobre.</summary>
        private Color hoverForeColor = SystemColors.Window;

        /// <summary>Cor da Borda do Controle.</summary>
        private Color borderColor = SystemColors.WindowFrame;

        /// <summary>Cor da Borda do Controle quando em Foco.</summary>
        private Color onFocusBorderColor = SystemColors.Highlight;

        public Size FontUnitSize => NhegazSizeMethods.FontUnitSize(Font);
       
        [Browsable(false)]
        public InnerControls InnerControls { get; }

        [Category("ControlPadding")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CustomControlPadding ControlPadding { get; }

        [Browsable(false)]
        public bool OnFocus
        {
            get => onFocus;
            set { onFocus = value; Invalidate(); }
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
        public Color HoverBackgroundColor
        {
            get => hoverBackgroundColor;
            set { hoverBackgroundColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public Color HoverForeColor
        {
            get => hoverForeColor;
            set { hoverForeColor = value; Invalidate(); }
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
            get => hoverBackgroundColor;
            set { hoverBackgroundColor = value; Invalidate(); }
        }

        [Category("Cores")]
        public virtual Color BackgroundColor
        {
            get => backgroundColor;
            set { backgroundColor = value; Invalidate(); }
        }
 
    }
}
