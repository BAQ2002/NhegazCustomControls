using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public static partial class NhegazDrawingMethods
    {

        public static GraphicsPath AddIconPath(Size iconSize, float locX, float locY)
        {
            GraphicsPath path = new GraphicsPath();

            float width = iconSize.Width;
            float height = iconSize.Height;

            float centerX = locX + (width / 2f);
            float centerY = locY + (height / 2f);

            float stroke = Math.Min(width, height) / 5f;   // espessura dos traços
            float halfStroke = stroke / 2f;

            // Linha horizontal ocupando toda a largura do retângulo
            path.StartFigure();
            path.AddRectangle(new RectangleF(
                locX,
                centerY - halfStroke,
                width,
                stroke));

            // Linha vertical ocupando toda a altura do retângulo
            path.StartFigure();
            path.AddRectangle(new RectangleF(
                centerX - halfStroke,
                locY,
                stroke,
                height));

            return path;
        }
        public static GraphicsPath AddIconPath(Size iconSize, Point location) => AddIconPath(iconSize, location.X, location.Y);


        public static GraphicsPath RightArrowGPath(Size iconSize, float locX, float locY)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float width = iconSize.Width;
            float height = iconSize.Height;

            float centerY = locY + (height / 2f);

            // Triângulo apontando para a direita dentro do retângulo [locX, locX+width] x [locY, locY+height]
            PointF leftTop = new PointF(locX, locY);
            PointF leftBottom = new PointF(locX, locY + height);
            PointF rightCenter = new PointF(locX + width, centerY);

            iconPath.StartFigure();
            iconPath.AddLine(leftTop, rightCenter);
            iconPath.AddLine(rightCenter, leftBottom);
            iconPath.AddLine(leftBottom, leftTop);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath RightArrowGPath(Size iconSize, Point location) => RightArrowGPath(iconSize, location.X, location.Y);


        public static GraphicsPath LeftArrowGPath(Size iconSize, float locX, float locY)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float width = iconSize.Width;
            float height = iconSize.Height;

            float centerY = locY + (height / 2f);

            // Triângulo apontando para a esquerda dentro do retângulo
            PointF rightTop = new PointF(locX + width, locY);
            PointF rightBottom = new PointF(locX + width, locY + height);
            PointF leftCenter = new PointF(locX, centerY);

            iconPath.StartFigure();
            iconPath.AddLine(rightTop, leftCenter);
            iconPath.AddLine(leftCenter, rightBottom);
            iconPath.AddLine(rightBottom, rightTop);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath LeftArrowGPath(Size iconSize, Point location) => LeftArrowGPath(iconSize, location.X, location.Y);

        public static GraphicsPath UpArrowGPath(Size iconSize, float locX, float locY)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float width = iconSize.Width; // altura do triângulo equilátero
            float height = iconSize.Height; // altura do triângulo equilátero
            float halfIconWidth = iconSize.Width / 2f;
            

            // Triângulo isósceles apontando para cima
            PointF bottomLeft  = new PointF(locX                , locY + height);
            PointF bottomRight = new PointF(locX + width        , locY + height);
            PointF topCenter   = new PointF(locX + halfIconWidth, locY);

            iconPath.StartFigure();
            iconPath.AddLine(bottomLeft, bottomRight);
            iconPath.AddLine(bottomRight, topCenter);
            iconPath.AddLine(topCenter, bottomLeft);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath UpArrowGPath(Size iconSize, Point location) => UpArrowGPath(iconSize, location.X, location.Y);

        public static GraphicsPath DownArrowGPath(Size iconSize, float locX, float locY)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float width = iconSize.Width;
            float height = iconSize.Height;
            float halfIconWidth = width / 2f;

            // Triângulo isósceles apontando para baixo
            PointF topLeft      = new PointF(locX                , locY);
            PointF topRight     = new PointF(locX + width        , locY);
            PointF bottomCenter = new PointF(locX + halfIconWidth, locY + height);

            iconPath.StartFigure();
            iconPath.AddLine(topLeft, topRight);
            iconPath.AddLine(topRight, bottomCenter);
            iconPath.AddLine(bottomCenter, topLeft);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath DownArrowGPath(Size iconSize, Point location) => DownArrowGPath(iconSize, location.X, location.Y);
    }
}
