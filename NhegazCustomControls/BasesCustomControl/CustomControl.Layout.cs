using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomControl
    {
        protected virtual void AdjustHoverColors()
        {

        }

        /// <summary>
        /// Método responsavel pelo ajuste do tamanho dos InnerControls.
        /// </summary>
        protected virtual void AdjustInnerSizes()
        { }
        protected virtual void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int ItemHeight)
        { }

        /// <summary>
        /// Metodo responsavel pelo ajuste das posicoes dos InnerControls.
        /// </summary>
        /// 
        protected virtual void AdjustInnerLocations()
        { }

        protected virtual void AdjustMatrixItemsLocations(int row, int col, int x, int y)
        { }

        /// <summary>
        /// Metodo responsavel por definir o MinimumSize a partir dos InnerControls.
        /// </summary>
        protected virtual void SetMinimumSize()
        {

        }

        /// <summary>
        /// Metodo que invoca todos ajustes de posicoes e tamanhos.
        /// </summary>
        public virtual void AdjustControlSize()
        {
            AdjustInnerLocations();
            AdjustInnerSizes();
            SetMinimumSize();
            Invalidate();
        }
    }
}
