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
        public TextBox selectedDay = new TextBox();   //Opção atualmente selecionada Dia
        public TextBox selectedMonth = new TextBox(); //Opção atualmente selecionada mes
        public TextBox selectedYear = new TextBox();  //Opção atualmente selecionada ano

        public InnerButton dayDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.SymmetricCircle);   //Botão para abrir DropDown
        public InnerButton monthDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.SymmetricCircle); //Botão para abrir DropDown
        public InnerButton yearDropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.SymmetricCircle);  //Botão para abrir DropDown

        private InnerLabel daySlashMonth = new InnerLabel();  //Elemento visual barra "/"
        private InnerLabel monthSlashYear = new InnerLabel(); //Elemento visual barra "/"
        private CustomControl dropDownInstance = null; //Referencia para o o DropDown que esta aberto

        private int dropDownsHeaderBorderRadius = 1;
        private int dropDownsHeaderHight = 1;

        private Color dropDownsHeaderColor = SystemColors.Highlight;

        private HeaderHeightMode dropDownsHeaderHeightMode = HeaderHeightMode.RelativeToFont;
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                selectedDay.Font = value; selectedMonth.Font = value; selectedYear.Font = value;
                dayDropDownIcon.Font = value; monthDropDownIcon.Font = value; yearDropDownIcon.Font = value;
                daySlashMonth.Font = value; monthSlashYear.Font = value;
                AdjustControlSize();
            }
        }

        public override Color BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
                selectedDay.BackColor = value; selectedMonth.BackColor = value; selectedYear.BackColor = value;
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
                selectedDay.ForeColor = value; selectedMonth.ForeColor = value; selectedYear.ForeColor = value;
                dayDropDownIcon.ForeColor = value; monthDropDownIcon.ForeColor = value; yearDropDownIcon.ForeColor = value;
                daySlashMonth.ForeColor = value; monthSlashYear.ForeColor = value;
                Invalidate();
            }
        }

        [Category("DropDowns")]
        public virtual Color DropDownsHeaderColor
        {
            get => dropDownsHeaderColor;
            set { dropDownsHeaderColor = value; Invalidate(); }
        }

        [Category("DropDowns")]
        public int DropDownsHeaderBorderRadius
        {
            get => dropDownsHeaderBorderRadius;
            set { dropDownsHeaderBorderRadius = value; Invalidate(); }
        }

        [Category("DropDowns")]
        public int DropDownsHeaderHight
        {
            get => dropDownsHeaderHight;
            set { dropDownsHeaderHight = value; Invalidate(); }
        }

        [Category("DropDowns")]
        public HeaderHeightMode DropDownsHeaderHeightMode
        {
            get => dropDownsHeaderHeightMode;
            set { dropDownsHeaderHeightMode = value; Invalidate(); }
        }
    }
}
