using System;
using System.Drawing;

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
        public void DrawThumb(PaintEventArgs e)
        {
           // if (!HasSelection) return;

            using var brush = new SolidBrush(ThumbColor);
            e.Graphics.FillRectangle(brush, thumbBounds);
        }

        /// <summary>
        /// Desenha a trilha e o thumb do scroll.
        /// </summary>
        /// <param name="e">Argumentos de desenho com o <see cref="Graphics"/> destino.</param>
        public override void OnPaint(PaintEventArgs e)
        {
            if (!Visible) return;                                              //Se não está visível -> não desenha.

            DrawBackground(e);                                                 //Desenha a trilha/fundo.

            if (thumbBounds.Width > 0 && thumbBounds.Height > 0)               //Se thumb tem dimensão válida -> desenha.
            { DrawThumb(e); } //Desenha o thumb.
        }

    }
}
