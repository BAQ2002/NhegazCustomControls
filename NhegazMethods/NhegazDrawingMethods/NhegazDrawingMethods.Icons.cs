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
        public static GraphicsPath AddIconPath(InnerControl innerControl, int iconSize)
        {
            GraphicsPath path = new GraphicsPath();

            float centerX = innerControl.Location.X + (innerControl.Width / 2f);
            float centerY = innerControl.Location.Y + (innerControl.Height / 2f);

            float halfThickness = iconSize / 6f; // espessura dos traços
            float halfLength = iconSize / 2f;

            // Linha horizontal
            path.StartFigure();
            path.AddRectangle(new RectangleF(
                centerX - halfLength,
                centerY - halfThickness,
                iconSize,
                halfThickness * 2));

            // Linha vertical
            path.StartFigure();
            path.AddRectangle(new RectangleF(
                centerX - halfThickness,
                centerY - halfLength,
                halfThickness * 2,
                iconSize));

            return path;
        }

        public static GraphicsPath DropDownIconPath(InnerControl innerControl, int iconSize)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float centerX = innerControl.Location.X + ((innerControl.Width - 1) / 2f);
            float centerY = innerControl.Location.Y + ((innerControl.Height - 1) / 2f);

            float halfIconSize = iconSize / 2f;
            float height = iconSize * (float)Math.Sqrt(3) / 2f; // altura de triângulo equilátero; pode ajustar se quiser mais achatado

            // Triângulo isósceles apontando para baixo
            PointF topLeft = new PointF(centerX - halfIconSize, centerY - height / 2);
            PointF topRight = new PointF(centerX + halfIconSize, centerY - height / 2);
            PointF bottomCenter = new PointF(centerX, centerY + height / 2);

            iconPath.StartFigure();
            iconPath.AddLine(topLeft, topRight);
            iconPath.AddLine(topRight, bottomCenter);
            iconPath.AddLine(bottomCenter, topLeft);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath ForwardIconPath(InnerControl innerControl, int iconSize)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float centerX = innerControl.Location.X + ((innerControl.Width - 1) / 2f);
            float centerY = innerControl.Location.Y + ((innerControl.Height - 1) / 2f);

            float halfIconSize = iconSize / 2f;
            float height = iconSize * (float)Math.Sqrt(3) / 2f;

            // Triângulo apontando para a direita
            PointF top = new PointF(centerX - height / 2, centerY - halfIconSize);
            PointF middleRight = new PointF(centerX + height / 2, centerY);
            PointF bottom = new PointF(centerX - height / 2, centerY + halfIconSize);

            iconPath.StartFigure();
            iconPath.AddLine(top, middleRight);
            iconPath.AddLine(middleRight, bottom);
            iconPath.AddLine(bottom, top);
            iconPath.CloseFigure();

            return iconPath;
        }
        public static GraphicsPath BackwardIconPath(InnerControl innerControl, int iconSize)
        {
            GraphicsPath iconPath = new GraphicsPath();

            float centerX = innerControl.Location.X + ((innerControl.Width - 1) / 2f);
            float centerY = innerControl.Location.Y + ((innerControl.Height - 1) / 2f);

            float halfIconSize = iconSize / 2f;
            float height = iconSize * (float)Math.Sqrt(3) / 2f;

            // Triângulo apontando para a esquerda
            PointF top = new PointF(centerX + height / 2, centerY - halfIconSize);
            PointF middleLeft = new PointF(centerX - height / 2, centerY);
            PointF bottom = new PointF(centerX + height / 2, centerY + halfIconSize);

            iconPath.StartFigure();
            iconPath.AddLine(top, middleLeft);
            iconPath.AddLine(middleLeft, bottom);
            iconPath.AddLine(bottom, top);
            iconPath.CloseFigure();

            return iconPath;
        }
    }
}
