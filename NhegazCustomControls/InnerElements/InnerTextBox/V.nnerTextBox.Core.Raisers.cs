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
        }

        public override void RaiseMouseDown(object sender, Point p)
        {
            base.RaiseMouseDown(sender, p);   //Método da classe base.

            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseDown.
            (p, RectangleCharWidth.Half);     //Usa a metade da largura dos carácteres para maior precisão.
                         
            if (index != -1)                  //Se existir texto no ponto de MouseDown  ->
            { 
                CaretIndex = index;           //Define CaretIndex = índice.
                isSelecting = true;           //Define que está sendo realizada a seleção no texto.
                StartSelection(index);        //Inicia seleção de texto a partir do índice.
            }        

            InvalidateParent?.Invoke();
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseUp"/> ->
        /// Aciona <see cref="InnerControl.MouseUp"/> e 
        /// define <see cref="isSelecting"/> = false.
        /// </summary>
        public override void RaiseMouseUp(object sender, Point p)
        {
            base.RaiseMouseUp(sender, p);

            if (!HasSelection)            //Se não houver uma seleção
            { ClearSelection(); }         //de texto válida -> limpa a seleção.

            else isSelecting = false;     //Se houver -> apenas define que não está selecionando ativamente com o mouse.
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
            InvalidateParent?.Invoke();
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
        /// <see cref="InnerControl.IsFocused"/> = true e caretTimer.Start().
        /// </summary>
        public override void RaiseGotFocus(object sender)
        {
            base.RaiseGotFocus(sender);
            caretTimer.Start();
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
            caretTimer.Stop();
        }

        /// <summary>
        /// Acionado exclusivamente por <see cref="InnerControlsCollection.HandleKeyDown"/> ->
        /// Executa ações apenas com "teclas" (Left, Right, Home, End, Back, Delete), 
        /// não com caracteres -> modifica indexFromPoint posição do caret <see cref="CaretIndex"/> ->
        /// Sempre protegemos os índices para não sair dos limites da string <see cref="Text"/>.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e)
        {
            // --- Atalhos de teclado com Ctrl --- //
            if (e.Control)
            {
                
                switch (e.KeyCode)
                {
                    case Keys.A:
                        SelectAllText(); e.Handled = true;  //Seleciona o texto inteiro.
                        break;

                    case Keys.C:
                        CopyText(); e.Handled = true;       //Copia o texto que estiver em seleção.
                        break;

                    case Keys.X:
                        CutText(); e.Handled = true;        //Recorta o texto que estiver em seleção.
                        break;

                    case Keys.V:
                        PasteText(); e.Handled = true;      //Cola o texto da área de tranferencia(se existir).
                        break;

                    case Keys.Z:
                        UndoLastChange(); e.Handled = true; //Desfaz a última alteração feita no texto.
                        break;
                }
            }

            // --- Seleção via teclado (Shift + setas/Home/End) --- //
            if (e.Shift)
            {                   
                if (!HasSelection) { StartSelection(CaretIndex); }                        //Se não houver seleção de texto -> Inicia na posição do Caret.
                 
                switch (e.KeyCode)
                {                
                    //Define o final da seleção uma posição a esquerda.
                    case Keys.Left:
                        selectionEndIndex = Math.Max(0, selectionEndIndex - 1);           //Define a posição final da seleção como uma antes a atual.
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                        return;

                    //Define o final da seleção uma posição a direita.
                    case Keys.Right:
                        selectionEndIndex = Math.Min(Text.Length, selectionEndIndex + 1); //Define a posição final da seleção como uma após a atual.
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                        return;

                    //Define a seleção até a primeira posição do texto.
                    case Keys.Home:
                        selectionEndIndex = 0;                                            //Define o final da seleção para a primeira posição do texto.
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                        return;

                    //Define a seleção até a última posição do texto.
                    case Keys.End:

                        selectionEndIndex = Text.Length;                                  //Define o final da seleção para a ultima posição do texto.
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                        return;

                    default:
                        // Não é uma tecla de movimento que nos interessa com Shift.
                        break;
                }              
            }

            // --- Teclas apenas de navegação e edição básica --- //
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (HasSelection) //Se houver seleção válida -> define o CaretIndex para a menor posição da seleção.
                    {
                        CaretIndex = Math.Min(selectionStartIndex, selectionEndIndex);
                        ClearSelection();
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);             //Chama o evento de KeyDown se não for nulo.
                    return;

                case Keys.Right:
                    if (HasSelection) //Se houver seleção válida -> define o CaretIndex para a maior posição da seleção.
                    {
                        CaretIndex = Math.Max(selectionStartIndex, selectionEndIndex);
                        ClearSelection();
                        e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    CaretIndex = Math.Min(Text.Length, CaretIndex + 1);
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);             //Chama o evento de KeyDown se não for nulo.
                    return;

                case Keys.Home:
                    ClearSelection(); CaretIndex = 0;
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                    return;

                case Keys.End:
                    ClearSelection(); CaretIndex = Text.Length;
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                    return; 

                case Keys.Back:
                    if (HasSelection) { DeleteSelection(); }                             //Se há seleção ativa -> exclui o texto selecionado.
                    else if (CaretIndex > 0 && Text.Length > 0)
                    {
                        PushUndoState(); Text = Text.Remove(CaretIndex - 1, 1);          //Salva o estado atual do textBox e remove um caractere.
                        CaretIndex = Math.Min(Text.Length, Math.Max(0, CaretIndex - 1)); //Define o índice do caret como uma posição antes da atual.
                    }
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.
                    return;

                case Keys.Delete:
                    if (HasSelection) { DeleteSelection(); }                             //Se há seleção ativa -> exclui o texto selecionado.
                    else if (CaretIndex < Text.Length && Text.Length > 0)                //
                    { PushUndoState(); Text = Text.Remove(CaretIndex, 1); }              //
                    e.Handled = true; KeyDown?.Invoke(this, EventArgs.Empty);         //Chama o evento de KeyDown se não for nulo.                                      //    
                    return;
            }
             
        }



        /// <summary>
        /// Acionado exclusivamente por <see cref="InnerControlsCollection.HandleKeyPress(KeyPressEventArgs)"/> ->
        /// Verifica se a tecla pressionada é um carácter -> verifica se a posição onde está sendo inserido
        /// é permitida dentro de <see cref="MaxLength"/> -> vericica se o carácter é aceito pelo filtro 
        /// definido em <see cref="CharFilter"/> -> se todas as verificações estiverem "ok" insere o carácter.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))                                  //Se tecla pressionada for de controle -> retorna.
            { e.Handled = true; return; }

            if (CharFilter != null && !CharFilter(e.KeyChar))               //Se houver filtro e ele bloquear o caractere -> retorna.
            { e.Handled = true; return; }

            int selectionLength = HasSelection ?                            //Comprimento do texto em seleção -> se não houver = 0.  
                Math.Abs(selectionEndIndex - selectionStartIndex) : 0;      

            int lengthAfterSelection = Text.Length - selectionLength + 1;   //Tamanho final após substituir seleção por 1 char.

            if (MaxLength > 0 && lengthAfterSelection > MaxLength)          //Se mesmo substituindo, ultrapassar MaxLength -> retorna.
            { e.Handled = true; return; }

            if (HasSelection) DeleteSelection();                            //Remove seleção (salva estado antes).
            else PushUndoState();                                           //Sem seleção: apenas salva estado.

            Text = Text.Insert(CaretIndex, e.KeyChar.ToString());           //Insere carácter na posição atual do caret. 
            CaretIndex++;                                                   //Aumenta o índice do caret.

            e.Handled = true; KeyPress?.Invoke(this, EventArgs.Empty);      //Chama o evento de KeyPress se não for nulo.
        }
    }
}
