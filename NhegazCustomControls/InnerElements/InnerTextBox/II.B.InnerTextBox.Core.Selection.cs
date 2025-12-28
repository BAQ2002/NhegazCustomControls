using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        /// <summary>
        /// Seleciona todo o texto do InnerTextBox.
        /// Usado, por exemplo, em Ctrl + A.
        /// </summary>
        private void SelectAllText()
        {
            if (string.IsNullOrEmpty(Text))
            {
                // Se não tem texto, não faz sentido manter seleção
                ClearSelection();
                return;
            }

            isMouseSelecting = false;             //Seleção feita via teclado não aciona o estado isMouseSelecting.

            SelectionStartIndex = 0;         //Índice mínimo do texto.
            SelectionEndIndex = Text.Length; //Índice máximo do texto.

            // Redesenha para mostrar o highlight da seleção
            InvalidateParent?.Invoke();
        }

        /// <summary>
        /// Acionado exclusivamente em <see cref="RaiseMouseDown"/> 
        /// e <see cref="RaiseKeyDown"/> ->
        /// Inicia a seleção de carácteres do texto a partir do índice do texto
        /// passado como parâmetro.
        /// </summary>
        private void StartSelection(int index)
        {
            if (index == -1) return;          //Se o índice retornado não existir(== -1).

            SelectionStartIndex = index;      //Âncora da seleção.
            SelectionEndIndex = index;        //Índice ativo (que acompanha o mouse) começa igual o índice âncora.
        }

        /// <summary>
        /// Acionado em <see cref="DeleteSelection"/>, <see cref="SelectAllText"/>
        /// <see cref="RaiseKeyDown"/> e <see cref="RaiseMouseUp"/> -> Limpa a seleção atual.
        /// </summary>
        private void ClearSelection()
        {
            isMouseSelecting = false;
            SelectionStartIndex = -1;
            SelectionEndIndex = -1;
        }

        /// <summary>
        /// Aciona <see cref="PushUndoState"/> ->
        /// Exclui todos os carácteres da seleção atual do texto ->
        /// Atualiza o <see cref="CaretIndex"/> após a exclusão dos carácteres.
        /// </summary>
        private void DeleteSelection()
        {
            if (!HasSelection)                                             //Se não existir seleção -> retorna.
                return;

            PushUndoState();                                               //Salva estado ANTES de alterar o texto.

            int start = Math.Min(SelectionStartIndex, SelectionEndIndex); //Define o índice inicial da seleção.
            int end = Math.Max(SelectionStartIndex, SelectionEndIndex); //Define o índice que acompanha o mouse.
            int length = end - start;                                      //Define o comprimento da seleção.

            Text = Text.Remove(start, length);                             //Remove o trecho selecionado.
            CaretIndex = start; ClearSelection();                          //Caret vai para o início da seleção, limpa os índices de seleção.      
        }
    }
}
