using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public static partial class NhegazDrawingMethods
    {
        /// <summary>
        /// Retorna uma nova cor que é a proporção entre de n/10 da primeira em relação a segunda.
        /// </summary>
        public static Color InterpolateColor(int n, Color color1, Color color2)
        {
            n = Math.Max(1, Math.Min(10, n));

            float ratio1 = n / 10f;
            float ratio2 = 1f - ratio1;

            int r = (int)(color1.R * ratio1 + color2.R * ratio2);
            int g = (int)(color1.G * ratio1 + color2.G * ratio2);
            int b = (int)(color1.B * ratio1 + color2.B * ratio2);

            return Color.FromArgb(r, g, b);
        }

        public static Color GoogleBlue() 
        {
            Color color = Color.FromArgb(11, 87, 208);
            return color;
        }


    }
}
