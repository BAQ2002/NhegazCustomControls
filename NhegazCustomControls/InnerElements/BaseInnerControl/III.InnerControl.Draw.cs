using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class InnerControl
    {
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
