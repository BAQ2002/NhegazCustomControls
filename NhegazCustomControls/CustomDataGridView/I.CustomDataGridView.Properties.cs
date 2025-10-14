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

        public int LinesWidth
        {
            get => linesWidth;
            set { linesWidth = value; Invalidate(); }
        }
        public bool DifferentColorsBetweenRows
        {
            get => differentColorsBetweenRows;
            set { differentColorsBetweenRows = value; Invalidate(); }
        }
        public bool LinesBetweenRows
        {
            get => linesBetweenRows;
            set { linesBetweenRows = value; Invalidate(); }
        }
        public bool LinesBetweenColumns
        {
            get => linesBetweenColumns;
            set { linesBetweenColumns = value; Invalidate(); }
        }
        public ColumnWidthMode ColumnWidthMode
        {
            get => columnWidthMode;
            set { columnWidthMode = value; Invalidate(); }
        }
        public int FixedCharCount
        {
            get => fixedCharCount;
            set { fixedCharCount = value; Invalidate(); }
        }

    }
}
