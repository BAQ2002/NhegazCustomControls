using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NhegazCustomControls
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public partial class HeaderFeature
    {
        private readonly CustomControl ownerControl;
        
        private float heightRelativePercent = 1;
        /// <summary></summary>
        private int borderWidth = 1;

        /// <summary></summary>
        private int borderRadius = 4;

        /// <summary></summary>
        private Color borderColor = SystemColors.WindowFrame;

        /// <summary></summary>
        private Color onFocusBorderColor = SystemColors.Highlight;

        /// <summary></summary>
        private Color foreColor = SystemColors.ControlText;

        /// <summary></summary>
        private Color backgroundColor = SystemColors.GrayText; //Cor do fundo do cabecalho
                                                               
        /// <summary></summary>
        private Color hoverBackgroundColor = SystemColors.ControlText;

        /// <summary></summary>
        private Color hoverForeColor = SystemColors.ControlText;

        /// <summary></summary>
        private Rectangle bounds = new Rectangle(0, 0, 0, 0);

        /// <summary></summary>
        private HeaderHeightMode heightMode = HeaderHeightMode.Absolute;

        /// <summary>Indica se o Controle possui visualmente uma borda.</summary>
        private bool HasBorder => borderWidth >= 1;

        /// <summary>
        /// Raio do arrendondamento das quinas do fundo do Controle baseado em
        /// <para>(BorderWidth  = 0 : BackgroundCornerRaidius = BorderRadius); </para>
        /// <para>(BorderWidth >= 1 : BackgroundCornerRaidius = BorderRadius - 1); </para>
        /// </summary>
        private int BackgroundCornerRaidus => HasBorder ? BorderRadius - 1 : BorderRadius;

        /// <summary> 
        /// Valor do deslocamento do GraphicsPath utilizado em DrawBackground baseado em
        /// <para>(BorderWidth = 0 : BackgroundOffset = 0); </para>
        /// <para>(BorderWidth = 1 : BackgroundOffset = 1); </para>
        /// <para>(BorderWidth > 1 : BackgroundOffset = BorderWidth-1); </para>
        /// </summary>
        private int BackgroundOffset =>
            BorderWidth <= 0 ? 0 :
            BorderWidth == 1 ? 1 :
            BorderWidth - 1;

        /// <summary>
        /// Retangulo que fornece o Size e Location para o Fundo do Controle
        /// <para>(X = BackgroundOffset : Width - (2 * BackgroundOffset)); </para>
        /// <para>(Y = BackgroundOffset : height = Height - (2 * BackgroundOffset)); </para>
        /// </summary>
        private Rectangle BackgroundRectangle
        {
            get
            {
                int locX = X + BackgroundOffset; int width = Width - (2 * BackgroundOffset);
                int locY = Y + BackgroundOffset; int height = Height - (2 * BackgroundOffset);

                return new( locX, locY, width, height);
            }
        }

        /// <summary>
        /// Define como será definida a altura do cabeçalho.
        /// </summary>
        [Category("Cabeçalho")]
        public HeaderHeightMode HeightMode
        {
            get => heightMode;
            set { heightMode = value; }
        }

        /// <summary>
        /// Raio do arredondamento das quinas da borda do cabeçalho.
        /// </summary>
        [Category("Cabeçalho-Borda")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; ownerControl.Invalidate(); }
        }
       
        [Category("Cabeçalho-Borda")]
        public int BorderWidth
        {
            get => borderWidth;
            set { borderWidth = value; ownerControl.Invalidate(); }
        }

        [Category("Cabeçalho-Borda")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; ownerControl.Invalidate(); }
        }

        [Category("Cabeçalho-Borda")]
        public Color OnFocusBorderColor
        {
            get => onFocusBorderColor;
            set { onFocusBorderColor = value; ownerControl.Invalidate(); }
        }

        /// <summary>
        /// Cor de fundo para cabeçalho.
        /// </summary>
        [Category("Cabeçalho")]
        public virtual Color BackgroundColor
        {
            get => backgroundColor;
            set 
            { 
                backgroundColor = value; 
                foreach (InnerControl innerControl in Controls.GetAll)               
                    innerControl.BackgroundColor = value;
                ownerControl.Invalidate();
            }

        }

        /// <summary>
        /// Cor de texto para cabeçalho.
        /// </summary>
        [Category("Cabeçalho-Cores")]
        public virtual Color ForeColor
        {
            get => foreColor;
            set
            {
                foreColor = value;
                foreach (InnerControl innerControl in Controls.GetAll) 
                    innerControl.ForeColor = value;
                ownerControl.Invalidate();
            }
        }

        [Category("Cabeçalho-Cores")]
        [Browsable(true)]
        public virtual Color HoverBackgroundColor
        {
            get => hoverBackgroundColor;
            set { hoverBackgroundColor = value; }

        }

        [Category("Cabeçalho-Cores")]
        [Browsable(true)]
        public virtual Color HoverForeColor
        {
            get => hoverForeColor;
            set { hoverForeColor = value; }

        }
        [Category("Cabeçalho-Cores")]
        public float HeightRelativePercent
        {
            get => heightRelativePercent;
            set
            {
                // Garante que esteja entre 0 e 2
                heightRelativePercent = Math.Max(0f, Math.Min(2f, value));
                if (HeightMode == HeaderHeightMode.RelativeToFont)
                {
                    ownerControl.Invalidate();
                }
            }
        }

        /// <summary>
        /// Área delimitadora do cabeçalho.
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds => bounds;
        
        [Browsable(false)]
        public Size Size
        { 
            get => bounds.Size; 
            set { bounds.Size = value; ownerControl.Invalidate(); }
        }

        [Browsable(false)]
        public Point Location 
        {
            get => bounds.Location;
            set { bounds.Location = value; ownerControl.Invalidate(); }
        }
        
        [Browsable(false)] public int X => bounds.X;         [Browsable(false)] public int Y => bounds.Y;
        [Browsable(false)] public int Top => bounds.Top;     [Browsable(false)] public int Right => bounds.Right;
        [Browsable(false)] public int Left => bounds.Left;   [Browsable(false)] public int Bottom => bounds.Bottom;
        [Browsable(false)] public int Width => bounds.Width; [Browsable(false)] public int Height => bounds.Height;

        /// <summary>
        /// Retorna a COORDENADA X centralizada
        /// em relação à largura do InnerControl 
        /// </summary>
        [Browsable(false)] 
        public int RelativeCenterX(int innerWidth)
        {
            int centerX = Left + (Width - innerWidth) / 2;
            return centerX;
        }
        ///<summary>
        /// Retorna a COORDENADA X centralizada
        /// em relação à largura do InnerControl 
        ///</summary>
        [Browsable(false)]
        public int RelativeCenterX(InnerControl innerControl) => RelativeCenterX(innerControl.Width);

        /// <summary> 
        /// Retorna a COORDENADA Y centralizada
        /// em relação à altura do InnerControl 
        /// </summary>
        [Browsable(false)]
        public int RelativeCenterY(int innerHeight)
        {
            int centerY = Top + (Height - innerHeight) / 2;
            return centerY;
        }
        /// <summary> 
        /// Retorna a COORDENADA Y centralizada em 
        /// relação à altura do InnerControl 
        /// </summary>
        [Browsable(false)]
        public int RelativeCenterY(InnerControl innerControl) => RelativeCenterY(innerControl.Height);
        /// <summary>
        /// Retorna a COORDENADA X encostado na EXTREMIDADE ESQUERDA em relação à 
        /// largura do InnerControl(respeitando padding/borda esquerda).
        /// </summary>
        public int RelativeLeftX()
        {
            // canto esquerdo do inner control fica exatamente no limite mínimo permitido
            return Left;
        }

        /// <summary>X para posicionar o InnerControl encostado na EXTREMIDADE DIREITA.</summary>
        public int RelativeRightX(int innerControlWidth)
        {
            // canto esquerdo = largura total - espessura direita - largura do inner
            return Right - innerControlWidth;
        }
        /// <summary>X para posicionar o InnerControl encostado na EXTREMIDADE DIREITA.</summary>
        public int RelativeRightX(InnerControl innerControl) => RelativeRightX(innerControl.Width);

        /// <summary>
        /// Coleção de elementos do cabeçalho.
        /// </summary>
        [Browsable(false)]
        public InnerControls Controls { get; }

        public HeaderFeature(CustomControl owner)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            Controls = new InnerControls(owner);
        }

        /// <summary>
        /// Define o tamanho da area do cabecalho;
        /// Se "HeaderHeightMode.RelativeToFont" a altura "height" passada como parametro será ignorada.
        /// </summary>
        public void SetSize(int width, int height)
        {
            if (HeightMode == HeaderHeightMode.RelativeToFont)
            {
                Size unit = NhegazSizeMethods.FontUnitSize(ownerControl.Font); //Tamanho "unit" unitario da fonte
                height = (int)Math.Round(unit.Height * HeightRelativePercent); 
            }
 
            Size = new Size( width, height);
        }

        /// <summary>
        /// Define a posicao da area de cabecalho.
        /// </summary>
        public void SetLocation(int x, int y)
        {
            Location = new Point(x, y);
        }
      
        public void AdjustHeaderColors()
        {
            foreach (InnerControl innerControl in Controls.GetAll)
            {
                innerControl.BackgroundColor = BackgroundColor;
                innerControl.ForeColor = ForeColor;

                innerControl.HoverBackgroundColor = HoverBackgroundColor;
                innerControl.HoverForeColor = HoverForeColor;
            }
            ownerControl.Invalidate();
        }

        public bool HandleClick(Point p) => Controls.HandleClick(ownerControl, p);
        public bool HandleDoubleClick(Point p) => Controls.HandleDoubleClick(ownerControl, p);
        public void HandleMouseMove(Point p) => Controls.HandleMouseMove(ownerControl, p);
        public bool HandleGotFocus(Point p) => Controls.HandleGotFocus(ownerControl, p);
        public bool HandleLostFocus(Point p) => Controls.HandleLostFocus(ownerControl, p);        

    }
}
