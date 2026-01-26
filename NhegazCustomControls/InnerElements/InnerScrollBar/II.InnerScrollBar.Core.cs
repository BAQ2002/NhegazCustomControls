using System;
using System.Drawing;

namespace NhegazCustomControls
{
    /// <summary>
    /// Define a orientação do <see cref="InnerScrollBar"/> ->
    /// Vertical ou Horizontal.
    /// </summary>
    public enum ScrollBarOrientation
    {
        Vertical,
        Horizontal
    }

    /// <summary>
    /// InnerControl responsável por representar uma barra de rolagem customizada.
    /// <para/>
    /// Controla um valor dentro de um intervalo (Minimum/Maximum) considerando
    /// o tamanho da área visível (<see cref="ViewportSize"/>), desenhando um
    /// "thumb" proporcional e emitindo <see cref="ValueChanged"/> a cada alteração.
    /// </summary>
    public partial class InnerScrollBar : InnerControl
    {
        

        /// <summary>
        /// Evento disparado sempre que o <see cref="Value"/> é alterado.
        /// </summary>
        public event EventHandler? ValueChanged;
        public InnerScrollBar() : base()
        {

        }

        public void UpdateViewRatio(Rectangle parentContentViewRect, Rectangle parentFullContentRect)
        {
            parentContentView = parentContentViewRect;
            ParentFullContent = parentFullContentRect;
        }

        
        

        /// <summary>Define qual a cor do texto.</summary>
        public override Color BackgroundColor { get; set; } = SystemColors.ControlText;

        /// <summary>
        /// Valor mínimo permitido para o <see cref="Value"/>.
        /// </summary>
        public int Minimum
        {
            get => minimum;
            set
            {
                minimum = value;                //Define o valor mínimo permitido.
                ClampValue();                  //Garante que Value continue dentro do range.
                UpdateThumb();                 //Atualiza o thumb após alteração do range.
            }
        }

        /// <summary>
        /// Valor máximo do conteúdo para cálculo do range de rolagem.
        /// </summary>
        public int Maximum
        {
            get => maximum;
            set
            {
                maximum = value;               //Define o valor máximo do conteúdo.
                ClampValue();                  //Garante que Value continue dentro do range.
                UpdateThumb();                 //Atualiza o thumb após alteração do range.
            }
        }

        /// <summary>
        /// Tamanho da área visível (viewport) utilizada para:
        /// cálculo do tamanho do thumb e limite superior de <see cref="Value"/>.
        /// </summary>
        public int ViewportSize
        {
            get => viewportSize;
            set
            {
                viewportSize = Math.Max(0, value); //Viewport não pode ser negativo.
                ClampValue();                      //Garante que Value continue dentro do range.
                UpdateThumb();                     //Atualiza o thumb após alteração do viewport.
            }
        }

        /// <summary>
        /// Valor atual de rolagem dentro do intervalo [Minimum .. Maximum - ViewportSize].
        /// </summary>
        public int Value
        {
            get => value;
            set
            {
                int newv = value;                                             //Candidato a novo valor.
                int minv = Minimum;                                           //Limite inferior do range.

                int maxv = Math.Max(Minimum, Maximum - ViewportSize);         //Limite superior ajustado pelo viewport.

                if (newv < minv) newv = minv;                                 //Se menor que o mínimo -> corrige.
                if (newv > maxv) newv = maxv;                                 //Se maior que o máximo -> corrige.

                if (this.value == newv) return;                               //Se não houve mudança -> retorna.

                this.value = newv;                                            //Aplica o novo valor.
                UpdateThumb();                                                //Atualiza o thumb conforme a nova posição.

                ValueChanged?.Invoke(this, EventArgs.Empty);                  //Dispara evento de alteração do valor.
                InvalidateParent?.Invoke();                                   //Solicita redesenho do controle pai.
            }
        }

        /// <summary>
        /// Delta de rolagem para movimentos pequenos
        /// (ex: wheel, teclas de seta).
        /// </summary>
        public int SmallChange { get; set; } = 16;                             //Deslocamento pequeno (ex: wheel ou setas).

        /// <summary>
        /// Delta de rolagem para movimentos grandes
        /// (ex: page up/page down, clique na trilha).
        /// </summary>
        public int LargeChange { get; set; } = 64;                             //Deslocamento grande (ex: page up/down).

        /// <summary>
        /// Trata o clique do mouse no scroll ->
        /// inicia arraste quando clica no thumb
        /// ou realiza page up/down quando clica na trilha.
        /// </summary>
        /// <param name="sender">Origem do evento de mouse.</param>
        /// <param name="mousePoint">Posição do mouse relativa ao controle pai.</param>
        public override void RaiseMouseDown(object sender, Point mousePoint)
        {
            base.RaiseMouseDown(sender, mousePoint);

            if (!Visible || !HitBox(mousePoint)) return; //Se não está visível ou se o clique não está no bounds -> retorna

            if (thumbBounds.Contains(mousePoint)) { isDragging = true; }

            


        }

