using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    /// <summary>
    /// CustomControl básico que implementa apenas um InnerTextBox funcional.
    /// A intenção é: "1 controle = 1 campo de texto", sem Header/DropDown/Matrix.
    /// </summary>
    [DefaultEvent(nameof(TextChanged))]
    public class CustomTextBox : CustomControl
    {
        /// <summary>Instância única do InnerTextBox deste CustomControl.</summary>
        [Browsable(false)]
        public InnerTextBox InnerTextBox { get; }

        public CustomTextBox()
        {
            InnerTextBox = new InnerTextBox
            {
                BackgroundColor = BackgroundColor,
                ForeColor = ForeColor,
                Font = Font,

                HoverBackgroundColor = BackgroundColor,
                HoverForeColor = ForeColor,
            };

            // --- Encaminha eventos principais ---
            // Mantém o comportamento "padrão WinForms": TextChanged do CustomControl quando o texto interno muda.
            //InnerTextBox.KeyPress += (s, e) => TextChanged?.Invoke(this, EventArgs.Empty);

            InnerControls.Add(InnerTextBox);

            UpdateLayout();
        }

        // =============================================================================================================
        //  API pública (espelhada) - mínima e funcional
        // =============================================================================================================

        /// <summary>
        /// Texto do controle (espelha o texto do InnerTextBox).
        /// </summary>
        [Category("Aparência")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => InnerTextBox.Text;
            set { InnerTextBox.Text = value; Invalidate(); }
        }

        /// <summary>
        /// Fonte do controle (espelha a fonte do InnerTextBox).
        /// </summary>
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                InnerTextBox.Font = value;
                UpdateLayout();
            }
        }

        /// <summary>
        /// Cor do texto (espelha a cor do InnerTextBox).
        /// </summary>
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                InnerTextBox.ForeColor = value;
                InnerTextBox.HoverForeColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Cor de fundo (espelha a cor do InnerTextBox).
        /// </summary>
        public override Color BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
                InnerTextBox.BackgroundColor = value;
                InnerTextBox.HoverBackgroundColor = value;
                Invalidate();
            }
        }

        [Category("Texto")]
        public TextCharFilter TextCharFilter
        {
            get => InnerTextBox.TextCharFilter;
            set => InnerTextBox.TextCharFilter = value;
        }

        [Category("Texto")]
        public TextFormatFilter TextFormatFilter
        {
            get => InnerTextBox.TextFormatFilter;
            set => InnerTextBox.TextFormatFilter = value;
        }

        [Category("Texto")]
        public int MaxLength
        {
            get => InnerTextBox.MaxLength;
            set => InnerTextBox.MaxLength = value;
        }

        [Category("Texto")]
        public bool UseEllipsis
        {
            get => InnerTextBox.UseEllipsis;
            set { InnerTextBox.UseEllipsis = value; Invalidate(); }
        }

        [Category("Seleção")]
        public Color SelectionBackgroundColor
        {
            get => InnerTextBox.SelectionBackgroundColor;
            set { InnerTextBox.SelectionBackgroundColor = value; Invalidate(); }
        }

        [Category("Seleção")]
        public Color SelectionForeColor
        {
            get => InnerTextBox.SelectionForeColor;
            set { InnerTextBox.SelectionForeColor = value; Invalidate(); }
        }

        // =============================================================================================================
        //  Layout (implementação obrigatória do CustomControl)
        // =============================================================================================================

        /// <summary>
        /// Define o tamanho do InnerTextBox para ocupar a área de conteúdo do CustomControl.
        /// </summary>
        protected override void SetInnerSizes()
        {
            int contentWidth = ContentRightBound - ContentLeftBound;
            int contentHeight = NhegazSizeMethods.TextExactSize("00", Font).Height;

            // Height mínimo baseado na fonte (para não esmagar o texto em fontes grandes).
            InnerTextBox.SetSize(contentWidth, contentHeight);
        }

        /// <summary>
        /// Posiciona o InnerTextBox no topo/esquerda da área de conteúdo.
        /// </summary>
        protected override void SetInnerLocations()
        {
            InnerTextBox.Location = new Point(RelativeCenterX(InnerTextBox), RelativeCenterY(InnerTextBox));
        }

        /// <summary>
        /// Para um textbox simples: conteúdo = dimensão do InnerTextBox.
        /// </summary>
        public override Size GetContentSize()
        {
            return new Size(InnerTextBox.Width, InnerTextBox.Height);
        }

        /// <summary>
        /// Para um textbox simples: padding = soma de borda + padding do CustomControl.
        /// </summary>
        public override Size GetPaddingSize()
        {
            return new Size(BorderHorizontalBoundsSum, BorderVerticalBoundsSum);
        }
    }
}
