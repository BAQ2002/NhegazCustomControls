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

        private bool onFocus = false;
        private bool layoutPending = false;
        private Color secondaryForeColor = SystemColors.ControlText; //Cor de textos secundarios

        private Color backgroundColor = SystemColors.Window; //Cor do fundo
        private Color secondaryBackgroundColor = SystemColors.ControlLightLight; //Cor do fundo secundaria

        private Color hoverBackgroundColor = SystemColors.Highlight;
        private Color hoverForeColor = SystemColors.Window;

        private Color borderColor = SystemColors.WindowFrame;
        private Color onFocusBorderColor = SystemColors.Highlight; //Cor da borda
       
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

        public int BorderWidhtSum
        {
            get => BorderWidth * 2;
        }
        #region----------------------------------------------------------------------------------------------------

        public int InnerHorizontalPadding => ControlPadding.EffectiveInnerHorizontal;
        public int InnerVerticalPadding => ControlPadding.EffectiveInnerVertical;

        /// <summary> </summary>
        public int BorderHorizontalBoundsSum =>
            ControlPadding.EffectiveBorderHorizontalSum + 2 * BorderWidth;

        /// <summary></summary>
        public int BorderVerticalBoundsSum =>
            ControlPadding.EffectiveBorderVerticalSum + 2 * BorderWidth;


        /// <summary>Retorna a COORDENADA Y mínima de onde um InnerControl pode ser posicionado em relação ao Top.</summary>      
        public int ContentTopBound =>
            BorderWidth + ControlPadding.EffectiveBorderTop;

        /// <summary>Retorna a COORDENADA X mínima de onde um InnerControl pode ser posicionado em relação ao left.</summary>      
        public int ContentLeftBound=>
            BorderWidth + ControlPadding.EffectiveBorderLeft;

        /// <summary>>Retorna a COORDENADA X máxima de onde um InnerControl pode ser posicionado em relação ao Right.</summary>      
        public int ContentRightBound =>
            Width - (BorderWidth + ControlPadding.EffectiveBorderRight);

        /// <summary>Retorna a COORDENADA Y máxima de onde um InnerControl pode ser posicionado em relação ao Bottom.</summary>
        public int ContentBottomBound =>
            Height - (BorderWidth + ControlPadding.EffectiveBorderBottom);

        /// <summary>ERRADO Retorna o valor que a espessura e o espaçamento da borda ocupam Horizontalmente.</summary>
        public int ContentHorizontalSum =>
            ContentLeftBound + ContentRightBound;

        /// <summary>ERRADO Retorna o valor que a espessura e o espaçamento da borda ocupam Verticalmente.</summary>
        public int ContentVerticalSum =>
            ContentTopBound + ContentBottomBound;

        /// <summary>Retorna a COORDENADA Y centralizada em relação à altura do InnerControl </summary>
        public int RelativeCenterY(int innerControlHeight)
        {
            int centerY = (Height - innerControlHeight) / 2;         // Centro absoluto vertical do controle
            int minY = ContentTopBound;                              // Mínimo: espessura e padding da borda superior
            int maxY = ContentBottomBound - innerControlHeight;      // Máximo: respeita espessura e padding da borda inferior
            return NhegazLocationMethods.Clamp(centerY, minY, maxY); // CLAMP dos valores
        }
        /// <summary>Retorna a COORDENADA Y centralizada em relação à altura do InnerControl </summary>
        public int RelativeCenterY(InnerControl innerControl) => RelativeCenterY(innerControl.Height);

        /// <summary>Retorna a COORDENADA X centralizada em relação à largura do InnerControl </summary>
        public int RelativeCenterX(int innerControlWidth)
        {
            int cx = (Width - innerControlWidth) / 2;                 // Centro absoluto horizontal do controle
            int minX = ContentLeftBound;                              // Mínimo: espessura e padding da borda esquerda
            int maxX = ContentRightBound - innerControlWidth; // Máximo: respeita espessura e padding da borda direita
            return NhegazLocationMethods.Clamp(cx, minX, maxX);       // CLAMP dos valores
        }
        /// <summary>Retorna a COORDENADA X centralizada em relação à largura do InnerControl </summary>
        public int RelativeCenterX(InnerControl innerControl) => RelativeCenterX(innerControl.Width);

        /// <summary>
        /// Retorna a COORDENADA X encostado na EXTREMIDADE ESQUERDA em relação à 
        /// largura do InnerControl(respeitando padding/borda esquerda).
        /// </summary>
        public int RelativeLeftX(){ return ContentLeftBound; }
        
        /// <summary>
        /// X para posicionar o InnerControl encostado na EXTREMIDADE DIREITA
        /// (respeitando padding/borda direita).
        /// </summary>
        public int RelativeRightX(int innerControlWidth)
        {
            // canto esquerdo = largura total - espessura direita - largura do inner
            return Width - ContentRightBound - innerControlWidth;
        }

        /// <summary>
        /// X para posicionar o InnerControl encostado na EXTREMIDADE DIREITA (respeitando padding/borda direita).</summary>
        public int RelativeRightX(InnerControl innerControl) => RelativeRightX(innerControl.Width);

        /// <summary>Y para posicionar o InnerControl encostado na EXTREMIDADE SUPERIOR (respeitando padding/borda superior).</summary>
        public int RelativeTopY()
        {
            return ContentTopBound;
        }

        /// <summary>Y para posicionar o InnerControl encostado na EXTREMIDADE INFERIOR (respeitando padding/borda inferior).</summary>
        public int RelativeBottomY(int innerControlHeight)
        {
            return Height - ContentBottomBound - innerControlHeight;
        }
        /// <summary>Y para posicionar o InnerControl encostado na EXTREMIDADE INFERIOR (respeitando padding/borda inferior).</summary>
        public int RelativeBottomY(InnerControl innerControl) => RelativeBottomY(innerControl.Height);

        #endregion--------------------------------------------------------------------------------------------------
    }
}
