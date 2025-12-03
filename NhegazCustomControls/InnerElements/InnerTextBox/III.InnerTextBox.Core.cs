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
        /// Se o <see cref="Point"/> Location 
        /// pertence a algum carácter do texto ->
        /// Retorna o índice do cáracter.
        /// </summary>
        public int GetTextIndexFromPoint(Point location)
        {
            Rectangle[] charHalfRects = new Rectangle[Text.Length * 2]; //Retângulos das metades de cada carácter do texto.

            for (int i = 0; i < charHalfRects.Length; i++)
            {
                int x = TextRectangle.X + i * (NhegazSizeMethods.FontUnitSize(Font).Width / 2); //Posição X acumulativa a partir do índice.
                int y = TextRectangle.Y;                                                        //Posição Y igual Y(0) do texto.
                int width = NhegazSizeMethods.FontUnitSize(Font).Width / 2;                     //Largura igual a metade da fonte.
                int height = Height;                                                            //Altura igual do InnerTextBox.

                charHalfRects[i] = new Rectangle(x, y, width, height);                          //Instância do retângulo.
                if (charHalfRects[i].Contains(location))                                        //Se o ponto de click pertece ao retângulo.
                {
                    return (int)Math.Ceiling(i / 2.0);                                          //Ajuste de valor.       
                }         
            }
            return -1;                                                                          //Se não existir caráter para aquele ponto retorna -1.
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
        private void DeleteSelection()
        {
            if (!HasSelection)
                return;

            int start = Math.Min(selectionStartIndex, selectionEndIndex);
            int end = Math.Max(selectionStartIndex, selectionEndIndex);
            int length = end - start;

            // Remove o trecho selecionado
            Text = Text.Remove(start, length);

            // Caret vai para o início da seleção
            CaretIndex = start;

            // Limpa os índices de seleção
            ClearSelection();
        }


    }
}
