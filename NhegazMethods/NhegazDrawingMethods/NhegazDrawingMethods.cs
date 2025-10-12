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
            int width = rect.Width - 1; //Ajuste necessario do Width para ficar dentro do tamanho do innerControl
            int height = rect.Height - 1; //Ajuste necessario do Height para ficar dentro do tamanho do innerControl

            GraphicsPath borderPath = new();

            var arcTopLeft = GenerateArc(borderRadius); // 0° → 90°
            var arcTopRight = arcTopLeft.Select(p => new PointF(width - p.X, p.Y)).Reverse();
            var arcBottomRight = arcTopLeft.Select(p => new PointF(width - p.X, height - p.Y));
            var arcBottomLeft = arcTopLeft.Select(p => new PointF(p.X, height - p.Y)).Reverse();

            borderPath.StartFigure();

            borderPath.AddLines(arcTopLeft.ToArray());
            borderPath.AddLines(arcTopRight.ToArray());
            borderPath.AddLines(arcBottomRight.ToArray());
            borderPath.AddLines(arcBottomLeft.ToArray());

            borderPath.CloseFigure();

            if (borderWidth > 1)
            {
                int offset = borderWidth - 1;
                var arcInner = GenerateArc(borderRadius * 1f); // já vem invertido

                borderPath.StartFigure();

                var innerTopLeft = arcInner.Select(p => new PointF(p.X + offset, p.Y + offset));
                var innerTopRight = arcInner.Select(p => new PointF(width - p.X - offset, p.Y + offset)).Reverse();
                var innerBottomRight = arcInner.Select(p => new PointF(width - p.X - offset, height - p.Y - offset));
                var innerBottomLeft = arcInner.Select(p => new PointF(p.X + offset, height - p.Y - offset)).Reverse();

                borderPath.AddLines(innerTopLeft.ToArray());
                borderPath.AddLines(innerTopRight.ToArray());
                borderPath.AddLines(innerBottomRight.ToArray());
                borderPath.AddLines(innerBottomLeft.ToArray());

                borderPath.CloseFigure();
            }
            return borderPath;
        }
        public static GraphicsPath HeaderRectBorderPath(Rectangle rect, float borderRadius, int borderWidth)
        {
            int locX = rect.X;
            int locY = rect.Y;

            int width = rect.Width - 1; //Ajuste necessario do Width para ficar dentro do tamanho do innerControl
            int height = rect.Height - 1; //Ajuste necessario do Height para ficar dentro do tamanho do innerControl

            GraphicsPath borderPath = new();

            var baseArc = GenerateArc(borderRadius); // 0°→90° (como hoje)

            // Borda externa com offset de localização
            var arcTopLeft = baseArc.Select(p => new PointF(locX + p.X, locY + p.Y));
            var arcTopRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + p.Y)).Reverse();
            var arcBottomRight = baseArc.Select(p => new PointF(locX + (width - p.X), locY + (height - p.Y)));
            var arcBottomLeft = baseArc.Select(p => new PointF(locX + p.X, locY + (height - p.Y))).Reverse();

            borderPath.StartFigure();

            borderPath.AddLines(arcTopLeft.ToArray());
            borderPath.AddLines(arcTopRight.ToArray());
            borderPath.AddLines(arcBottomRight.ToArray());
            borderPath.AddLines(arcBottomLeft.ToArray());

            borderPath.CloseFigure();

            if (borderWidth > 1)
            {
                int offset = borderWidth - 1;
                var arcInner = GenerateArc(borderRadius * 1f); // já vem invertido

                var innerTopLeft = arcInner.Select(p => new PointF(locX + p.X + offset, locY + p.Y + offset));
                var innerTopRight = arcInner.Select(p => new PointF(locX + (width - p.X) - offset, locY + p.Y + offset)).Reverse();
                var innerBottomRight = arcInner.Select(p => new PointF(locX + (width - p.X) - offset, locY + (height - p.Y) - offset));
                var innerBottomLeft = arcInner.Select(p => new PointF(locX + p.X + offset, locY + (height - p.Y) - offset)).Reverse();

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

        public static GraphicsPath RectBackgroundPath(Rectangle rect, int radius)
        {
            if (radius <= 0) //Se radius for <= 0 retorna um retangulo
            {
                GraphicsPath path = new();
                path.AddRectangle(rect);
                return path;
            }

            int locX = rect.Location.X;
            int locY = rect.Location.Y;

            int width = rect.Width-1;
            int height = rect.Height-1;

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