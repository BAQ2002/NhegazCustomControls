using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    [TypeConverter(typeof(DropDownFeatureTypeConverter))] // expandir no PropertyGrid
    public class DropDownFeature
    {
        //private readonly CustomControl ownerControl;
        //private readonly CustomControl[] DropDownControls;

        private Color headerBackgroundColor = SystemColors.GrayText; //Cor do fundo do cabecalho 
        private Color headerForeColor = SystemColors.ControlText;

        private readonly HashSet<Type> ControlsTypes = new();

        /// <summary>
        /// Exposição somente leitura dos tipos registrados.
        /// Mantém o encapsulamento: ninguém consegue modificar por fora.
        /// </summary>
        public IReadOnlyCollection<Type> DropDownsControlsTypes => ControlsTypes;

        /// <summary>
        /// Propriedade BOOLEANA calculada:
        /// retorna true se houver ao menos um tipo não-abstrato que implemente IHasHeader.
        /// 
        /// Observação importante:
        ///  - typeof(IHasHeader).IsAssignableFrom(t) considera herança.
        ///    Se uma classe base já implementa IHasHeader, as derivadas também “herdam” a implementação,
        ///    e o teste retorna true (comportamento normalmente desejado).
        ///  - Se você quiser considerar APENAS interfaces declaradas diretamente na classe,
        ///    poderia usar: t.GetInterfaces().Contains(typeof(IHasHeader)) (geralmente não é necessário).
        /// </summary>
        public bool AnyIsHasHeader =>
            ControlsTypes.Any(t => !t.IsAbstract && typeof(IHasHeader).IsAssignableFrom(t));

        /// <summary>
        /// Adiciona um tipo via genéricos, garantindo em compile-time que T deriva de CustomControl.
        /// Ex.: registry.Add<DropDownDay>();
        /// </summary>
        public void Add<T>() where T : CustomControl => Add(typeof(T));

        /// <summary>
        /// Adiciona um tipo dinamicamente (quando você já tem o Type em tempo de execução).
        /// Faz validações úteis:
        ///  - null check
        ///  - o tipo deve derivar de CustomControl
        ///  - o tipo não pode ser abstrato (já que não instancia diretamente)
        /// </summary>
        public void Add(Type t)
        {
            if (t is null)
                throw new ArgumentNullException(nameof(t), "Tipo não pode ser nulo.");

            if (!typeof(CustomControl).IsAssignableFrom(t))
                throw new InvalidOperationException(
                    $"Tipo {t.FullName} não deriva de {nameof(CustomControl)}.");

            if (t.IsAbstract)
                throw new InvalidOperationException(
                    $"Tipo {t.FullName} é abstrato e não pode ser registrado.");

            ControlsTypes.Add(t);
        }

        /// <summary>
        /// Remove um tipo via genéricos. Útil para manter a mesma ergonomia do Add&lt;T&gt;().
        /// </summary>
        public bool Remove<T>() where T : CustomControl => ControlsTypes.Remove(typeof(T));

        /// <summary>
        /// Remove um tipo quando você já tem o Type em tempo de execução.
        /// Retorna true se removeu, false se o tipo não estava registrado.
        /// </summary>
        public bool Remove(Type t) => ControlsTypes.Remove(t);
        /// <summary>
        /// Cor de fundo para cabeçalho.
        /// </summary>
        [Category("DropDowns")]
        [Browsable(true)]
        public virtual Color HeaderBackgroundColor
        {
            get => headerBackgroundColor;
            set { headerBackgroundColor = value; }

        }
        [Category("DropDowns")]
        [Browsable(true)]
        public virtual Color HeaderForeColor
        {
            get => headerForeColor;
            set { headerForeColor = value; }

        }

        public DropDownFeature( )
        {            
            //ownerControl = owner ?? throw new ArgumentNullException(nameof(owner));
            //DropDownControls = dropDowns ?? throw new ArgumentNullException(nameof(dropDowns));
        }
    }
}
