namespace NhegazCustomControls
{
    public class CustomControlPadding
    {
        private int innerHorizontal, innerVertical, borderLeft, borderTop, borderRight, borderBottom;

        private CustomControl Owner;
        public int BorderLeft
        {
            get => borderLeft;
            set
            {
                if (borderLeft != value)
                {
                    borderLeft = value;
                    Owner.Update();
                }
            }
        }

        public int BorderTop
        {
            get => borderTop;
            set
            {
                if (borderTop != value)
                {
                    borderTop = value;
                    Owner.Update();
                }
            }
        }

        public int BorderRight
        {
            get => borderRight;
            set
            {
                if (borderRight != value)
                {
                    borderRight = value;
                    Owner.Update();
                }
            }
        }

        public int BorderBottom
        {
            get => borderBottom;
            set
            {
                if (borderBottom != value)
                {
                    borderBottom = value;
                    Owner.Update();
                }
            }
        }
        public int BorderHorizontal
        {
            get => borderBottom;
            set
            {
                if (borderBottom != value)
                {
                    borderLeft = value; borderRight = value;
                    Owner.Update();
                }
            }
        }
        public int InnerHorizontal
        {
            get => innerHorizontal;
            set
            {
                if (innerHorizontal != value)
                {
                    innerHorizontal = value;
                    Owner.Update();
                }
            }
        }
        public int InnerVertical
        {
            get => innerVertical;
            set
            {
                if (innerVertical != value)
                {
                    innerVertical = value;
                    Owner.Update();
                }
            }
        }
        public CustomControlPadding(CustomControl owner) : this(owner, 0, 0, 0, 0) { }
        public CustomControlPadding(CustomControl owner, int all) : this(owner, all, all, all, all) { }
        public CustomControlPadding(CustomControl owner, int left, int top, int right, int bottom)
        {
            Owner = owner;

            BorderLeft = left;
            BorderTop = top;
            BorderRight = right;
            BorderBottom = bottom;
        }

        //public static implicit operator Padding(Padding ip) =>
        //new Padding(ip.Left, ip.Top, ip.Right, ip.Bottom);

        //public static implicit operator Padding(Padding p) =>
        //new Padding(p.Left, p.Top, p.Right, p.Bottom);
    }
}

