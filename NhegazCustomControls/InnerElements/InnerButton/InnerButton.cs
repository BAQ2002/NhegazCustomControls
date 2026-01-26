using System.Drawing.Drawing2D;


namespace NhegazCustomControls
{
    
    public class InnerButton : InnerControl
    {
        /// <summary></summary>
        public float IconSizePercent { get; set; } = 1f;

        /// <summary></summary>
        public Size IconSize { get; set; } = new(10, 10);

        public Point IconLocation => new(RelativeCenterX(IconSize.Width), RelativeCenterY(IconSize.Height));

        /// <summary></summary>
        private IconSizeMode iconSizeMode = IconSizeMode.RelativeToFont;

        /// <summary></summary>
        public IconSizeMode IconSizeMode 
        {
            get => iconSizeMode;
            set
            {
                iconSizeMode = value;
                AdjustIconSize();
            }
        }
        protected override void UpdateLayout() 
        {
            base.UpdateLayout(); AdjustIconSize();
        }
        public ButtonIcon ButtonIcon { get; set; } = ButtonIcon.None;

        public InnerButton(ButtonIcon? icon = null, BackGroundShape? backGroundShape = null, IconSizeMode? iconSizeMode = null)
        {
            if (icon.HasValue)
                ButtonIcon = icon.Value;            

            if (backGroundShape.HasValue)
                BackGroundShape = backGroundShape.Value;

            if (iconSizeMode.HasValue)
                IconSizeMode = iconSizeMode.Value;
        }

        protected virtual void AdjustIconSize()
        {
            if (IconSizeMode == IconSizeMode.RelativeToFont)
            {
                Size fontSize = NhegazSizeMethods.FontUnitSize(Font);
                IconSize = new((int)(fontSize.Width * IconSizePercent), (int)(fontSize.Width * IconSizePercent));
            }
        }
        //protected override void UpdateLayout()
        //{
        //    if (ButtonIcon == ButtonIcon.None);
        //}

        private GraphicsPath? GetIconPath()
        {
            return ButtonIcon switch
            {
                ButtonIcon.UpArrow => NhegazDrawingMethods.UpArrowGPath(IconSize, IconLocation.X, IconLocation.Y),
                ButtonIcon.DownArrow => NhegazDrawingMethods.DownArrowGPath(IconSize, IconLocation.X, IconLocation.Y),
                ButtonIcon.RightArrow => NhegazDrawingMethods.RightArrowGPath(IconSize, IconLocation.X, IconLocation.Y),
                ButtonIcon.LeftArrow => NhegazDrawingMethods.LeftArrowGPath(IconSize, IconLocation.X, IconLocation.Y),
                ButtonIcon.Add => NhegazDrawingMethods.AddIconPath(IconSize, IconLocation.X, IconLocation.Y),
                ButtonIcon.Delete => NhegazDrawingMethods.AddIconPath(IconSize, IconLocation.X, IconLocation.Y),
                _ => null
            };
        }
        public void DrawIcon(PaintEventArgs e)
        {
            Color iconColor = IsHovering ? HoverForeColor : ForeColor;

            using var iconPath = GetIconPath();
            if (iconPath == null) return;

            using (SolidBrush brush = new SolidBrush(iconColor))
            {
                e.Graphics.FillPath(brush, iconPath);

                using (Pen pen = new Pen(iconColor, 1f))
                {e.Graphics.DrawPath(pen, iconPath);}
            }

        }

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); DrawIcon(e);
        }
    }
}
