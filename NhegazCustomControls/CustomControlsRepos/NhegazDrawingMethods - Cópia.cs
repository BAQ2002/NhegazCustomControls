using NhegazCustomControls.PL.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    public static class NhegazDrawingMethodsExplanation
    {
        public static void DrawControl(CustomControl control, PaintEventArgs e)
        {
            int borderRadius = control.BorderRadius;
            Color BackgroundColor = control.BackgroundColor;
            Rectangle rect = new Rectangle(0, 0, control.Width - 1, control.Height - 1);

            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = 18;//OnFocusBool ? (borderRadius * 2) + onFocusBorderExtraWidth : borderRadius * 2;
                int radius = diameter / 2;              //int centerX = (borderRadius % 2 == 0) ? borderRadius / 2 : (borderRadius + 1) / 2;

                path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90); //Arco superior Esquerdo
                path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90); //Arco superior Direito
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90); //Arco Infeiror Direito
                path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90); //Arco Infeiror Esquerdo
                path.CloseFigure();
                using (Region region = new Region(path))
                {
                    e.Graphics.Clip = region;

                    using (SolidBrush brush = new SolidBrush(BackgroundColor))
                    {
                        e.Graphics.FillRectangle(brush, rect);
                    }
                    e.Graphics.ResetClip();
                }

                int borderWidth = control.BorderWidth * 2 - 1;
                int borderFocusWidth = (control.BorderWidth + control.OnFocusBorderExtraWidth) * 2 - 1;

                Pen pen = new(control.OnFocusBool ? control.OnFocusBorderColor : control.BorderColor,
                              control.OnFocusBool ? borderFocusWidth : borderWidth);

                Color arcsColor = Color.FromArgb(128, pen.Color.R, pen.Color.G, pen.Color.B);
                Pen arcsPen = new(arcsColor, 1);

                int innerDiameter = Math.Max(1, diameter - (control.BorderWidth - 1) * 2);
                int positiveOffSet = control.BorderWidth - 1;
                int negativeOffSet = diameter - positiveOffSet;

                using (GraphicsPath path2 = new GraphicsPath()) //Arco Superior Direito
                {
                    path2.AddArc(rect.Right - negativeOffSet, rect.Top + positiveOffSet, innerDiameter, innerDiameter, 265, 100);
                    path2.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 360, -90); //Arco Externo

                    path2.CloseFigure();
                    SolidBrush brush = new SolidBrush(arcsColor);

                    e.Graphics.DrawPath(arcsPen, path2);
                    e.Graphics.FillPath(brush, path2);

                }
                using (GraphicsPath path1 = new GraphicsPath()) //Arco Superior Esquerdo
                {
                    path1.AddArc(rect.Left + positiveOffSet, rect.Top + positiveOffSet, innerDiameter, innerDiameter, 175, 100); //Arco Interno
                    path1.AddArc(rect.Left, rect.Top, diameter, diameter, 270, -90); //Arco Externo

                    path1.CloseFigure();
                    SolidBrush brush = new SolidBrush(pen.Color);

                    e.Graphics.DrawPath(arcsPen, path1);
                    //e.Graphics.FillPath(brush, path1);

                }

                using (GraphicsPath path3 = new GraphicsPath()) //Arco Infeiror Esquerdo
                {
                    path3.AddArc(rect.Left + positiveOffSet, rect.Bottom - negativeOffSet, innerDiameter, innerDiameter, 85, 100); //Arco Interno
                    path3.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 180, -90); //Arco Externo

                    path3.CloseFigure();
                    SolidBrush brush = new SolidBrush(arcsColor);

                    e.Graphics.DrawPath(arcsPen, path3);
                    //e.Graphics.FillPath(brush, path3);
                }
                using (GraphicsPath path4 = new GraphicsPath()) //Arco Inferior Direito
                {
                    path4.AddArc(rect.Right - negativeOffSet, rect.Bottom - negativeOffSet, innerDiameter, innerDiameter, -5, 100); //Arco Interno
                    path4.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 90, -90); //Arco Externo

                    path4.CloseFigure();
                    SolidBrush brush = new SolidBrush(arcsColor);

                    e.Graphics.DrawPath(arcsPen, path4);
                    //e.Graphics.FillPath(brush, path4);
                }

                using (GraphicsPath arcPath = new GraphicsPath())
                {
                    arcPath.AddArc(rect.Left, rect.Top, diameter, diameter, 270, -90); // Arco Externo

                    if (control.BorderWidth > 1)
                    {
                        arcPath.AddArc(rect.Left + positiveOffSet, rect.Top + positiveOffSet, innerDiameter, innerDiameter, 175, 100); // Arco Interno
                        arcPath.CloseFigure();

                        using (SolidBrush brush = new SolidBrush(arcsColor))
                        {
                            e.Graphics.FillPath(brush, arcPath);
                        }
                    }
                    e.Graphics.DrawPath(arcsPen, arcPath);

                    RectangleF bounds = arcPath.GetBounds();
                    PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);

                    for (int i = 1; i <= 3; i++)
                    {
                        bool isLeft = i == 1; // Left -> Right -> Right
                        bool isTop = i == 2; // -> Bottom -> Top-> Bottom

                        using (GraphicsPath newArcPath = (GraphicsPath)arcPath.Clone()) // Clonar o caminho
                        {
                            using (Matrix matrix = new Matrix())
                            {
                                int moveX = isLeft ? 0 : rect.Width; int moveY = isTop ? 0 : rect.Height;
                                int flipHorizontal = isLeft ? 1 : -1; int flipVertical = isTop ? 1 : -1;

                                matrix.Translate(moveX, moveY);
                                matrix.Scale(flipHorizontal, flipVertical);
                                newArcPath.Transform(matrix);
                            }
                            if (control.BorderWidth > 1)
                            {
                                using (SolidBrush brush = new SolidBrush(arcsColor))
                                {
                                    e.Graphics.FillPath(brush, newArcPath);
                                }
                            }
                            e.Graphics.DrawPath(arcsPen, newArcPath); // Desenhar o arco transformado
                        }
                    }
                }
                int ExtraLenght = pen.Width > 1 ? 1 : 0;
                e.Graphics.DrawLine(pen, rect.Left + radius, rect.Top, rect.Right - radius + ExtraLenght, rect.Top); //Linha Superior
                e.Graphics.DrawLine(pen, rect.Left, rect.Top + radius, rect.Left, rect.Bottom - radius + ExtraLenght); //Linha Esquerda
                e.Graphics.DrawLine(pen, rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius + ExtraLenght); //Linha Direita
                e.Graphics.DrawLine(pen, rect.Left + radius, rect.Bottom, rect.Right - radius + ExtraLenght, rect.Bottom); //Linha Inferior
                //SmoothBorderArcs(diameter, rect, pen, e);
            }
        }

        public static void SmoothBorderArcs(int diameter, Rectangle rect, Pen pen, PaintEventArgs e)
        {
            Color smoothingColor = Color.FromArgb(32, pen.Color.R, pen.Color.G, pen.Color.B);
            Pen smoothArcPen = new(smoothingColor, pen.Width + 0.75f);

            e.Graphics.DrawArc(smoothArcPen, rect.Left, rect.Top, diameter, diameter, 190, 70); //Arco superior Esquerdo
            e.Graphics.DrawArc(smoothArcPen, rect.Right - diameter, rect.Top, diameter, diameter, 280, 70); //Arco superior Direito
            e.Graphics.DrawArc(smoothArcPen, rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 10, 70); //Arco Infeiror Direito
            e.Graphics.DrawArc(smoothArcPen, rect.Left, rect.Bottom - diameter, diameter, diameter, 100, 70); //Arco Infeiror Esquerdo

            Color extraSmoothingColor = Color.FromArgb(16, pen.Color.R, pen.Color.G, pen.Color.B);
            Pen extraSmoothArcPen = new(extraSmoothingColor, pen.Width + 1.5f);

            e.Graphics.DrawArc(extraSmoothArcPen, rect.Left, rect.Top, diameter, diameter, 180, 90); //Arco superior Esquerdo
            e.Graphics.DrawArc(extraSmoothArcPen, rect.Right - diameter, rect.Top, diameter, diameter, 0, -90); //Arco superior Direito
            e.Graphics.DrawArc(extraSmoothArcPen, rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90); //Arco Infeiror Direito
            e.Graphics.DrawArc(extraSmoothArcPen, rect.Left, rect.Bottom - diameter, diameter, diameter, 180, -90); //Arco Infeiror Esquerdo            
        }
    }
}
