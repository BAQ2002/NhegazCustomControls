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
        private float relInnerH = 0.60f, relInnerV = 0.60f;
        private float relBorderLeft, relBorderTop, relBorderRight, relBorderBottom;

        private PaddingMode mode = PaddingMode.Absolute;
        private readonly CustomControl Owner;

        public event EventHandler? Changed;

        public CustomControlPadding(CustomControl owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Owner.FontChanged += (_, __) =>
            {
                if (Mode == PaddingMode.RelativeToFont)
                {
                    NotifyOwner(); // força re-layout; valores efetivos mudaram
                    TypeDescriptor.Refresh(this); // atualiza grid se aberto
                }
            };
        }

        // ============= MODO =============
        [Category("ControlPadding")]
        [RefreshProperties(RefreshProperties.All)]
        public PaddingMode Mode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                mode = value;
                NotifyOwner();           // layout passa a usar Effective*
                TypeDescriptor.Refresh(this); // grid reconsulta ReadOnly
            }
        }

        // ============= REL (%) =============
        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentInnerHorizontal
        {
            get => relInnerH;
            set { if (SetFloat(ref relInnerH, Clamp02(value))) { OnRelativeChanged(); } }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentInnerVertical
        {
            get => relInnerV;
            set { if (SetFloat(ref relInnerV, Clamp02(value))) { OnRelativeChanged(); } }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentBorderLeft
        {
            get => relBorderLeft;
            set { if (SetFloat(ref relBorderLeft, Clamp02(value))) { OnRelativeChanged(); } }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentBorderTop
        {
            get => relBorderTop;
            set { if (SetFloat(ref relBorderTop, Clamp02(value))) { OnRelativeChanged(); } }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentBorderRight
        {
            get => relBorderRight;
            set { if (SetFloat(ref relBorderRight, Clamp02(value))) { OnRelativeChanged(); } }
        }

        [Category("ControlPadding (Relative % )")]
        [RefreshProperties(RefreshProperties.All)]
        public float RelativePercentBorderBottom
        {
            get => relBorderBottom;
            set { if (SetFloat(ref relBorderBottom, Clamp02(value))) { OnRelativeChanged(); } }
        }

        private void OnRelativeChanged()
        {
            if (Mode == PaddingMode.RelativeToFont)
            {
                NotifyOwner();           // efetivos mudaram
                TypeDescriptor.Refresh(this);
            }
        }

        // ============= ABS (px) editáveis condicionalmente =============
        [Category("ControlPadding (Absolute px)")]
        public int InnerHorizontal
        {
            get => innerHorizontal;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return; // bloqueia edição
                if (innerHorizontal == value) return;
                innerHorizontal = value; NotifyOwner();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int InnerVertical
        {
            get => innerVertical;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return;
                if (innerVertical == value) return;
                innerVertical = value; NotifyOwner();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderLeft
        {
            get => borderLeft;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return;
                if (borderLeft == value) return;
                borderLeft = value; NotifyOwner();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderTop
        {
            get => borderTop;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return;
                if (borderTop == value) return;
                borderTop = value; NotifyOwner();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderRight
        {
            get => borderRight;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return;
                if (borderRight == value) return;
                borderRight = value; NotifyOwner();
            }
        }

        [Category("ControlPadding (Absolute px)")]
        public int BorderBottom
        {
            get => borderBottom;
            set
            {
                if (Mode == PaddingMode.RelativeToFont) return;
                if (borderBottom == value) return;
                borderBottom = value; NotifyOwner();
            }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("ControlPadding (Absolute px)")]
        [Description("Soma das bordas horizontais (esquerda + direita) em pixels.")]
        public int BorderHorizontalSum => BorderLeft + BorderRight;

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("ControlPadding (Absolute px)")]
        [Description("Soma das bordas verticais (topo + base) em pixels.")]
        public int BorderVerticalSum => BorderTop + BorderBottom;

        // Evita que o Designer serialize absolutos quando estiver relativo
        public bool ShouldSerializeInnerHorizontal() => Mode == PaddingMode.Absolute;
        public bool ShouldSerializeInnerVertical() => Mode == PaddingMode.Absolute;
        public bool ShouldSerializeBorderLeft() => Mode == PaddingMode.Absolute;
        public bool ShouldSerializeBorderTop() => Mode == PaddingMode.Absolute;
        public bool ShouldSerializeBorderRight() => Mode == PaddingMode.Absolute;
        public bool ShouldSerializeBorderBottom() => Mode == PaddingMode.Absolute;

        // ============= EFETIVOS (somente leitura) =============
        // Use estes no layout/desenho:
        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveInnerHorizontal => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Width * relInnerH)
            : innerHorizontal;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveInnerVertical => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Height * relInnerV)
            : innerVertical;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderLeft => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Width * relBorderLeft)
            : borderLeft;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderRight => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Width * relBorderRight)
            : borderRight;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderTop => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Height * relBorderTop)
            : borderTop;

        [Browsable(true), ReadOnly(true), Category("ControlPadding (Effective)")]
        public int EffectiveBorderBottom => (Mode == PaddingMode.RelativeToFont)
            ? (int)Math.Round(NhegazSizeMethods.FontUnitSize(Owner.Font).Height * relBorderBottom)
            : borderBottom;

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("ControlPadding (Effective)")]
        [Description("Soma efetiva das bordas horizontais (esquerda + direita) considerando o PaddingMode atual.")]
        public int EffectiveBorderHorizontalSum => EffectiveBorderLeft + EffectiveBorderRight;

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("ControlPadding (Effective)")]
        [Description("Soma efetiva das bordas verticais (topo + base) considerando o PaddingMode atual.")]
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
            Owner?.AdjustControlSize();
            Owner?.Invalidate();
        }

        public override string ToString()
            => $"Abs Inner(H:{innerHorizontal},V:{innerVertical}) " +
               $"Abs Border(L:{borderLeft},T:{borderTop},R:{borderRight},B:{borderBottom}) " +
               $"Mode:{Mode} | Rel Inner(H:{relInnerH},V:{relInnerV}) " +
               $"Rel Border(L:{relBorderLeft},T:{relBorderTop},R:{relBorderRight},B:{relBorderBottom})";
    }
}

