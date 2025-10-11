using System;
using System.ComponentModel;
using System.Linq;

namespace NhegazCustomControls
{
    // Converter que permite dinâmica de ReadOnly por propriedade
    public class CustomControlPaddingTypeConverter : ExpandableObjectConverter
    {
        private static readonly string[] AbsoluteProps =
        {
            nameof(CustomControlPadding.InnerHorizontal),
            nameof(CustomControlPadding.InnerVertical),
            nameof(CustomControlPadding.BorderLeft),
            nameof(CustomControlPadding.BorderTop),
            nameof(CustomControlPadding.BorderRight),
            nameof(CustomControlPadding.BorderBottom),
        };

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) => true;

        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            var pdc = TypeDescriptor.GetProperties(value, attributes);
            if (value is not CustomControlPadding padding) return pdc;

            bool lockAbsolutes = padding.Mode == PaddingMode.RelativeToFont;

            var list = pdc.Cast<PropertyDescriptor>()
                          .Select(pd =>
                          {
                              if (AbsoluteProps.Contains(pd.Name))
                              {
                                  return new ReadOnlySwitchingPropertyDescriptor(pd, lockAbsolutes);
                              }
                              return pd;
                          })
                          .ToArray();

            return new PropertyDescriptorCollection(list, readOnly: true);
        }

        private sealed class ReadOnlySwitchingPropertyDescriptor : PropertyDescriptor
        {
            private readonly PropertyDescriptor inner;
            private readonly bool forceReadOnly;

            public ReadOnlySwitchingPropertyDescriptor(PropertyDescriptor inner, bool forceReadOnly)
                : base(inner)
            {
                this.inner = inner;
                this.forceReadOnly = forceReadOnly;
            }

            public override bool IsReadOnly => forceReadOnly || inner.IsReadOnly;

            public override Type ComponentType => inner.ComponentType;
            public override Type PropertyType => inner.PropertyType;
            public override bool CanResetValue(object component) => inner.CanResetValue(component);
            public override object GetValue(object component) => inner.GetValue(component);
            public override void ResetValue(object component) => inner.ResetValue(component);

            public override void SetValue(object component, object value)
            {
                if (IsReadOnly) return; // bloqueia edição no designer
                inner.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component) => inner.ShouldSerializeValue(component);

            public override string Category => inner.Category;
            public override string Description => inner.Description;
            public override string DisplayName => inner.DisplayName;
            public override AttributeCollection Attributes => inner.Attributes;
        }
    }
}
