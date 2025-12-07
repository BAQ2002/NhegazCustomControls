using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace NhegazCustomControls
{

    /// <summary>
    /// Define que esse <see cref="CustomControl"/> implementa um <see cref="HeaderFeature"/>.
    /// </summary>
    public interface IHasHeader
    {
        HeaderFeature Header { get; set; }
    }

    /// <summary>
    /// Define que esse <see cref="CustomControl"/> implementa um <see cref="DropDownFeature"/>.
    /// </summary>
    public interface IHasDropDown
    {
        DropDownFeature DropDownFeatures { get; }
    }

    /// <summary>
    /// Define que esse <see cref="CustomControl"/> implementa um <see cref="MatrixFeature"/>.
    /// </summary>
    public interface IHasMatrix
    {
        MatrixFeature Matrix { get; }
    }

    /// <summary>
    /// Define que esse <see cref="CustomControl"/> implementa um <see cref="VectorFeature"/>.
    /// </summary>
    public interface IHasVector
    {
        VectorFeature Vector { get; }     
    }
    public abstract partial class CustomControl : UserControl
    {              
         
        public CustomControl()
        {
            SetStyle(ControlStyles.Selectable, true); TabStop = true; //Torna o Controle selecionável.
            DoubleBuffered = true; BackColor = Color.Transparent;     //Ajuste visual necessário.
            InnerControls = new InnerControlsCollection(this);        //Coleção de InnerControlsCollection.
            ControlPadding = new CustomControlPadding(this);          //Propriedades de Padding.
        }

        /// <summary>
        /// Construtor opcional que pode receber um
        /// <see cref="CustomControl"/> como "pai"
        /// para executar <see cref="CopyVisualFrom"/>.
        /// </summary>
        /// <param name="parent">CustomControl passado como pai.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CustomControl(CustomControl parent) : this()
        {
            if (parent is null)                                  //Execeção se:
                throw new ArgumentNullException(nameof(parent)); //parent for nulo.

            DoubleBuffered = true; BackColor = Color.Transparent; //Ajuste visual necessário.
            InnerControls = new InnerControlsCollection(this);    //Coleção de InnerControlsCollection.
            ControlPadding = new CustomControlPadding(this);      //Propriedades de Padding.

            CopyVisualFrom(parent);                               //Copia os visuais do parent.
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

            //Se paretControl tiver DropDowns que possuem Header 
            if (parentControl is IHasDropDown sourceHasDropDown &&
                sourceHasDropDown.DropDownFeatures.AnyIsHasHeader)
            {
                destinationHeader.BorderRadius = sourceHasDropDown.DropDownFeatures.HeaderBorderRadius;
                destinationHeader.BorderWidth = sourceHasDropDown.DropDownFeatures.HeaderBorderWidth;

                destinationHeader.BackgroundColor = sourceHasDropDown.DropDownFeatures.HeaderBackgroundColor;
                destinationHeader.ForeColor = sourceHasDropDown.DropDownFeatures.HeaderForeColor;

                destinationHeader.BorderColor = sourceHasDropDown.DropDownFeatures.HeaderBorderColor;
                destinationHeader.OnFocusBorderColor = sourceHasDropDown.DropDownFeatures.HeaderOnFocusBorderColor;

                destinationHeader.HoverBackgroundColor = sourceHasDropDown.DropDownFeatures.HeaderHoverBackgroundColor;
                destinationHeader.HoverForeColor = sourceHasDropDown.DropDownFeatures.HeaderHoverForeColor;
            }                                       
        }

        
    }
}
