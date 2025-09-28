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
}
