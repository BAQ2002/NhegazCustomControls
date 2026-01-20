using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Nhegaz;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        /// <summary>Representa se está selecionando ativamente com o mouse.</summary>      
        private bool isMouseSelecting = false;

        /// <summary>Representa o índice inicial(âncora) da seleção.</summary>    
        private int selectionStartIndex = 0;

        /// <summary>
        /// Representao índice atual(ativo) da seleção ->
        /// acompanha o mouse enquanto estiver pressionado ou
        /// os atalhos com teclas.
        /// </summary>    
        private int selectionEndIndex = 0;

        /// <summary>
        /// Representa se há uma seleção válida de texto. Condicão para "true" ->
        /// <para><see cref="SelectionEndIndex"/> != <see cref="SelectionStartIndex"/>.</para>
        /// </summary>
        private bool HasSelection => SelectionStartIndex != SelectionEndIndex;

        /// <summary>
        /// 
        /// </summary>
        public int SelectionStartIndex
        {
            get => selectionStartIndex;
            private set
            {
                selectionStartIndex = Nhegaz.MathMethods.Clamp(value, 0, Text.Length); //Valor limitado entre 0 e Text.Length.
            }
        }

        public int SelectionEndIndex
        {
            get => selectionEndIndex;
            private set
            {
                selectionEndIndex = Nhegaz.MathMethods.Clamp(value, 0, Text.Length); //Valor limitado entre 0 e Text.Length.
            }
        }

        /// <summary>Retorna o menor índice que estiver dentro da seleção de texto.</summary>
        public int SelectionMinIndex => Math.Min(SelectionStartIndex, SelectionEndIndex);

        /// <summary>Retorna o maior índice que estiver dentro da seleção de texto.</summary>
        public int SelectionMaxIndex => Math.Max(SelectionStartIndex, SelectionEndIndex);    

        /// <summary>Retorna o texto em seleção.</summary>
        public string SelectionText => HasSelection ? Text.Substring(SelectionMinIndex, SelectionMaxIndex - SelectionMinIndex) : string.Empty;

        /// <summary>Retorna a coordenada(X,Y) do texto em seleção.</summary>
        public Point SelectionLocation => NhegazSizeMethods.LocationByIndex(TextLocation, Text, SelectionMinIndex, Font);

        /// <summary>Retorna o tamanho visual do texto em seleção.</summary>
        public Size SelectionSize => NhegazSizeMethods.TextExactSize(SelectionText, Font);

        /// <summary>Retorna o retângulo do texto em seleção.</summary>
        public Rectangle SelectionRectangle => new(SelectionLocation, SelectionSize);

        /// <summary>Cor de fundo quando está sendo realizada a seleção de carácteres com o mouse.</summary>
        public Color SelectionBackgroundColor { get; set; } = SystemColors.Highlight;

        /// <summary>Cor do texto quando está sendo realizada a seleção de carácteres com o mouse.</summary>
        public Color SelectionForeColor { get; set; } = SystemColors.Window;
    }
}
