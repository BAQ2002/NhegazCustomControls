using NhegazCustomControls.PL.CustomControls;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomComboBox
    {
        private StringCollection itemList = new(); //Opções da combo Box     

        public InnerLabel selectIndex = new(); //Opção atualmente selecionada
        private InnerButton dropDownIcon = new(ButtonIcon.DropDown, BackGroundShape.SymmetricCircle); //Icone de visual

        private ComboBoxDropDown dropDownInstance = null;
        public string SelectIndexText
        {
            get => selectIndex.Text;
            set { selectIndex.Text = value; Invalidate(); }
        }
        public StringCollection ItemList
        {
            get => itemList;
            set { itemList = value; Invalidate(); }
        }
        public override Color BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                base.BackgroundColor = value;
                selectIndex.BackgroundColor = value; dropDownIcon.BackgroundColor = value; Invalidate();
            }
        }
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                selectIndex.ForeColor = value; dropDownIcon.ForeColor = value; Invalidate();
            }
        }
        public override Font Font
        {
            get => base.Font;
            set { base.Font = value; selectIndex.Font = value; dropDownIcon.Font = value; AdjustControlSize(); }
        }

    }
}
