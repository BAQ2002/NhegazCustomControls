using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class ComboBoxDropDown : CustomControl, IHasVector
    {
        private CustomComboBox ParentControl;
        VectorFeature IHasVector.Vector => OptionsLabels;
        public VectorFeature OptionsLabels { get; private set; }
        private int NumberOfColumns;
        public ComboBoxDropDown(CustomComboBox parent) : base(parent)
        {
            ParentControl = parent;

            NumberOfColumns = parent.ItemList.Count;

            OptionsLabels ??= new VectorFeature(this, NumberOfColumns);

            CreateOptionsLabels();
            UpdateLayout();
        }

        private void CreateOptionsLabels()
        {
            if (ParentControl.ItemList == null || ParentControl.ItemList.Count == 0) return;

            for (int col = 0; col < NumberOfColumns; col++) //Cria uma Label para cada item do ItemList
            {
                string labelText = ParentControl.ItemList[col];
                int currentCol = col;

                InnerLabel InnerLabel = new InnerLabel
                {
                    Text = labelText,
                    Font = Font,
                    ForeColor = ForeColor,
                    BackgroundColor = BackgroundColor,
                    TextVerticalAlignment = TextVerticalAlignment.Center,
                    HorizontalPaddingMode = HorizontalPaddingMode.Absolute,
                };


                InnerLabel.Click += (s, e) => OnLabelClick(InnerLabel.Text);

                OptionsLabels.AddItem(InnerLabel, col);
            }

        }
        //Método para ajuste automatizado do tamanho do elemento
        

        //Método para: ao clicar em uma das Opções, transfere a opção selecionada para o ComboBox
        private void OnLabelClick(string labelText)
        {

            if (ParentControl is CustomComboBox cb && cb != null)
            {
                cb.selectIndex.Text = labelText;
            }

            Parent?.Controls.Remove(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

        }
    }
}
