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
            //Posições(X,Y) do Background. //Tamanhos(Width, Height) do Background. //
            int locX = BorderWidth;        int width = Width - (2 * BorderWidth);
            int locY = BorderWidth;        int height = Height - (2 * BorderWidth);
            //Serão diferentes das Propriedades Originais apenas se BorderWidth >=1.//

            Rectangle backgroundRect = new(locX, locY, width, height);
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath backgroundPath = NhegazDrawingMethods.RectBackgroundPath(backgroundRect, BorderRadius))
            {
                // Preenche o fundo com a cor do controle
                using (SolidBrush brush = new(BackgroundColor))
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }
                e.Graphics.DrawPath(new Pen(BackgroundColor, 1f), backgroundPath);

                // Define a área de recorte (clip) para limitar os próximos desenhos, se necessário
                e.Graphics.SetClip(backgroundPath);
            }
        }


        /// <summary>
        /// Método que realiza o desenho dos InnerControls.
        /// </summary>
        protected virtual void DrawInnerControls(PaintEventArgs e)
        {
            InnerControls.OnPaintAll(e);       
        }

        /// <summary>
        /// Método que realiza o desenho da borda do CustomControl.
        /// </summary>
        protected virtual void DrawBorder(PaintEventArgs e)
        {
            e.Graphics.ResetClip();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle borderRect = new(Point.Empty, Size);
            Color borderColor = OnFocus ? OnFocusBorderColor : BorderColor;
            int borderWidth = OnFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;
            
            using (GraphicsPath borderPath = NhegazDrawingMethods.RectBorderPath(borderRect, BorderRadius, borderWidth))
            {
                if (borderWidth > 1)
                {
                    using (SolidBrush borderBrush = new(borderColor))
                    {e.Graphics.FillPath(borderBrush, borderPath);}
                }
                e.Graphics.DrawPath(new Pen(borderColor, 1f), borderPath);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);                             //Invoca o evento base de Windows.Forms.Control.
            DrawBackground(e); DrawInnerControls(e);     //Desenha o Background; Desenha os InnerControls.
            (this as IHasHeader)?.Header.OnPaint(e);     //Se tiver Header: Desenha Header.
            if(HasBorder == true)DrawBorder(e);          //Se tiver Border: Desenha Border.
        }

    }
}
