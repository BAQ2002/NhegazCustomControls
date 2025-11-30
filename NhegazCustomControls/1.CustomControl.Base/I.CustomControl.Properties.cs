using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /// <summary>Indica se a borda deve expandir quando o Controle esta em foco.</summary>
        private bool inflateBorderOnFocus = false;

        /// <summary></summary>
        private bool layoutPending = false;

        /// <summary>Indica se o Controle possui visualmente uma borda.</summary>
        protected bool HasBorder => borderWidth >= 1;

        /// <summary>
        /// Valor do deslocamento do GraphicsPath utilizado em DrawBackground baseado em
        /// <para>(BorderWidth = 0 : BackgroundOffset = 0); </para>
        /// <para>(BorderWidth = 1 : BackgroundOffset = 1); </para>
        /// <para>(BorderWidth > 1 : BackgroundOffset = BorderWidth-1); </para>
        /// </summary>
        protected int BackgroundOffset=>
            BorderWidth <= 0 ? 0 :
            BorderWidth == 1 ? 1 :
            BorderWidth - 1;

        /// <summary>
        /// Retangulo que fornece o Size e Location para o Fundo do Controle
        /// <para>(X = BackgroundOffset : Width - (2 * BackgroundOffset)); </para>
        /// <para>(Y = BackgroundOffset : height = Height - (2 * BackgroundOffset)); </para>
        /// </summary>
        protected Rectangle BackgroundRectangle
        {
            get
            {
                int locX = BackgroundOffset; int width = Width - (2 * BackgroundOffset);
                int locY = BackgroundOffset; int height = Height - (2 * BackgroundOffset);

                return new(locY, locX, width, height);
            }
        }
        /// <summary>
        /// Raio do arrendondamento das quinas do fundo do Controle baseado em
        /// <para>(BorderWidth  = 0 : BackgroundCornerRaidius = BorderRadius); </para>
        /// <para>(BorderWidth >= 1 : BackgroundCornerRaidius = BorderRadius - 1); </para>
        /// </summary>
        protected int BackgroundCornerRaidus => HasBorder ? BorderRadius - 1 : BorderRadius;
  
        /// <summary>Cor de textos secundários.</summary>
        private Color secondaryForeColor = SystemColors.ControlText;

        /// <summary>Cor de fundo do Controle. </summary>
        private Color backgroundColor = SystemColors.Window;

        /// <summary>Cor de fundo secundária do Controle.</summary>
        private Color secondaryBackgroundColor = SystemColors.ControlLightLight;

        /// <summary>Cor de texto quando o cursor do mouse está sobre o <see cref="CustomControl"/>.</summary>
        private Color hoverBackgroundColor = SystemColors.Highlight;

        /// <summary>Cor de texto quando o cursor do mouse está sobre o <see cref="CustomControl"/>.</summary>
        private Color hoverForeColor = SystemColors.Window;

        /// <summary>Cor da Borda do Controle.</summary>
        private Color borderColor = SystemColors.WindowFrame;

        /// <summary>Cor da Borda do Controle quando em Foco.</summary>
        private Color onFocusBorderColor = SystemColors.Highlight;

        public Size FontUnitSize => NhegazSizeMethods.FontUnitSize(Font);
       
        [Browsable(false)]
        public InnerControlsCollection InnerControls { get; }

        [Category("ControlPadding")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CustomControlPadding ControlPadding { get; }

        [Category("Borda")]
        [Description("Define o raio do arrendondamento das quinas da borda em pixels.")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; Invalidate(); }
        }

        [Category("Borda")]
        [Description("Define a largura da borda em pixels.")]
        public int BorderWidth
        {
            get => borderWidth;
            set { borderWidth = value; Invalidate(); }
        }

        [Category("Borda")]
        [Description("Define a largura adicional da borda em pixels quando o Controle está em foco.")]
        public int OnFocusBorderExtraWidth
        {
            get => onFocusBorderExtraWidth;
            set { onFocusBorderExtraWidth = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor de fundo quando um elemento do Controle está com o mouse por cima.")]
        public Color HoverBackgroundColor
        {
            get => hoverBackgroundColor;
            set { hoverBackgroundColor = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor da escrita quando um elemento do Controle está com o mouse por cima.")]
        public Color HoverForeColor
        {
            get => hoverForeColor;
            set { hoverForeColor = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor secundária de fundo do Controle.")]
        public Color SecondaryBackgroundColor
        {
            get => secondaryBackgroundColor;
            set { secondaryBackgroundColor = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor secundária da escrita do Controle.")]
        public Color SecondaryForeColor
        {
            get => secondaryForeColor;
            set { secondaryForeColor = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor padrão da borda do Controle.")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("Cores")]
        [Description("Define a cor da borda quando o Controle está em foco.")]
        public Color OnFocusBorderColor
        {
            get => onFocusBorderColor;
            set { onFocusBorderColor = value; Invalidate(); }
        }
    
        [Category("Cores")]
        [Description("Define a cor de fundo padrão do Controle.")]
        public virtual Color BackgroundColor
        {
            get => backgroundColor;
            set { backgroundColor = value; Invalidate(); }
        }
 
    }
}
