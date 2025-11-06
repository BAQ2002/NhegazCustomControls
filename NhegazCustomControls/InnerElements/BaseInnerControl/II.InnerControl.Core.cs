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
        void OnParentKeyPress(KeyPressEventArgs e);
        void OnParentKeyDown(KeyEventArgs e);
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
        public event EventHandler? GotFocus;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? LostFocus;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? MouseEnter;

        /// <summary>Evento que pode invocar métodos e funções ao ser acionado.</summary> 
        public event EventHandler? MouseLeave;

        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleClick"/> -> 
        /// Aciona <see cref="Click"/>.
        /// </summary>
        public void RaiseClick(object sender)
        {
            Click?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleDoubleClick"/> ->
        /// Aciona <see cref="DoubleClick"/>.
        /// </summary>
        public void RaiseDoubleClick(object sender)
        {
            DoubleClick?.Invoke(sender, EventArgs.Empty);
        }
        
        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleGotFocus"/> -> 
        /// Aciona <see cref="GotFocus"/>.
        /// </summary>
        public void RaiseGotFocus(object sender)
        {
            GotFocus?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleLostFocus"/> -> 
        /// Aciona <see cref="LostFocus"/>.
        /// </summary>
        public void RaiseLostFocus(object sender)
        {
            LostFocus?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="AbleToHover"/> e <see cref="isHovering"/> ->
        /// Aciona <see cref="MouseEnter"/>.
        /// </summary>
        public virtual void RaiseMouseEnter()
        {
            if (!AbleToHover || isHovering) return;

            isHovering = true;
            MouseEnter?.Invoke(this, EventArgs.Empty);         
        }

        /// <summary>
        /// Acionado em <see cref="InnerControls.HandleMouseMove"/> -> 
        /// Verifica os estados de <see cref="AbleToHover"/> e <see cref="isHovering"/> ->
        /// Aciona <see cref="MouseLeave"/>.
        /// </summary>
        public virtual void RaiseMouseLeave()
        {
            if (!AbleToHover || !isHovering) return; 

            isHovering = false;
            MouseLeave?.Invoke(this, EventArgs.Empty);
            
        }

        /// <summary>Método responsável por acionar os ajustes de posições e tamanhos.</summary>
        protected virtual void AdjustControlSize()
        {
            if (BackGroundShape == BackGroundShape.SymmetricCircle)
            {
                SymmetricalCircleAdjust();
            }
        }

        /// <summary>
        /// Método responsável por realizar ajustes se <see cref="BackgroundShape"/> 
        /// = <see cref="SymmetricalCircle"/> ->
        /// Define a altura e largura sempre iguais à maior entre as duas.
        /// </summary>
        protected virtual void SymmetricalCircleAdjust()
        {
            if (Size.Width == Size.Height)
                return;

            int reference = Math.Max(Width, Height); Size = new Size(reference, reference);
        }

        public virtual void Update()
        {

        }

        public virtual void SetLocation(int x, int y)
        {
            Location = new Point(x, y);
        }
        public virtual void SetLocation(Point location)
        {
            Location = new Point(location.X, location.Y);
        }

        public virtual void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public virtual void SetSize(Size size)
        {
            Width = size.Width; 
            Height = size.Height;
        }      
    }
}
