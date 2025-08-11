using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls.PL.CustomControls
{
    public class RoundedTextBox : CustomControl
    {
        private int borderRadius = 5;
        private Color borderColor = Color.Red;
        private Color borderColorFocus = Color.Blue;
        private int borderWidth = 1;
        private Color backgroundColor = Color.White;

        private TextBox innerTextBox = new TextBox();

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; Invalidate(); }
        }
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; innerTextBox.BackColor = value; Invalidate(); }
        }

        public override string Text
        {
            get { return innerTextBox.Text; }
            set { innerTextBox.Text = value; }
        }

        public override Font Font
        {
            get { return innerTextBox.Font; }
            set { innerTextBox.Font = value; AdjustTextBoxSize(); }
        }

        public override Color ForeColor
        {
            get { return innerTextBox.ForeColor; }
            set { innerTextBox.ForeColor = value; }
        }

        public RoundedTextBox()
        {
            
            this.Padding = new Padding(5);
            this.MinimumSize = new Size(50, 30);
            this.Size = new Size(149, 23);
            this.DoubleBuffered = true;

            // Configuração do TextBox interno
            innerTextBox.BorderStyle = BorderStyle.None;
            innerTextBox.Multiline = false;
            innerTextBox.Font = new Font("Arial", 9);
            innerTextBox.ForeColor = Color.Black;
            innerTextBox.BackColor = BackgroundColor;
            innerTextBox.TextAlign = HorizontalAlignment.Left;

            Controls.Add(innerTextBox);

            AdjustTextBoxSize();
        }

        /// Ajusta a posição e o tamanho do TextBox interno para alinhar corretamente dentro da borda arredondada.
        private void AdjustTextBoxSize()
        {
            innerTextBox.Location = new Point(borderWidth + 3, (this.Height - innerTextBox.Font.Height) / 2);
            innerTextBox.Width = this.Width-25;// - (1 * borderWidth) - 10;
            innerTextBox.Height = this.Height-5;// - (1 * borderWidth);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      
            using (GraphicsPath path = new GraphicsPath())
            {
                Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                int r = borderRadius * 2;
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();

                // Agora o fundo azul NÃO ultrapassa a borda
                using (Region region = new Region(path))
                {
                    e.Graphics.Clip = region; // Garante que o fundo fique dentro da borda
                    using (SolidBrush brush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(brush, rect); // Desenha o fundo apenas dentro do retângulo
                    }
                    e.Graphics.ResetClip(); // Remove a limitação de recorte após o preenchimento
                }

                using (Pen pen = new Pen(borderColor, borderWidth))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustTextBoxSize();
            Invalidate();
        }
    }
}
