using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public static class NhegazLocationMethods
    {
        /// <summary>
        /// Se value menor que minimum: vira minimum. Se value > maximum: vira maximum. Se minimum ≤ value ≤ maximum: fica value.
        /// </summary>
        public static int Clamp(int value, int minValue, int maxValue)
        {
            if (maxValue < minValue) return minValue;     // guarda-corpo p/ casos patológicos
            return value < minValue ?                                  
                                minValue : (value > maxValue ? maxValue : value);
        }

    }
}
