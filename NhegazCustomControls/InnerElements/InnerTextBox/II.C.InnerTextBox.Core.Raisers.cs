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
        /// Aciona <see cref="InnerControl.Click"/>.
        /// </summary>
        public override void RaiseClick(object sender, Point clickLocation)
        {
            base.RaiseClick(sender, clickLocation);   //Método da classe base.   
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleDoubleClick"/> ->
        /// Aciona <see cref="InnerControl.DoubleClick"/>.
        /// </summary>
        public override void RaiseDoubleClick(object sender, Point clickLocation)
        {
            base.RaiseDoubleClick(sender, clickLocation);   //Método da classe base.
                                                            
            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseDown.
            (clickLocation, RectangleCharWidth.Half);     //Usa a metade da largura dos carácteres para maior precisão.

            if (index != -1)                  //Se existir texto no ponto de MouseDown  ->
            {
                SelectWordAtIndex(index);
            }
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseDown"/> -> 
        /// Aciona <see cref="MouseDown"/>, define o <see cref="CaretIndex"/>  
        /// para o valor de <see cref="GetTextIndexFromPoint"/> => (<paramref name="p"/>) e
        /// Aciona <see cref="InnerControl.InvalidateParent"/> . 
        /// </summary>
        public override void RaiseMouseDown(object sender, Point p)
        {
            base.RaiseMouseDown(sender, p);   //Método da classe base.

            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseDown.
            (p, RectangleCharWidth.Half);     //Usa a metade da largura dos carácteres para maior precisão.
                         
            if (index != -1)                  //Se existir texto no ponto de MouseDown  ->
            { 
                CaretIndex = index;           //Define CaretIndex = índice.
                isMouseSelecting = true;           //Define que está sendo realizada a seleção no texto.
                StartSelection(index);        //Inicia seleção de texto a partir do índice.
            }        

            InvalidateParent?.Invoke();
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseUp"/> ->
        /// Aciona <see cref="InnerControl.MouseUp"/> e 
        /// define <see cref="isMouseSelecting"/> = false.
        /// </summary>
        public override void RaiseMouseUp(object sender, Point p)
        {
            base.RaiseMouseUp(sender, p);

            if (!HasSelection)            //Se não houver uma seleção
            { ClearSelection(); }         //de texto válida -> limpa a seleção.

            else isMouseSelecting = false;     //Se houver -> apenas define que não está selecionando ativamente com o mouse.
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

            if (!isMouseSelecting) return;         //Se não estiver fazendo seleção: retorna e interrompe o restante.

            int index = GetTextIndexFromPoint //Retorna um índice a partir do ponto de MouseMove.
            (p, RectangleCharWidth.Half);     //Usa a metade da largura dos carácteres para maior precisão
  
            if (index == -1) return;          //Se não existe texto no ponto do mouse(índice = -1): retorna e interrompe o restante.

            SelectionEndIndex = index;        //Índice da seleção que acompanha o mouse.
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
            if (e.Control){ e.Handled = CtrlCase(e); }     //Atalhos de teclado com Ctrl.
            else if (e.Shift){ e.Handled = ShiftCase(e); } //Seleção via teclado (Shift + setas/Home/End).
            else { e.Handled = NavigationCase(e); }        //Teclas apenas de navegação e edição básica.

            if (e.Handled == true)                         //Se algum dos cases foi acionado e torno e.Handle = true ->
                KeyDown?.Invoke(this, EventArgs.Empty);    //Aciona o evento KeyDown se não for nulo.
        }



        /// <summary>
        /// Acionado exclusivamente por <see cref="InnerControlsCollection.HandleKeyPress(KeyPressEventArgs)"/> ->
        /// Verifica se a tecla pressionada é um carácter -> verifica se a posição onde está sendo inserido
        /// é permitida dentro de <see cref="MaxLength"/> -> vericica se o carácter é aceito pelo filtro 
        /// definido em <see cref="CharFilter"/> -> se todas as verificações estiverem "ok" insere o carácter.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))                                  //Se a tecla pressionada for de controle -> retorna.
            { e.Handled = true; return; }

            if (CharFilter != null && !CharFilter(e.KeyChar))               //Se houver filtro e ele bloquear o caractere -> retorna.
            { e.Handled = true; return; }

            int selectionLength = HasSelection ?                            //Comprimento do texto em seleção -> se não houver = 0.  
                Math.Abs(SelectionEndIndex - SelectionStartIndex) : 0;      

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
