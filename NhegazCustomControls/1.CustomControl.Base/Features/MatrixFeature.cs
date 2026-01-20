using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace NhegazCustomControls
{
    /// <summary>
    /// Essa Feature configura uma propriedade que pode ser atribuída a um <see cref="CustomControl"/>
    /// que permite realizar alterações como: Adicionar, Remover e Modificar tamanho/localização em uma conjunto
    /// bidimensional (N*M) específico de <see cref="InnerControl"/>'s pertencentes a uma <see cref="InnerControlsCollection"/>.
    /// </summary>
    public class MatrixFeature
    {
        private readonly CustomControl ownerControl;
        private readonly InnerControlsCollection target;
        private InnerControl?[,] itemsMatrix;

        public InnerControl?[,] ItemsMatrix => itemsMatrix;

        /// <summary>
        /// Retorna a quantidade de items na dimensão 0.
        /// Exemplo:
        /// <para>|1| |2| |3| |4| |5|</para>
        /// <para>[_] [_] [_] [_] [_]</para>
        /// <para>[_] [_] [_] [_] [_]</para>
        /// <para>[_] [_] [_] [_] [_]</para>
        /// <para>[_] [_] [_] [_] [_]</para>
        /// Retorna 5.
        /// </summary>
        public int GetRowsLenght => itemsMatrix.GetLength(0);

        /// <summary>
        /// Retorna a quantidade de items na dimensão 0.
        /// Exemplo:
        /// <para>|1| [_] [_] [_] [_] [_]</para>
        /// <para>|2| [_] [_] [_] [_] [_]</para>
        /// <para>|3| [_] [_] [_] [_] [_]</para>
        /// <para>|4| [_] [_] [_] [_] [_]</para>
        /// <para>|5| [_] [_] [_] [_] [_]</para>
        /// Retorna 5.
        /// </summary>
        public int GetColsLenght => itemsMatrix.GetLength(1);

        /// <summary>
        /// Retorna true se o controle estiver em tempo de design (Designer do VS),
        /// com base em LicenseManager.UsageMode e Site?.DesignMode.
        /// </summary>
        private bool InDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime
                                     || (ownerControl?.Site?.DesignMode ?? false);

        public MatrixFeature(CustomControl owner, int rows, int cols, InnerControlsCollection? targetCollection = null)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            if (rows <= 0 || cols <= 0) throw new ArgumentOutOfRangeException("rows/cols devem ser > 0.");
            itemsMatrix = new InnerControl?[rows, cols];
            target = targetCollection ?? owner.InnerControls; // se não informado, mantém comportamento atual
        }

        /// <summary>
        /// Adiciona o innerControl à matriz na posição [row,col].
        /// </summary>
        public void AddItem( InnerControl innerControl, int row, int col)
        {
            EnsureInside(row, col); //Valida índices do parametro
            itemsMatrix[row, col] = innerControl;
            target.Add(innerControl);
        }


        /// <summary>
        /// Retorna o tamanho total (Width, Height) ocupado por todos os <see cref="InnerControl"/>'s
        /// presentes na matriz, calculando um "bounding box" (retângulo envolvente) a partir de:
        /// <para>menor <see cref="InnerControl.Left"/> e <see cref="InnerControl.Top"/>,</para>
        /// <para>maior <see cref="InnerControl.Right"/> e <see cref="InnerControl.Bottom"/>.</para>
        /// </summary>
        /// <returns>Size(totalWidth, totalHeight)</returns>
        public Size GetItemsFullSize()
        {
            int minLeft = int.MaxValue; int maxRight = int.MinValue;
            int minTop = int.MaxValue; int maxBottom = int.MinValue;

            for (int row = 0; row < GetRowsLenght; row++)
            {
                for (int col = 0; col < GetColsLenght; col++)
                {
                    var item = GetItem(row, col);

                    if (item.Left < minLeft) minLeft = item.Left;
                    if (item.Top < minTop) minTop = item.Top;
                    if (item.Right > maxRight) maxRight = item.Right;
                    if (item.Bottom > maxBottom) maxBottom = item.Bottom;
                }
            }

            int width = maxRight - minLeft;
            int height = maxBottom - minTop;

            return new Size(width, height);
        }

        /// <summary>
        /// Retorna o retângulo total (X, Y, Width, Height) ocupado por todos os <see cref="InnerControl"/>'s
        /// presentes na matriz, calculando um "bounding box" (retângulo envolvente) a partir de:
        /// <para>menor <see cref="InnerControl.Left"/> e <see cref="InnerControl.Top"/>,</para>
        /// <para>maior <see cref="InnerControl.Right"/> e <see cref="InnerControl.Bottom"/>.</para>
        /// </summary>
        /// <returns>Rectangle(minLeft, minTop, totalWidth, totalHeight)</returns>
        public Rectangle GetItemsFullRect()
        {
            int minLeft = int.MaxValue; int maxRight = int.MinValue;
            int minTop = int.MaxValue; int maxBottom = int.MinValue;

            for (int row = 0; row < GetRowsLenght; row++)
            {
                for (int col = 0; col < GetColsLenght; col++)
                {
                    var item = GetItem(row, col);

                    if (item.Left < minLeft) minLeft = item.Left;
                    if (item.Top < minTop) minTop = item.Top;
                    if (item.Right > maxRight) maxRight = item.Right;
                    if (item.Bottom > maxBottom) maxBottom = item.Bottom;
                }
            }

            int width = maxRight - minLeft;
            int height = maxBottom - minTop;

            return new Rectangle(minLeft, minTop, width, height);
        }


        /// <summary>
        /// Redimensiona à matriz preservando os <see cref="InnerControl"/>'s
        /// que couberem no novo tamanho [<paramref name="row"/>,<paramref name="col"/>] do conjunto. 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Resize(int newRows, int newCols)
        {
            if (newRows <= 0 || newCols <= 0) throw new ArgumentOutOfRangeException();

            var newMatrix = new InnerControl?[newRows, newCols];
            int rowsToCopy = Math.Min(GetRowsLenght, newRows);
            int colsToCopy = Math.Min(GetColsLenght, newCols);

            for (int r = 0; r < rowsToCopy; r++)
                for (int c = 0; c < colsToCopy; c++)
                    newMatrix[r, c] = itemsMatrix[r, c];

            itemsMatrix = newMatrix; // <<< importante
        }

        /// <summary>WIP</summary>
        public void OnItemMouseEnter(object? sender, EventArgs e)
        {
            var item = (InnerControl)sender!;
            item.ForeColor = ownerControl.BackgroundColor;         // texto "inverso"
            item.BackgroundColor = ownerControl.HoverBackgroundColor; // highlight de fundo
            ownerControl.Invalidate();
        }

        /// <summary>Define novos valores para alargura e altura de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemSize(int row, int col, int itemWidth, int itemHeight)
        {
            var item = GetItem(row, col);
            item.Width = itemWidth;
            item.Height = itemHeight;
        }

        /// <summary>Define novos valores para alargura e altura de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemSize(int row, int col, Size itemSize)
        {
            var item = GetItem(row, col);
            item.Width = itemSize.Width;
            item.Height = itemSize.Height;
        }

        /// <summary>Define um nova coordenada de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemLocation(int row, int col, int x, int y)
        {
            var item = GetItem(row, col);
            item.SetLocation(x, y);
        }

        /// <summary>Define um nova coordenada de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemLocation(int row, int col, Point itemLocation)
        {
            var item = GetItem(row, col);
            item.SetLocation(itemLocation);
        }

        /// <summary>
        /// Verifica se a posição [row,col] esta nos limites da Matriz
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void EnsureInside(int row, int col)
        {
            if (row < 0 || col < 0 || row >= GetRowsLenght || col >= GetColsLenght)
                throw new ArgumentOutOfRangeException($"Índices [{row},{col}] fora dos limites ({GetRowsLenght}positionX{GetColsLenght}).");
        }

        /// <summary>
        /// Verifica se existe um InnerControl na posição 
        /// [<paramref name="row"/>,<paramref name="col"/>] da Matriz.
        /// </summary>
        /// <returns>InnerControl</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public InnerControl GetItem(int row, int col)
        {
            //Valida a posição ->   //Referência ao item[row, col] -> //Se existir: retorne ele.
            EnsureInside(row, col); var item = itemsMatrix[row, col]; if (item != null) return item;

            //Se estiver InDesignMode -> retorna item "PlaceHolder" um item fantasma.
            if (InDesignMode){ return AddPlaceholderItem(row, col); } 

            throw new InvalidOperationException($"Célula [{row},{col}] ainda não foi preenchida.");
        }

        /// <summary>
        /// Cria e adiciona um <see cref="InnerControl"/> na
        /// posição [<paramref name="row"/>,<paramref name="col"/>] da Matriz.
        /// </summary>
        public InnerControl AddPlaceholderItem(int row, int col)
        {
            InnerLabel item = new()
            {
                Text = " ",
                Font = ownerControl.Font,
                BackgroundColor = ownerControl.BackgroundColor,
                ForeColor = ownerControl.ForeColor
            };
            itemsMatrix[row, col] = item;
            target.Add(item);
            return item;
        }
    }
}
