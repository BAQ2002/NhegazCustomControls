// I.CustomDatePicker.Properties.cs  (substitua o conteúdo dos campos e pontos indicados)
using System;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NhegazCustomControls
{
    public partial class CustomDatePicker : CustomControl
    {
        private DateOnly? date;

        public InnerTextBox dayTextBox = new InnerTextBox();
        public InnerTextBox monthTextBox = new InnerTextBox();
        public InnerTextBox yearTextBox = new InnerTextBox();

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
                date = value; SyncTextsFromProperties(); Invalidate(); //Sincroniza os InnerTextBox's com a data atualizada
                                                                       // e atualiza o visual do Controle.
            }
        }

        /// <summary>
        /// get -> Retorna o <see cref="DateOnly.Day"/> de <see cref="Date"/>.
        /// <para>
        /// set -> Invocado apenas por <see cref="DropDownDay.OnDayItemLabelClick"/>
        /// e <see cref="SyncPropertiesFromTexts"/> :<para>
        /// apenas modifica o <see cref="Date"/> se
        /// <see cref="Date"/>.Day for diferente do valor passado.</para>
        /// Realiza a limitação dos valores da data considerando 
        /// os limites de calendário.
        /// </para>
        /// </summary>
        [Browsable(false)]
        public int Day
        {
            get => Date.Day;
            set
            {
                if (Date.Day != value)
                {
                    var year = Date.Year;                                                      //Não modifica o valor atual do ano.
                    var month = Date.Month;                                                    //Não modifica o valor atual do mês.     
                    var day = Math.Max(1, Math.Min(DateTime.DaysInMonth(year, month), value)); //Limita o novo valor entre 1 e DateTime.DaysInMonth.

                    Date = new DateOnly(year, month, day);
                }
            }
        }

        /// <summary>
        /// get -> Retorna o <see cref="DateOnly.Month"/> de <see cref="Date"/>.
        /// <para>
        /// set -> Invocado apenas por <see cref="DropDownDay.OnDayItemLabelClick"/>
        /// e <see cref="SyncPropertiesFromTexts"/> :<para>
        /// apenas modifica o <see cref="Date"/> se
        /// <see cref="Date"/>.Month for diferente do valor passado.</para>
        /// Realiza a limitação dos valores da data considerando 
        /// os limites de calendário.
        /// </para>
        /// </summary>
        [Browsable(false)]
        public int Month
        {
            get => Date.Month;
            set
            {
                if (Date.Month != value)
                {
                    var year = Date.Year;                                            //Não modifica o valor atual do ano.   
                    var month = Math.Max(1, Math.Min(12, value));                    //Limita o novo valor entre 1 e 12.
                    var day = Math.Min(Date.Day, DateTime.DaysInMonth(year, month)); //Define o valor do dia para o menor valor entre
                                                                                     //o valor atual e DateTime.DaysInMonth.
                    Date = new DateOnly(year, month, day);
                }
            }
        }

        /// <summary>
        /// get -> Retorna o <see cref="DateOnly.Year"/> de <see cref="Date"/>.
        /// <para>
        /// set -> Invocado apenas por <see cref="DropDownDay.OnDayItemLabelClick"/>
        /// e <see cref="SyncPropertiesFromTexts"/> :<para>
        /// apenas modifica o <see cref="Date"/> se
        /// <see cref="Date"/>.Year for diferente do valor passado.</para>
        /// Realiza a limitação dos valores da data considerando 
        /// os limites de calendário.
        /// </para>
        /// </summary>
        [Browsable(false)]
        public int Year
        {
            get => Date.Year;
            set
            {
                if (Date.Year != value)
                {
                    var year = Math.Max(DateOnly.MinValue.Year, Math.Min(DateOnly.MaxValue.Year, value)); //Limita o novo valor de ano entre DateOnly.MinValue e DateOnly.MaxValue.
                    var month = Date.Month;                                                               //Mês atual.
                    var day = Math.Min(Date.Day, DateTime.DaysInMonth(year, month));                      //Menor valor entre o dia atual de Date e o
                                                                                                          //dia máximo do mês atual do novo ano selecionado.
                    Date = new DateOnly(year, month, day);
                }
            }
        }

        
    }
}
