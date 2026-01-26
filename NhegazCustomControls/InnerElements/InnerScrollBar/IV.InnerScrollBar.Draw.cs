using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{

    /// <summary>
    /// InnerControl responsável por representar uma barra de rolagem customizada.
    /// <para/>
    /// Controla um valor dentro de um intervalo (Minimum/Maximum) considerando
    /// o tamanho da área visível (<see cref="ViewportSize"/>), desenhando um
    /// "thumb" proporcional e emitindo <see cref="ValueChanged"/> a cada alteração.
    /// </summary>
    public partial class InnerScrollBar
    {
        public void DrawIcons(PaintEventArgs e)
        {
            Color iconColor = IsHovering ? HoverForeColor : ForeColor;

            using var upIconPath = NhegazDrawingMethods.UpArrowGPath(IconSize, UpIconLocation.X, UpIconLocation.Y);
            using var downIconPath = NhegazDrawingMethods.DownArrowGPath(IconSize, DownIconLocation.X, DownIconLocation.Y);

            using (SolidBrush brush = new SolidBrush(iconColor))
            {
                e.Graphics.FillPath(brush, upIconPath);
                e.Graphics.FillPath(brush, downIconPath);

                using (Pen pen = new Pen(iconColor, 1f))
                {
                    e.Graphics.DrawPath(pen, upIconPath);
                    e.Graphics.DrawPath(pen, downIconPath);
                }
            }             
               
        }

        public void DrawThumb(PaintEventArgs e)
        {

            NhegazDrawingMethods.DrawRectangularPath(e, ThumbRectangle, 1, ThumbColor, true);
            
        }

        /// <summary>
        /// Desenha a trilha e o thumb do scroll.
        /// </summary>
        /// <param name="e">Argumentos de desenho com o <see cref="Graphics"/> destino.</param>
        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawIcons(e);
            if (thumbBounds.Width > 0 && thumbBounds.Height > 0)               //Se thumb tem dimensão válida -> desenha.
            { DrawThumb(e); } //Desenha o thumb.
        }

    }
}
