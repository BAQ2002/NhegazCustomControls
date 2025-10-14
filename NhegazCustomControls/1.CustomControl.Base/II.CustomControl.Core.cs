using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{
    public interface IHasHeader
    {
        HeaderFeature Header { get; set; }
    }
    public interface IHasDropDown
    {
        DropDownFeature DropDownFeatures { get; }
    }

    /// <summary>
    /// Define que esse CustomControl deve implementar uma MatrixFeature
    /// </summary>
    public interface IHasMatrix
    {
        MatrixFeature Matrix { get; }
    }

    /// <summary>
    /// Define que esse CustomControl deve implementar um VectorFeature
    /// </summary>
    public interface IHasVector
    {
        VectorFeature Vector { get; }     
    }
    public abstract partial class CustomControl : UserControl
    {              
         
        public CustomControl()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            InnerControls = new InnerControls(this);
            ControlPadding = new CustomControlPadding(this);
        }
        
        // ctor opcional: recebe um pai e copia estilo
        protected CustomControl(CustomControl parent) : this()
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            InnerControls = new InnerControls(this);
            ControlPadding = new CustomControlPadding(this);

            CopyVisualFrom(parent);
        }

        /// <summary>
        /// Copia propriedades visuais compartilhadas do controle <paramref name="parentControl"/>.
        /// Chame este método somente quando fizer sentido (ex.: drop-downs).
        /// </summary>
        protected virtual void CopyVisualFrom(CustomControl parentControl, bool? copyWidth = false, bool? copyHeight = false)
        {
            if (parentControl is null)
                throw new ArgumentNullException(nameof(parentControl));

            // --- Visuals "core" compartilhados ---
            BorderRadius = parentControl.BorderRadius;
            BorderWidth = parentControl.BorderWidth;

            BorderColor = parentControl.BorderColor;
            BackgroundColor = parentControl.BackgroundColor;

            HoverBackgroundColor = parentControl.HoverBackgroundColor;
            HoverForeColor = parentControl.HoverForeColor;

            ForeColor = parentControl.ForeColor;
            Font = parentControl.Font;

            ControlPadding.Mode = parentControl.ControlPadding.Mode;
            // Se quiser copiar percentuais:
            ControlPadding.RelativePercentInnerHorizontal = parentControl.ControlPadding.RelativePercentInnerHorizontal;
            ControlPadding.RelativePercentInnerVertical = parentControl.ControlPadding.RelativePercentInnerVertical;
            ControlPadding.RelativePercentBorderLeft = parentControl.ControlPadding.RelativePercentBorderLeft;
            ControlPadding.RelativePercentBorderTop = parentControl.ControlPadding.RelativePercentBorderTop;
            ControlPadding.RelativePercentBorderRight = parentControl.ControlPadding.RelativePercentBorderRight;
            ControlPadding.RelativePercentBorderBottom = parentControl.ControlPadding.RelativePercentBorderBottom;

            ControlPadding.BorderTop = parentControl.ControlPadding.BorderTop;
            ControlPadding.BorderLeft = parentControl.ControlPadding.BorderLeft;
            ControlPadding.BorderRight = parentControl.ControlPadding.BorderRight;
            ControlPadding.BorderBottom = parentControl.ControlPadding.BorderBottom;
            ControlPadding.InnerHorizontal = parentControl.ControlPadding.InnerHorizontal;
            ControlPadding.InnerVertical = parentControl.ControlPadding.InnerVertical;

            if (copyWidth == true)
                Width = parentControl.Width;
            if (copyHeight == true)
                Height = parentControl.Height;

            // Se o destino não implementa cabeçalho, não há o que fazer
            if (this is not IHasHeader destinationControl)
                return;

            // Cria/pega o mesmo header UMA vez e reutiliza
            var destinationHeader = destinationControl.Header ??= new HeaderFeature(this);

            // Copia de IHasHeader -> IHasHeader
            if (parentControl is IHasHeader sourceControl)
            {
                var sourceHeader = sourceControl.Header;
                destinationHeader.BackgroundColor = sourceHeader.BackgroundColor;
                destinationHeader.ForeColor = sourceHeader.ForeColor;
                destinationHeader.HeightMode = sourceHeader.HeightMode;
                destinationHeader.HeightRelativePercent = sourceHeader.HeightRelativePercent;
                destinationHeader.BorderRadius = sourceHeader.BorderRadius;
            }

            // Copia de IHasDropDown -> IHasHeader (cores “padrão” do header dos dropdowns)
            if (parentControl is IHasDropDown srcDrop &&
                srcDrop.DropDownFeatures.AnyIsHasHeader)
            {
                destinationHeader.BorderRadius = srcDrop.DropDownFeatures.HeaderBorderRadius;
                destinationHeader.BorderWidth = srcDrop.DropDownFeatures.HeaderBorderWidth;

                destinationHeader.BackgroundColor = srcDrop.DropDownFeatures.HeaderBackgroundColor;
                destinationHeader.ForeColor = srcDrop.DropDownFeatures.HeaderForeColor;

                destinationHeader.BorderColor = srcDrop.DropDownFeatures.HeaderBorderColor;
                destinationHeader.OnFocusBorderColor = srcDrop.DropDownFeatures.HeaderOnFocusBorderColor;

                destinationHeader.HoverBackgroundColor = srcDrop.DropDownFeatures.HeaderHoverBackgroundColor;
                destinationHeader.HoverForeColor = srcDrop.DropDownFeatures.HeaderHoverForeColor;
            }                                       
        }
  
        /// <summary>
        /// Override do evento de clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do clique.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            InnerControls.HandleClick(this, e.Location); 
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleClick(e.Location);
        }

        /// <summary>
        /// Override do evento de duplo clique. Encaminha o evento para os InnerControls.
        /// </summary>
        /// <param name="e">Argumentos do duplo clique.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            InnerControls.HandleDoubleClick(this, e.Location); 
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleDoubleClick(e.Location);
        }

        /// <summary>
        /// Override do evento de movimento do mouse. Propaga o evento para os InnerControls.
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
        /// Override do evento quando o controle ganha foco.
        /// </summary>
        /// <param name="e">Argumentos do foco.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (Focused) 
            {
                InnerControls.HandleGotFocus(this, PointToClient(Cursor.Position));
                var headerFeature = (this as IHasHeader)?.Header;
                headerFeature?.HandleGotFocus(PointToClient(Cursor.Position));

            }               
        }

        /// <summary>
        /// Override do evento quando o controle perde o foco.
        /// </summary>
        /// <param name="e">Argumentos do evento de perda de foco.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            InnerControls.HandleLostFocus(this, PointToClient(Cursor.Position));
            var headerFeature = (this as IHasHeader)?.Header;
            headerFeature?.HandleLostFocus(PointToClient(Cursor.Position));
        }
        
        
    }
}
