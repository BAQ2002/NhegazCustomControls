using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public partial class CustomDataGridView
    {
        private List<object> DataSource = new(); //Lista de objetos que formada pelos 
        private List<PropertyInfo> Properties = new();
        private List<InnerLabel> HeaderLabels = new();

        private InnerLabel[,]? DataLabels = null;
        private ColumnWidthMode columnWidthMode = ColumnWidthMode.HeaderWidth;

        private int fixedCharCount = 10;
        private int linesWidth = 1;

        private bool linesBetweenColumns;
        private bool linesBetweenRows;
        private bool differentColorsBetweenRows;
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
