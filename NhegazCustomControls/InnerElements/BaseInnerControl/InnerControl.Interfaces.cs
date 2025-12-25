using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    /// <summary>
    /// Define que esse <see cref="InnerControl"/> implementa um <see cref="TextFeature"/>.
    /// </summary>
    public interface IHasText
    {
       public TextFeature TextFeatures { get; }
    }
}
