using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        public InnerTextBox(bool autoSizeBasedOnText = false) : base()
        {
            SizeBasedOnText = autoSizeBasedOnText;

            caretTimer = new System.Windows.Forms.Timer { Interval = CaretBlinkIntervalMs };
            caretTimer.Tick += (s, e) => RestartCaretBlink();
        }

        /// <summary>
        /// Método que é resposável por atualizar o visual do caret.
        /// </summary>
        private void RestartCaretBlink()
        {
            caretVisible = caretVisible ? false : true; //Inverte o estado visual do caret.
            InvalidateParent?.Invoke();                 //Atualiza o visual a partir de CustomControl.Invalidate().
        }

        /// <summary>Inicia o temporizador do Caret e o torna visível. </summary>
        public void StartCaret()
        {
            caretTimer.Start(); caretVisible = true;
        }


        /// <summary>Encerra o temporizador do Caret e o torna invisível. </summary>
        public void StopCaret()
        {
            caretTimer.Stop(); caretVisible = false;
        }

        /// <summary>    
        /// Retorna um índice de texto a 
        /// partir de um <see cref="Point"/>.
        /// <para>
        /// Valor utilizado em <see cref="RaiseClick"/>,
        /// <see cref="RaiseMouseDown"/> e <see cref="RaiseMouseMove"/>.
        /// </para> 
        /// Se não houver texto -> Índice = 0.
        /// <para>
        /// Se o <see cref="Point"/> Location 
        /// estiver mais a direita do que o fim do texto ->
        /// Retorna o índice do fim do texto. 
        /// </para>
        /// <para>
        /// Se o <see cref="Point"/> Location 
        /// pertence a algum carácter do texto ->
        /// Retorna o índice do cáracter.
        /// </para>
        /// se não pertencer a nenhum -> Retorna -1.
        /// </summary>
        public int GetTextIndexFromPoint(Point location, RectangleCharWidth rectangleCharWidth)
        {
            if (Text.Length == 0){ return 0; }                          //Se não houver texto -> Índice obrigatório ser no início(= 0).
            if (location.X >= TextRectangle.Right){return Text.Length;} //Se o ponto for depois do texto -> Índice no fim do texto.

            bool useHalfWidth =                                         //Se rectangleCharWidth == RectangleCharWidth.Half = true.                                                  
            rectangleCharWidth == RectangleCharWidth.Half;                     

            int factor = useHalfWidth ? 2 : 1;                          //Define se será dividido em metades (2) ou largura inteira (1).
            int amountOfRects = Text.Length * factor;                   //Quantidade de retângulos (1 por carácter ou 2 por carácter).
            int charWidth = NhegazSizeMethods.FontUnitSize(Font).Width; //Largura base de um carácter.
            int step = charWidth / factor;                              //Distância em X entre cada retângulo.

            for (int i = 0; i < amountOfRects; i++)                     //Para cada retângulo calculado.
            {
                int x = TextRectangle.X + i * step;                     //Posição X acumulativa a partir do índice.
                int y = TextRectangle.Y;                                //Posição Y igual Y(0) do texto.
                int width = charWidth / factor;                         //Largura de cada retângulo (inteiro ou metade da fonte).
                int height = Height;                                    //Altura igual do InnerTextBox.

                Rectangle charRect = new(x, y, width, height);          //Instância do retângulo.
                if (charRect.Contains(location))                        //Se o ponto do mouse pertence ao retângulo.
                {
                    if (useHalfWidth)                                   //Modo "meia largura" (metade esquerda/direita).
                    { return (int)Math.Ceiling(i / 2.0); }              //Mapeia o índice do retângulo para índice de caret.              
                    else { return i; }                                  //Modo "largura inteira", retorna o índice do carácter.
                }
            }

            return -1;                                                  //Se não existir carácter para aquele ponto retorna -1.
        }
    
        /// <summary>
        /// Acionado se o <see cref="TextFormatFilter"/> for definido
        /// diferente de <see cref="TextFormatFilter.None"/> e
        /// em <see cref="InnerControl.LostFocus"/> ->
        /// Aplica o formato de texto definido em 
        /// <see cref="TextFormatFilter"/>.
        /// </summary>
        private void ApplyTextFormat()
        {
            if (TextFormatFilter == TextFormatFilter.D2)
            {
                if (Text == string.Empty)
                    return;

                int textValue = int.Parse(Text); //Valor numérico do texto.
                Text = textValue.ToString("D2"); //Formatado para "D2".
            }

            else if (TextFormatFilter == TextFormatFilter.D4)
            {
                if (Text == string.Empty)
                    return;

                int textValue = int.Parse(Text); //Valor numérico do texto.
                Text = textValue.ToString("D4"); //Formatado para "D2".
            }
        }

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
            
            isSelecting = false;             //Seleção feita via teclado não aciona o estado isSelecting.

            selectionStartIndex = 0;         //Índice mínimo do texto.
            selectionEndIndex = Text.Length; //Índice máximo do texto.

            
            CaretIndex = Text.Length;        //Caret vai para o final do texto.

            // Redesenha para mostrar o highlight da seleção
            InvalidateParent?.Invoke();
        }

        /// <summary>Salva o estado atual na pilha de Undo.</summary>
        private void PushUndoState()
        {
            undoStack.Push(new UndoState
            {
                Text = Text,
                CaretIndex = CaretIndex,
                SelectionStartIndex = selectionStartIndex,
                SelectionEndIndex = selectionEndIndex
            });
        }

        /// <summary>Restaura o último estado salvo (Ctrl + Z).</summary>
        private void UndoLastChange()
        {
            if (undoStack.Count == 0)
                return;

            var state = undoStack.Pop();

            // Evita usar o setter de CaretIndex/Text para não gerar Undo novo.
            text = state.Text ?? string.Empty;
            caretIndex = Math.Max(0, Math.Min(text.Length, state.CaretIndex));

            selectionStartIndex = state.SelectionStartIndex;
            selectionEndIndex = state.SelectionEndIndex;

            // Garante que o caret volte a piscar e o layout seja atualizado
            RestartCaretBlink();
            UpdateLayout();
            InvalidateParent?.Invoke();
        }

        /// <summary>
        /// Acionado exclusivamente em <see cref="RaiseMouseDown"/> ->
        /// Inicia a seleção de carácteres do texto a partir do índice do texto
        /// passado como parâmetro.
        /// </summary>
        private void StartSelection(int index)
        {
            isSelecting = true;               //Define que estamos realizando a seleção no texto.
            
            if (index == -1) return;          //Se o índice retornado não existir(== -1).

            selectionStartIndex = index;      //Âncora da seleção.
            selectionEndIndex = index;        //Índice ativo (que acompanha o mouse) começa igual o índice âncora.
            CaretIndex = index;               //Faz com que o caret acompanhe o índice.
        }

        /// <summary>
        /// Acionado exclusivamente em <see cref="DeleteSelection"/> ->
        /// Limpa a seleção atual.
        /// </summary>
        private void ClearSelection()
        {
            isSelecting = false;
            selectionStartIndex = -1;
            selectionEndIndex = -1;
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

            int start  = Math.Min(selectionStartIndex, selectionEndIndex); //Define o índice inicial da seleção.
            int end    = Math.Max(selectionStartIndex, selectionEndIndex); //Define o índice que acompanha o mouse.
            int length = end - start;                                      //Define o comprimento da seleção.
           
            Text = Text.Remove(start, length);                             //Remove o trecho selecionado.
            CaretIndex = start; ClearSelection();                          //Caret vai para o início da seleção, limpa os índices de seleção.      
        }


    }
}
