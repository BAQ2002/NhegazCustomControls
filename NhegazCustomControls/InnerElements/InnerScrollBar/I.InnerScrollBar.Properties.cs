using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerScrollBar
    {

        private int minimum = 0;                       //Valor mínimo do range.
        private int maximum = 0;                       //Valor máximo do range.
        private int value = 0;                       //Valor atual da barra.
        private int viewportSize = 0;                       //Tamanho da área visível (viewport).



        /// <summary>
        /// Orientação da barra de rolagem ->
        /// Vertical ou Horizontal.
        /// </summary>
        public ScrollBarOrientation Orientation { get; set; } = ScrollBarOrientation.Vertical;
        public Point UpIconLocation => new(RelativeCenterX(IconSize.Width), Top);
        public Point DownIconLocation => new(RelativeCenterX(IconSize.Width), Top + Height - IconSize.Height);

        public Size IconSize => new(10, 10);

        /// <summary>
        /// Tamanho da área visível do CustomControl.
        /// </summary>
        public Rectangle parentContentView = Rectangle.Empty;

        /// <summary>
        /// Tamanho de todo o conteúdo do CustomControl.
        /// </summary>
        public Rectangle ParentFullContent = Rectangle.Empty;

        /// <summary>
        /// Proporção entre a largura Completa do Conteúdo do CustomControl
        /// e a largura do área visível do CustomControl.
        /// </summary>
        public float HorizontalRatio => ParentFullContent.Width / parentContentView.Width;

        /// <summary>
        /// Proporção entre a Altura Completa do Conteúdo do CustomControl
        /// e a Altura do área visível do CustomControl.
        /// </summary>
        public float VerticalRatio => parentContentView.Height / ParentFullContent.Height;

        private bool isDragging = false;                   //Indica se o thumb está sendo arrastado.
        private int dragOffset = 0;                       //Offset entre o ponto clicado e o início do thumb.

        private Rectangle thumbBounds = Rectangle.Empty;         //Retângulo atual do thumb na trilha.
    }
}
