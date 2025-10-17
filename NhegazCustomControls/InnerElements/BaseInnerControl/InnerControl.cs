using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;

namespace NhegazCustomControls
{
    public abstract class InnerControl
    {
        private Rectangle bounds = new(0, 0, 0, 0);
        private BackGroundShape backGroundShape = BackGroundShape.FitRectangle;
        private int cornerRadius = 0;
        public bool Visible { get; set; } = true;
        public bool AbleToHover { get; set; } = true;
        public virtual Font Font { get; set; } = SystemFonts.DefaultFont;
        public Color ForeColor { get; set; } = SystemColors.ControlText;
        public Color HoverForeColor { get; set; } = SystemColors.Window;
        public Color BackgroundColor { get; set; } = SystemColors.Window;
        public Color HoverBackgroundColor { get; set; } = SystemColors.Highlight;
        public InnerControlPadding Padding { get; }
        public bool HitBox(Point p) => Bounds.Contains(p);
        public Rectangle Bounds => bounds;
        public int CornerRadius
        {
            get => cornerRadius;
            set { cornerRadius = value; }
        }
        public Size Size
        {
            get => bounds.Size;
            set { bounds.Size = value;  }
        }

        public Point Location
        {
            get => bounds.Location;
            set { bounds.Location = value; }
        }

        public BackGroundShape BackGroundShape
        {
            get => backGroundShape;
            set { backGroundShape = value; AdjustControlSize(); }
        }
        public virtual int Width
        {
            get => Size.Width;
            set { Size = new Size(value, Size.Height); AdjustControlSize(); }
        }
        public virtual int Height
        {
            get => Size.Height;
            set { Size = new Size(Size.Width, value); AdjustControlSize(); }
        }
        public int X => Location.X; public int Y => Location.Y;
        public int Top => Location.Y; public int Right => Location.X + Size.Width;
        public int Left => Location.X; public int Bottom => Location.Y + Size.Height;

        public InnerControl()
        {
            Padding = new(this);
        }
        private bool isHovering = false; public bool IsHovering => AbleToHover? isHovering : false;

        public event EventHandler? Click;
        public event EventHandler? DoubleClick;

        public event EventHandler? GotFocus;
        public event EventHandler? LostFocus;

        public event EventHandler? MouseEnter;
        public event EventHandler? MouseLeave;

        public void RaiseClick(object sender)
        {
            Click?.Invoke(sender, EventArgs.Empty);
        }
        public void RaiseDoubleClick(object sender)
        {
            DoubleClick?.Invoke(sender, EventArgs.Empty);
        }
        public void RaiseGotFocus(object sender)
        {
            GotFocus?.Invoke(sender, EventArgs.Empty);
        }
        public void RaiseLostFocus(object sender)
        {
            LostFocus?.Invoke(sender, EventArgs.Empty);
        }
        public virtual void OnMouseEnter()
        {
            if (!AbleToHover || isHovering) return;

            isHovering = true;
            MouseEnter?.Invoke(this, EventArgs.Empty);
            
        }
        public virtual void OnMouseLeave()
        {
            if (!AbleToHover || !isHovering) return;

            isHovering = false;
            MouseLeave?.Invoke(this, EventArgs.Empty);
            
        }
        protected virtual void AdjustControlSize()
        {
            if (BackGroundShape == BackGroundShape.SymmetricCircle)
            {
                SymmetricalCircleAdjust();
            }
        }
 
        /// <summary>
        /// Metodo responsavel por realizar ajustes para BackgroundShape.SymmetricalCircle
        /// </summary>
        protected virtual void SymmetricalCircleAdjust()
        {
            if (Size.Width == Size.Height)
                return;

            int reference = Math.Max(Width, Height); Size = new Size(reference, reference);
        }

        public virtual void Update()
        {

        }
        public virtual void SetLocation(int x, int y)
        {
            Location = new Point(x, y);
        }
        public virtual void SetLocation(Point location)
        {
            Location = new Point(location.X, location.Y);
        }

        public virtual void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public virtual void SetSize(Size size)
        {
            Width = size.Width; 
            Height = size.Height;
        }

        public void DrawBackground(PaintEventArgs e)
        {
            Color backgroundColor = IsHovering ? HoverBackgroundColor : BackgroundColor;

            switch (BackGroundShape)
            {
                case BackGroundShape.FitRectangle:
                    NhegazDrawingMethods.DrawRectangularPath(e, Bounds, 0, backgroundColor);
                    break;

                case BackGroundShape.SymmetricCircle:
                    NhegazDrawingMethods.DrawSymmetricCirclePath(e, Bounds, backgroundColor);
                    break;

                case BackGroundShape.RoundedRectangle:
                    NhegazDrawingMethods.DrawRectangularPath(e, Bounds, cornerRadius, backgroundColor);
                    break;
            }
        }
        public virtual void OnPaint(PaintEventArgs e)
        {
            if (!Visible) return;
            DrawBackground(e);
        }
    }
}
