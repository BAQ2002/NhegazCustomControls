namespace NhegazCustomControls
{
    public class InnerControlPadding
    {
        private int left, top, right, bottom;

        private InnerControl Owner;
        public int Left
        {
            get => left;
            set
            {
                if (left != value)
                {
                    left = value;
                    Owner.Update();
                }
            }
        }

        public int Top
        {
            get => top;
            set
            {
                if (top != value)
                {
                    top = value;
                    Owner.Update();
                }
            }
        }

        public int Right
        {
            get => right;
            set
            {
                if (right != value)
                {
                    right = value;
                    Owner.Update();
                }
            }
        }

        public int Bottom
        {
            get => bottom;
            set
            {
                if (bottom != value)
                {
                    bottom = value;
                    Owner.Update();
                }
            }
        }
        public InnerControlPadding(InnerControl owner) : this(owner, 0, 0, 0, 0) { }
        public InnerControlPadding(InnerControl owner, int all) : this(owner, all, all, all, all) { }
        public InnerControlPadding(InnerControl owner, int left, int top, int right, int bottom)
        {
            Owner = owner;
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        //public static implicit operator Padding(Padding ip) =>
        //new Padding(ip.Left, ip.Top, ip.Right, ip.Bottom);

        //public static implicit operator Padding(Padding p) =>
        //new Padding(p.Left, p.Top, p.Right, p.Bottom);
    }
}

