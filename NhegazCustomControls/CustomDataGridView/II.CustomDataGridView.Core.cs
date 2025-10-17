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

            DataIsSourced = true;

            // Armazena os dados internamente como uma lista de objetos
            DataSource = _source.Cast<object>().ToList(); //Passa os elementos de source para o 
            
            // Descobre as propriedades públicas da classe T
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead)
                                  .ToList();

            // Solicita o redesenho do controle
            CreateHeadersLabels();
            CreateDataLabels();
            UpdateLayout();
        }

        public void CreateHeadersLabels()
        {

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
                columnHeader.Click += (s, e) => MessageBox.Show(columnHeader.Size.ToString());
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

                    cell.Click += (s, e) => MessageBox.Show(cell.Size.ToString());

                    // coloca no (r,c) e adiciona em InnerControls via MatrixFeature
                    DataLabels.AddItem(cell, r, c);
                }
            }
        }
  
        
       

    }
}
