using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls.NhegazCustomControls.InnerElements
{
    public class InnerHeaderContainer : InnerControl
    {
        private List<InnerControl> HeaderElements = new();

        public void Add(InnerControl innerControl)
        {
            HeaderElements.Add(innerControl);
        }

        public void Remove(InnerControl innerControl)
        {
            HeaderElements.Remove(innerControl);
        }

        public void Clear()
        {
            HeaderElements.Clear();
        }

        public override void OnPaint(CustomControl parent, PaintEventArgs e)
        {
            base.OnPaint(parent, e);

            for (int i = 0; i < HeaderElements.Count; i++) 
            {
                var element = HeaderElements[i];
                element.OnPaint(parent, e);

            }

        }
    }
}
