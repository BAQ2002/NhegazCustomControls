using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
        private DateOnly? date;

        public TextBox dayTextBox = new TextBox();   //Opção atualmente selecionada Dia
        public TextBox monthTextBox = new TextBox(); //Opção atualmente selecionada mes
        public TextBox yearTextBox = new TextBox();  //Opção atualmente selecionada ano

        public InnerButton dayDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle);   //Botão para abrir DropDown
        public InnerButton monthDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle); //Botão para abrir DropDown
        public InnerButton yearDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.RoundedRectangle);  //Botão para abrir DropDown

        private InnerLabel daySlashMonth = new InnerLabel();  //Elemento visual barra "/"
        private InnerLabel monthSlashYear = new InnerLabel(); //Elemento visual barra "/"
        private CustomControl dropDownInstance = null; //Referencia para o o DropDown que esta aberto

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
                dayTextBox.BackColor = value; monthTextBox.BackColor = value; yearTextBox.BackColor = value;
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
                // Limites defensivos, ajuste se quiser aceitar qualquer ano válido do DateOnly
                var year = Math.Max(DateOnly.MinValue.Year, Math.Min(DateOnly.MaxValue.Year, value));
                var month = Date.Month;
                var day = Math.Min(Date.Day, DateTime.DaysInMonth(year, month));

                Date = new DateOnly(year, month, day);
                SyncTextsFromProperties();
                Invalidate();
            }
        }
        /// <summary>
        /// Sincroniza os TextBox (dayTextBox/Month/Year) a partir das propriedades.
        /// </summary>
        private void SyncTextsFromProperties()
        {
            // D2 para dia/mês, D4 para ano
            dayTextBox.Text = Day.ToString("D2");
            monthTextBox.Text = Month.ToString("D2");
            yearTextBox.Text = Year.ToString("D4");
        }

    }
}
