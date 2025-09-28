using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public interface IMatrixAdjustable
    {
        void AdjustInnerSize(int row, int col, int itemWidth, int itemHeight);
        void AdjustInnerLocation(int row, int col, int x, int y);
    }

    public interface IHasHeader
    {
        HeaderFeature Header { get; set; }
    }
    public interface IHasDropDown
    {
        DropDownFeature DropDownFeatures { get; }
    }

    /// <summary>
    /// Define que esse CustomControl deve implementar uma MatrixFeature
    /// </summary>
    public interface IHasMatrix
    {
        MatrixFeature Matrix { get; }
    }
    public interface ILinearAdjustable
    {
        void AdjustInnerSize(int index, int itemWidth, int itemHeight);
        void AdjustInnerLocation(int index, int x, int y);
    }
    public abstract partial class CustomControl : UserControl
    {              
         
        public CustomControl()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            InnerControls = new InnerControls(this);
        }

        // ctor opcional: recebe um pai e copia estilo
        protected CustomControl(CustomControl parent) : this()
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            InnerControls = new InnerControls(this);

            CopyVisualFrom(parent);
        }

        /// <summary>
        /// Copia propriedades visuais compartilhadas do controle <paramref name="parentControl"/>.
        /// Chame este método somente quando fizer sentido (ex.: drop-downs).
        /// </summary>
        protected virtual void CopyVisualFrom(CustomControl parentControl)
        {
            if (parentControl is null)
                throw new ArgumentNullException(nameof(parentControl));

            // --- Visuals "core" compartilhados ---
            BorderRadius = parentControl.BorderRadius;
            BorderWidth = parentControl.BorderWidth;
            BorderColor = parentControl.BorderColor;
            BackgroundColor = parentControl.BackgroundColor;
            ForeColor = parentControl.ForeColor;
            Font = parentControl.Font;
            HorizontalPadding = parentControl.HorizontalPadding;
            VerticalPadding = parentControl.VerticalPadding;
            Width = parentControl.Width;


            // Se o destino não implementa cabeçalho, não há o que fazer
            if (this is not IHasHeader destinationControl)
                return;

            // Cria/pega o mesmo header UMA vez e reutiliza
            var destinationHeader = destinationControl.Header ??= new HeaderFeature(this);

            // Copia de IHasHeader -> IHasHeader
            if (parentControl is IHasHeader sourceControl)
            {
                var sourceHeader = sourceControl.Header;
                destinationHeader.BackgroundColor = sourceHeader.BackgroundColor;
                destinationHeader.ForeColor = sourceHeader.ForeColor;
                destinationHeader.HeightMode = sourceHeader.HeightMode;
                destinationHeader.HeightRelativePercent = sourceHeader.HeightRelativePercent;
                destinationHeader.BorderRadius = sourceHeader.BorderRadius;
            }

            // Copia de IHasDropDown -> IHasHeader (cores “padrão” do header dos dropdowns)
            if (parentControl is IHasDropDown srcDrop &&
                srcDrop.DropDownFeatures.AnyIsHasHeader)
            {
                destinationHeader.BackgroundColor = srcDrop.DropDownFeatures.HeaderBackgroundColor;
                destinationHeader.ForeColor = srcDrop.DropDownFeatures.HeaderForeColor;

                destinationHeader.HoverBackgroundColor = srcDrop.DropDownFeatures.HeaderHoverBackgroundColor;
                destinationHeader.HoverForeColor = srcDrop.DropDownFeatures.HeaderHoverForeColor;
            }                                       
        }
  
        /// <summary>
        /// Override do evento de clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do clique.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            InnerControls.HandleClick(this, e.Location); // detecta clique virtual
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleMouseClick(e.Location);
        }

        /// <summary>
        /// Override do evento de duplo clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do duplo clique.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            InnerControls.HandleDoubleClick(this, e.Location); 
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleMouseDoubleClick(e.Location);
        }

        /// <summary>
        /// Override do evento de movimento do mouse. Propaga o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do movimento do mouse.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InnerControls.HandleMouseMove(this, e.Location); 
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleMouseMove(e.Location);
        }

        /// <summary>
        /// Override do evento quando o controle ganha foco.
        /// </summary>
        /// <param name="e">Argumentos do foco.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (Focused) 
            {
                InnerControls.HandleGotFocus(this, PointToClient(Cursor.Position));
                var headerFeature = (this as IHasHeader)?.Header;
                headerFeature?.HandleGotFocus(PointToClient(Cursor.Position));

            }               
        }

        /// <summary>
        /// Override do evento quando o controle perde o foco.
        /// </summary>
        /// <param name="e">Argumentos do evento de perda de foco.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            InnerControls.HandleLostFocus(this, PointToClient(Cursor.Position));
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleLostFocus(PointToClient(Cursor.Position));
        }
        
        /// <summary>
        /// Realiza o desenho do fundo do controle, respeitando a área interna delimitada pela borda.
        /// Usa RectBackgroundPath com offset calculado baseado em ClientRectangle.
        /// </summary>
        protected virtual void DrawControlBackground(PaintEventArgs e)
        {
            // Calcula o offset interno com base na espessura da borda
            int offset = BorderWidth > 1 ? BorderWidth - 1 : 0;

            // Usa ClientRectangle e aplica offset interno, garantindo que o Path fique dentro da área do controle
            Rectangle backgroundRect = new Rectangle(
                offset,
                offset,
                Width - (2 * offset),
                Height - (2 * offset)
            );

            // Garante que largura e altura sejam válidas
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;

            // Cria o path com raio original
            using (GraphicsPath backgroundPath = NhegazDrawingMethods.RectBackgroundPath(backgroundRect, BorderRadius))
            {
                // Preenche o fundo com a cor do controle
                using (SolidBrush brush = new SolidBrush(BackgroundColor))
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }

                // Define a área de recorte (clip) para limitar os próximos desenhos, se necessário
                e.Graphics.SetClip(backgroundPath);
            }
        }


        /// <summary>
        /// Metodo que realiza o desenho dos InnerControls.
        /// </summary>
        protected virtual void DrawInnerControls(PaintEventArgs e)
        {
            InnerControls.OnPaintAll(this, e); 
            e.Graphics.ResetClip();
        }

        /// <summary>
        /// Metodo que realiza o desenho da borda do CustomControl.
        /// </summary>
        protected virtual void DrawBorder(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color paintBorderColor = OnFocus ? OnFocusBorderColor : BorderColor;
            int borderWidth = OnFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;

            using (GraphicsPath borderPath = NhegazDrawingMethods.ControlBorderPath(new Rectangle(Location, Size), BorderRadius, borderWidth))
            {              
                if (borderWidth > 1)
                {                   
                    using (SolidBrush borderBrush = new SolidBrush(paintBorderColor))
                    {
                        e.Graphics.FillPath(borderBrush, borderPath);
                    }
                }
                e.Graphics.DrawPath(new Pen(paintBorderColor, 1f), borderPath);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.None;
            DrawControlBackground(e);
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.PaintHeader(e);
            DrawInnerControls(e);
            DrawBorder(e);
        }

    }
}
