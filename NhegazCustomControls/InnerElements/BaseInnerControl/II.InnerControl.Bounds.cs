using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerControl
    {
        /// <summary>Retorna a COORDENADA Y centralizada em relação à altura do InnerControl </summary>
        public int RelativeCenterY(int height)
        {
            int centerY = Top + (Height - height) / 2;         // Centro 
            return centerY;
        }

        /// <summary>Retorna a COORDENADA X centralizada em relação à largura do InnerControl </summary>
        public int RelativeCenterX(int width)
        {
            int centerX = Left + (Width - width) / 2;                 // Centro absoluto horizontal do controle
            return centerX;       // CLAMP dos valores
        }
       
        /// <summary>
        /// Retorna a COORDENADA X encostado na EXTREMIDADE ESQUERDA em relação à 
        /// largura do InnerControl(respeitando padding/borda esquerda).
        /// </summary>
        public int RelativeLeftX() { return Left; }

        /// <summary>
        /// X para posicionar o InnerControl encostado na EXTREMIDADE DIREITA
        /// (respeitando padding/borda direita).
        /// </summary>
        public int RelativeRightX(int width)
        {
            // canto esquerdo = largura total - espessura direita - largura do inner
            return Right - width;
        }

        /// <summary>Y para posicionar o InnerControl encostado na EXTREMIDADE SUPERIOR (respeitando padding/borda superior).</summary>
        public int RelativeTopY() { return Top; }


        /// <summary>Y para posicionar o InnerControl encostado na EXTREMIDADE INFERIOR (respeitando padding/borda inferior).</summary>
        public int RelativeBottomY(int height)
        {
            return Bottom - height;
        }

    }
}
