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
    /// <summary>
    /// Controle de grade de dados customizado baseado em <see cref="CustomControl"/>.
    /// Implementa <see cref="IHasHeader"/>, <see cref="IHasMatrix"/> e <see cref="IHasVector"/>
    /// para organizar visualmente cabeçalho e células através de <see cref="HeaderFeature"/>,
    /// <see cref="VectorFeature"/> e <see cref="MatrixFeature"/>.
    /// </summary>
    public partial class CustomDataGridView : CustomControl, IHasHeader, IHasMatrix, IHasVector
    {
        /// <summary>
        /// Construtor padrão do <see cref="CustomDataGridView"/> ->
        /// Inicializa o <see cref="Header"/> pronto para desenhar/propagar cores e
        /// cria placeholders para <see cref="HeaderLabels"/> e <see cref="DataLabels"/>,
        /// evitando nulos antes da chamada de <see cref="SetDataSource{T}(List{T})"/>.
        /// </summary>
        public CustomDataGridView()
        {
            // Header pronto para desenhar e propagar cores
            Header = new HeaderFeature(this);
            Header.AdjustHeaderColors();

            // Placeholders (evitam nulos antes de SetDataSource)
            HeaderLabels = new VectorFeature(this, 1, Header.Controls);
            DataLabels   = new MatrixFeature(this, 1, 1);

            // Barra de rolagem vertical
            verticalScrollBar = new InnerScrollBar
            {
                Orientation = ScrollBarOrientation.Vertical,
                Visible = true,
                BackgroundColor = BackgroundColor,
                ForeColor = ForeColor
            };


            verticalScrollBar.ValueChanged += VerticalScrollBar_ValueChanged; //Reposiciona as células quando o valor muda.
        }

        /// <summary>
        /// Define a fonte de dados do controle a partir de uma lista do tipo <typeparamref name="T"/>.
        /// <para/>
        /// Armazena internamente a lista como <see cref="object"/>, descobre as propriedades
        /// públicas instanciáveis de <typeparamref name="T"/> e, em seguida, cria
        /// os rótulos de cabeçalho e de dados, finalizando com <see cref="UpdateLayout"/>.
        /// </summary>
        /// <typeparam name="T">Tipo de cada item da lista de dados.</typeparam>
        /// <param name="_source">Lista de instâncias de <typeparamref name="T"/> usada como fonte de dados.</param>
        public void SetDataSource<T>(List<T> _source)
        {
            if (_source == null || _source.Count == 0) return; //Se não a fonte de dados(list) for null ou não tiver 

            DataSource = _source.Cast<object>().ToList();      //Passa os elementos de source para o para uma List de objetos.
            DataIsSourced = true;                              //Define que a tabela foi preenchida com os dados.

            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) //Para todas as propriedades publicas da Classe.  
                                  .Where(p => p.CanRead)                                      //Se a propriedade for passível de ser lida.
                                  .ToList();                                                  //Transforma em um objeto List formada pelos nomes das propriadedes.

            CreateHeadersLabels(); //Cria os items de cabeçalho.
            CreateDataLabels();    //Cria os items da tabela de dados.
            CreateScrollBar();
            UpdateLayout();        //Atualiza as posições e tamanhos dos items.

        }

        public void CreateScrollBar()
        {
            InnerControls.Add(verticalScrollBar); //Adiciona a barra de rolagem à coleção interna de InnerControls.

        }
        /// <summary>
        /// Cria os rótulos de cabeçalho (<see cref="HeaderLabels"/>) a partir da lista de
        /// <see cref="Properties"/> configurada em <see cref="SetDataSource{T}(List{T})"/>.
        /// <para/>
        /// Cada coluna do cabeçalho é representada por um <see cref="InnerLabel"/> com o
        /// nome da propriedade correspondente.
        /// </summary>
        public void CreateHeadersLabels()
        {

            int cols = Properties.Count; //Quantidade de Propriades públicas e "Read".
            if (cols <= 0) return;       //Retorna e não faz nada;

            HeaderLabels.Resize(cols);   //Redimensiona o tamanho do vetor para a quantidade de Propriades.

            for (int i = 0; i < Properties.Count; i++)
            {
                InnerLabel columnHeader = new InnerLabel()  //Cria uma instancia de InnerLabel.
                {Text = Properties[i].Name,Font = Font,     //Define o texto e a fonte do InnerLabel.
                 BackgroundColor = Header.BackgroundColor}; //Define a Cor de fundo do InnerLabel.

                HeaderLabels.AddItem(columnHeader, i);      //Adiciona ao Vetor de items de cabeçalho.
            }
        }

        /// <summary>
        /// Cria os rótulos de dados (<see cref="DataLabels"/>) a partir da combinação de
        /// <see cref="DataSource"/> (linhas) e <see cref="Properties"/> (colunas).
        /// <para/>
        /// Cada célula da "tabela" é representada por um <see cref="InnerLabel"/> que
        /// exibe o valor da propriedade correspondente, respeitando cores, fonte e
        /// configuração de linhas alternadas (<see cref="DifferentColorsBetweenRows"/>).
        /// </summary>
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

                    cell.Click += (s, e) => MessageBox.Show(cell.Size.ToString());

                    // coloca no (r,c) e adiciona em InnerControlsCollection via MatrixFeature
                    DataLabels.AddItem(cell, r, c);
                }
            }
        }

        private void VerticalScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            if (DataIsSourced == false) return; //Se a tabela ainda não foi carregada -> não há o que reposicionar.

            SetInnerLocations();                //Recalcula apenas as posições internas com base no novo Value.
            Invalidate();                       //Atualiza o visual do controle.
        }





    }
}
