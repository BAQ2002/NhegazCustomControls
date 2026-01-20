using System;
using System.Drawing;

namespace NhegazCustomControls
{

    public partial class InnerScrollBar
    {
     
        private void MathThumbSize()
        {
            int relative = Size.Height / parentFullContentHeight; 
        }

        /// <summary>
        /// Recalcula explicitamente o <see cref="thumbBounds"/> com base
        /// no estado atual do scroll (equivalente a chamar <see cref="UpdateThumb"/>).
        /// </summary>
        public void UpdateThumbBounds()
        {
            UpdateThumb();                                                      //Recalcula thumbBounds com base no estado atual.
        }
    }
}
