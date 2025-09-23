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
        private Color foreColor = SystemColors.ControlText;
        private Color backgroundColor = SystemColors.GrayText; //Cor do fundo do cabecalho     
        private HeaderHeightMode heightMode = HeaderHeightMode.Absolute;
        private float heightRelativePercent = 1;
        private int borderRadius = 1;

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
                {
                    innerControl.BackgroundColor = value;
                }
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
                {
                    innerControl.ForeColor = value;
                }
                ownerControl.Invalidate();
            }
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
        public Rectangle HeaderBounds { get; set; }

        /// <summary>
        /// Coleção de elementos do cabeçalho.
        /// </summary>
        [Browsable(false)]
        public InnerControls Controls { get; }

        public HeaderFeature(CustomControl owner)
        {
            this.ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            Controls = new InnerControls(owner);
        }

        /// <summary>
        /// Define o tamanho da area do cabecalho.
        /// </summary>
        public void AdjustHeaderSize(int width, int height)
        {
            if (HeightMode == HeaderHeightMode.RelativeToFont)
            {
                Size unit = NhegazSizeMethods.FontUnitSize(ownerControl.Font); //Tamanho "unit" unitario da fonte
                height = (int)Math.Round(unit.Height * HeightRelativePercent);
            }

            HeaderBounds = new Rectangle(HeaderBounds.Location.X, HeaderBounds.Location.Y, width, height);
        }

        /// <summary>
        /// Define a posicao da area de cabecalho.
        /// </summary>
        public void AdjustHeaderLocation(int x, int y)
        {
            HeaderBounds = new Rectangle(x, y, HeaderBounds.Width, HeaderBounds.Height);
        }
        public bool HandleMouseClick(Point p) => Controls.HandleClick(ownerControl, p);
        public bool HandleMouseDoubleClick(Point p) => Controls.HandleDoubleClick(ownerControl, p);
        public void HandleMouseMove(Point p) => Controls.HandleMouseMove(ownerControl, p);
        public bool HandleGotFocus(Point p) => Controls.HandleGotFocus(ownerControl, p);
        public bool HandleLostFocus(Point p) => Controls.HandleLostFocus(ownerControl, p);
        
        
        /// <summary>
        /// Desenha o fundo do cabecalho.
        /// </summary>
        public void PaintHeader(PaintEventArgs e)
        {
            using var path = NhegazDrawingMethods.RectBackgroundPath(HeaderBounds, borderRadius);
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
