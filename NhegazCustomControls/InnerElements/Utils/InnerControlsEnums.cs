using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    /// <summary>
    /// Define qual o formato de InnerControl
    /// </summary>
    public enum BackGroundShape
    {
        SymmetricCircle,
        FitRectangle,
        RoundedRectangle
    }

    /// <summary>
    /// Define em qual posicao horizontal o texto sera alinhado 
    /// </summary>
    public enum TextHorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Define em qual posicao vertical o texto sera alinhado.
    /// </summary>  
    public enum TextVerticalAlignment
    {
        Top,
        Center,
        Bottom,
    }

    /// <summary>
    /// Define qual tipo de padding sera adicionado em relacao a posicao horizontal do texto.
    /// </summary>    
    public enum HorizontalPaddingMode
    {
        None,
        HalfFontWidth,
        OneFourthFontWidth,
        Absolute
    }

    /// <summary>
    /// Define qual tipo de padding sera adicionado em relacao a posicao vertical do texto.
    /// </summary>
    public enum VerticalPaddingMode
    {
        None,
        HalfFontHeight,
        OneFourthFontHeight,
        Absolute
    }
}
