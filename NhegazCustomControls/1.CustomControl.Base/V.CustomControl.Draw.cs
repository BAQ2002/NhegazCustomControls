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
            if (BackgroundRectangle.Width <= 0 || BackgroundRectangle.Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.None;

            NhegazDrawingMethods.DrawRectangularPath(e, BackgroundRectangle, BackgroundCornerRaidus, BackgroundColor, true);
        }


        /// <summary>
        /// Método que realiza o desenho dos InnerControlsCollection.
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

            bool  hasFocus    = ContainsFocus; // cobre o próprio controle e filhos reais
            int   borderWidth = hasFocus ? BorderWidth + OnFocusBorderExtraWidth : BorderWidth;
            Color borderColor = hasFocus ? OnFocusBorderColor                    : BorderColor;
            
            NhegazDrawingMethods.DrawBorderPath(e, borderRect, BorderRadius, borderWidth, borderColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);                             //Invoca o evento base de Windows.Forms.Control.
            DrawBackground(e); DrawInnerControls(e);     //Desenha o Background; Desenha os InnerControlsCollection.
            (this as IHasHeader)?.Header.OnPaint(e);     //Se tiver Header: Desenha Header.
            if(HasBorder == true)DrawBorder(e);          //Se tiver Border: Desenha Border.
        }

    }
}
