using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        public void DrawText(PaintEventArgs e)
        {
            Color foreColor = IsHovering ? HoverForeColor : ForeColor;

            var flags = UseEllipsis? 
                TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine :
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping;

            TextRenderer.DrawText(
                e.Graphics,
                string.IsNullOrEmpty(Text) ? " " : Text,
                Font,
                new Rectangle(TextLocation.X, TextLocation.Y, Width - textRelativeLocation.X, Height - textRelativeLocation.Y),
                foreColor,
                flags
            );
        }

        public void DrawCaret(PaintEventArgs e) 
        {
            Color foreColor = IsHovering ? HoverForeColor : ForeColor;
   
            NhegazDrawingMethods.DrawCaret(e, CaretRectangle, foreColor);           
        }

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawText(e); if (Focused && caretVisible) { DrawCaret(e); }
        }
    }
}
