using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NhegazCustomControls
{
    public static partial class NhegazDrawingMethods
    {
            
        /// <summary>
        /// A partir das propriedades de CustomControl retorna um GraphicsPath que representa a area da sua Border.
        /// </summary>
        public static GraphicsPath RectBorderPath(Rectangle rect, float borderRadius, int borderWidth)
        {
            int locX = rect.Location.X; int width = rect.Width - 1;
            int locY = rect.Location.Y; int height = rect.Height - 1;

            GraphicsPath borderPath = new();

            var baseArc = GenerateArc(borderRadius);

            var arcTopLeft = baseArc.Select(p => new PointF(p.X, p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(width - p.X, p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(width - p.X, height - p.Y));
            var arcBottomLeft = baseArc.Select(p => new PointF(p.X, height - p.Y)).Reverse();

            borderPath.StartFigure();

            borderPath.AddLines(arcTopLeft.ToArray());
            borderPath.AddLines(arcTopRight.ToArray());
            borderPath.AddLines(arcBottomRight.ToArray());
            borderPath.AddLines(arcBottomLeft.ToArray());

            borderPath.CloseFigure();

            if (borderWidth > 1)
            {
                int offset = borderWidth - 1;
                var baseInnerArc = GenerateArc(borderRadius * 1f); // já vem invertido

                

                var innerTopLeft = baseInnerArc.Select(p => new PointF(p.X + offset, p.Y + offset));
                var innerTopRight = baseInnerArc.Select(p => new PointF(width - p.X - offset, p.Y + offset)).Reverse();
                var innerBottomRight = baseInnerArc.Select(p => new PointF(width - p.X - offset, height - p.Y - offset));
                var innerBottomLeft = baseInnerArc.Select(p => new PointF(p.X + offset, height - p.Y - offset)).Reverse();

                borderPath.StartFigure();

                borderPath.AddLines(innerTopLeft.ToArray());
                borderPath.AddLines(innerTopRight.ToArray());
                borderPath.AddLines(innerBottomRight.ToArray());
                borderPath.AddLines(innerBottomLeft.ToArray());

                borderPath.CloseFigure();
            }
            return borderPath;
        }
      

        /// <summary>
        /// A partir das propriedades de InnerControl retorna um GraphicsPath que representa a area interna do InnerControl.
        /// </summary>
        public static GraphicsPath InnerControlBackgroundPath(InnerControl innerControl)
        {
            int reference = innerControl.Height;
            float radius = reference / 2;         
            
            int locX = innerControl.Location.X;
            int locY = innerControl.Location.Y;

            int width = innerControl.Width - 1;
            int height = innerControl.Height - 1;
            
            GraphicsPath FullPath = new();
            FullPath.StartFigure();

            switch (innerControl.BackGroundShape)
            {               
                case BackGroundShape.FitRectangle:

                    Rectangle rect = new(innerControl.Location, innerControl.Size);
                    FullPath.AddRectangle(rect);

                    FullPath.CloseFigure();
                    return FullPath;

                case BackGroundShape.SymmetricCircle:
                    radius = reference / 2;

                    break;
                case BackGroundShape.RoundedRectangle:
                    radius = reference / 8;

                    break;                       
            }

            var baseArc = GenerateArc(radius);

            var arcTopLeft = baseArc.Select(p => new PointF(locX + p.X, locY + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + (height - p.Y)));
            var arcBottomLeft = baseArc.Select(p => new PointF(locX + p.X, locY + (height - p.Y))).Reverse();

            FullPath.AddLines(arcTopLeft.ToArray());
            FullPath.AddLines(arcTopRight.ToArray());
            FullPath.AddLines(arcBottomRight.ToArray());
            FullPath.AddLines(arcBottomLeft.ToArray());

            FullPath.CloseFigure();      
            return FullPath;
        }

        /// <summary>
        /// Retorna um GraphicsPath com forma de um retangulo com quinas arredondadas com base em um Rectangle e um int Radius.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath RectBackgroundPath(Rectangle rect, int radius)
        {
            if (radius <= 0) //Se radius for <= 0 retorna um retangulo
            {
                GraphicsPath path = new();
                path.AddRectangle(rect);
                return path;
            }

            int locX = rect.Location.X; int width = rect.Width;
            int locY = rect.Location.Y; int height = rect.Height;

            radius = Math.Min(radius, Math.Min(width, height) / 2);

            GraphicsPath FullPath = new();
            FullPath.StartFigure();

            var baseArc = GenerateArc(radius);

            var arcTopLeft = baseArc.Select(p => new PointF(locX + p.X, locY + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + (height - p.Y)));
            var arcBottomLeft = baseArc.Select(p => new PointF(locX + p.X, locY + (height - p.Y))).Reverse();

            FullPath.AddLines(arcTopLeft.ToArray());
            FullPath.AddLines(arcTopRight.ToArray());
            FullPath.AddLines(arcBottomRight.ToArray());
            FullPath.AddLines(arcBottomLeft.ToArray());

            FullPath.CloseFigure();
            return FullPath;
        }
    }
}