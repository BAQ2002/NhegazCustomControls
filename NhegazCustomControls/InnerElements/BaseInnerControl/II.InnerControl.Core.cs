using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;

namespace NhegazCustomControls
{
    /// <summary>Define que o InnerControl aceita eventos de teclado do pai.</summary>
    public interface IAcceptsKeyboard
    {
        /// <summary>
        /// Implementação obrigatória de <see cref="IAcceptsKeyboard"/> : 
        /// Acionado em <see cref="InnerControlsCollection.HandleKeyPress"/> -> 
        /// Aciona <see cref="KeyPress"/>.
        /// </summary>
        public void RaiseKeyPress(KeyPressEventArgs e);

        /// <summary>
        /// Implementação obrigatória de <see cref="IAcceptsKeyboard"/> : 
        /// Acionado em <see cref="InnerControlsCollection.HandleKeyDown"/> -> 
        /// Aciona <see cref="KeyDown"/>.
        /// </summary>
        public void RaiseKeyDown(KeyEventArgs e);

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyPress;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? KeyDown;
    }

    public abstract partial class InnerControl
    {      
        public InnerControl()
        {
            Padding = new(this);
        }

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? Click;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? DoubleClick;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler<Point>? MouseDown;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler<Point>? MouseUp;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler<Point>? MouseMove;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? GotFocus;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? LostFocus;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? MouseEnter;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? MouseLeave;

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> -> 
        /// Aciona <see cref="Click"/>.
        /// </summary>
        public virtual void RaiseClick(object sender, Point clickLocation)
        {
            Click?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleDoubleClick"/> ->
        /// Aciona <see cref="DoubleClick"/>.
        /// </summary>
        public virtual void RaiseDoubleClick(object sender, Point clickLocation)
        {
            DoubleClick?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseDown"/> ->
        /// Aciona <see cref="MouseDown"/>.
        /// </summary>
        public virtual void RaiseMouseDown(object sender, Point p)
        {
            MouseDown?.Invoke(sender, p);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseUp"/> ->
        /// Aciona <see cref="MouseUp"/>.
        /// </summary>
        public virtual void RaiseMouseUp(object sender, Point p)
        {
            MouseUp?.Invoke(sender, p);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> ->
        /// Aciona <see cref="MouseMove"/>.
        /// </summary>
        public virtual void RaiseMouseMove(object sender, Point p)
        {
            MouseMove?.Invoke(sender, p);
        }
        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> 
        /// ou <see cref="InnerControlsCollection.HandleGotFocus"/> -> 
        /// Aciona <see cref="GotFocus"/>, <see cref="IsFocused"/> = true.
        /// </summary>
        public virtual void RaiseGotFocus(object sender)
        {
            GotFocus?.Invoke(sender, EventArgs.Empty); IsFocused = true;
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleClick"/> 
        /// ou <see cref="InnerControlsCollection.HandleLostFocus"/> -> 
        /// Aciona <see cref="LostFocus"/>, <see cref="IsFocused"/> = false.
        /// </summary>
        public virtual void RaiseLostFocus(object sender)
        {
            LostFocus?.Invoke(sender, EventArgs.Empty); IsFocused = false;
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="AbleToHover"/> e <see cref="isHovering"/> ->
        /// Aciona <see cref="MouseEnter"/> e torna <see cref="IsHovering"/> = true.
        /// </summary>
        public virtual void RaiseMouseEnter()
        {
            if (!AbleToHover || isHovering) return;

            isHovering = true;
            MouseEnter?.Invoke(this, EventArgs.Empty);         
        }

        /// <summary>
        /// Acionado em <see cref="InnerControlsCollection.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="AbleToHover"/> e <see cref="isHovering"/> ->
        /// Aciona <see cref="MouseLeave"/> e torna <see cref="IsHovering"/> = false.
        /// </summary>
        public virtual void RaiseMouseLeave()
        {
            if (!AbleToHover || !isHovering) return; 

            isHovering = false;
            MouseLeave?.Invoke(this, EventArgs.Empty);
            
        }
    }
}
