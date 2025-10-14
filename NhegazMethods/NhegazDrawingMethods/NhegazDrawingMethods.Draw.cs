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

            rect.Size = new Size(rect.Width - 1, rect.Height - 1);

            int left = rect.Left; int top = rect.Top;
            int right = rect.Right; int bottom = rect.Bottom;

           // radius = Math.Min(radius, Math.Min(width, height) / 2);

            GraphicsPath FullPath = new();
            FullPath.StartFigure();

            var baseArc = GenerateArc(radius);

            var arcTopLeft = baseArc.Select(p => new PointF(left + p.X, top + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(right - p.X, top + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(right - p.X, bottom - p.Y));
            var arcBottomLeft = baseArc.Select(p => new PointF(left + p.X, bottom - p.Y)).Reverse();

            FullPath.AddLines(arcTopLeft.ToArray());
            FullPath.AddLines(arcTopRight.ToArray());
            FullPath.AddLines(arcBottomRight.ToArray());
            FullPath.AddLines(arcBottomLeft.ToArray());

            FullPath.CloseFigure();
            return FullPath;
        }

        /// <summary>
        /// A partir das propriedades de CustomControl retorna um GraphicsPath que representa a area da sua Border.
        /// </summary>

        public static GraphicsPath RectBorderPath(Rectangle rect, float borderRadius, int borderWidth)
        {
            rect.Size = new Size(rect.Width - 1, rect.Height - 1);

            int left = rect.Left; int top = rect.Top;
            int right = rect.Right; int bottom = rect.Bottom;

            GraphicsPath borderPath = new();

            var baseArc = GenerateArc(borderRadius);

            var arcTopLeft = baseArc.Select(p => new PointF(left + p.X, top + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(right - p.X, top + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(right - p.X, bottom - p.Y));
            var arcBottomLeft = baseArc.Select(p => new PointF(left + p.X, bottom - p.Y)).Reverse();

            borderPath.StartFigure();

            borderPath.AddLines(arcTopLeft.ToArray());
            borderPath.AddLines(arcTopRight.ToArray());
            borderPath.AddLines(arcBottomRight.ToArray());
            borderPath.AddLines(arcBottomLeft.ToArray());

            borderPath.CloseFigure();

            if (borderWidth > 1)
            {
                int offset = borderWidth - 1;
                var baseInnerArc = GenerateArc(borderRadius * 1f);

                var innerTopLeft = baseInnerArc.Select(p => new PointF(left + p.X + offset, top + p.Y + offset));
                var innerTopRight = baseInnerArc.Select(p => new PointF(right - p.X - offset, top + p.Y + offset)).Reverse();
                var innerBottomRight = baseInnerArc.Select(p => new PointF(right - p.X - offset, bottom - p.Y - offset));
                var innerBottomLeft = baseInnerArc.Select(p => new PointF(left + p.X + offset, bottom - p.Y - offset)).Reverse();

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

            int left = innerControl.Left; int top = innerControl.Top;
            int right = innerControl.Right; int bottom = innerControl.Bottom;

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

            var arcTopLeft = baseArc.Select(p => new PointF(left + p.X, top + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(right - p.X, top + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(right - p.X, bottom - p.Y));
            var arcBottomLeft = baseArc.Select(p => new PointF(left + p.X, bottom - p.Y)).Reverse();

            FullPath.AddLines(arcTopLeft.ToArray());
            FullPath.AddLines(arcTopRight.ToArray());
            FullPath.AddLines(arcBottomRight.ToArray());
            FullPath.AddLines(arcBottomLeft.ToArray());

            FullPath.CloseFigure();      
            return FullPath;
        }

        
    }
}