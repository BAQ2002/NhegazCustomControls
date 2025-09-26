using NhegazCustomControls.NhegazCustomControls.InnerElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NhegazCustomControls
{
    public class InnerControls
    {
        private List<InnerControl> elements = new();
        private CustomControl? Parent;

        public List<InnerControl> GetAll
        {
            get => elements;
        }
        public void Add(InnerControl innerControl)
        {
            elements.Add(innerControl);
        }

        public void Remove(InnerControl innerControl)
        {
            elements.Remove(innerControl);
        }

        public void Clear()
        {
            elements.Clear();
        }
    
        public InnerControls(CustomControl parent) { Parent = parent; }
        public void OnPaintAll(CustomControl parent, PaintEventArgs e)
        {
            foreach (var element in elements)
            {
                if (element.Visible)
                    element.OnPaint(parent, e);
            }
        }
        public bool HandleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseClick(parent);
                    return true;
                }
            }
            return false;
        }
        public bool HandleDoubleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseDoubleClick(parent);
                    return true;
                }
            }
            return false;  
        }
        public void HandleMouseMove(CustomControl parent, Point mouseLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible)
                {
                    bool contains = element.HitBox(mouseLocation);

                    if (contains && !element.IsHovering)
                    {
                        element.OnMouseEnter();
                        parent.Invalidate();  // força repaint do controle pai para refletir a mudança
                    }
                    else if (!contains && element.IsHovering)
                    {
                        element.OnMouseLeave();
                        parent.Invalidate();
                    }
                }
            }
        }
        public bool HandleGotFocus(CustomControl parent, Point focusLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(focusLocation))
                {
                    element.RaiseGotFocus(parent);
                    return true;
                }
            }
            return false;
        }

        public bool HandleLostFocus(CustomControl parent, Point focusLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(focusLocation))
                {
                    element.RaiseLostFocus(parent);
                    return true;
                }
            }
            return false;
        }
    }


    public abstract class InnerControl
    {
        private Rectangle bounds = new(0, 0, 0, 0);
        private BackGroundShape backGroundShape = BackGroundShape.FitRectangle;
        public bool Visible { get; set; } = true;
        public virtual Font Font { get; set; } = SystemFonts.DefaultFont;
        public Color ForeColor { get; set; } = SystemColors.ControlText;
        //public Color HoverForeColor { get; set; } = SystemColors.ControlText;
        public Color BackgroundColor { get; set; } = SystemColors.Control;
        //public Color hoverBackgroundColor { get; set; } = SystemColors.Control;
        public InnerControlPadding Padding { get; }
        public bool HitBox(Point p) => Bounds.Contains(p);
        public Rectangle Bounds => bounds;
        public Size Size
        {
            get => bounds.Size;
            set { bounds.Size = value;}
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
        public int X => Location.X;    public int Y => Location.Y;
        public int Top => Location.Y;  public int Right => Location.X + Size.Width;
        public int Left => Location.X; public int Bottom => Location.Y + Size.Height;

        public InnerControl()
        {
            Padding = new(this);
        }      
        private bool isHovering = false; public bool IsHovering => isHovering;

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
            if (!isHovering)
            {
                isHovering = true;
                MouseEnter?.Invoke(this, EventArgs.Empty);
            }
        }
        public virtual void OnMouseLeave()
        {
            if (isHovering)
            {
                isHovering = false;
                MouseLeave?.Invoke(this, EventArgs.Empty);
            }
        }
        protected virtual void AdjustControlSize()
        {
            if (BackGroundShape == BackGroundShape.SymmetricCircle) 
            {
                SymmetricalCircleAdjust();
            }
        }

        //public virtual void SetColor(Color backgroundColor, Color foreColor) 
        //{
         //   BackgroundColor = backgroundColor;  
        //    ForeColor = foreColor;
        //}

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
        public void SetHoverColors(Color foreColorOnHover, Color backColorOnHover)
        {
            Color originalFore = ForeColor;
            Color originalBack = BackgroundColor;

            MouseEnter += (s, e) =>
            {
                ForeColor = foreColorOnHover;
                BackgroundColor = backColorOnHover;
            };

            MouseLeave += (s, e) =>
            {
                ForeColor = originalFore;
                BackgroundColor = originalBack;
            };
        }

        public virtual void OnPaint(CustomControl parent, PaintEventArgs e)
        {
            if (!Visible) return;
            using (GraphicsPath backgroundPath = NhegazDrawingMethods.InnerControlBackgroundPath(this)) //Define o GraphicsPath da area interna do InnerControl
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; 

                using (SolidBrush brush = new SolidBrush(BackgroundColor)) //Preenche a area com o BackgroundColor
                {
                    e.Graphics.FillPath(brush, backgroundPath);
                }          
            }
        }
    }
}
