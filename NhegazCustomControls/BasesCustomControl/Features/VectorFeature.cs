using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public class VectorFeature
    {
        private readonly CustomControl ownerControl;
        private readonly InnerControls target; // <- NOVO
        private InnerControl?[] items;

        public InnerControl?[] Items => items;
        public int Length => items.Length;

        /// <summary>
        /// Retorna true se o controle estiver em tempo de design (Designer do VS),
        /// com base em LicenseManager.UsageMode e Site?.DesignMode.
        /// </summary>
        private bool InDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime
                                     || (ownerControl?.Site?.DesignMode ?? false);
        public VectorFeature(CustomControl owner, int length, InnerControls? targetCollection = null)
        {
            ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));

            items = new InnerControl?[length];
            target = targetCollection ?? owner.InnerControls; // se não informado, mantém comportamento atual
        }

        /// <summary>Adiciona/substitui o InnerControl na posição index.</summary>
        public void AddItem(InnerControl innerControl, int index)
        {
            EnsureInside(index);
            items[index] = innerControl;
            target.Add(innerControl); // <- usa a coleção de destino
        }

        /// <summary>Remove todos os itens do PropertyBag de InnerControls e esvazia o vetor.</summary>
        public void Clear()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is InnerControl ic)
                    target.Remove(ic); // <- remove da coleção de destino
                items[i] = null;
            }
        }

        /// <summary>Redimensiona preservando o que couber.</summary>
        public void Resize(int newLength)
        {
            if (newLength <= 0) throw new ArgumentOutOfRangeException(nameof(newLength));
            var newArr = new InnerControl?[newLength];
            int toCopy = Math.Min(Length, newLength);
            for (int i = 0; i < toCopy; i++) newArr[i] = items[i];
            items = newArr;
        }
       
        /// <summary>Define largura/altura de um item.</summary>
        public void SetItemSize(int index, int itemWidth, int itemHeight)
        {
            var item = GetItem(index);
            item.SetSize(itemWidth, itemHeight);
        }

        /// <summary>Define posição de um item.</summary>
        public void SetItemLocation(int index, int x, int y)
        {
            var item = GetItem(index);
            item.SetLocation(x, y);
        }

        /// <summary>Itera com índice (útil para operações em lote).</summary>
        public void ForEach(Action<int, InnerControl> action)
        {
            if (action is null) return;
            for (int i = 0; i < items.Length; i++)
                if (items[i] is InnerControl ic) action(i, ic);
        }
        public InnerControl GetItem(int index)
        {
            EnsureInside(index);
            var item = items[index];
            if (item != null) return item;

            if (InDesignMode)
                return AddPlaceholderItem(index); // <- retorna o dummy no design-time

            throw new InvalidOperationException($"Item [{index}] ainda não foi preenchido.");
        }

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

            // use a mesma coleção que você já usa para AddItem (ex.: target ou InnerControls)
            target.Add(item); // se tiver 'target'; senão: ownerControl.InnerControls.Add(item);

            return item;
        }

        private void EnsureInside(int index)
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index), $"Índice {index} fora de [0..{Length - 1}].");
        }
    }
}
