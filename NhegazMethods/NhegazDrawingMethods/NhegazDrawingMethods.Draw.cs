using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;


namespace NhegazCustomControls
{
    public static partial class NhegazDrawingMethods
    {
        /// <summary>
        /// Instancia e executa o Paint de um GraphicsPath; 
        /// Delimita o ClipArea do "e.Graphics" para o GraphicsPath gerado.
        /// <para>e : define qual PaintEventArgs ira realizar o DrawBackground.</para>
        /// <para>rect : Rectangle que fornece o Size e Location para o GraphicsPath.</para>
        /// <para>cornerRadius : int que define o arredondamento das quinas para o GraphicsPath.</para>
        /// <para>color : Color utilizada para o Paint do GraphicsPath.</para> 
        /// <para>setClip : Determina se deve delimitar o ClipArea do "e.Graphics".</para> 
        /// </summary>
        /// <param name="e">define a PaintEventArgs ira realizar o DrawBackground.</param>
        /// <param name="rect">Rectangle que fornece o Size e Location para o GraphicsPath.</param>
        /// <param name="cornerRadius">int que define o arredondamento das quinas para o GraphicsPath.</param>
        /// <param name="color">Color utilizada para o Paint do GraphicsPath.</param>     
        public static void DrawRectangularPath(PaintEventArgs e, Rectangle rect , int cornerRadius, Color color, bool setClip = false)
        {
            using (GraphicsPath backgroundPath = RectangularPath(rect, cornerRadius))
            {
                // Preenche o fundo com a cor do controle
                using (SolidBrush brush = new(color))
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }
                e.Graphics.DrawPath(new Pen(color, 1f), backgroundPath);

                // Define a área de recorte (clip) para limitar os próximos desenhos, se necessário
                if (setClip == true) 
                {
                    if (e.Graphics.IsClipEmpty == false) 
                        e.Graphics.IntersectClip(new Region(backgroundPath));
                    else 
                        e.Graphics.SetClip(backgroundPath);
                }                                
            }
        }

        public static void DrawSymmetricCirclePath(PaintEventArgs e, Rectangle rect, Color color, bool setClip = false)
        {
            using (GraphicsPath backgroundPath = SymmetricCirclePath(rect))
            {
                // Preenche o fundo com a cor do controle
                using (SolidBrush brush = new(color))
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }
                e.Graphics.DrawPath(new Pen(color, 1f), backgroundPath);

                // Define a área de recorte (clip) para limitar os próximos desenhos, se necessário
                if (setClip == true) e.Graphics.SetClip(backgroundPath);
            }
        }

        public static void DrawBorderPath(PaintEventArgs e, Rectangle rect, int borderRadius, int borderWidth, Color borderColor)
        {
            using (GraphicsPath borderPath = RectBorderPath(rect, borderRadius, borderWidth))
            {
                if (borderWidth > 1)
                {
                    using (SolidBrush borderBrush = new(borderColor))
                    { e.Graphics.FillPath(borderBrush, borderPath); }
                }
                e.Graphics.DrawPath(new Pen(borderColor, 1f), borderPath);
            }
        }


    }
}