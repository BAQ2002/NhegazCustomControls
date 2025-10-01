using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView : CustomControl, IHasHeader, IHasMatrix, IHasVector
    {
        public CustomDataGridView()
        {
            // Header pronto para desenhar e propagar cores
            Header = new HeaderFeature(this);
            Header.AdjustHeaderColors();

            // Placeholders (evitam nulos antes de SetDataSource)
            HeaderLabels = new VectorFeature(this, 1, Header.Controls);
            DataLabels = new MatrixFeature(this, 1, 1);
        }
        public void SetDataSource<T>(List<T> _source)
        {
            if (_source == null || _source.Count == 0) //Se não a fonte de dados(list) for null ou não tiver 
                return;

            // Armazena os dados internamente como uma lista de objetos
            DataSource = _source.Cast<object>().ToList(); //Passa os elementos de source para o 

            // Descobre as propriedades públicas da classe T
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead)
                                  .ToList();

            // Solicita o redesenho do controle
            CreateHeadersLabels();
            CreateDataLabels();
            AdjustControlSize();
            Invalidate();
        }
        public void CreateHeadersLabels()
        {
            // zera o cabeçalho anterior a partir da coleção do Header
            HeaderLabels.Clear();

            int cols = Properties.Count;
            if (cols <= 0) return;

            // redimensiona o vetor
            HeaderLabels.Resize(cols);

            for (int i = 0; i < Properties.Count; i++)
            {
                InnerLabel columnHeader = new InnerLabel()
                {
                    Text = Properties[i].Name,
                    Font = Font,
                    BackgroundColor = Header.BackgroundColor        
                };

                HeaderLabels.AddItem(columnHeader, i);
            }
        }
        private void CreateDataLabels()
        {
            if (DataSource == null || Properties == null) return;

            int rows = DataSource.Count;
            int cols = Properties.Count;
            if (rows <= 0 || cols <= 0) return;

            // redimensiona a matriz (preserva o que couber)
            DataLabels.Resize(rows, cols);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    object val = Properties[c].GetValue(DataSource[r]) ?? "";
                    string text = val.ToString();

                    var cell = new InnerLabel
                    {
                        SizeBasedOnText = false,
                        Text = text,
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = (DifferentColorsBetweenRows && (r % 2 == 1))
                                          ? SecondaryBackgroundColor
                                          : BackgroundColor
                    };

                    cell.Click += (s, e) => MessageBox.Show(cell.Text);

                    // coloca no (r,c) e adiciona em InnerControls via MatrixFeature
                    DataLabels.AddItem(cell, r, c);
                }
            }
        }
        /// <summary>
        /// Responsavel por ajustar dinamicamente o tamanho e a posicao dos elementos visuais 
        /// da grade do CustomDataGridView, incluindo o cabecalho e as celulas de dados.
        /// </summary>
        /// <remarks>
        /// Este metodo e chamado automaticamente quando o controle e redimensionado, alterado 
        /// ou quando ha necessidade de reorganizar os elementos internos.
        ///
        /// A logica e separada em duas etapas:
        /// 1. Ajuste das colunas do cabecalho.
        /// 2. Ajuste das celulas de dados (em matriz).
        ///
        /// Cada coluna pode ter largura diferente, por isso acumuladores de posicao `headerX` e `cellX`
        /// sao usados para garantir espaco e alinhamento corretos entre colunas.
        /// </remarks>
        public override void AdjustControlSize()
        {
            base.AdjustControlSize();

            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;
            if (rows <= 0 || cols <= 0) return;

            int yPadding = InnerVerticalPadding;
            int borderWidth = BorderWidth;

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;

            int[] columnWidth = new int[cols];
            int itemHeight = yPadding + NhegazSizeMethods.FontUnitSize(Font).Height;

            // ---- Header (Vector) ----
            int totalHeaderWidth = 0;
            int headerX = borderWidth;
            for (int c = 0; c < cols; c++)
            {
                var h = HeaderLabels.GetItem(c);
                int width = ColumnWidth(ColumnWidthMode, h.Width, InnerHorizontalPadding);

                columnWidth[c] = width;
                totalHeaderWidth += width;

                HeaderLabels.SetItemSize(c, width, itemHeight);
                HeaderLabels.SetItemLocation(c, headerX, borderWidth);

                headerX += width + lineBetweenCol;
            }

            Header.SetSize(totalHeaderWidth, itemHeight);
            Header.SetLocation(borderWidth, borderWidth);

            // ---- Células (Matrix) ----
            for (int r = 0; r < rows; r++)
            {
                int x = borderWidth;
                int y = borderWidth + (r + 1) * (itemHeight + lineBetweenRow);

                for (int c = 0; c < cols; c++)
                {
                    int width = columnWidth[c];

                    DataLabels.SetItemSize(r, c, width, itemHeight);
                    DataLabels.SetItemLocation(r, c, x, y);

                    x += width + lineBetweenCol;
                }
            }
        }

        protected override void AdjustInnerSizes()
        { }

        protected override void AdjustInnerLocations()
        { }

        /// <summary>
        /// Com 
        /// </summary>
        public int ColumnWidth(ColumnWidthMode columnWidthMode, int headerWidth, int xPadding)
        {
            int columnWidth = 0;

            if (columnWidthMode == ColumnWidthMode.HeaderWidth)
                columnWidth = headerWidth + xPadding;

            if (columnWidthMode == ColumnWidthMode.FixedCharWidth)
            {
                string sample = new string('0', FixedCharCount);
                columnWidth = NhegazSizeMethods.TextExactSize(sample, Font).Width + xPadding;
            }

            return columnWidth;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int rows = DataLabels.GetRowsLenght;
            int cols = DataLabels.GetColsLenght;
            if (rows <= 0 || cols <= 0) return;

            if (LinesBetweenColumns && cols > 1)
            {
                using var pen = new Pen(BorderColor, LinesWidth);
                for (int c = 0; c < cols - 1; c++)
                {
                    var h = HeaderLabels.GetItem(c);
                    int x = h.Location.X + h.Width;
                    e.Graphics.DrawLine(pen, new Point(x, BorderWidth), new Point(x, Bottom - BorderWidth));
                }
            }

            if (LinesBetweenRows && rows > 0)
            {
                using var pen = new Pen(BorderColor, LinesWidth);
                for (int r = 0; r < rows; r++)
                {
                    var firstCell = DataLabels.GetItem(r, 0);
                    int y = firstCell.Location.Y;
                    e.Graphics.DrawLine(pen, new Point(BorderWidth, y), new Point(Right, y));
                }
            }
        }

    }
}
