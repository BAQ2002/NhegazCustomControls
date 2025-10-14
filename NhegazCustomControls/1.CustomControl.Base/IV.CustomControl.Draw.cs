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
        protected void DrawBackground(PaintEventArgs e)
        {

            // Usa ClientRectangle e aplica offset interno, garantindo que o Path fique dentro da área do controle
            Rectangle backgroundRect = new(Point.Empty, Size);

            // Garante que largura e altura sejam válidas
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;
            e.Graphics.SmoothingMode = SmoothingMode.None;
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
            InnerControls.OnPaintAll(e);
            e.Graphics.ResetClip();
        }

        /// <summary>
        /// Metodo que realiza o desenho da borda do CustomControl.
        /// </summary>
        protected virtual void DrawBorder(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor = OnFocus ? OnFocusBorderColor : BorderColor;
            int borderWidth = OnFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;

            using (GraphicsPath borderPath = NhegazDrawingMethods.RectBorderPath(Bounds, BorderRadius, borderWidth))
            {
                if (borderWidth > 1)
                {
                    using (SolidBrush borderBrush = new(borderColor))
                    {
                        e.Graphics.FillPath(borderBrush, borderPath);
                    }
                }
                e.Graphics.DrawPath(new Pen(borderColor, 1f), borderPath);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);                             //Invoca o evento base de Windows.Forms.Control.
            DrawBackground(e); DrawInnerControls(e);     //Desenha o Background; Desenha os InnerControls.
            (this as IHasHeader)?.Header.PaintHeader(e); //Se tiver Header: Desenha Header.
            if(HasBorder == true)DrawBorder(e);          //Se tiver Border: Desenha Border.
        }

    }
}
