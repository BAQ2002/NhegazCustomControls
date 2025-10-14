using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomControl
    {
        protected virtual void AdjustHoverColors(){}

        /// <summary>
        /// Método responsavel pelo ajuste do tamanho dos InnerControls.
        /// </summary>
        protected abstract void SetInnerSizes();

        /// <summary>
        /// Metodo responsavel pelo ajuste das posicoes dos InnerControls.
        /// </summary>
        protected abstract void SetInnerLocations();
  
        /// <summary>
        /// Retorna os valores de Largura e Altura que os InnerControls ocupam
        /// com base em uma política específica para cada CustomControl.
        /// </summary>
        /// <returns>Size(contentWidth, contentHeight)</returns>
        public abstract Size GetContentSize();

        /// <summary>
        /// Retorna os valores de Largura e Altura que as propriedades de Padding
        /// ocupam com base em uma política específica para cada CustomControl.
        /// </summary>
        /// <returns>Size(paddingWidth, paddingHeight)</returns>
        public abstract Size GetPaddingSize();

        /// <summary>
        /// Retorna os valores de Largura e Altura com base
        /// nas políticas específicas de GetContentSize e GetPaddingSize
        /// de cada CustomControl.
        /// </summary>
        /// <returns>Size(controlWidth, controlHeight)</returns>
        public Size GetControlSize()
        {
            Size controlSize = GetContentSize() + GetPaddingSize();
            return controlSize;
        }

        /// <summary>
        /// Metodo responsavel por definir o MinimumSize a partir dos InnerControls.
        /// </summary>
        protected void SetMinimumSize()
        {
            int minimumWidth = GetControlSize().Width;
            int minimumHeight = GetControlSize().Height;

            MinimumSize = new Size(minimumWidth, minimumHeight);
        }

        /// <summary>
        /// Método que invoca todos ajustes de posições e tamanhos.
        /// Cada CustomControl pode passar uma variável "bool" específica para confirmação
        /// prévia se os ajustes devem ser realizados.
        /// </summary>
        /// <param name="verifyCondition">
        /// Condição de confirmação adaptável para cada CustomControl.</param>
        public virtual void UpdateLayout()
        {
            SetInnerSizes(); SetInnerLocations(); SetMinimumSize();
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
        }
    }
}