        /// <summary>
        /// Trata o movimento do mouse ->
        /// se estiver em modo de arraste, converte a posição
        /// do mouse em um novo <see cref="Value"/>.
        /// </summary>
        /// <param name="sender">Origem do evento de mouse.</param>
        /// <param name="mousePoint">Posição atual do mouse relativa ao controle pai.</param>
        public override void RaiseMouseMove(object sender, Point mousePoint)
        {
            base.RaiseMouseMove(sender, mousePoint);

            if (!Visible) return;                                              //Se não está visível -> não interage.
            if (!isDragging) return;                                           //Se não está arrastando -> retorna.

            Point offset = ThumbMouseOffset(mousePoint);
            ThumbLocation = new(mousePoint.X - offset.X, mousePoint.Y - offset.Y);

        }

        /// <summary>
        /// Finaliza o arraste do thumb quando o botão do mouse é liberado.
        /// </summary>
        /// <param name="sender">Origem do evento de mouse.</param>
        /// <param name="p">Posição do mouse no momento do MouseUp.</param>
        public override void RaiseMouseUp(object sender, Point p)
        {
            base.RaiseMouseUp(sender, p);

            if (!Visible) return;                                              //Se não está visível -> não interage.

            isDragging = false;                                                //Finaliza o estado de arraste.
        }

        /// <summary>
        /// Garante que o campo interno <see cref="value"/> esteja
        /// sempre dentro do range permitido considerando <see cref="ViewportSize"/>.
        /// </summary>
        private void ClampValue()
        {
            int minv = Minimum;                                                //Limite inferior.
            int maxv = Math.Max(Minimum, Maximum - ViewportSize);              //Limite superior ajustado pelo viewport.

            if (value < minv) value = minv;                                    //Se menor que o mínimo -> corrige.
            if (value > maxv) value = maxv;                                    //Se maior que o máximo -> corrige.
        }

        /// <summary>
        /// Atualiza o retângulo do thumb (<see cref="thumbBounds"/>)
        /// com base no estado atual da barra (Value, Minimum, Maximum, ViewportSize).
        /// </summary>
        private void UpdateThumb()
        {
            if (!Visible)                                                      //Se invisível -> não há thumb.
            {
                thumbBounds = Rectangle.Empty;
                return;
            }

            int trackStart;
            int trackLength;
            int thumbLength;

            GetTrackInfo(out trackStart, out trackLength, out thumbLength);    //Obtém métricas da trilha e do thumb.

            if (trackLength <= 0 || thumbLength <= 0)                          //Se não houver trilha/thumb -> limpa.
            {
                thumbBounds = Rectangle.Empty;
                return;
            }

            int range = Math.Max(1, (Maximum - Minimum - ViewportSize));      //Range de valores efetivo.
            int pixels = Math.Max(1, (trackLength - thumbLength));             //Range de pixels efetivo.

            double t = (Value - Minimum) / (double)range;                      //Normaliza Value para [0..1].
            int pos = trackStart + (int)Math.Round(t * pixels);               //Converte para posição em pixels.

            if (Orientation == ScrollBarOrientation.Vertical)
                thumbBounds = new Rectangle(Bounds.X, pos, Bounds.Width, thumbLength); //Thumb vertical.
            else
                thumbBounds = new Rectangle(pos, Bounds.Y, thumbLength, Bounds.Height); //Thumb horizontal.
        }

        /// <summary>
        /// Calcula informações da trilha de rolagem:
        /// posição inicial, comprimento disponível e tamanho do thumb.
        /// </summary>
        /// <param name="trackStart">Posição inicial da trilha no eixo de rolagem.</param>
        /// <param name="trackLength">Comprimento total da trilha no eixo de rolagem.</param>
        /// <param name="thumbLength">Tamanho calculado do thumb.</param>
        private void GetTrackInfo(out int trackStart,
                                  out int trackLength,
                                  out int thumbLength)
        {
            int minThumbSize = 16;                                              //Tamanho mínimo visual do thumb.

            if (Orientation == ScrollBarOrientation.Vertical)
            {
                trackStart = Bounds.Y;                                         //Início da trilha vertical.
                trackLength = Bounds.Height;                                    //Comprimento total da trilha.

                int contentSize = Math.Max(1, Maximum - Minimum);              //Conteúdo total (mínimo 1).
                int viewportSize = Math.Max(0, ViewportSize);                   //Área visível (mínimo 0).

                thumbLength = (int)Math.Round(
                                trackLength * (viewportSize / (double)contentSize)
                              );                                                //Thumb proporcional ao viewport.

                if (thumbLength < minThumbSize) thumbLength = minThumbSize;     //Se menor que o mínimo -> corrige.
                if (thumbLength > trackLength) thumbLength = trackLength;      //Se maior que a trilha -> corrige.
            }
            else
            {
                trackStart = Bounds.X;                                         //Início da trilha horizontal.
                trackLength = Bounds.Width;                                     //Comprimento total da trilha.

                int contentSize = Math.Max(1, Maximum - Minimum);              //Conteúdo total (mínimo 1).
                int viewportSize = Math.Max(0, ViewportSize);                   //Área visível (mínimo 0).

                thumbLength = (int)Math.Round(
                                trackLength * (viewportSize / (double)contentSize)
                              );                                                //Thumb proporcional ao viewport.

                if (thumbLength < minThumbSize) thumbLength = minThumbSize;     //Se menor que o mínimo -> corrige.
                if (thumbLength > trackLength) thumbLength = trackLength;      //Se maior que a trilha -> corrige.
            }
        }


    }
}
