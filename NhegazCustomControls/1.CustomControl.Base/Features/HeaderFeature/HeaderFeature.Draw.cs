using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class HeaderFeature
    {
        private void DrawBackground(PaintEventArgs e)
        {
   
            if (BackgroundRectangle.Width <= 0 || BackgroundRectangle.Height <= 0)
                return;

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            NhegazDrawingMethods.DrawRectangularPath(e, BackgroundRectangle, BackgroundCornerRaidus, BackgroundColor, true);
        }

        private void DrawInnerControls(PaintEventArgs e)
        {
            Controls.OnPaintAll(e);
            e.Graphics.ResetClip();
        }

        private void DrawBorder(PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var borderPath = NhegazDrawingMethods.RectBorderPath(Bounds, BorderRadius, BorderWidth))
            {
                if (borderWidth > 1)
                {
                    using var brush = new SolidBrush(BorderColor);
                    e.Graphics.FillPath(brush, borderPath);
                }

                using var pen = new Pen(BorderColor, 1f);
                e.Graphics.DrawPath(pen, borderPath);
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            DrawBackground(e); DrawInnerControls(e); //Desenha o Background; Desenha os InnerControlsCollection.
            if (HasBorder == true) DrawBorder(e);    //Se tiver Border: Desenha Border.
        }
    }
}
