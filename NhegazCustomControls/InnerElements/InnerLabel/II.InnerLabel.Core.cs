using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public partial class InnerLabel : InnerControl, IHasText
    {        

        public TextFeature TextFeatures { get;}

        /// <summary>
        /// Construtor opcional para definir <see cref="SizeBasedOnText"/>.
        /// </summary>
        /// <param name="autoSizeBasedOnText"></param>
        public InnerLabel(bool autoSizeBasedOnText = true) : base()
        {
            TextFeatures = new TextFeature(this, () => Text);
            SizeBasedOnText = autoSizeBasedOnText;
        }
         
    }
}
