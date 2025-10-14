namespace NhegazCustomControls
{
    public partial class CustomComboBox
    {       
        private int GetItemsMaxTextPixelWidth()
        {
            if (ItemList == null || ItemList.Count == 0)
                return 0;

            int maxWidth = 0;
            foreach (string? @string in ItemList)
            {
                var text = @string ?? string.Empty;
                int width = NhegazSizeMethods.TextExactSize(text, Font).Width; // mede em px
                if (width > maxWidth) maxWidth = width;
            }

            return maxWidth;
        }
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
        protected override void SetInnerSizes()
        {
            selectIndex.SetSize(GetItemsMaxTextPixelWidth(), selectIndex.Height);
            dropDownIcon.SetSize(Font.Height, Font.Height);
        }

        protected override void SetInnerLocations()
        {
            selectIndex.SetLocation(RelativeLeftX(), RelativeCenterY(selectIndex));
            dropDownIcon.SetLocation(RelativeRightX(dropDownIcon), RelativeCenterY(dropDownIcon));
        }

        protected override void AdjustHoverColors()
        {
            dropDownIcon.MouseEnter += (s, e) =>
            {
                dropDownIcon.ForeColor = BackgroundColor;
                dropDownIcon.BackgroundColor = HoverColor;
            };

            dropDownIcon.MouseLeave += (s, e) =>
            {
                dropDownIcon.ForeColor = ForeColor;
                dropDownIcon.BackgroundColor = BackgroundColor;
            };
        }
    }
}