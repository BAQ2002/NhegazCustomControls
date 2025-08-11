using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls.PL.Templates
{
    public class CustomButton : UserControl
    {
        private int borderRadius = 5;
        private Color borderColor = Color.Red;
        private Color borderColorFocus = Color.Blue;
        private int borderWidth = 1;
        private Color backgroundColor = Color.White;

        private Label innerLabel = new Label();

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
            set { backgroundColor = value; innerLabel.BackColor = value; Invalidate(); }
        }

        public override string Text
        {
            get { return innerLabel.Text; }
            set { innerLabel.Text = value; }
        }

        public override Font Font
        {
            get { return innerLabel.Font; }
            set { innerLabel.Font = value; AdjustTextBoxSize(); }
        }

        public override Color ForeColor
        {
            get { return innerLabel.ForeColor; }
            set { innerLabel.ForeColor = value; }
        }

        public CustomButton()
        {

            this.Padding = new Padding(5);
            this.MinimumSize = new Size(50, 30);
            this.Size = new Size(149, 23);
            this.DoubleBuffered = true;

            // Configuração do TextBox interno
            innerLabel.BorderStyle = BorderStyle.None;
            innerLabel.Font = new Font("Arial", 9);
            innerLabel.ForeColor = Color.Black;
            innerLabel.BackColor = BackgroundColor;
            Controls.Add(innerLabel);

            AdjustTextBoxSize();
        }

        /// Ajusta a posição e o tamanho do TextBox interno para alinhar corretamente dentro da borda arredondada.
        private void AdjustTextBoxSize()
        {
            innerLabel.Location = new Point(borderWidth + 3, (this.Height - innerLabel.Font.Height) / 2);
            innerLabel.Width = this.Width - 25;// - (1 * borderWidth) - 10;
            innerLabel.Height = this.Height - 5;// - (1 * borderWidth);
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
