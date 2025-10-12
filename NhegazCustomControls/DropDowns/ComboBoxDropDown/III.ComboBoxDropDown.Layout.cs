using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls 
{ 
    public partial class ComboBoxDropDown
    {
        public override Size GetContentSize()
        {
            return new(0, 0);
            // throw new NotImplementedException();
        }
        public override Size GetPaddingSize()
        {
            return new(0, 0);
            //throw new NotImplementedException();
        }
        public override void UpdateLayout()
        {

            base.UpdateLayout();

            if (ParentControl.ItemList == null || ParentControl.ItemList.Count == 0) return;


            int height = Font.Height + ControlPadding.EffectiveInnerVertical / 2;
            int boundsHeight = Font.Height + ControlPadding.EffectiveBorderTop + ControlPadding.EffectiveInnerVertical;

            for (int col = 0; col < NumberOfColumns; col++)
            {
                var lbl = (InnerLabel)OptionsLabels.GetItem(col);
                lbl.Padding.Left = ControlPadding.BorderLeft;

                if (col == 0 || col == NumberOfColumns)
                    OptionsLabels.SetItemSize(col, Width, boundsHeight);
                else
                    OptionsLabels.SetItemSize(col, Width, height);

                OptionsLabels.SetItemLocation(col, BorderWidth, Height);


                Height += OptionsLabels.GetItem(col).Height;
            }
        }

        protected override void SetInnerSizes()
        {
        }
        protected override void SetInnerLocations()
        {
        }
    }
}
