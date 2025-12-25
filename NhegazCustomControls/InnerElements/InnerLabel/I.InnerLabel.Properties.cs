using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerLabel
    {
        private string text = "";
        

        /// <summary>Define se o tamanho deve ser baseado no texto.</summary>
        public bool SizeBasedOnText { get; set; } = true;

        public string Text
        {
            get => text;
            set { text = value; UpdateLayout(); }
        }

        public override Font Font
        {
            get => base.Font;
            set { base.Font = value; UpdateLayout(); }
        }

        public override int Height
        {
            get => base.Height;
            set { SizeBasedOnText = false; base.Height = value; }
        }

        public override int Width
        {
            get => base.Width;
            set { SizeBasedOnText = false; base.Width = value; }
        }

        public Point TextLocation
        {
            get
            {
                int x = Location.X + TextFeatures.TextLocation.X;
                int y = Location.Y + TextFeatures.TextLocation.Y;
                return new(x, y);
            }
        }    
    }
}
