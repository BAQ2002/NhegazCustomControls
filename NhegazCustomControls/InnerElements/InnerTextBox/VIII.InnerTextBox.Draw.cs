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
        private void DrawSelection(PaintEventArgs e)
        {
            if (!HasSelection) return;

            int start = Math.Min(selectionStartIndex, selectionEndIndex);
            int end = Math.Max(selectionStartIndex, selectionEndIndex);

            int charWidth = NhegazSizeMethods.FontUnitSize(Font).Width;

            int x1 = TextLocation.X + start * charWidth;
            int x2 = TextLocation.X + end * charWidth;

            Rectangle rect = new Rectangle(
                x1,
                TextLocation.Y,
                x2 - x1,
                TextSize.Height
            );

            using var brush = new SolidBrush(SelectionBackgroundColor);
            e.Graphics.FillRectangle(brush, rect);
        }

        public void DrawText(PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            int charWidth = NhegazSizeMethods.FontUnitSize(Font).Width;

            int start = HasSelection ? Math.Min(selectionStartIndex, selectionEndIndex) : -1;
            int end = HasSelection ? Math.Max(selectionStartIndex, selectionEndIndex) : -1;

            // Definição das três partes
            string left = (HasSelection && start > 0) ? Text.Substring(0, start) : (!HasSelection ? Text : "");
            string middle = (HasSelection) ? Text.Substring(start, end - start) : "";
            string right = (HasSelection && end < Text.Length) ? Text.Substring(end, Text.Length - end) : "";

            // Flags originais
            var flags = UseEllipsis ?
                TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine :
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping;

            int x = TextLocation.X;
            int y = TextLocation.Y;

            //Left (antes da seleção)
            if (left.Length > 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    left,
                    Font,
                    new Point(x, y),
                    ForeColor,
                    flags
                );
                x += left.Length * charWidth;
            }

            //Middle (texto selecionado → SelectionForeColor)
            if (middle.Length > 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    middle,
                    Font,
                    new Point(x, y),
                    SelectionForeColor,
                    flags
                );
                x += middle.Length * charWidth;
            }

            //Right (após a seleção)
            if (right.Length > 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    right,
                    Font,
                    new Point(x, y),
                    ForeColor,
                    flags
                );
            }
        }


        public void DrawCaret(PaintEventArgs e) 
        {
            Color caretColor = IsHovering ? CaretHoverColor : CaretColor;
   
            NhegazDrawingMethods.DrawCaret(e, CaretRectangle, caretColor);           
        }

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawSelection(e); DrawText(e);
            if (CaretVisible) { DrawCaret(e); }
        }
    }
}
