using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        /// <summary>
        /// Acionado exclusivamente em <see cref="RaiseKeyDown"/> ->
        /// Verifica qual tecla está pressionada além do Ctrl ->
        /// Executa atalhos base em casos específicos para cada 
        /// tecla -> retorna "true" se algum caso for acionado.
        /// </summary>
        public bool CtrlCase(KeyEventArgs e)
        {          
            switch (e.KeyCode)
            {
                case Keys.A: SelectAllText();  return true; //Seleciona o texto inteiro.
                case Keys.C: CopyText();       return true; //Copia o texto que estiver em seleção.
                case Keys.X: CutText();        return true; //Recorta o texto que estiver em seleção.
                case Keys.V: PasteText();      return true; //Cola o texto da área de tranferencia(se existir).
                case Keys.Z: UndoLastChange(); return true; //Desfaz a última alteração feita no texto.
            }
            return false; //Se nenhum dos casos foi acionado -> retorna false.
        }

        /// <summary>
        /// Acionado exclusivamente em <see cref="RaiseKeyDown"/> ->
        /// Verifica qual tecla está pressionada além do Shift ->
        /// Modifica a seleção de texto com base em casos específicos 
        /// para cada tecla -> retorna "true" se algum caso for acionado.
        /// </summary>
        public bool ShiftCase(KeyEventArgs e)
        {
            if (!HasSelection) { StartSelection(CaretIndex); }              //Se não houver seleção de texto -> Inicia na posição do Caret.
            switch (e.KeyCode)
            {
                //Define o final da seleção uma posição a esquerda.
                case Keys.Left:  SelectionEndIndex = SelectionEndIndex - 1; return true; //Define a posição final da seleção como uma antes a atual.
                //Define o final da seleção uma posição a direita.
                case Keys.Right: SelectionEndIndex = SelectionEndIndex + 1; return true; //Define a posição final da seleção como uma após a atual.
                //Define a seleção até a primeira posição do texto.
                case Keys.Home:  SelectionEndIndex = 0;                     return true; //Define o final da seleção para a primeira posição do texto.
                //Define a seleção até a última posição do texto.
                case Keys.End:   SelectionEndIndex = Text.Length;           return true; //Define o final da seleção para a ultima posição do texto.

            }
            return false;
        }

        /// <summary>
        /// Acionado exclusivamente em <see cref="RaiseKeyDown"/> ->
        /// Verifica qual tecla está pressionada ->
        /// Executa comandos de texto com base em casos específicos 
        /// para cada tecla -> retorna "true" se algum caso for acionado.
        /// </summary>
        public bool NavigationCase(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (HasSelection) //Se houver seleção válida -> define o CaretIndex para a menor posição da seleção.
                    { CaretIndex = SelectionMinIndex; ClearSelection();   return true; }
                    else
                    { CaretIndex = Math.Max(0, CaretIndex - 1);           return true; }

                case Keys.Right:
                    if (HasSelection) //Se houver seleção válida -> define o CaretIndex para a maior posição da seleção.
                    { CaretIndex = SelectionMaxIndex; ClearSelection();   return true; }
                    else 
                    { CaretIndex = Math.Min(Text.Length, CaretIndex + 1); return true; }
                        
                case Keys.Home: 
                    ClearSelection(); CaretIndex = 0; return true;
                case Keys.End: 
                    ClearSelection(); CaretIndex = Text.Length; return true;

                case Keys.Back:
                    if (HasSelection) { DeleteSelection(); }                             //Se há seleção ativa -> exclui o texto selecionado.
                    else if (CaretIndex > 0 && Text.Length > 0)
                    {
                        PushUndoState(); Text = Text.Remove(CaretIndex - 1, 1);          //Salva o estado atual do textBox e remove um caractere.
                        CaretIndex = Math.Min(Text.Length, Math.Max(0, CaretIndex - 1)); //Define o índice do caret como uma posição antes da atual.
                    }
                    return true;

                case Keys.Delete:
                    if (HasSelection) { DeleteSelection(); }                             //Se há seleção ativa -> exclui o texto selecionado.
                    else if (CaretIndex < Text.Length && Text.Length > 0)                //
                    { PushUndoState(); Text = Text.Remove(CaretIndex, 1); }              //
                    return true;
            }
            return false;
        }
    }
}
