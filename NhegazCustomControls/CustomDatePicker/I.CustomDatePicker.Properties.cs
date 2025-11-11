// I.CustomDatePicker.Properties.cs  (substitua o conteúdo dos campos e pontos indicados)
using System;
using System.ComponentModel;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
        private DateOnly? date;

        // SUBSTITUIR estes 3:
        // public TextBox dayTextBox = new TextBox();
        // public TextBox monthTextBox = new TextBox();
        // public TextBox yearTextBox = new TextBox();

        // POR estes 3:
        public InnerTextBox dayTextBox = new InnerTextBox();
        public InnerTextBox monthTextBox = new InnerTextBox();
        public InnerTextBox yearTextBox = new InnerTextBox();

        // Guarda quem está “ativo” para rotear teclado/caret
        private InnerTextBox? activeInnerTextBox = null;

        public InnerButton dayDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle);
        public InnerButton monthDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle);
        public InnerButton yearDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle);

        private InnerLabel daySlashMonth = new InnerLabel();
        private InnerLabel monthSlashYear = new InnerLabel();
        private CustomControl? dropDownInstance = null;

        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                dayTextBox.Font = value; monthTextBox.Font = value; yearTextBox.Font = value;
                dayDropDownIcon.Font = value; monthDropDownIcon.Font = value; yearDropDownIcon.Font = value;
                daySlashMonth.Font = value; monthSlashYear.Font = value;
                UpdateLayout();
            }
        }

        public override Color BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
                // trocou BackColor -> BackgroundColor
                dayTextBox.BackgroundColor = value; monthTextBox.BackgroundColor = value; yearTextBox.BackgroundColor = value;
                dayDropDownIcon.BackgroundColor = value; monthDropDownIcon.BackgroundColor = value; yearDropDownIcon.BackgroundColor = value;
                daySlashMonth.BackgroundColor = value; monthSlashYear.BackgroundColor = value;
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                dayTextBox.ForeColor = value; monthTextBox.ForeColor = value; yearTextBox.ForeColor = value;
                dayDropDownIcon.ForeColor = value; monthDropDownIcon.ForeColor = value; yearDropDownIcon.ForeColor = value;
                daySlashMonth.ForeColor = value; monthSlashYear.ForeColor = value;
                Invalidate();
            }
        }

        [Category("DropDowns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DropDownFeature DropDownFeatures { get; } = new();

        [Category("Behavior")]
        [Description("Data atual do DatePicker (apenas data, sem hora). Ideal para integração com colunas DATE de banco.")]
        public DateOnly Date
        {
            get => date ?? DateOnly.FromDateTime(DateTime.Today);
            set
            {
                date = value;
                SyncTextsFromProperties();
                Invalidate();
            }
        }

        [Browsable(false)]
        public int Day
        {
            get => Date.Day;
            set
            {
                var year = Date.Year;
                var month = Date.Month;
                var day = Math.Max(1, Math.Min(DateTime.DaysInMonth(year, month), value));
                Date = new DateOnly(year, month, day);
                SyncTextsFromProperties();
                Invalidate();
            }
        }

        [Browsable(false)]
        public int Month
        {
            get => Date.Month;
            set
            {
                var year = Date.Year;
                var month = Math.Max(1, Math.Min(12, value));
                var day = Math.Min(Date.Day, DateTime.DaysInMonth(year, month));
                Date = new DateOnly(year, month, day);
                SyncTextsFromProperties();
                Invalidate();
            }
        }

        [Browsable(false)]
        public int Year
        {
            get => Date.Year;
            set
            {
                var year = Math.Max(DateOnly.MinValue.Year, Math.Min(DateOnly.MaxValue.Year, value));
                var month = Date.Month;
                var day = Math.Min(Date.Day, DateTime.DaysInMonth(year, month));

                Date = new DateOnly(year, month, day);
                SyncTextsFromProperties();
                Invalidate();
            }
        }

        /// <summary>Sincroniza os InnerTextBox a partir das propriedades.</summary>
        private void SyncTextsFromProperties()
        {
            dayTextBox.Text = Day.ToString("D2");
            monthTextBox.Text = Month.ToString("D2");
            yearTextBox.Text = Year.ToString("D4");
        }
    }
}
