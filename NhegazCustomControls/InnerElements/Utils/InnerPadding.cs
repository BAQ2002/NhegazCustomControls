namespace NhegazCustomControls
{
    public class InnerControlPadding
    {
        private int left, top, right, bottom;

        private InnerControl Owner;

        /// <summary>
        /// A ação é atribuída automaticamente 
        /// no construtor de <see cref="InnerControl"/>
        /// para -> <see cref="InnerControl.UpdateLayout"/>.
        /// </summary>
        public Action? UpdateLayoutOwner { get; set; }

        public int Left
        {
            get => left;
            set
            {
                if (left != value)
                {
                    left = value;
                    UpdateLayoutOwner?.Invoke();
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
                    UpdateLayoutOwner?.Invoke();
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
                    UpdateLayoutOwner?.Invoke();
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
                    UpdateLayoutOwner?.Invoke();
                }
            }
        }

        /// <summary>
        /// Construtor secundário
        /// </summary>
        /// <param name="owner"></param>
        public InnerControlPadding(InnerControl owner) : this(owner, 0, 0, 0, 0) { }

        /// <summary>
        /// Construtor secundário -> passa um único valor para todos os Padding's.
        /// </summary>
        public InnerControlPadding(InnerControl owner, int all) : this(owner, all, all, all, all) { }

        /// <summary>
        /// Construtor secundário -> passa um valor especifico para cada Padding.
        /// </summary>
        public InnerControlPadding(InnerControl owner, int left, int top, int right, int bottom)
        {
            Owner = owner;
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        //public static implicit operator ControlPadding(ControlPadding ip) =>
        //new ControlPadding(ip.Left, ip.Top, ip.Right, ip.Bottom);

        //public static implicit operator ControlPadding(ControlPadding p) =>
        //new ControlPadding(p.Left, p.Top, p.Right, p.Bottom);
    }
}

