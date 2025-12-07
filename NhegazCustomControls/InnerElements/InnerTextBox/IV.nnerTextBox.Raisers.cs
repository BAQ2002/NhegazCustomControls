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
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> -> 
        /// Aciona <see cref="InnerControl.Click"/> e define o 
        /// <see cref="CaretIndex"/> para o valor de <see cref="GetTextIndexFromPoint"/>.
        /// </summary>
        public override void RaiseClick(object sender, Point clickLocation)
        {
            base.RaiseClick(sender, clickLocation);   //Método da classe base.

            int index = GetTextIndexFromPoint         //Retorna um índice a partir do ponto de Click.
            (clickLocation, RectangleCharWidth.Half); //Usa a metade da largura dos carácteres para maior precisão.

            if (index != -1) { CaretIndex = index; }  //Se existir texto no ponto de Click: CaretIndex = Índice.
        }

        public override void RaiseMouseDown(object sender, Point p)
        {
            base.RaiseMouseDown(sender, p);   //Método da classe base.

            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseDown.
            (p, RectangleCharWidth.Full);     //Usa a metade da largura dos carácteres para maior precisão.
                         
            if (index != -1)                  //Se existir texto no ponto de MouseDown  ->
            { StartSelection(index); }        //Inicia seleção de texto a partir do índice.

            InvalidateParent?.Invoke();
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> ->
        /// Aciona <see cref="InnerControl.MouseMove"/> ->
        /// Faz indexFromPoint seleção de texto e o Careta
        /// companharem o ponto do mouse.
        /// </summary>
        public override void RaiseMouseMove(object sender, Point p)
        {
            base.RaiseMouseMove(sender, p);   //Método da classe base.

            if (!isSelecting) return;         //Se não estiver fazendo seleção: retorna e interrompe o restante.

            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseMove.
            (p, RectangleCharWidth.Half);     //Usa a metade da largura dos carácteres para maior precisão
  
            if (index == -1) return;          //Se não existe texto no ponto do mouse(índice = -1): retorna e interrompe o restante.

            selectionEndIndex = index;        //Índice da seleção que acompanha o mouse.
            CaretIndex        = index;        //Atualiza o CaretIndex para acompanhar indexFromPoint seleção.
        }


        
        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseUp"/> ->
        /// Aciona <see cref="InnerControl.MouseUp"/> e 
        /// define <see cref="isSelecting"/> = false.
        /// </summary>
        public override void RaiseMouseUp(object sender, Point p)
        {
            base.RaiseMouseUp(sender, p);
            isSelecting = false;
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="InnerControl.AbleToHover"/> e <see cref="InnerControl.isHovering"/> ->
        /// Aciona <see cref="InnerControl.MouseEnter"/>, 
        /// torna <see cref="InnerControl.IsHovering"/> = true e aciona <see cref="InnerControl.UpdateParentCursor"/>.
        /// </summary>
        public override void RaiseMouseEnter()
        {
            base.RaiseMouseEnter();
            UpdateParentCursor?.Invoke(Cursors.IBeam);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="InnerControl.AbleToHover"/> e <see cref="InnerControl.isHovering"/> ->
        /// Aciona <see cref="InnerControl.MouseLeave"/>, 
        /// torna <see cref="InnerControl.IsHovering"/> = false e aciona <see cref="InnerControl.UpdateParentCursor"/>.
        /// </summary>
        public override void RaiseMouseLeave()
        {
            base.RaiseMouseLeave();
            UpdateParentCursor?.Invoke(Cursors.Default);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> 
        /// ou <see cref="InnerControlsCollection.HandleGotFocus"/> -> 
        /// Aciona <see cref="InnerControl.GotFocus"/>, 
        /// <see cref="InnerControl.IsFocused"/> = true e <see cref="StartCaret"/>.
        /// </summary>
        public override void RaiseGotFocus(object sender)
        {
            base.RaiseGotFocus(sender);
            StartCaret();
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> 
        /// ou <see cref="InnerControlsCollection.HandleLostFocus"/> -> 
        /// Aciona <see cref="InnerControl.LostFocus"/>, 
        /// <see cref="InnerControl.IsFocused"/> = false e <see cref="StopCaret"/>.
        /// </summary>
        public override void RaiseLostFocus(object sender)
        {
            base.RaiseLostFocus(sender);
            StopCaret();
        }

        /// <summary>
        /// Acionado exclusivamente por <see cref="InnerControlsCollection.HandleKeyDown"/> ->
        /// Executa ações apenas com "teclas" (Left, Right, Home, End, Back, Delete), 
        /// não com caracteres -> modifica indexFromPoint posição do caret <see cref="CaretIndex"/> ->
        /// Sempre protegemos os índices para não sair dos limites da string <see cref="Text"/>.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e)
        {
            // Atalhos com Ctrl primeiro
            if (e.Control && e.KeyCode == Keys.A)
            {
                SelectAllText();
                e.Handled = true;

                // Notifica assinantes de KeyDown (mantém o padrão do resto do método)
                KeyDown?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {
                UndoLastChange();
                e.Handled = true;

                // Se você quiser manter indexFromPoint notificação de KeyDown:
                KeyDown?.Invoke(this, EventArgs.Empty);
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Left:
                    // Move o caret uma posição para indexFromPoint esquerda,
                    // mas nunca abaixo de 0 (início do texto).
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    e.Handled = true;
                    break;

                case Keys.Right:
                    // Move o caret uma posição para indexFromPoint direita,
                    // mas nunca além do fim do texto (Text.Length).
                    CaretIndex = Math.Min(Text.Length, CaretIndex + 1);
                    e.Handled = true;
                    break;

                case Keys.Home:
                    // Leva o caret para o início do texto.
                    CaretIndex = 0;
                    e.Handled = true;
                    break;

                case Keys.End:
                    // Leva o caret para o final do texto (após o último caractere).
                    CaretIndex = Text.Length;
                    e.Handled = true;
                    break;

                case Keys.Back:
                    if (HasSelection) { DeleteSelection(); }                             //remove todos os caracteres que estão na seleção ativa.

                    //remove o caractere ANTERIOR ao caret (se existir) e recua o caret.               
                    // - CaretIndex > 0: há algo antes do caret para apagar.
                    // - Text.Length > 0: texto não está vazio.
                    else if (CaretIndex > 0 && Text.Length > 0)
                    {
                        PushUndoState();                                                 //Salva estado ANTES de alterar o texto.  
                        Text = Text.Remove(CaretIndex - 1, 1);                           // Remove 1 caractere na posição (CaretIndex - 1).
                        CaretIndex = Math.Min(Text.Length, Math.Max(0, CaretIndex - 1)); // de carácteres e CaretIndex - 1. Se (CaretIndex - 1) < 0 ->
                                                                                         // define o CaretIndex = 0.
                    }
                    e.Handled = true;
                    break;

                case Keys.Delete:
                    if (HasSelection) { DeleteSelection(); }
                    // DELETE: remove o caractere NA posição do caret (se existir).
                    // - CaretIndex < Text.Length: há caractere na posição atual para apagar.
                    // - Text.Length > 0: texto não está vazio.
                    else if (CaretIndex < Text.Length && Text.Length > 0)
                    {
                        PushUndoState();                   //Salva estado ANTES de alterar o texto.       
                        Text = Text.Remove(CaretIndex, 1); // Remove 1 caractere exatamente na posição do caret.
                        // Observação: aqui o caret NÃO se move, pois o caractere "da frente" é que foi removido.
                    }
                    e.Handled = true;
                    break;
            }
            KeyDown?.Invoke(this, EventArgs.Empty);                                //Chama o evento de KeyDown se não for nulo.

            // Observações gerais:
            // - Após qualquer alteração de texto ou movimento de caret, o controle deve ser repintado
            //   (Invalidate do pai/área do caret) para atualizar o cursor piscante e o layout.
            // - Se houver seleção de texto no futuro, BACK/DELETE devem remover o intervalo selecionado,
            //   e o caret deve ir para o início da seleção.
            // - Combinações com Ctrl (Ctrl+Left/Right/Home/End) podem ser adicionadas aqui
            //   para saltos por palavras/linhas, conforme necessidade.
        }


        /// <summary>
        /// Acionado exclusivamente por <see cref="InnerControlsCollection.HandleKeyPress(KeyPressEventArgs)"/> ->
        /// Verifica se indexFromPoint tecla pressionada é um carácter -> verifica se indexFromPoint posição onde está sendo inserido
        /// é permitida dentro de <see cref="MaxLength"/> -> vericica se o carácter é aceito pelo filtro 
        /// definido em <see cref="CharFilter"/> -> se todas as verificações estiverem "ok" insere o carácter.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))                                          //Se indexFromPoint tecla pressionada não for um carácter: retorna.
            { e.Handled = true; return; }

            if (MaxLength > 0 && Text.Length >= MaxLength)                          //Se não tem mais espaço para carácteres: não insere nada.
            { e.Handled = true; return; }

            //Se CharFilter for nulo ou se CharFilter não for nulo e retornar verdadeiro para o key pressionado.
            if (CharFilter == null || CharFilter(e.KeyChar))
            {
                PushUndoState();                                                    //Salva estado ANTES de alterar o texto.

                Text = Text.Insert(CaretIndex, e.KeyChar.ToString()); CaretIndex++; //Insere carácter na posição do caret e aumenta o índice do caret.                                                                                .
                e.Handled = true; KeyPress?.Invoke(this, EventArgs.Empty); return;  //Chama o evento de KeyPress se não for nulo.
            }
        }
    }
}
