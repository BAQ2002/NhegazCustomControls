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
    public partial class CustomDataGridView : CustomControlWithHeader
    {          
        public CustomDataGridView()
        {
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
            foreach (var header in HeaderLabels)
                InnerControls.Remove(header);

            HeaderLabels.Clear();

            for (int i = 0; i < Properties.Count; i++)
            {
                InnerLabel columnHeader = new InnerLabel()
                {
                    Text = Properties[i].Name,
                    Font = Font,
                    BackgroundColor = HeaderBackgroundColor        
                };

                HeaderControls.Add(columnHeader);
                HeaderLabels.Add(columnHeader);
            }
        }
        public void CreateDataLabels()
        {
            if (DataSource == null || Properties == null) return;

            int rows = DataSource.Count;
            int cols = Properties.Count;

            DataLabels = new InnerLabel[rows, cols];

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < cols; colIndex++)
                {
                    object val = Properties[colIndex].GetValue(DataSource[rowIndex]) ?? "";
                    string text = val.ToString();

                    InnerLabel InnerLabel = new InnerLabel()
                    {
                        SizeBasedOnText = false,
                        Text = text,
                        Font = Font,
                        ForeColor = ForeColor,
                        BackgroundColor = DifferentColorsBetweenRows && rowIndex % 2 == 1 ? SecondaryBackgroundColor : BackgroundColor,
                    };
                    InnerLabel.Click += (s, e) => MessageBox.Show(InnerLabel.Text);
                    InnerControls.Add(InnerLabel);

                    DataLabels[rowIndex, colIndex] = InnerLabel;
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
        protected override void AdjustControlSize()
        {
            base.AdjustControlSize();

            // Evita execucao se os dados ainda nao foram inicializados
            if (DataLabels == null || HeaderLabels == null) return;

            // === Propriedades base extraidas do proprio controle ===
            int xPadding = HorizontalPadding;
            int yPadding = VerticalPadding;
            int borderWidth = BorderWidth;

            int NumberOfRows = DataLabels.GetLength(0); // Linhas (dados)
            int NumberOfColumns = DataLabels.GetLength(1); // Colunas (propriedades)       

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;
            
            //Armazena a largura de cada coluna (usado tanto para o cabecalho quanto para as celulas)
            int[] columnWidth = new int[NumberOfColumns];

            //Altura uniforme das celulas, baseada na altura da fonte + padding vertical
            int itemHeight = yPadding + NhegazSizeMethods.FontUnitSize(Font).Height;

            // === 1. Ajuste do cabecalho ===
            

            int totalHeaderWidth = 0;
            int headerX = borderWidth;
            for (int col = 0 ; col < NumberOfColumns; col++)
            {        
                //Calcula a largura ideal da coluna com base no conteudo e padding horizontal
                int width = ColumnWidth(ColumnWidthMode, HeaderLabels[col].Width, xPadding);
                columnWidth[col] = width; totalHeaderWidth += width;

                //Define posicao fixa na linha de cabecalho
                int y = borderWidth;

                // Aplica tamanho e posicao ao elemento de cabecalho
                AdjustVectorItemsSizes(col, width, itemHeight); //Ajusta o tamaho 
                AdjustVectorItemsLocations(col, headerX, y); //Ajusta a posicao
                
                // Atualiza posicao horizontal acumulada para a proxima coluna
                headerX += width + lineBetweenCol;
            }

            AdjustHeaderSize( totalHeaderWidth, itemHeight);
            AdjustHeaderLocation(borderWidth, borderWidth);

            // === 2. Ajuste das celulas de dados ===
            // Cada linha representa um registro da tabela
            for (int row = 0; row < NumberOfRows; row++)
            {
                int x = borderWidth;
                int y = borderWidth + (row + 1) * (itemHeight + lineBetweenRow);
                
                for (int col = 0; col < NumberOfColumns; col++)
                {
                    int width = columnWidth[col];
                    
                    AdjustMatrixItemsSizes(row, col, width, itemHeight); //Ajusta o tamaho 
                    AdjustMatrixItemsLocations(row, col, x, y); //Ajusta a posicao

                    x += width + lineBetweenCol;
                }
            }
        }

        protected override void AdjustInnerSizes()
        { }
        protected override void AdjustVectorItemsSizes(int index, int itemWidth, int ItemHeight)
        {
            var label = HeaderLabels[index];
            label.Width = itemWidth;
            label.Height = ItemHeight;

        }
        protected override void AdjustMatrixItemsSizes(int row, int col, int itemWidth, int ItemHeight)
        {
            var label = DataLabels[row, col];
            label.Width = itemWidth;
            label.Height = ItemHeight;
        }

        protected override void AdjustInnerLocations()
        { }
        protected override void AdjustVectorItemsLocations(int index, int x, int y)
        {
            var label = HeaderLabels[index];
            label.SetLocation(x, y);
        }
        protected override void AdjustMatrixItemsLocations(int row, int col, int x, int y)
        {
            var label = DataLabels[row, col];
            label.SetLocation(x, y);
        }
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
            
            if (DataLabels == null || HeaderLabels == null)
                return;

            if (LinesBetweenColumns)
            {
                int col = HeaderLabels.Count - 1;
                Pen pen = new(BorderColor, LinesWidth);
                for (int i = 0; i < col; i++)
                {
                    int locX = HeaderLabels[i].Location.X + HeaderLabels[i].Width;
                    e.Graphics.DrawLine(pen, new Point(locX, BorderWidth), new Point(locX, Bottom - BorderWidth));
                }
            }

            if (LinesBetweenRows) 
            {
                int row = DataLabels.GetLength(0);
                Pen pen = new(BorderColor, LinesWidth);
                for (int i = 0; i < row; i++)
                {
                    int locY = DataLabels[i, 0].Location.Y;// + DataLabels[i,0].Height;
                    e.Graphics.DrawLine(pen, new Point(BorderWidth, locY), new Point(Right, locY));
                }
            }

            
        }

    }
}
