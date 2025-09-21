using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace NhegazCustomControls
{
    public abstract class CustomControlWithHeader : CustomControl
    {
        private Color headerBackgroundColor = SystemColors.GrayText; //Cor do fundo do cabecalho     
        private HeaderHeightMode headerHeightMode = HeaderHeightMode.Absolute;
        private float headerHeightRelativePercent = 1;
        private int headerBorderRadius = 1;
        /// <summary>
        /// Define como será definida a altura do cabeçalho.
        /// </summary>
        [Category("Cabeçalho")]
        public HeaderHeightMode HeaderHeightMode
        {
            get => headerHeightMode; 
            set { headerHeightMode = value; }
        }

        /// <summary>
        /// Raio do arredondamento das quinas da borda do cabeçalho.
        /// </summary>
        [Category("Cabeçalho")]
        public int HeaderBorderRadius
        {
            get => headerBorderRadius;
            set { headerBorderRadius = value; Invalidate(); }
        }

        /// <summary>
        /// Cor de fundo para cabeçalho.
        /// </summary>
        [Category("Cores")]
        public virtual Color HeaderBackgroundColor
        {
            get => headerBackgroundColor;
            set { headerBackgroundColor = value; Invalidate(); }
        }

        [Category("Cabeçalho")]
        public float HeaderHeightRelativePercent
        {
            get => headerHeightRelativePercent;
            set
            {
                // Garante que esteja entre 0 e 2
                headerHeightRelativePercent = Math.Max(0f, Math.Min(2f, value));
                if (HeaderHeightMode == HeaderHeightMode.RelativeToFont)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Área delimitadora do cabeçalho.
        /// </summary>
        public Rectangle HeaderBounds { get; set; }

        /// <summary>
        /// Coleção de elementos do cabeçalho.
        /// </summary>
        public InnerControls HeaderControls { get; }

        public CustomControlWithHeader()
        {
            HeaderControls = new InnerControls(this);
        }
       
        protected CustomControlWithHeader(CustomControl parent) : base(parent) 
        {
            HeaderControls = new InnerControls(this);
        }
        
        /// <summary>
        /// Define o tamanho da area do cabecalho.
        /// </summary>
        protected virtual void AdjustHeaderSize(int width, int height)
        {
            if (HeaderHeightMode == HeaderHeightMode.RelativeToFont)
            {
                Size unit = NhegazSizeMethods.FontUnitSize(Font); //Tamanho "unit" unitario da fonte
                height = (int)Math.Round(unit.Width * HeaderHeightRelativePercent);
            }

            HeaderBounds = new Rectangle(HeaderBounds.Location.X, HeaderBounds.Location.Y, width, height);
        }
 
        /// <summary>
        /// Define a posicao da area de cabecalho.
        /// </summary>
        protected virtual void AdjustHeaderLocation(int x, int y)
        {
            HeaderBounds = new Rectangle(x, y, HeaderBounds.Width, HeaderBounds.Height);
        }

        /// <summary>
        /// Define a posição dos elementos do cabeçalho (ex: botões, rótulos).
        /// Pode ser sobrescrito por controles derivados.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!HeaderControls.HandleClick(this, e.Location))
                return; // Não chama novamente — deixa que CustomControl chamou os InnerControls
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!HeaderControls.HandleDoubleClick(this, e.Location))
                return; 
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);// hover corpo (InnerControls)
            HeaderControls.HandleMouseMove(this, e.Location);   // hover cabeçalho
                                          
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);   // propaga para InnerControls

            var loc = PointToClient(Cursor.Position);

            if (HeaderControls.HandleGotFocus(this, loc))
                return;      
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);  // propaga para InnerControls

            var loc = PointToClient(Cursor.Position);

            if (HeaderControls.HandleLostFocus(this, loc))
                return;         
        }

        /// <summary>
        /// Desenha o fundo do cabecalho.
        /// </summary>
        protected virtual void DrawHeader(PaintEventArgs e)
        {
            using var path = NhegazDrawingMethods.RectBackgroundPath(HeaderBounds, HeaderBorderRadius);
            using var brush = new SolidBrush(HeaderBackgroundColor);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
            e.Graphics.DrawPath(new Pen(HeaderBackgroundColor, 1f), path);
            e.Graphics.SetClip(path);
        }

        /// <summary>
        /// Desenha os elementos internos do cabeçalho.
        /// </summary>
        protected virtual void DrawHeaderElements(PaintEventArgs e)
        {
            HeaderControls.OnPaintAll(this, e);
            e.Graphics.ResetClip();
        }

        /// <summary>
        /// Redefine o ajuste do controle para incluir o cabeçalho.
        /// </summary>
        //protected override void AdjustControlSize()
        //{
        //    base.AdjustControlSize();
        //    AdjustHeaderSize();
        //    AdjustHeaderLayout();
        //}

        /// <summary>
        /// Redesenha o controle com suporte a cabeçalho.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.None;
            base.DrawControlBackground(e);
            DrawHeader(e);
            DrawHeaderElements(e);
            base.DrawInnerControls(e);
            base.DrawBorder(e);
        }
    }
}

