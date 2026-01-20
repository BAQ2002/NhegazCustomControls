using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhegaz
{
    public class MathMethods
    {
        /// <summary>
        /// Garante que um valor inteiro não seja menor que um mínimo nem maior que um máximo.
        /// </summary>
        /// <param name="value">Valor original a ser validado.</param>
        /// <param name="minValue">Limite inferior do intervalo.</param>
        /// <param name="maxValue">Limite superior do intervalo.</param>
        /// <returns>
        /// <paramref name="value"/> se ele estiver dentro do intervalo;
        /// <paramref name="minValue"/> se for menor que o mínimo;
        /// <paramref name="maxValue"/> se for maior que o máximo.
        /// </returns>
        public static int Clamp(int value, int minValue, int maxValue)
        {
            return Math.Max(minValue, Math.Min(maxValue, value));
        }

        /// <summary> 
        /// Arrendonda o valor para o 
        /// Múltiplo de 10 mais próximo. 
        /// MOT = Multiple Of Ten.
        /// </summary>
        public static int RoundToMOT(int value)
        {
            return (int)(MathF.Round(value / 10f, MidpointRounding.AwayFromZero) * 10f);
        }


    }
}
