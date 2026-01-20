using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        /// <summary>
        /// Atualiza o Layout interno do controle.
        /// <para/>
        /// Apenas executa o Layout base se já houver dados carregados
        /// via <see cref="SetDataSource{T}(System.Collections.Generic.List{T})"/>,
        /// evitando cálculos desnecessários enquanto o controle estiver vazio.
        /// </summary>
        public override void UpdateLayout()
        {
            if (DataIsSourced == false) return; //Se ainda não houver dados vinculados -> não há Layout para atualizar.

            base.UpdateLayout();                //Chama a lógica padrão de Layout definida em CustomControl.
        }

        /// <summary>
        /// Retorna o tamanho do conteúdo interno do controle.
        /// <para/>
        /// Implementação atual retorna (0,0) porque o dimensionamento efetivo
        /// é calculado diretamente em <see cref="SetInnerSizes"/> e aplicado aos InnerControls.
        /// </summary>
        /// <returns>Sempre retorna <c>new(0, 0)</c> na implementação atual.</returns>
        public override Size GetContentSize()
        {
            return new(0, 0);               //Tamanho de conteúdo não é utilizado diretamente para este controle.
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna o tamanho extra relacionado ao Padding de conteúdo.
        /// <para/>
        /// Implementação atual retorna (0,0) porque o Padding efetivo é tratado
        /// pelos cálculos de posição e tamanho dos elementos em <see cref="SetInnerSizes"/>
        /// e <see cref="SetInnerLocations"/>.
        /// </summary>
        /// <returns>Sempre retorna <c>new(0, 0)</c> na implementação atual.</returns>
        public override Size GetPaddingSize()
        {
            return new(0, 0);               //Padding adicional não é considerado como Size independente neste controle.
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Define os tamanhos de cada elemento interno:
        /// cabeçalhos (<see cref="HeaderLabels"/>) e células de dados (<see cref="DataLabels"/>).
        /// <para/>
        /// Calcula a largura de cada coluna com base em <see cref="ColumnWidth(int)"/>
        /// e a altura das linhas a partir de <see cref="FontUnitSize"/> e
        /// <see cref="InnerVerticalPadding"/>.
        /// </summary>
        protected override void SetInnerSizes()
        {
            int rows = DataLabels.GetRowsLenght; //Quantidade de linhas da matriz de dados.
            int cols = DataLabels.GetColsLenght; //Quantidade de colunas da matriz de dados.

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;  //Espessura das linhas verticais entre colunas (ou 0 se desativado).
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;      //Valor = LinesWidth ou 0 (linhas horizontais entre linhas).

            int NumberOfCloumnsLines = DataLabels.GetColsLenght - 1;    //Quantidade de linhas verticais entre colunas (não utilizado diretamente).

            int rowHeight = FontUnitSize.Height + InnerVerticalPadding; //Altura de cada linha (texto + padding vertical interno).

            Size[] columnsSizes = new Size[cols];                       //Vetor com o tamanho de cada coluna.

            for (int col = 0; col < cols; col++)
            {
                // Calcula largura base da coluna a partir do tamanho do cabeçalho.
                columnsSizes[col] = new(ColumnWidth(HeaderLabels.GetItem(col).Width), rowHeight);

                if (col == 0) { columnsSizes[col].Width += BorderLeftPadding; }  //Adiciona padding à primeira coluna (borda esquerda).
                if (col == cols - 1) { columnsSizes[col].Width += BorderRightPadding; } //Adiciona padding à última coluna (borda direita).

                HeaderLabels.SetItemSize(col, columnsSizes[col]);                     //Aplica o tamanho calculado ao cabeçalho da coluna.
            }

            int headerTotalWidth = HeaderLabels.ItemsWidthSum + (cols - 1) * lineBetweenCol; //Largura total do cabeçalho (somatório das colunas + linhas entre colunas).

            Header.SetSize(headerTotalWidth, rowHeight);                              //Define o tamanho do Header (largura total x altura da linha).

            // Aplica o mesmo tamanho de coluna para todas as células de dados.
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemSize(row, col, columnsSizes[col]);              //Define o tamanho da célula [row,col] igual ao da coluna.
                }
            }

            // --- Configuração da barra de rolagem vertical ---
            if (rows <= 0 || cols <= 0) return;                        //Se não houver dados -> não há o que rolar.

            int contentHeight = rows * rowHeight                      //Altura total das linhas da tabela.
                               + (rows - 1) * lineBetweenRow;

            int viewportHeight = Height - BorderWidth * 2 - Header.Height; //Altura visível disponível para as linhas.

            if (viewportHeight < 0) viewportHeight = 0;                //Evita valores negativos.

            verticalScrollBar.Minimum = 0;                        //Começa sempre em 0.
            verticalScrollBar.Maximum = contentHeight;            //Conteúdo total para rolagem.
            verticalScrollBar.ViewportSize = viewportHeight;           //Tamanho da área visível.

            int scrollWidth = 12;                                      //Espessura fixa da barra de rolagem.

            verticalScrollBar.Visible = contentHeight > viewportHeight; //Só exibe se houver necessidade de rolagem.

            if (verticalScrollBar.Visible)
            {
                verticalScrollBar.SetSize(scrollWidth, viewportHeight); //Define o tamanho da barra de rolagem.
            }

        }


        /// <summary>
        /// Define as posições (Location) dos elementos internos:
        /// cabeçalhos (<see cref="HeaderLabels"/>) e células de dados (<see cref="DataLabels"/>).
        /// <para/>
        /// Considera largura da borda, linhas entre colunas/linhas e altura do cabeçalho
        /// para posicionar o grid de forma consistente.
        /// </summary>
        /// 
        protected override void SetInnerLocations()
        {
            int rows = DataLabels.GetRowsLenght; //Quantidade de linhas da matriz de dados.
            int cols = DataLabels.GetColsLenght; //Quantidade de colunas da matriz de dados.

            int lineBetweenCol = LinesBetweenColumns ? LinesWidth : 0;   //Valor = LinesWidth ou 0 (linhas verticais entre colunas).
            int lineBetweenRow = LinesBetweenRows ? LinesWidth : 0;      //Valor = LinesWidth ou 0 (linhas horizontais entre linhas).

            int itemHeight = FontUnitSize.Height + InnerVerticalPadding; //Altura de cada linha (texto + padding vertical interno).

            int headerItemX = BorderWidth;                               //Posição X inicial do primeiro cabeçalho.
            Header.SetLocation(BorderWidth, BorderWidth);                //Posiciona o Header respeitando a borda do controle.

            // Posiciona cada cabeçalho de coluna na horizontal.
            for (int col = 0; col < cols; col++)
            {
                HeaderLabels.SetItemLocation(col, headerItemX, BorderWidth);               //Define a posição do cabeçalho da coluna.
                headerItemX += HeaderLabels.GetItem(col).Width + lineBetweenCol;           //Avança X somando largura da coluna e linha entre colunas.
            }

            // --- Posição da barra de rolagem vertical ---
            int scrollWidth = 12;                                        //Mesma espessura definida em SetInnerSizes.
            int yScrollStart = Header.Bottom;                            //Início logo abaixo do Header.

            
            int scrollX = Width - BorderWidth - scrollWidth;         //Encostado na borda direita interna.
            verticalScrollBar.SetLocation(RelativeRightX(verticalScrollBar), RelativeTopY());    //Define a posição da barra de rolagem.
            

            int yOffset = (verticalScrollBar.Visible)                    //Deslocamento vertical conforme o Value do scroll.
                        ? verticalScrollBar.Value
                        : 0;

            // --- Posição das células de dados ---
            for (int row = 0; row < rows; row++)
            {
                int x = BorderWidth;
                int y = Header.Bottom + row * (itemHeight + lineBetweenRow) - yOffset;

                for (int col = 0; col < cols; col++)
                {
                    DataLabels.SetItemLocation(row, col, x, y);

                    x += lineBetweenCol + HeaderLabels.GetItem(col).Width;
                }
            }
        }

        /// <summary>
        /// Calcula a largura final de uma coluna de dados a partir da largura
        /// do cabeçalho ou de uma largura fixa baseada na unidade da fonte,
        /// de acordo com o <see cref="ColumnWidthMode"/>.
        /// </summary>
        /// <param name="headerWidth">Largura do cabeçalho da coluna.</param>
        /// <returns>
        /// Largura da coluna considerando o modo configurado
        /// e o <see cref="InnerHorizontalPadding"/>.
        /// </returns>
        public int ColumnWidth(int headerWidth)
        {
            int columnWidth = 0; //Largura final da coluna.

            if (ColumnWidthMode == ColumnWidthMode.HeaderWidth)
                columnWidth = headerWidth + InnerHorizontalPadding; //Baseado na largura do cabeçalho + padding horizontal interno.

            if (ColumnWidthMode == ColumnWidthMode.FixedCharWidth)
            {
                columnWidth = FixedCharCount
                            * NhegazSizeMethods.FontUnitSize(Font).Width  //Largura fixa baseada na unidade da fonte.
                            + InnerHorizontalPadding;                     //Adiciona o padding horizontal interno.
            }

            return columnWidth; //Retorna a largura calculada para a coluna.
        }
    }
}
