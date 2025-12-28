using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        /// <summary>Representa se está selecionando ativamente com o mouse.</summary>      
        private bool isMouseSelecting = false;

        /// <summary>Representa o índice inicial(âncora) da seleção.</summary>    
        private int selectionStartIndex = -1;

        /// <summary>
        /// Representao índice atual(ativo) da seleção ->
        /// acompanha o mouse enquanto estiver pressionado ou
        /// os atalhos com teclas.
        /// </summary>    
        private int selectionEndIndex = -1;

        /// <summary>
        /// Representa se há uma seleção válida de texto. Condicão para "true" ->
        /// <para><see cref="SelectionEndIndex"/> != <see cref="SelectionStartIndex"/>.</para>
        /// </summary>
        private bool HasSelection => SelectionStartIndex != SelectionEndIndex;


        public int SelectionStartIndex
        {
            get => selectionStartIndex;
            private set
            {
                int limitedValue = NhegazMathMethods.Clamp(value, 0, Text.Length); //Valor limitado entre -1 e Text.Length.
                selectionStartIndex = limitedValue;
            }
        }

        public int SelectionEndIndex
        {
            get => selectionEndIndex;
            private set
            {
                int limitedValue = NhegazMathMethods.Clamp(value, 0, Text.Length); //Valor limitado entre -1 e Text.Length.
                selectionEndIndex = limitedValue;
            }
        }

        /// <summary>Retorna o menor índice que estiver dentro da seleção de texto.</summary>
        public int SelectionMinIndex => Math.Min(SelectionStartIndex, SelectionEndIndex);

        /// <summary>Retorna o maior índice que estiver dentro da seleção de texto.</summary>
        public int SelectionMaxIndex => Math.Max(SelectionStartIndex, SelectionEndIndex);

        /// <summary>Cor de fundo quando está sendo realizada a seleção de carácteres com o mouse.</summary>
        public Color SelectionBackgroundColor { get; set; } = SystemColors.Highlight;

        /// <summary>Cor do texto quando está sendo realizada a seleção de carácteres com o mouse.</summary>
        public Color SelectionForeColor { get; set; } = SystemColors.Window;
    }
}
