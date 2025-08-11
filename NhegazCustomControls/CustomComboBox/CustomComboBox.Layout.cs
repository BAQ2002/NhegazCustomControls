namespace NhegazCustomControls.PL.CustomControls
{
    public partial class CustomComboBox
    {
        protected override void AdjustInnerSizes()
        {
            dropDownIcon.BackGroundShape = BackGroundShape.SymmetricCircle;
            dropDownIcon.Height = Font.Height;
        }
        protected override void AdjustInnerLocations()
        {
            int selectIndexX = BorderWidth + HorizontalPadding;
            int selectIndexY = (Height - selectIndex.Height) / 2;
            selectIndex.Location = new Point(selectIndexX, selectIndexY);

            int dropDownIconX = Width - (dropDownIcon.Width + HorizontalPadding + BorderWidth);
            int dropDownIconY = (Height - dropDownIcon.Height) / 2;
            dropDownIcon.Location = new Point(dropDownIconX, dropDownIconY);
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