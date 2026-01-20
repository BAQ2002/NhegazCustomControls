using System;
using System.ComponentModel;
using System.Drawing;
namespace NhegazCustomControls
{
    [TypeConverter(typeof(CustomControlPaddingTypeConverter))]
    public class CustomControlPadding
    {
        // --------- Absolutos armazenados ---------
        private int innerHorizontal = 1, innerVertical = 1;
        private int borderLeft, borderTop, borderRight, borderBottom;

        // --------- Relativos (percentuais) ---------
        private int relInnerH = 60,      relInnerV = 60;
        private int relBorderLeft = 60,  relBorderTop = 60;
        private int relBorderRight = 60, relBorderBottom = 60;

        private PaddingMode paddingMode = PaddingMode.RelativeToFont;
        private readonly CustomControl Owner;

        public event EventHandler? Changed;

        public CustomControlPadding(CustomControl owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Owner.FontChanged += (_, __) =>
            {
                if (PaddingMode == PaddingMode.RelativeToFont)
                {
                    NotifyOwner(); // força re-layout; valores efetivos mudaram
                    TypeDescriptor.Refresh(this); // atualiza grid se aberto
                }
            };
        }

        // ============= MODO =============
        [Category("ControlPadding")]
        [RefreshProperties(RefreshProperties.All)]
        public PaddingMode PaddingMode
        {
            get => paddingMode;
            set// => paddingMode = paddingMode != value?  value: ;
            {
                if (paddingMode == value) return;
                paddingMode = value;
                NotifyOwner();           // layout passa a usar Effective*
                TypeDescriptor.Refresh(this); // grid reconsulta ReadOnly
            }
        }

