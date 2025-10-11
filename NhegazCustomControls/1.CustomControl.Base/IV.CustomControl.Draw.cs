using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class CustomControl
    {
        /// <summary>
        /// Realiza o desenho do fundo do controle, respeitando a área interna delimitada pela borda.
        /// Usa RectBackgroundPath com offset calculado baseado em ClientRectangle.
        /// </summary>
        protected virtual void DrawControlBackground(PaintEventArgs e)
        {
            // Calcula o offset interno com base na espessura da borda
            int offset = BorderWidth > 1 ? BorderWidth - 1 : 0;

            // Usa ClientRectangle e aplica offset interno, garantindo que o Path fique dentro da área do controle
            Rectangle backgroundRect = new Rectangle(
                offset,
                offset,
                Width - (2 * offset),
                Height - (2 * offset)
            );

            // Garante que largura e altura sejam válidas
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;

            // Cria o path com raio original
            using (GraphicsPath backgroundPath = NhegazDrawingMethods.RectBackgroundPath(backgroundRect, BorderRadius))
            {
                // Preenche o fundo com a cor do controle
                using (SolidBrush brush = new SolidBrush(BackgroundColor))
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }

                // Define a área de recorte (clip) para limitar os próximos desenhos, se necessário
                e.Graphics.SetClip(backgroundPath);
            }
        }


        /// <summary>
        /// Metodo que realiza o desenho dos InnerControls.
        /// </summary>
        protected virtual void DrawInnerControls(PaintEventArgs e)
        {
            InnerControls.OnPaintAll(this, e);
            e.Graphics.ResetClip();
        }

        /// <summary>
        /// Metodo que realiza o desenho da borda do CustomControl.
        /// </summary>
        protected virtual void DrawBorder(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color BorderColor = OnFocus ? OnFocusBorderColor : this.BorderColor;
            int borderWidth = OnFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;

            using (GraphicsPath borderPath = NhegazDrawingMethods.RectBorderPath(new Rectangle(Location, Size), BorderRadius, borderWidth))
            {
                if (borderWidth > 1)
                {
                    using (SolidBrush borderBrush = new SolidBrush(BorderColor))
                    {
                        e.Graphics.FillPath(borderBrush, borderPath);
                    }
                }
                e.Graphics.DrawPath(new Pen(BorderColor, 1f), borderPath);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.None;
            DrawControlBackground(e);
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.PaintHeader(e);
            DrawInnerControls(e);
            DrawBorder(e);
        }

    }
}
