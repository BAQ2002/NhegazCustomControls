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

        public override void RaiseClick(object sender, Point clickLocation)
        {
            base.RaiseClick(sender, clickLocation); 
            SetCaretLocation(clickLocation);
        }

        /// <summary>     
        /// Se o <see cref="Point"/> clickLocation 
        /// pertence a algum carácter do texto ->
        /// Atualiza o <see cref="CaretIndex"/> : consequentimente -> 
        /// Atualiza o <see cref="CaretLocation"/>.
        /// </summary>
        public void SetCaretLocation(Point clickLocation)
        {            
            Rectangle[] charHalfRects = new Rectangle[Text.Length * 2]; //Retângulos das metades de cada carácter do texto.

            for(int i = 0; i < charHalfRects.Length; i++) 
            {
                int x = TextRectangle.X + i * (NhegazSizeMethods.FontUnitSize(Font).Width / 2); //Posição X acumulativa a partir do índice.
                int y = TextRectangle.Y;                                                        //Posição Y igual Y(0) do texto.
                int width = NhegazSizeMethods.FontUnitSize(Font).Width / 2;                     //Largura igual a metade da fonte.
                int height = Height;                                                            //Altura igual do InnerTextBox.
                    
                charHalfRects[i] = new Rectangle(x, y, width, height);                          //Instância do retângulo.
                if (charHalfRects[i].Contains(clickLocation))                                   //Se o ponto de click pertece ao retângulo.
                {
                    CaretIndex = (int)Math.Ceiling(i / 2.0); return;                            //Ajuste de valor.       
                }              
            }
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
        /// <see cref="InnerControl.Focused"/> = true e <see cref="StartCaret"/>.
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
        /// <see cref="InnerControl.Focused"/> = false e <see cref="StopCaret"/>.
        /// </summary>
        public override void RaiseLostFocus(object sender)
        {
            base.RaiseLostFocus(sender);       
            StopCaret();
        }
        
        /// <summary>
        /// Manipula teclas de navegação e edição (KeyDown).
        /// Observações:
        /// - KeyDown trabalha com "teclas" (Left, Right, Home, End, Back, Delete), não com caracteres.
        /// - Aqui movemos o caret e removemos caracteres quando necessário.
        /// - Sempre protegemos os índices para não sair dos limites da string.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e)
        {
            

            switch (e.KeyCode)
            {
                case Keys.Left:
                    // Move o caret uma posição para a esquerda,
                    // mas nunca abaixo de 0 (início do texto).
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    e.Handled = true;
                    break;

                case Keys.Right:
                    // Move o caret uma posição para a direita,
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
                    // BACKSPACE: remove o caractere ANTERIOR ao caret (se existir) e recua o caret.
                    // Condições:
                    // - CaretIndex > 0: há algo antes do caret para apagar.
                    // - Text.Length > 0: texto não está vazio.
                    if (CaretIndex > 0 && Text.Length > 0)
                    {                       
                        Text = Text.Remove(CaretIndex - 1, 1);                           // Remove 1 caractere na posição (CaretIndex - 1).
                        CaretIndex = Math.Min(Text.Length, Math.Max(0, CaretIndex - 1)); // Atualiza o CaretIndex para o menor valor entre a quantidade
                                                                                         // de carácteres e CaretIndex - 1. Se (CaretIndex - 1) < 0 ->
                                                                                         // define o CaretIndex = 0.
                    }
                    e.Handled = true;
                    break;

                case Keys.Delete:
                    // DELETE: remove o caractere NA posição do caret (se existir).
                    // Condições:
                    // - CaretIndex < Text.Length: há caractere na posição atual para apagar.
                    // - Text.Length > 0: texto não está vazio.
                    if (CaretIndex < Text.Length && Text.Length > 0)
                    {
                        // Remove 1 caractere exatamente na posição do caret.
                        Text = Text.Remove(CaretIndex, 1);
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

        // A infra atual chama RaiseGotFocus/RaiseLostFocus sem payload;
        // então conectamos aqui via inscrição no próprio construtor do controle pai (externo).
        // Sugestão: ao instanciar, fazer:
        // innerTextBox.GotFocus += (s,e) => innerTextBox.OnInnerGotFocus();
        // innerTextBox.LostFocus += (s,e) => innerTextBox.OnInnerLostFocus();

        // ======== Entrada de teclado (encaminhada pelo CustomControl) ========
        public void RaiseKeyPress(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))                                          //Se a tecla pressionada não for um carácter: retorna.
            { e.Handled = true; return; }

            if (MaxLength > 0 && Text.Length >= MaxLength)                          //Se não tem mais espaço para carácteres: não insere nada.
            { e.Handled = true; return; }

            //Se CharFilter for nulo ou se CharFilter não for nulo e retornar verdadeiro para o key pressionado.
            if (CharFilter == null || CharFilter(e.KeyChar))
            {
                Text = Text.Insert(CaretIndex, e.KeyChar.ToString()); CaretIndex++; //Insere carácter na posição do caret e aumenta o índice do caret.                                                                                .
                e.Handled = true; KeyPress?.Invoke(this, EventArgs.Empty); return;  //Chama o evento de KeyPress se não for nulo.
            }
        }
    }
}
