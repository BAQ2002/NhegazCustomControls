using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public partial class InnerLabel
    {
        public void DrawText(PaintEventArgs e) 
        {
            Color foreColor = IsHovering ? HoverForeColor : ForeColor;

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(TextLocation.X, TextLocation.Y, Width - TextFeatures.TextLocation.X, Height - TextFeatures.TextLocation.Y),
                foreColor,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis
            );
        }

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); DrawText(e);
        }    
    }
}
