using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class InnerControl
    {
        public static void DrawRectangularPath(PaintEventArgs e, Rectangle rect, int cornerRadius, Color color, bool setClip = false)
        {
            using var path = NhegazDrawingMethods.RectangularPath(rect, cornerRadius);
            using (var brush = new SolidBrush(color))
            { e.Graphics.FillPath(brush, path); }

            if (setClip)
            {
                using var region = new Region(path);
                if (!e.Graphics.IsClipEmpty)
                    e.Graphics.IntersectClip(region);
                else
                    e.Graphics.SetClip(region, CombineMode.Replace);
            }
        }

        public void DrawBackground(PaintEventArgs e)
        {
            Color backgroundColor = IsHovering ? HoverBackgroundColor : BackgroundColor;

            switch (BackGroundShape)
            {
                case BackGroundShape.FitRectangle:
                    NhegazDrawingMethods.DrawRectangularPath(e, Bounds, 0, backgroundColor);
                    break;

                case BackGroundShape.SymmetricCircle:
                    NhegazDrawingMethods.DrawSymmetricCirclePath(e, Bounds, backgroundColor);
                    break;

                case BackGroundShape.RoundedRectangle:
                    NhegazDrawingMethods.DrawRectangularPath(e, Bounds, cornerRadius, backgroundColor);
                    break;
            }
        }

        public virtual void OnPaint(PaintEventArgs e)
        {
            if (!Visible) return;
            DrawBackground(e);
        }

    }
}
