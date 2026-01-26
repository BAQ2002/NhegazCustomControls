using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class InnerControl
    {
        /// <summary>Método responsável por acionar os ajustes de posições e tamanhos.</summary>
        protected virtual void UpdateLayout()
        {
            if (BackGroundShape == BackGroundShape.SymmetricCircle)
            {
                SymmetricalCircleAdjust();
            }
        }

        /// <summary>
        /// Método responsável por realizar ajustes se <see cref="BackgroundShape"/> 
        /// = <see cref="SymmetricalCircle"/> ->
        /// Define a altura e largura sempre iguais à maior entre as duas.
        /// </summary>
        protected virtual void SymmetricalCircleAdjust()
        {
            if (Size.Width == Size.Height)
                return;

            int reference = Math.Max(Width, Height); Size = new Size(reference, reference);
        }


        public virtual void SetLocation(int x, int y)
        {
            Location = new Point(x, y);
        }
        public virtual void SetLocation(Point location)
        {
            Location = new Point(location.X, location.Y);
        }

        public virtual void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public virtual void SetSize(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }
    }
}
