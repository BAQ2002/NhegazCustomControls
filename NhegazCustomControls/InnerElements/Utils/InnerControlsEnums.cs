using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    /// <summary>Define qual o formato do InnerControl.</summary>
    public enum BackGroundShape
    {
        SymmetricCircle,
        FitRectangle,
        RoundedRectangle
    }

    /// <summary>Define qual posição horizontal o texto deve usar como âncora para ser alinhado.</summary> 
    public enum TextHorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    /// <summary>Define qual posição vertical o texto deve usar como âncora para ser alinhado.</summary>  
    public enum TextVerticalAlignment
    {
        Top,
        Center,
        Bottom,
    }

    /// <summary>Define qual a escala de padding é adicionada em relação a posição horizontal do texto.</summary>    
    public enum HorizontalPaddingMode
    {
        None,
        HalfFontWidth,
        OneFourthFontWidth,
        Absolute
    }

    /// <summary>Define qual a escala de padding é adicionada em relação a posição vertical do texto.</summary>
    public enum VerticalPaddingMode
    {
        None,
        HalfFontHeight,
        OneFourthFontHeight,
        Absolute
    }

    /// <summary>Define qual o ícone é usado pelo InnerButton.</summary>
    public enum ButtonIcon
    {
        None,
        DropDown,
        Forward,
        Backward,
        Add,
        Edit,
        Delete
    }

    /// <summary>Define qual a escala do tamanho do ícone é usada pelo InnerButton.</summary>
    public enum IconSizeMode
    {
        Absolute,
        RelativeToFont
    }

    [Flags]
    public enum ControlAdjustments
    {
        None,
        Padding,
        InnerLocation,
        Size,
        All = Padding | InnerLocation | Size
    }

}
