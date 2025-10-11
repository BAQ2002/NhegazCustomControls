using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace NhegazCustomControls
{
    public class MatrixFeature
    {
        private readonly CustomControl ownerControl;
        private readonly InnerControls target;
        private InnerControl?[,] itemsMatrix;

        public InnerControl?[,] ItemsMatrix => itemsMatrix;
        public int GetRowsLenght => itemsMatrix.GetLength(0);
        public int GetColsLenght => itemsMatrix.GetLength(1);

        /// <summary>
        /// Retorna true se o controle estiver em tempo de design (Designer do VS),
        /// com base em LicenseManager.UsageMode e Site?.DesignMode.
        /// </summary>
        private bool InDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime
                                     || (ownerControl?.Site?.DesignMode ?? false);

        public MatrixFeature(CustomControl owner, int rows, int cols, InnerControls? targetCollection = null)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            if (rows <= 0 || cols <= 0) throw new ArgumentOutOfRangeException("rows/cols devem ser > 0.");
            itemsMatrix = new InnerControl?[rows, cols];
            target = targetCollection ?? owner.InnerControls; // se não informado, mantém comportamento atual
        }

        /// <summary>
        /// Adiciona o innerControl à Matrix na posição [row,col]
        /// </summary>
        public void AddItem( InnerControl innerControl, int row, int col)
        {
            EnsureInside(row, col); //Valida índices do parametro
            itemsMatrix[row, col] = innerControl;
            target.Add(innerControl);
        }

        /// <summary>
        /// Redimensiona a matriz preservando o que couber no novo tamanho
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

        public void OnItemMouseEnter(object? sender, EventArgs e)
        {
            var item = (InnerControl)sender!;
            item.ForeColor = ownerControl.BackgroundColor;         // texto "inverso"
            item.BackgroundColor = ownerControl.HoverColor; // highlight de fundo
            ownerControl.Invalidate();
        }

 
        public void SetItemSize(int row, int col, int itemWidth, int itemHeight)
        {
            var item = GetItem(row, col);
            item.Width = itemWidth;
            item.Height = itemHeight;
        }

        public void SetItemSize(int row, int col, Size itemSize)
        {
            var item = GetItem(row, col);
            item.Width = itemSize.Width;
            item.Height = itemSize.Height;
        }

        public void SetItemLocation(int row, int col, int x, int y)
        {
            var item = GetItem(row, col);
            item.SetLocation(x, y);
        }
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
                throw new ArgumentOutOfRangeException($"Índices [{row},{col}] fora dos limites ({GetRowsLenght}x{GetColsLenght}).");
        }

        /// <summary>
        /// Verifica se existe um InnerControl na posição [row,col] da Matriz
        /// </summary>
        /// <returns>InnerControl</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public InnerControl GetItem(int row, int col)
        {
            EnsureInside(row, col);
            var item = itemsMatrix[row, col];
            if (item != null) return item;

            if (InDesignMode)
            {
                return AddPlaceholderItem(row, col);
            }

            throw new InvalidOperationException($"Célula [{row},{col}] ainda não foi preenchida.");
        }

        /// <summary> InnerControl "dummy" utilizado em Design-time </summary>
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
