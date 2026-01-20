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

            using var brush = new SolidBrush(SelectionBackgroundColor);
            e.Graphics.FillRectangle(brush, SelectionRectangle);
        }

        public void DrawText(PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
                return;

            int min = HasSelection ? SelectionMinIndex : -1;
            int max = HasSelection ? SelectionMaxIndex : -1;

            // Definição das três partes
            string left = (HasSelection && min > 0) ? Text.Substring(0, min) : (!HasSelection ? Text : "");
            string right = (HasSelection && max < Text.Length) ? Text.Substring(max, Text.Length - max) : "";

            string cu = (HasSelection && SelectionMinIndex > 0) ? Text.Substring(0, SelectionMinIndex) : Text;
            // Flags originais
            var flags = UseEllipsis ?
                TextFormatFlags.EndEllipsis | TextFormatFlags.WordEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine :
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping;

            
            //Left (antes da seleção)
            if (left.Length > 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    left,
                    Font,
                    TextLocation,
                    ForeColor,
                    flags
                );
            }
            

            //Middle (texto selecionado → SelectionForeColor)
            if (SelectionText.Length > 0)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    SelectionText,
                    Font,
                    SelectionLocation,
                    SelectionForeColor,
                    flags
                );
            }


            //Right (após a seleção)
            if (right.Length > 0)
            {
                Point rightLocation = NhegazSizeMethods.LocationByIndex(TextLocation, Text, SelectionMaxIndex, Font);

                TextRenderer.DrawText(
                    e.Graphics,
                    right,
                    Font,
                    rightLocation,
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
