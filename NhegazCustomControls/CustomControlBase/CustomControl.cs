using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public interface IMatrixAdjustable
    {
        void AdjustInnerSize(int row, int col, int itemWidth, int itemHeight);
        void AdjustInnerLocation(int row, int col, int x, int y);
    }
    public interface IHeaderContainer
    {
        Rectangle HeaderBounds { get; set; }  // onde o cabeçalho está no controle
        void AdjustHeaderContainerSize();
        void AdjustHeaderLayout();            // organiza os elementos internos
    }

    public interface ILinearAdjustable
    {
        void AdjustInnerSize(int index, int itemWidth, int itemHeight);
        void AdjustInnerLocation(int index, int x, int y);
    }
    public abstract class CustomControl : UserControl
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
        private Color dropdownBorderColor = Color.Green;

        private Color onFocusBorderColor = SystemColors.Highlight; //Cor da borda
        private Color hoverColor = SystemColors.Highlight;

        private PaddingMode paddingMode = PaddingMode.Absolute;      

        private void RequestLayout()
        {
            if (layoutPending) return;          // já há uma requisição na fila
            layoutPending = true;

            // Delega a execução para o loop de mensagens do WinForms.
            // BeginInvoke garante que o código rode na UI thread,
            // depois que todos os setters atuais terminarem.
            BeginInvoke((Action)(() =>
            {
                layoutPending = false;
                AdjustControlSize();            // calcula padding, posições, tamanhos
                Invalidate();                   // UM repaint
            }));
        }

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

        public float PaddingRelativePercent
        {
            get => paddingRelativePercent;
            set
            {
                // Garante que esteja entre 0 e 1
                paddingRelativePercent = Math.Max(0f, Math.Min(1f, value));
                if (PaddingMode == PaddingMode.RelativeToFont)
                {
                    Invalidate();
                }
            }
        }

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
            BorderRadius = parentControl.BorderRadius;
            BorderWidth = parentControl.BorderWidth;
            BorderColor = parentControl.BorderColor;
            BackgroundColor = parentControl.BackgroundColor;
            ForeColor = parentControl.ForeColor;
            Font = parentControl.Font;
            HorizontalPadding = parentControl.HorizontalPadding;
            VerticalPadding = parentControl.VerticalPadding;
            Width = parentControl.Width;

            // Copia parte do cabeçalho só se AMBOS tiverem cabeçalho
            if (this is CustomControlWithHeader dstHeader &&
                parentControl is CustomControlWithHeader srcHeader)
            {
                dstHeader.HeaderBackgroundColor = srcHeader.HeaderBackgroundColor;
                // …outras props do header…
            }
        }

        //protected abstract void SetHooverColors();

        /// <summary>
        /// Metodo responsavel pelo ajuste dos valores de Padding.
        /// </summary>
        protected virtual void AdjustPadding()
        {
            if (PaddingMode == PaddingMode.RelativeToFont)
            {
                Size unit = NhegazSizeMethods.FontUnitSize(Font); //Tamanho "unit" unitario da fonte
                HorizontalPadding = (int)Math.Round(unit.Width * paddingRelativePercent);
                VerticalPadding = (int)Math.Round(unit.Height * paddingRelativePercent);
            }
            // Se for Absolute, não altera — valores já foram definidos diretamente
        }

        protected virtual void AdjustHoverColors()
        {

        }

        /// <summary>
        /// Método responsavel pelo ajuste do tamanho dos InnerControls.
        /// </summary>
        protected virtual void AdjustInnerSizes()
        { }    
        protected virtual void AdjustInnerSizes(int index, int itemWidth, int ItemHeight)
        { }
        protected virtual void AdjustInnerSizes(int row, int col, int itemWidth, int ItemHeight)
        { }

        /// <summary>
        /// Metodo responsavel pelo ajuste das posicoes dos InnerControls.
        /// </summary>
        /// 
        protected virtual void AdjustInnerLocations()
        { }
        protected virtual void AdjustInnerLocations(int index, int x, int y)
        { }
        protected virtual void AdjustInnerLocations(int row, int col, int x, int y)
        { }     

        /// <summary>
        /// Metodo responsavel por definir o MinimumSize a partir dos InnerControls.
        /// </summary>
        protected virtual void SetMinimumSize()
        {

        }

        /// <summary>
        /// Metodo que invoca todos ajustes de posicoes e tamanhos.
        /// </summary>
        protected virtual void AdjustControlSize()
        {
            AdjustPadding();
            AdjustInnerLocations();
            AdjustInnerSizes();
            SetMinimumSize();
            Invalidate();
        }

        /// <summary>
        /// Override do evento de clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do clique.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            InnerControls.HandleClick(this, e.Location); // detecta clique virtual
        }

        /// <summary>
        /// Override do evento de duplo clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do duplo clique.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            InnerControls.HandleDoubleClick(this, e.Location);
        }

        /// <summary>
        /// Override do evento de movimento do mouse. Propaga o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do movimento do mouse.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InnerControls.HandleMouseMove(this, e.Location);
        }

        /// <summary>
        /// Override do evento quando o controle ganha foco.
        /// </summary>
        /// <param name="e">Argumentos do foco.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (Focused)
                InnerControls.HandleGotFocus(this, PointToClient(Cursor.Position));
        }

        /// <summary>
        /// Override do evento quando o controle perde o foco.
        /// </summary>
        /// <param name="e">Argumentos do evento de perda de foco.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            InnerControls.HandleLostFocus(this, PointToClient(Cursor.Position));
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

            Color paintBorderColor = OnFocusBool ? OnFocusBorderColor : BorderColor;
            int borderWidth = OnFocusBool ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;

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
            DrawInnerControls(e);
            DrawBorder(e);
        }

    }
}
