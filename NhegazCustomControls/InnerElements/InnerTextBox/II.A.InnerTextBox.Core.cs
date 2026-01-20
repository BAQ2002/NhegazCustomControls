using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
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
            TextFeatures = new TextFeature(this, () => Text);
            caretTimer = new System.Windows.Forms.Timer { Interval = CaretBlinkIntervalMs };
            caretTimer.Tick += (s, e) => CaretBlink();
        }

        /// <summary>Inverte o estado do <see cref="caretBlinkState"/> criando o efeito do Caret "piscar".</summary>
        private void CaretBlink()
        {
            caretBlinkState = !caretBlinkState; //Inverte o estado visual do caret.
            InvalidateParent?.Invoke();         //Atualiza o visual a partir de CustomControl.Invalidate().
        }

        /// <summary>
        /// Reinicia o intervalo de tempo do <see cref="caretTimer"/>.
        /// </summary>
        private void RestartCaretBlink()
        {
            caretTimer.Stop();               //Desliga o timer.
            caretBlinkState = true;          //Força o estado do Caret para visível.
            InvalidateParent?.Invoke();      //Atualiza o visual a partir de CustomControl.Invalidate().
            caretTimer.Start();              //Inicia novamente o timer.
        }
        /// <summary>
        /// Retorna um indice de um texto a partir de um <see cref="Point"/> -> 
        /// para cada caracter de <see cref="Text"/> texto há duas Hitbox's. 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public int GetTextIndexFromPoint(Point location)
        {
            int amountOfRects = Text.Length * 2;
            

            for (int i = 0; i < amountOfRects; i++)   //Para cada char em Text * 2.
            {
                int charIndex = i / 2;

                //Representação visual das Hitbox's    
                //-------------------------------------//
                //  _._  _._  _._  _._  _._  _._  _._
                // |   ||   ||   ||   ||   ||   ||   | //
                // |_0_||_1_||_2_||_3_||_4_||_5_||_5_| //
                // | | || | || | || | || | || | || | | //
                // |0|1||1|2||2|3||3|4||4|5||5|6||5|6| //
                //-------------------------------------//

                Rectangle charRect = NhegazSizeMethods.
                TextCharRect(TextLocation, Text, charIndex, Font); //Retângulo(X, Y, Width, Height)
                charRect.Width = charRect.Width / 2;               //Width = Width / 2.
                charRect.X    += charRect.Width * (i % 2);         //X = X + Width * (i % 2).

                if (charRect.Contains(location))                        //Se o ponto do mouse pertence ao retângulo.
                {
                   return (int)Math.Ceiling(i / 2.0);
                }
            }


            return -1;
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

        

        /// <summary>Salva o estado atual na pilha de Undo.</summary>
        private void PushUndoState()
        {
            undoStack.Push(new UndoState
            {
                Text = Text,
                CaretIndex = CaretIndex,
                SelectionStartIndex = SelectionStartIndex,
                SelectionEndIndex = SelectionEndIndex
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

            SelectionStartIndex = state.SelectionStartIndex;
            SelectionEndIndex = state.SelectionEndIndex;

            // Garante que o caret volte a piscar e o layout seja atualizado
            CaretBlink();
            UpdateLayout();
            InvalidateParent?.Invoke();
        }      
    }
}
