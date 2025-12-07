using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace NhegazCustomControls
{
    public class InnerControlsCollection
    {
        private List<InnerControl> elements = new();
        private CustomControl Parent;

        /// <summary>
        /// Referência ao <see cref="InnerControl"/> 
        /// que estiver em foco se existir.
        /// </summary>
        public InnerControl? FocusedInnerControl { get; private set; }

        /// <summary>Retorna a coleção interna de <see cref="InnerControl"/>'s.</summary>
        public List<InnerControl> GetAll
        {
            get => elements;
        }

        /// <summary>Adiciona o <see cref="InnerControl"/> à coleção interna de InnerControl's.</summary>
        public void Add(InnerControl innerControl)
        {
            elements.Add(innerControl);
            if (innerControl is InnerTextBox)                 //Se for um InnerTextBox (ou outros que precisarem no futuro), 
            {
                innerControl.InvalidateParent = Parent.Invalidate; //devido à funcionalidades: Torna capaz de atualizar o Parent.
                innerControl.UpdateParentCursor = cursor =>
                {Parent.Cursor = cursor;};
            }          
        }

        /// <summary>Remove o <see cref="InnerControl"/> da coleção interna de InnerControl's.</summary>
        public void Remove(InnerControl innerControl)
        {
            elements.Remove(innerControl);
        }

        /// <summary>Remove todos os <see cref="InnerControl"/> da coleção interna de InnerControl's.</summary>
        public void Clear()
        {
            elements.Clear();
        }

        /// <summary>
        /// Construtor padrão de <see cref="InnerControlsCollection"/> ->
        /// Define inicialmente apenas o <see cref="Parent"/>
        /// </summary>
        public InnerControlsCollection(CustomControl parent) 
        { 
            Parent = parent;    
        }

        /// <summary>
        /// Verifica todos os <see cref="InnerControl.Visible"/> ->
        /// Executa <see cref="InnerControl.OnPaint"/> se verdadeiro.
        /// </summary>
        public void OnPaintAll(PaintEventArgs e)
        {
            foreach (var element in elements)
            {
                if (element.Visible)
                    element.OnPaint(e);
            }
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnMouseClick"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/>  
        /// e <see cref="InnerControl.HitBox"/> ->
        /// Executa <see cref="InnerControl.RaiseClick"/>
        /// e <see cref="Control.Invalidate()"/>.
        /// </summary>
        public void HandleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseClick(parent, clickLocation);                    //Aciona o Click do elemento que o ponto de click pertence.

                    if (element != FocusedInnerControl)                           //Se o elemento clicado não for o atual em foco.
                    { FocusedInnerControl?.RaiseLostFocus(parent); }              //Se o elemento interno com foco existir -> desfoca.                  

                    element.RaiseGotFocus(parent); FocusedInnerControl = element; //Atualiza para o elemento que o ponto de click pertence.
                                                                                  
                    parent.Invalidate();                                          //Atualiza o visual a partir do CustomControl parent.                 
                }
            }
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnMouseDoubleClick"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/>  
        /// e <see cref="InnerControl.HitBox"/> ->
        /// Executa <see cref="InnerControl.RaiseDoubleClick"/>
        /// e <see cref="Control.Invalidate()"/>.
        /// </summary>
        public void HandleDoubleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseDoubleClick(parent, clickLocation);

                    if (element != FocusedInnerControl)                           //Se o elemento clicado não for o atual em foco.
                    { FocusedInnerControl?.RaiseLostFocus(parent); }              //Se o elemento interno com foco existir -> desfoca.                  

                    element.RaiseGotFocus(parent); FocusedInnerControl = element; //Atualiza para o elemento que o ponto de click pertence.

                    parent.Invalidate();                                          //Atualiza o visual a partir do CustomControl parent.
                }
            }
        }
        public void HandleMouseDown(CustomControl parent, Point location)
        {
            foreach (var element in elements)
                if (element.Visible && element.HitBox(location))
                    element.RaiseMouseDown(parent, location);
        }

        public void HandleMouseUp(CustomControl parent, Point location)
        {
            foreach (var element in elements)
                if (element.Visible)
                    element.RaiseMouseUp(parent, location);
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnMouseMove"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/> e <see cref="InnerControl.HitBox"/> ->
        /// Verifica o estado de <see cref="InnerControl.IsHovering"/> ->
        /// Executa <see cref="InnerControl.RaiseMouseEnter"/>
        /// ou <see cref="InnerControl.RaiseMouseLeave"/>.
        /// </summary>
        public void HandleMouseMove(CustomControl parent, Point mouseLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible)
                {
                    bool contains = element.HitBox(mouseLocation); //Se o elemento contém a posição atual do mouse.

                    if (contains && element.IsFocused)
                        element.RaiseMouseMove(parent, mouseLocation);

                    if (contains && !element.IsHovering)           //Se o elemento contém a posição atual do mouse e NÃO estava em hover.
                    {
                        element.RaiseMouseEnter();
                        parent.Invalidate();                       //Atualiza o visual a partir do CustomControl parent.
                    }
                    else if (!contains && element.IsHovering)      //Se o elemento NÃO contém a posição atual do mouse e estava em hover.
                    {
                        element.RaiseMouseLeave();                  
                        parent.Invalidate();                       //Atualiza o visual a partir do CustomControl parent.
                    }
                }
            }
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnGotFocus"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/>  
        /// e <see cref="InnerControl.HitBox"/> ->
        /// Executa <see cref="InnerControl.RaiseGotFocus"/>.
        /// </summary>
        public void HandleGotFocus(CustomControl parent, Point focusLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(focusLocation))
                {
                    //element.RaiseGotFocus(parent); FocusedInnerControl = element;
                    parent.Invalidate();
                }
            }
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnLostFocus"/> ->
        /// Se <see cref="FocusedInnerControl"/> existir ->
        /// Executa <see cref="InnerControl.RaiseLostFocus"/> ->
        /// Define <see cref="FocusedInnerControl"/> = null.
        /// </summary>
        public void HandleLostFocus(CustomControl parent)
        {
            if(FocusedInnerControl != null) 
            { FocusedInnerControl.RaiseLostFocus(parent); FocusedInnerControl = null;}   
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnKeyDown"/> ->
        /// Verifica se o <see cref="FocusedInnerControl"/> 
        /// é <see cref="IAcceptsKeyboard"/> : se for ->
        /// Executa <see cref="IAcceptsKeyboard.RaiseKeyDown"/>.
        /// </summary>
        public void HandleKeyDown(KeyEventArgs e)
        {
            if (FocusedInnerControl is IAcceptsKeyboard kb)
            {
                kb.RaiseKeyDown(e);
            }
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnKeyPress"/> ->
        /// Verifica se o <see cref="FocusedInnerControl"/> 
        /// é <see cref="IAcceptsKeyboard"/> : se for ->
        /// Executa <see cref="IAcceptsKeyboard.RaiseKeyPress"/>.
        /// </summary>
        public void HandleKeyPress(KeyPressEventArgs e)
        {
            if (FocusedInnerControl is IAcceptsKeyboard kb)
            {
                kb.RaiseKeyPress(e);
            }
        }

        
    } 
}
