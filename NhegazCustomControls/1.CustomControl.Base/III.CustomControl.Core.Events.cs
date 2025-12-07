using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomControl
    {
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        /// <summary>
        /// Override do evento de clique. Propaga o evento para os <see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="e">Argumentos do clique.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);                             //Método base.
            InnerControls.HandleClick(this, e.Location);      //InnerControlsCollection verifica se a posição pertence a um InnerControl.
            var headerFeature = (this as IHasHeader)?.Header; //Se o Controle possuir Header.
            headerFeature?.HandleClick(e.Location);           //HeaderFeature verifica se a posição pertence ao Header.
        }

        /// <summary>
        /// Override do evento de duplo clique. Propaga o evento para os <see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="e">Argumentos do duplo clique.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);                        //Método base.
            InnerControls.HandleDoubleClick(this, e.Location); //InnerControlsCollection verifica se a posição pertence a um InnerControl.
            var headerFeature = (this as IHasHeader)?.Header;  //Se o Controle possuir Header.
            headerFeature?.HandleDoubleClick(e.Location);      //HeaderFeature verifica se a posição pertence ao Header.
        }

        /// <summary>
        /// Override do evento de movimento do mouse. Propaga o evento para os <see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="e">Argumentos do movimento do mouse.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InnerControls.HandleMouseMove(this, e.Location);
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleMouseMove(e.Location);
        }

        /// <summary>
        /// Override do evento quando o controle ganha foco. Propaga o evento para os <see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="e">Argumentos do foco.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (Focused)
            {
                InnerControls.HandleGotFocus(this, PointToClient(Cursor.Position)); //Executa o foco do elemento interno se coincidir com a localização.
                var headerFeature = (this as IHasHeader)?.Header;                   //Verifica se o Controle possui um cabeçalho.
                headerFeature?.HandleGotFocus(PointToClient(Cursor.Position));      //Se o cabeçalho existir: executa o foco dele se coincidir com a localização.

            }
        }

        /// <summary>
        /// Override do evento quando o controle perde o foco. Propaga o evento para os <see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="e">Argumentos do evento de perda de foco.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            InnerControls.HandleLostFocus(this);              //Executa o foco do elemento interno se coincidir com a localização.
            var headerFeature = (this as IHasHeader)?.Header; //Verifica se o Controle possui um cabeçalho.
            headerFeature?.HandleLostFocus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            InnerControls.HandleMouseDown(this, e.Location);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            InnerControls.HandleMouseUp(this, e.Location);
        }

        protected override void OnEnter(EventArgs e) { base.OnEnter(e); Invalidate(); }
        protected override void OnLeave(EventArgs e) { base.OnLeave(e); Invalidate(); }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            InnerControls.HandleKeyDown(e); Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.Handled) return; // EVITA DUPLA INSERÇÃO: se alguém já tratou, não redistribua

            InnerControls.HandleKeyPress(e); Invalidate();
        }
    }
}
