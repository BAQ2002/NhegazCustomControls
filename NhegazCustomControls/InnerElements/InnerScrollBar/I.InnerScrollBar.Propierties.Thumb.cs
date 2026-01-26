using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerScrollBar 
    {

        private int thumbHeight = 0;

        private int thumbWidth = 10;
        public int ThumbCornerRadius { get; set; } = 0;

       


        public int ThumbHeight => Orientation == ScrollBarOrientation.Vertical ? (int)(Height / VerticalRatio) : thumbHeight;
        public int ThumbWidth => Orientation == ScrollBarOrientation.Horizontal ? (int)(Height / HorizontalRatio) : thumbWidth;

        public Size ThumbSize => new(ThumbWidth, ThumbHeight);

        public Point ThumbMaxLocation => Location;

        public Point ThumbMinLocation  => new(Location.X + Width - ThumbSize.Width, Location.Y + Height - ThumbSize.Height);


        public Point ThumbLocation { get; set; }

        public Rectangle ThumbRectangle => new(ThumbLocation, ThumbSize);

        /// <summary>
        /// Valor da diferença entre o ponto que foi clicado no Thumb e a coordenada <see cref="ThumbLocation"/>.
        /// </summary>
        public Point ThumbMouseOffset(Point mousePoint) => new(mousePoint.X - ThumbLocation.X, mousePoint.Y - ThumbLocation.Y);


        /// <summary>Cor do "Thumb" - retângulo que se move com o deslizar.</summary>
        public Color ThumbColor { get; set; } = SystemColors.ControlText;

        /// <summary>Cor do "Thumb" - retângulo que se move com o deslizar.</summary>
        public Color ThumbHoverColor { get; set; } = SystemColors.Highlight;
    }
}
