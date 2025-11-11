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

        /// <summary>Referência a um <see cref="InnerControl"/> se estiver em foco.</summary>
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
            if (innerControl is InnerTextBox)                      //Se for um InnerTextBox (ou outros que precisarem no futuro), 
                innerControl.InvalidateParent = Parent.Invalidate; //devido à funcionalidades: Torna capaz de atualizar o Parent.
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
        public bool HandleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseClick(parent, clickLocation);                        //Aciona o Click do elementoque o ponto de click pertence.

                    if (element != FocusedInnerControl)                               //Se o elemento não for o atual em foco.
                    {
                        FocusedInnerControl?.RaiseLostFocus(parent);                  //Se existir um elemento interno com foco: desfoca.
                        element.RaiseGotFocus(parent); FocusedInnerControl = element; //Atualiza para o elemento que o ponto de click pertence.
                    }                 
                    parent.Invalidate();                                              //Atualiza o visual a partir do CustomControl parent.
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnMouseDoubleClick"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/>  
        /// e <see cref="InnerControl.HitBox"/> ->
        /// Executa <see cref="InnerControl.RaiseDoubleClick"/>
        /// e <see cref="Control.Invalidate()"/>.
        /// </summary>
        public bool HandleDoubleClick(CustomControl parent, Point clickLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(clickLocation))
                {
                    element.RaiseDoubleClick(parent, clickLocation);

                    parent.Invalidate();
                    return true;
                }
            }
            return false;  
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnMouseMove"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/> e <see cref="InnerControl.HitBox"/> ->
        /// Verifica o estado de <see cref="InnerControl.IsHovering"/> ->
        /// Executa <see cref="InnerControl.RaiseMouseEnter"/>
        /// ou <see cref="InnerControl.RaiseMouseEnter"/>.
        /// </summary>
        public void HandleMouseMove(CustomControl parent, Point mouseLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible)
                {
                    bool contains = element.HitBox(mouseLocation);

                    if (contains && !element.IsHovering)
                    {
                        element.RaiseMouseEnter();
                        parent.Invalidate();  // força repaint do controle pai para refletir a mudança
                    }
                    else if (!contains && element.IsHovering)
                    {
                        element.RaiseMouseLeave();
                        parent.Invalidate();
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
        public bool HandleGotFocus(CustomControl parent, Point focusLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(focusLocation))
                {
                    element.RaiseGotFocus(parent); FocusedInnerControl = element;
                    parent.Invalidate();
                    return true;                    
                }
            }
            return false;
        }

        /// <summary>
        /// Acionado em <see cref="CustomControl.OnLostFocus"/> ->
        /// Verifica se há um <see cref="InnerControl"/> 
        /// com <see cref="InnerControl.Visible"/>  
        /// e <see cref="InnerControl.HitBox"/> ->
        /// Executa <see cref="InnerControl.RaiseLostFocus"/> ->
        /// se quem perdeu foco era o atual <see cref="FocusedInnerControl"/> ->
        /// Define <see cref="FocusedInnerControl"/> = null.
        /// </summary>
        public bool HandleLostFocus(CustomControl parent, Point focusLocation)
        {
            foreach (var element in elements)
            {
                if (element.Visible && element.HitBox(focusLocation))
                {
                    element.RaiseLostFocus(parent);                  
                    if (FocusedInnerControl == element) FocusedInnerControl = null; //se quem perdeu foco era o atual, limpa
                    return true;
                }
            }
            // se o pai perdeu foco por completo, zera
            FocusedInnerControl = null;
            return false;
        }

        /// <summary>Encaminha KeyPress ao Inner focado se ele aceitar teclado.</summary>
        public bool DispatchKeyPress(KeyPressEventArgs e)
        {
            if (FocusedInnerControl is IAcceptsKeyboard kb)
            {
                kb.OnParentKeyPress(e);
                return true;
            }
            return false;
        }

        /// <summary>Encaminha KeyDown ao Inner focado se ele aceitar teclado.</summary>
        public bool DispatchKeyDown(KeyEventArgs e)
        {
            if (FocusedInnerControl is IAcceptsKeyboard kb)
            {
                kb.OnParentKeyDown(e);
                return true;
            }
            return false;
        }
    } 
}
