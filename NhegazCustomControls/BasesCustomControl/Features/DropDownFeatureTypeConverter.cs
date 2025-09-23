using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    /// <summary>
    /// Conversor que controla dinamicamente quais propriedades aparecerão no PropertyGrid.
    /// Se DropDownFeature.HasAnyWithHeader == false, oculta as propriedades relacionadas ao cabeçalho.
    /// </summary>
    public sealed class DropDownFeatureTypeConverter : ExpandableObjectConverter
    {
        // Lista de nomes de propriedades que só devem aparecer se houver header.
        // Ajuste os nomes conforme os membros reais da sua classe.
        private static readonly HashSet<string> _headerProps = new(StringComparer.Ordinal)
        {
            //nameof(DropDownFeature.Header),
            nameof(DropDownFeature.HeaderForeColor),
            nameof(DropDownFeature.HeaderBackgroundColor),
        };

        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            // 1) Pega a lista "normal" de propriedades do ExpandableObjectConverter
            var all = base.GetProperties(context, value, attributes);

            // 2) Se não for a DropDownFeature, devolve tudo (fallback)
            if (value is not DropDownFeature feature)
                return all;

            bool showHeaderProps = feature.AnyIsHasHeader; // "chave" de exibição

            // 3) Filtra: se não há header, remove as propriedades relacionadas
            var filtered = new List<PropertyDescriptor>(all.Count);
            foreach (PropertyDescriptor pd in all)
            {
                // Se a prop é de header e não devemos mostrar, pula
                if (!showHeaderProps && _headerProps.Contains(pd.Name))
                    continue;

                // Mantém a propriedade
                filtered.Add(pd);
            }

            // 4) Retorna a coleção filtrada (o PropertyGrid usará só essas)
            return new PropertyDescriptorCollection(filtered.ToArray(), /*readOnly*/ true);
        }

        // Opcional: permite que o PropertyGrid ainda trate o objeto como "expandível"
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;
    }
}

