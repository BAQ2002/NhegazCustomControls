using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    /// <summary>
    /// Essa Feature configura uma propriedade que pode ser atribuída a um <see cref="CustomControl"/>
    /// que permite realizar alterações como: Adicionar, Remover e Modificar tamanho/localização em uma conjunto
    /// unidimensional (N*1) específico de <see cref="InnerControl"/>'s pertencentes a uma <see cref="InnerControlsCollection"/>.
    /// </summary>
    public class VectorFeature
    {
        private readonly CustomControl ownerControl;
        /// <summary>
        /// Coleção que o <see cref="VectorFeature"/>
        /// será capaz de adicionar, deletar 
        /// </summary>
        private readonly InnerControlsCollection target;
        private InnerControl[] items;
        
        /// <summary>
        /// Vetor de <see cref="InnerControl"/>'s 
        /// </summary>
        public InnerControl[] Items => items;
        public int Length => items.Length;

        /// <summary>
        /// Retorna true se o controle estiver em tempo de design (Designer do VS),
        /// com base em LicenseManager.UsageMode e Site?.DesignMode.
        /// </summary>
        private bool InDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime
                                     || (ownerControl?.Site?.DesignMode ?? false);

        /// <summary>
        /// Construtor Primário: caso não seja passado o parâmetro <paramref name="targetCollection"/>
        /// a <see cref="InnerControlsCollection"/> <see cref="target"/> será a mesma coleção que
        /// <paramref name="owner"/>.<see cref="InnerControlsCollection"/>.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="length"></param>
        /// <param name="targetCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public VectorFeature(CustomControl owner, int length, InnerControlsCollection? targetCollection = null)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));

            items = new InnerControl[length];
            target = targetCollection ?? owner.InnerControls; // se não informado, mantém comportamento atual
        }

        /// <summary>Adiciona/substitui o InnerControl na posição index.</summary>
        public void AddItem(InnerControl innerControl, int index)
        {
            EnsureInside(index);
            items[index] = innerControl;
            target.Add(innerControl); // <- usa a coleção de destino
        }

        /// <summary>Remove todos os itens do PropertyBag de InnerControlsCollection e esvazia o vetor.</summary>
        public void Clear()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is InnerControl ic)
                    target.Remove(ic); // <- remove da coleção de destino
                items[i] = null;
            }
        }

        /// <summary>
        /// Retorna a soma da largura de todos os
        /// <see cref="InnerControl"/>'s pertencentes ao conjunto.
        /// </summary>
        public int ItemsWidthSum
        {
            get 
            {   
                int sum = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    sum += GetItem(i).Width;
                }
                return sum;
            }           
        }

        /// <summary>
        /// Redimensiona o vetor preservando os <see cref="InnerControl"/>'s
        /// que couberem no novo tamanho <paramref name="newLength"/> do conjunto. 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Resize(int newLength)
        {
            if (newLength <= 0) throw new ArgumentOutOfRangeException(nameof(newLength));
            var newArr = new InnerControl?[newLength];
            int toCopy = Math.Min(Length, newLength);
            for (int i = 0; i < toCopy; i++) newArr[i] = items[i];
            items = newArr;
        }

        /// <summary>Define novos valores para alargura e altura de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemSize(int index, int itemWidth, int itemHeight)
        {
            var item = GetItem(index);
            item.SetSize(itemWidth, itemHeight);
        }

        /// <summary>Define novos valores para alargura e altura de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemSize(int index, Size itemSize)
        {
            var item = GetItem(index);
            item.SetSize(itemSize);
        }

        /// <summary>Define um nova coordenada de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemLocation(int index, int x, int y)
        {
            var item = GetItem(index);
            item.SetLocation(x, y);
        }

        /// <summary>Define um nova coordenada de um item <see cref="InnerControl"/> do conjunto.</summary>
        public void SetItemLocation(int index, Point itemLocation)
        {
            var item = GetItem(index);
            item.SetLocation(itemLocation);
        }

        /// <summary>Itera com índice (útil para operações em lote).</summary>
        public void ForEach(Action<int, InnerControl> action)
        {
            if (action is null) return;
            for (int i = 0; i < items.Length; i++)
                if (items[i] is InnerControl ic) action(i, ic);
        }

        /// <summary>
        /// Verifica se existe um InnerControl na posição 
        /// [<paramref name="index"/>] do conjunto.
        /// </summary>
        /// <returns>InnerControl</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public InnerControl GetItem(int index)
        {
            //Valida a posição ->//Referência ao item[index] -> //Se existir: retorne ele.
            EnsureInside(index); var item = items[index]; if (item != null) return item;

            //Se estiver InDesignMode -> retorna item "PlaceHolder" um item fantasma.
            if (InDesignMode) return AddPlaceholderItem(index);

            throw new InvalidOperationException($"Item [{index}] ainda não foi preenchido.");
        }

        /// <summary>
        /// Cria e adiciona um <see cref="InnerControl"/> na
        /// posição [<paramref name="index"/>] do conjunto.
        /// </summary>
        private InnerControl AddPlaceholderItem(int index)
        {
            var item = new InnerLabel
            {
                Text = " ",
                Font = ownerControl.Font,
                BackgroundColor = ownerControl.BackgroundColor,
                ForeColor = ownerControl.ForeColor
            };
            items[index] = item;

            // use a mesma coleção que você já usa para AddItem (ex.: target ou InnerControlsCollection)
            target.Add(item); // se tiver 'target'; senão: ownerInnerControl.InnerControlsCollection.Add(item);

            return item;
        }

        private void EnsureInside(int index)
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index), $"Índice {index} fora de [0..{Length - 1}].");
        }
    }
}
