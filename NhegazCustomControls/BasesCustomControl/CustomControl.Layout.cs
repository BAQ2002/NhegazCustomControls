using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomControl
    {
        /// <summary>
        /// Metodo responsavel pelo ajuste dos valores de Padding.
        /// </summary>
        protected virtual void AdjustPadding()
        {
            if (PaddingMode == PaddingMode.RelativeToFont)
            {
                Size unit = NhegazSizeMethods.FontUnitSize(Font); //Tamanho "unit" unitario da fonte
                HorizontalPadding = (int)Math.Round(unit.Width * paddingRelativePercent);
                VerticalPadding = (int)Math.Round(unit.Height * paddingRelativePercent);
            }
            // Se for Absolute, não altera — valores já foram definidos diretamente
        }

        protected virtual void AdjustHoverColors()
        {

        }

        /// <summary>
        /// Método responsavel pelo ajuste do tamanho dos InnerControls.
        /// </summary>
        protected virtual void AdjustInnerSizes()
        { }
        protected virtual void AdjustVectorItemsSizes(int index, int itemWidth, int ItemHeight)
        { }
        protected virtual void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int ItemHeight)
        { }

        /// <summary>
        /// Metodo responsavel pelo ajuste das posicoes dos InnerControls.
        /// </summary>
        /// 
        protected virtual void AdjustInnerLocations()
        { }
        protected virtual void AdjustVectorItemsLocations(int index, int x, int y)
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
        protected virtual void AdjustControlSize()
        {
            AdjustPadding();
            AdjustInnerLocations();
            AdjustInnerSizes();
            SetMinimumSize();
            Invalidate();
        }
    }
}
