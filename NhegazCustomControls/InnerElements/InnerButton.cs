using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace NhegazCustomControls
{
    public enum ButtonIcon
    {
        None,
        DropDown,
        Forward,
        Backward,
        Add,
        Edit,
        Delete
    }
    public enum IconSizeMode
    {
        Absolute,
        RelativeToFont
    }
    public class InnerButton : InnerControl
    {
        public float IconSizePercent { get; set; } = 0.4f;
        public int IconSize { get; set; } = 10;

        private IconSizeMode iconSizeMode = IconSizeMode.RelativeToFont;
        public IconSizeMode IconSizeMode 
        {
            get => iconSizeMode;
            set
            {
                iconSizeMode = value;
                AdjustIconSize();
            }
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
                int fontHeight = NhegazSizeMethods.FontUnitSize(Font).Height;
                IconSize = (int)(fontHeight * IconSizePercent);
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
                ButtonIcon.DropDown => NhegazDrawingMethods.DropDownIconPath(this, IconSize),
                ButtonIcon.Forward => NhegazDrawingMethods.ForwardIconPath(this, IconSize),
                ButtonIcon.Backward => NhegazDrawingMethods.BackwardIconPath(this, IconSize),
                ButtonIcon.Add => NhegazDrawingMethods.AddIconPath(this, IconSize),
                ButtonIcon.Delete => NhegazDrawingMethods.AddIconPath(this, IconSize),
                _ => null
            };
        }
        
        public override void OnPaint(CustomControl parent, PaintEventArgs e)
        {
            base.OnPaint(parent, e);

            Color iconColor = IsHovering ? HoverForeColor : ForeColor;

            using var iconPath = GetIconPath();
            if (iconPath == null) return;

            using (SolidBrush brush = new SolidBrush(iconColor))
                e.Graphics.FillPath(brush, iconPath);

            using (Pen pen = new Pen(iconColor, 1f))
                e.Graphics.DrawPath(pen, iconPath);
        }
    }
}
