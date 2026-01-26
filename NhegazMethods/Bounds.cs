using NhegazCustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazObjects
{
    public interface IHasBounds
    {
        public Bounds Bounds { get; }
    }

    //public class Point { }
    public class Bounds
    {
        public Point Location {  get; set; }
        public Size Size { get; set; }

        public int X
        {
            get => Location.X;
            set => Location = new(value, Y);
        }
        public int Y 
        {
            get => Location.Y;
            set => Location = new(X, value);
        }
        
        public int Width 
        {
            get => Size.Width;
            set => Size = new(value, Height);
        }

        public int Height
        {
            get => Size.Height;
            set => Size = new(Width, value);
        }

        public int Top => Location.Y;
        public int Left => Location.X;
        public int Right => Location.X + Size.Width;
        public int Bottom => Location.Y + Size.Height;


        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna a diferença  
        /// entre a altura deste e de <paramref name="obj"/>.
        /// </summary>
        public int HeightDifference(object? obj = null)
        {            
            Bounds? bounds = (obj as IHasBounds)?.Bounds;       
            if (bounds != null){return Height - bounds.Height; }
            else return 0;                              
        }


        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna a diferença  
        /// entre a largura deste e de <paramref name="obj"/>.
        /// </summary>
        public int WidthDifference(object? obj = null)
        {
            Bounds? bounds = (obj as IHasBounds)?.Bounds;
            if (bounds != null) { return Width - bounds.Width; }
            else return 0;
        }

        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna a coordenada X 
        /// do centro horizonal em relação à largura de <paramref name="obj"/>.
        /// Se <paramref name="obj"/> for null -> retorna o centro X deste <see cref="Bounds"/>.
        /// </summary>
        public int CenterX(object? obj = null)
        {
            Bounds? bounds = (obj as IHasBounds)?.Bounds;
            if (bounds != null)
            {
                int WidthDiff = WidthDifference(obj);
                int centerX = X + WidthDiff / 2;

                return centerX;
            }
            else return X + Width / 2;
        }

        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna a coordenada Y 
        /// do centro vertical em relação à altura de <paramref name="obj"/>.
        /// Se <paramref name="obj"/> for null -> retorna o centro Y deste <see cref="Bounds"/>.
        /// </summary>
        public int CenterY(object? obj = null)
        {
            Bounds? bounds = (obj as IHasBounds)?.Bounds;
            if (bounds != null)
            {
                int heightDiff = HeightDifference(obj);
                int centerY = Y + heightDiff / 2;         

                return centerY; 
            }
            else return Y;
        }


        /// <summary>
        /// Retorna a coordenada X da extremidade esquerda
        /// interna deste <see cref="Bounds"/>.
        /// </summary>
        public int InnerLeftX(object? obj = null) => Left;

        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna 
        /// a coordenada X da extremidade esquerda interna
        /// em relação à largura de <paramref name="obj"/>.
        /// Se <paramref name="obj"/> for null -> retorna <see cref="Right"/>.
        /// </summary>
        public int InnerRightX(object? obj = null)
        {
            Bounds? bounds = (obj as IHasBounds)?.Bounds;
            if (bounds != null) { return Right - bounds.Width; }

            else return Right;     
        }

        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna 
        /// a coordenada Y da extremidade superior interna
        /// em relação à largura de <paramref name="obj"/>.
        /// Se <paramref name="obj"/> for null -> retorna <see cref="Right"/>.
        /// </summary>
        public int InnerTopY(object? obj = null) => Top;

        /// <summary>
        /// Se <paramref name="obj"/>
        /// for um <see cref="IHasBounds"/> -> Retorna 
        /// a coordenada X da extremidade inferior interna
        /// em relação à largura de <paramref name="obj"/>.
        /// Se <paramref name="obj"/> for null -> retorna <see cref="Bottom"/>.
        /// </summary>
        public int InnerBottomY(object? obj = null)
        {
            Bounds? bounds = (obj as IHasBounds)?.Bounds;
            if (bounds != null) { return Bottom - bounds.Height; }

            else return Bottom;
        }


        public Point InnerLeftTop(object? obj = null)
            => new(InnerLeftX(obj), InnerTopY(obj));

        public Point InnerCenterTop(object? obj = null)
            => new(CenterX(obj), InnerTopY(obj));

        public Point InnerRightTop(object? obj = null)
            => new(InnerRightX(obj), InnerTopY(obj));

        public Point InnerLeftCenter(object? obj = null)
            => new(InnerLeftX(obj), CenterY(obj));

        public Point InnerCenter(object? obj = null)
            => new(CenterX(obj), CenterY(obj));

        public Point InnerRightCenter(object? obj = null)
            => new(InnerRightX(obj), CenterY(obj));

        public Point InnerLeftBottom(object? obj = null)
            => new(InnerLeftX(obj), InnerBottomY(obj));

        public Point InnerCenterBottom(object? obj = null)
            => new(CenterX(obj), InnerBottomY(obj));

        public Point InnerRightBottom(object? obj = null)
            => new(InnerRightX(obj), InnerBottomY(obj));

    }
}
