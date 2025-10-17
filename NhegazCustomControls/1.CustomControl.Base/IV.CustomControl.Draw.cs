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
        /// Usa RectangularPath com offset calculado baseado em ClientRectangle.
        /// </summary>
        protected void DrawBackground(PaintEventArgs e)
        {          
            
            int cornerRadius = HasBorder ? BorderRadius - 1 : BorderRadius;

            //Posições(X,Y) do Background. //Tamanhos(Width, Height) do Background. //
            int locX = BackgroundOffset;        int width = Width - (2 * BackgroundOffset);
            int locY = BackgroundOffset;        int height = Height - (2 * BackgroundOffset);
            //Serão diferentes das Propriedades Originais apenas se BorderWidth >=1.//
          
            Rectangle backgroundRect = new(locX, locY, width, height);
            if (backgroundRect.Width <= 0 || backgroundRect.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.None;

            NhegazDrawingMethods.DrawRectangularPath(e, backgroundRect, cornerRadius, BackgroundColor, true);

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
            e.Graphics.SmoothingMode = SmoothingMode.None;

            Rectangle borderRect = new(Point.Empty, Size);
            Color borderColor = OnFocus ? OnFocusBorderColor                    : BorderColor;
            int borderWidth   = OnFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;

            NhegazDrawingMethods.DrawBorderPath(e, borderRect, BorderRadius, borderWidth, borderColor);
  
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
