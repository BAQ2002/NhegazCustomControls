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
    public class HeaderFeature
    {
        private readonly CustomControl ownerControl;
        
        private float heightRelativePercent = 1; 

        private int borderWidth = 1;
        private int borderRadius = 4;

        private bool HasBorder => borderWidth > 0;

        private Color borderColor = SystemColors.WindowFrame;
        private Color onFocusBorderColor = SystemColors.Highlight;

        private Color foreColor = SystemColors.ControlText;
        private Color backgroundColor = SystemColors.GrayText; //Cor do fundo do cabecalho    

        private Color hoverBackgroundColor = SystemColors.ControlText;
        private Color hoverForeColor = SystemColors.ControlText;

        private Rectangle bounds = new Rectangle(0, 0, 0, 0);
        private HeaderHeightMode heightMode = HeaderHeightMode.Absolute;

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

        private void DrawBackground(PaintEventArgs e)
        {
            //Posições(X,Y) do Background. //Tamanhos(Width, Height) do Background. //
            int locX = X + BorderWidth; int width = Width - (2 * BorderWidth);
            int locY = Y + BorderWidth; int height = Height - (2 * BorderWidth);
            //Serão diferentes das Propriedades Originais apenas se BorderWidth >=1.//

            Rectangle backgroundRect = new(locX, locY, width, height);
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var backgroundPath = NhegazDrawingMethods.RectangularPath(Bounds, BorderRadius))
            {
                using (var brush = new SolidBrush(BackgroundColor))
                {e.Graphics.FillPath(brush, backgroundPath);}

                e.Graphics.IntersectClip(new Region(backgroundPath));
            }     
            
        }
 
        private void DrawInnerControls(PaintEventArgs e)
        {
            Controls.OnPaintAll(e);
            e.Graphics.ResetClip();
        }

        private void DrawBorder(PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var borderPath = NhegazDrawingMethods.RectBorderPath(Bounds, BorderRadius, BorderWidth))
            {
                if (borderWidth > 1)
                {
                    using var brush = new SolidBrush(BorderColor);
                    e.Graphics.FillPath(brush, borderPath); 
                }

                using var pen = new Pen(BorderColor, 1f);
                e.Graphics.DrawPath(pen, borderPath);
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            DrawBackground(e); DrawInnerControls(e); //Desenha o Background; Desenha os InnerControls.
            if (HasBorder == true) DrawBorder(e);    //Se tiver Border: Desenha Border.
        }
 

    }
}
