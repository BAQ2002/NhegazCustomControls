using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        /// <summary> Lista de objetos SetDataSource</summary>
        private bool DataIsSourced = false;

        private InnerScrollBar verticalScrollBar; //Barra de rolagem vertical da tabela.

        /// <summary> Lista de objetos SetDataSource</summary>
        private List<object> DataSource = new();

        private List<PropertyInfo> Properties = new();
       
        private ColumnWidthMode columnWidthMode = ColumnWidthMode.HeaderWidth;

        private int fixedCharCount = 10;
        private int linesWidth = 1;
       
        private bool linesBetweenColumns;
        private bool linesBetweenRows;
        private bool differentColorsBetweenRows;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Cabeçalho")]
        public HeaderFeature Header {  get; set; }

        MatrixFeature IHasMatrix.Matrix => DataLabels;
        public MatrixFeature DataLabels { get; private set; }

        VectorFeature IHasVector.Vector => HeaderLabels;
        public VectorFeature HeaderLabels { get; private set; }

        [Category("Aparência - Tabela")]
        [Description("Define a espessura das linhas divisórias.")]
        public int LinesWidth
        {
            get => linesWidth;
            set { linesWidth = value; Invalidate(); }
        }

        [Category("Aparência - Tabela")]
        [Description("Define se deve ser exibida cores diferentes entre linhas pares e impares da tabela.")]
        public bool DifferentColorsBetweenRows
        {
            get => differentColorsBetweenRows;
            set { differentColorsBetweenRows = value; Invalidate(); }
        }

        [Category("Aparência - Tabela")]
        [Description("Define se deve haver linhas divisórias entre as linhas de dados.")]
        public bool LinesBetweenRows
        {
            get => linesBetweenRows;
            set { linesBetweenRows = value; Invalidate(); }
        }

        [Category("Aparência - Tabela")]
        [Description("Define se deve haver linhas divisórias entre as colunas.")]
        public bool LinesBetweenColumns
        {
            get => linesBetweenColumns;
            set { linesBetweenColumns = value; Invalidate(); }
        }

        [Category("Aparência - Tabela")]
        [Description("Define qual regra a largura das colunas deve seguir.")]
        public ColumnWidthMode ColumnWidthMode
        {
            get => columnWidthMode;
            set { columnWidthMode = value; Invalidate(); }
        }

        [Category("Aparência - Tabela")]
        [Description("Quantidade de caracteres utilizado em ColumnWidthMode = FixedCharWidth")]
        public int FixedCharCount
        {
            get => fixedCharCount;
            set { fixedCharCount = value; Invalidate(); }
        }

        public Rectangle ContentBounds => new(BorderWidth, BorderWidth, Width - (2 * BorderWidth), Height - (2 * BorderWidth)); 

    }
}
