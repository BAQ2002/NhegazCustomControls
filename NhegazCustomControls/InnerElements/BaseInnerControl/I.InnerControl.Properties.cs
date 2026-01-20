using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public abstract partial class InnerControl
    {
        private Rectangle bounds = new(0, 0, 0, 0);
        private BackGroundShape backGroundShape = BackGroundShape.FitRectangle;
        private int cornerRadius = 0; 
        private bool isHovering = false; public bool IsHovering => AbleToHover ? isHovering : false;
        private bool isFocused = false;     

        /// <summary>Define se o elemento é visível.</summary>
        public bool Visible { get; set; } = true;

        /// <summary>Define se o elemento pode ficar em destaque quando o mouse estiver por cima.</summary>
        public bool AbleToHover { get; set; } = true;

        /// <summary>Define qual a fonte utiliza no texto.</summary>
        public virtual Font Font { get; set; } = SystemFonts.DefaultFont;

        /// <summary>Define qual a cor do texto.</summary>
        public virtual Color ForeColor { get; set; } = SystemColors.ControlText;

        /// <summary>Define qual a cor do texto quando o mouse estiver por cima.</summary>
        public virtual Color HoverForeColor { get; set; } = SystemColors.Window;

        /// <summary>Define qual a cor padrão do fundo.</summary>
        public virtual Color BackgroundColor { get; set; } = SystemColors.Window;

        /// <summary>Define qual a cor do fundo quando o mouse estiver por cima.</summary>
        public virtual Color HoverBackgroundColor { get; set; } = SystemColors.Highlight;

        /// <summary>Instância das propriedades de padding do elemento.</summary>
        public InnerControlPadding Padding { get; }

        /// <summary>Confirma se determinado ponto(positionX,y) pertence ao elemento.</summary>
        public bool HitBox(Point point) => Bounds.Contains(point);

        /// <summary>
        /// A ação é atribuída automaticamente  
        /// em <see cref="InnerControlsCollection.Add(InnerControl)"/> 
        /// para todos os derivados de <see cref="InnerControl"/>
        /// que forem especificados dentro do método citado.
        /// </summary>
        public Action? InvalidateParent { get; set; }

        /// <summary>
        /// A ação é atribuída automaticamente  
        /// em <see cref="InnerControlsCollection.Add(InnerControl)"/> 
        /// para todos os derivados de <see cref="InnerControl"/>
        /// que forem especificados dentro do método citado.
        /// </summary>
        public Action<Cursor>? UpdateParentCursor { get; set; }

        /// <summary>
        /// Indica se o elemento está em foco
        /// (é o que está em interação no momento).
        /// </summary>
        public bool IsFocused 
        {
            get => isFocused;
            set { isFocused = value; } 
        }

        /// <summary>Retângulo delimitiador do elemento.</summary>
        public Rectangle Bounds => bounds;

        /// <summary>
        /// Define o raio do arrendondamento das quinas da borda em píxels,
        /// utlizável apenas se <see cref="BackGroundShape"/>
        /// == <see cref="BackGroundShape.FitRectangle"/>.
        /// </summary>
        public int CornerRadius
        {
            get => cornerRadius;
            set { cornerRadius = value; }
        }

        public Size Size
        {
            get => bounds.Size;
            set { bounds.Size = value; }
        }

        public Point Location
        {
            get => bounds.Location;
            set { bounds.Location = value; }
        }

        /// <summary>
        /// Define o formato do elemento.
        /// </summary>
        public BackGroundShape BackGroundShape
        {
            get => backGroundShape;
            set { backGroundShape = value; UpdateLayout(); }
        }

        public virtual int Width
        {
            get => Size.Width;
            set { Size = new Size(value, Size.Height); UpdateLayout(); }
        }

        public virtual int Height
        {
            get => Size.Height;
            set { Size = new Size(Size.Width, value); UpdateLayout(); }
        }
        public int X => Location.X; public int Y => Location.Y;
        public int Top => Location.Y; public int Right => Location.X + Size.Width;
        public int Left => Location.X; public int Bottom => Location.Y + Size.Height;
    }
}
