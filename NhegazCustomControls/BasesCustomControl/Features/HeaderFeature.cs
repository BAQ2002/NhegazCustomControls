using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
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
        private int borderRadius = 1;

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
        [Category("Cabeçalho")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; ownerControl.Invalidate(); }
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
        [Category("Cabeçalho")]
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

        [Category("Cabeçalho")]
        [Browsable(true)]
        public virtual Color HoverBackgroundColor
        {
            get => hoverBackgroundColor;
            set { hoverBackgroundColor = value; }

        }

        [Category("Cabeçalho")]
        [Browsable(true)]
        public virtual Color HoverForeColor
        {
            get => hoverForeColor;
            set { hoverForeColor = value; }

        }
        [Category("Cabeçalho")]
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
        
        
        /// <summary>
        /// Desenha o fundo do cabecalho.
        /// </summary>
        public void PaintHeader(PaintEventArgs e)
        {
            using var path = NhegazDrawingMethods.RectBackgroundPath(Bounds, borderRadius);
            using var brush = new SolidBrush(backgroundColor);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
            e.Graphics.DrawPath(new Pen(backgroundColor, 1f), path);
            e.Graphics.SetClip(path);

            // elementos internos do cabeçalho
            Controls.OnPaintAll(ownerControl, e);
            e.Graphics.ResetClip();
        } 
    }
}
