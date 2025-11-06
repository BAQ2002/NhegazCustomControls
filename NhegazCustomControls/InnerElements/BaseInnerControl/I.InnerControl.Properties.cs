using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class InnerControl
    {
        private Rectangle bounds = new(0, 0, 0, 0);
        private BackGroundShape backGroundShape = BackGroundShape.FitRectangle;
        private int cornerRadius = 0; 
        private bool isHovering = false; public bool IsHovering => AbleToHover ? isHovering : false;
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
            set { bounds.Size = value; }
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
    }
}