        // ============= REL (%) =============
        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public int RelativePercentInnerHorizontal
        {
            get => relInnerH;
            set
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relInnerH = relativePercent; OnRelativeChanged();
            }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public int RelativePercentInnerVertical
        {
            get => relInnerV;
            set
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relInnerV = relativePercent; OnRelativeChanged();
            }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public int RelativePercentBorderLeft
        {
            get => relBorderLeft;
            set
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relBorderLeft = relativePercent; OnRelativeChanged();
            }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public int RelativePercentBorderTop
        {
            get => relBorderTop;
            set
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relBorderTop = relativePercent; OnRelativeChanged();
            }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public int RelativePercentBorderRight
        {
            get => relBorderRight;
            set
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relBorderRight = relativePercent; OnRelativeChanged();
            }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(0)]
        public int RelativePercentBorderBottom
        {
            get => relBorderBottom;
            set 
            {
                int clampedValue = Nhegaz.MathMethods.Clamp(value, 0, 200);
                int relativePercent = Nhegaz.MathMethods.RoundToMOT(clampedValue);
                relBorderBottom = relativePercent; OnRelativeChanged();
            }
        }

        
        // ============= ABS (px) editáveis condicionalmente =============
        [Category("ControlPadding (Absolute px)")]
        public int InnerHorizontal
        {
            get => innerHorizontal;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return; // bloqueia edição
                if (innerHorizontal == value) return;
                innerHorizontal = value; NotifyOwner(); TypeDescriptor.Refresh(this);
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int InnerVertical
        {
            get => innerVertical;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return;
                if (innerVertical == value) return;
                innerVertical = value; NotifyOwner(); TypeDescriptor.Refresh(this);
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderLeft
        {
            get => borderLeft;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return;
                if (borderLeft == value) return;
                borderLeft = value;

                OnAbsoluteChanged();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderTop
        {
            get => borderTop;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return;
                if (borderTop == value) return;
                borderTop = value; NotifyOwner(); TypeDescriptor.Refresh(this);
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderRight
        {
            get => borderRight;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return;
                if (borderRight == value) return;
                borderRight = value; NotifyOwner(); TypeDescriptor.Refresh(this);
            }
        }
        
        [Category("ControlPadding (Absolute px)")]
        public int BorderBottom
        {
            get => borderBottom;
            set
            {
                if (PaddingMode == PaddingMode.RelativeToFont) return;
                if (borderBottom == value) return;
                borderBottom = value; NotifyOwner(); TypeDescriptor.Refresh(this);
            }
        }
        private void OnRelativeChanged()
        {
            if (PaddingMode == PaddingMode.RelativeToFont)
            {
                NotifyOwner();           // efetivos mudaram
                TypeDescriptor.Refresh(this);
            }
        }
        private void OnAbsoluteChanged()
        {
            if (PaddingMode == PaddingMode.Absolute)
            {
                NotifyOwner();           // efetivos mudaram
                TypeDescriptor.Refresh(this);
            }
        }

        //[Browsable(true)]
        //[ReadOnly(true)]
        //[Category("ControlPadding (Absolute px)")]
        //[Description("Soma das bordas horizontais (esquerda + direita) em pixels.")]
        //public int BorderHorizontalSum => BorderLeft + BorderRight;

        //[Browsable(true)]
        //[ReadOnly(true)]
        //[Category("ControlPadding (Absolute px)")]
        //[Description("Soma das bordas verticais (topo + base) em pixels.")]
        //public int BorderVerticalSum => BorderTop + BorderBottom;


        // ============= EFETIVOS (somente leitura) =============
        // Use estes no layout/desenho:
        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveInnerHorizontal => (PaddingMode == PaddingMode.RelativeToFont)?
            (Owner.FontUnitSize.Height * (int)(relInnerH / 100.0f)) : innerHorizontal;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveInnerVertical => (PaddingMode == PaddingMode.RelativeToFont)?
            (int)(Owner.FontUnitSize.Height * (relInnerV / 100.0f)) : innerVertical;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderLeft => (PaddingMode == PaddingMode.RelativeToFont)?
            (int)(Owner.FontUnitSize.Height * (relBorderLeft / 100.0f)) : borderLeft;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderRight => (PaddingMode == PaddingMode.RelativeToFont)?
            (int)(Owner.FontUnitSize.Height * (relBorderRight / 100.0f)) : borderRight;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderTop => (PaddingMode == PaddingMode.RelativeToFont)?
            (int)(Owner.FontUnitSize.Height * (relBorderTop / 100.0f)) : borderTop;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderBottom => (PaddingMode == PaddingMode.RelativeToFont)?
            (int)(Owner.FontUnitSize.Height * (relBorderBottom / 100.0f)) : borderBottom;

        /// <summary>
        /// Soma efetiva das bordas horizontais (esquerda + direita) considerando o PaddingMode atual.
        /// </summary>
        public int EffectiveBorderHorizontalSum => EffectiveBorderLeft + EffectiveBorderRight;

        /// <summary>
        /// Soma efetiva das bordas verticais (topo + base) considerando o PaddingMode atual.
        /// </summary>
        public int EffectiveBorderVerticalSum => EffectiveBorderTop + EffectiveBorderBottom;

        // ============= Utilidades/conveniências =============
        private static float Clamp02(float v) => Math.Max(0f, Math.Min(2f, v));
        private static bool SetFloat(ref float field, float v)
        {
            if (Math.Abs(field - v) < float.Epsilon) return false;
            field = v; return true;
        }

        private void NotifyOwner()
        {
            Changed?.Invoke(this, EventArgs.Empty);
            Owner?.UpdateLayout();
        }

        public override string ToString()
            => $"Abs Inner(H:{innerHorizontal},V:{innerVertical}) " +
               $"Abs Border(L:{borderLeft},T:{borderTop},R:{borderRight},B:{borderBottom}) " +
               $"PaddingMode:{PaddingMode} | Rel Inner(H:{relInnerH},V:{relInnerV}) " +
               $"Rel Border(L:{relBorderLeft},T:{relBorderTop},R:{relBorderRight},B:{relBorderBottom})";
    }
}

