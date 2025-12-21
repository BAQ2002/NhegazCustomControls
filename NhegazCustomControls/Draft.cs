using System;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    /// <summary>
    /// Força a perda de foco de um <see cref="CustomControl"/> quando ocorre um clique
    /// fora de qualquer instância de <see cref="CustomControl"/> na aplicação.
    /// 
    /// Funcionamento:
    /// - Instala um <see cref="IMessageFilter"/> que intercepta mensagens de clique do mouse (WM_*BUTTONDOWN).
    /// - Se o controle clicado NÃO pertence a um <see cref="CustomControl"/> (nem a seus pais),
    ///   limpa o foco do <see cref="Form.ActiveControl"/> quando este for um <see cref="CustomControl"/>.
    /// 
    /// Observação:
    /// - O comportamento padrão do WinForms NÃO desfoca ao clicar em áreas não focáveis (Ex.: Panel, fundo do Form).
    ///   Este helper implementa o “desfocar ao clicar fora” de modo global e centralizado.
    /// </summary>
    public static class OutsideClickBlur
    {
        /// <summary>Define se a funcionalidade está habilitada.</summary>
        public static bool Enabled { get; set; } = true;

        /// <summary>Marca se o filtro global já foi instalado para evitar múltiplas inscrições.</summary>
        private static bool installed;

        /// <summary>
        /// Instala o filtro global de mouse uma única vez em toda a aplicação.
        /// </summary>
        public static void InstallOnce()
        {
            if (installed) return;

            // Registra filtro que:
            // 1) Detecta cliques do mouse.
            // 2) Verifica se o alvo pertence a algum CustomControl.
            // 3) Se não pertencer, solicita a limpeza do foco do CustomControl ativo.
            Application.AddMessageFilter(new MouseDownMessageFilter(IsInsideAnyCustomControl, ClearFocusedCustomControl));
            installed = true;
        }

        /// <summary>
        /// Verifica se o controle informado está contido (ele ou algum pai) dentro de um <see cref="CustomControl"/>.
        /// </summary>
        /// <param name="control">Controle de origem do clique (controle mais interno que recebeu a mensagem).</param>
        /// <returns>
        /// <c>true</c> se o controle pertence a um <see cref="CustomControl"/> (ele ou algum pai imediato);
        /// caso contrário, <c>false</c>.
        /// </returns>
        private static bool IsInsideAnyCustomControl(Control? control)
        {
            // Sobe na árvore de pais até a raiz procurando um CustomControl.
            while (control != null)
            {
                if (control is CustomControl) return true;   // Achou um CustomControl “pai/ancestro”.
                control = control.Parent;              // Continua subindo (inclui filhos reais, p.ex. TextBox).
            }
            return false;
        }

        /// <summary>
        /// Limpa o foco do <see cref="CustomControl"/> atualmente ativo, se houver.
        /// 
        /// Detalhes:
        /// - Usa <see cref="Control.BeginInvoke(Delegate)"/> para postergar a alteração do foco,
        ///   evitando conflito com o processamento do clique que ainda está em curso.
        /// - Apenas limpa quando o <see cref="Form.ActiveControl"/> pertence (ele ou algum pai) a um <see cref="CustomControl"/>.
        /// </summary>
        private static void ClearFocusedCustomControl()
        {
            var activeForm = Form.ActiveForm;
            if (activeForm?.ActiveControl != null)
            {
                // Sobe na hierarquia a partir do ActiveControl para verificar se pertence a um CustomControl.
                var activeControl = activeForm.ActiveControl;
                while (activeControl != null && activeControl is not CustomControl) activeControl = activeControl.Parent;

                if (activeControl is CustomControl)
                {
                    // Posta a limpeza de foco para depois do término do processamento da mensagem atual.
                    activeForm.BeginInvoke(new Action(() =>
                    {
                        try { activeForm.ActiveControl = null; } catch { /* No-op: ambiente pode negar a mudança em estados específicos */ }
                    }));
                }
            }
        }

        /// <summary>
        /// Filtro de mensagens para capturar cliques do mouse de forma global.
        /// </summary>
        private sealed class MouseDownMessageFilter : IMessageFilter
        {
            /// <summary>Função que determina se um controle pertence a algum <see cref="CustomControl"/>.</summary>
            private readonly Func<Control?, bool> _isInsideAnyCustomControl;

            /// <summary>Ação usada para limpar o foco de um <see cref="CustomControl"/> ativo.</summary>
            private readonly Action _clearFocused;

            /// <summary>
            /// Constrói o filtro definindo os delegados para verificação do alvo e limpeza de foco.
            /// </summary>
            /// <param name="isInsideAnyCustomControl">Delegado para checagem de pertencimento a <see cref="CustomControl"/>.</param>
            /// <param name="clearFocused">Delegado que executa a limpeza do foco do <see cref="CustomControl"/> ativo.</param>
            public MouseDownMessageFilter(Func<Control?, bool> isInsideAnyCustomControl, Action clearFocused)
            {
                _isInsideAnyCustomControl = isInsideAnyCustomControl;
                _clearFocused = clearFocused;
            }

            // Mensagens de clique tratadas (botão esquerdo, direito e do meio).
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_RBUTTONDOWN = 0x0204;
            private const int WM_MBUTTONDOWN = 0x0207;

            /// <summary>
            /// Intercepta a mensagem antes que o WinForms a encaminhe ao controle de destino.
            /// Se for uma mensagem de clique e o alvo não pertencer a um <see cref="CustomControl"/>,
            /// solicita a limpeza do foco do <see cref="CustomControl"/> ativo.
            /// </summary>
            /// <param name="m">Mensagem do Windows a ser filtrada.</param>
            /// <returns>
            /// <c>false</c> para não consumir a mensagem (permite o fluxo normal),
            /// pois o objetivo é apenas acionar a limpeza de foco quando aplicável.
            /// </returns>
            public bool PreFilterMessage(ref Message m)
            {
                // Se estiver desabilitado, não faz nada.
                if (!OutsideClickBlur.Enabled)
                    return false;

                if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN || m.Msg == WM_MBUTTONDOWN)
                {
                    // Control.FromHandle(m.HWnd) retorna o controle mais interno (child) que recebeu a mensagem.
                    var clicked = Control.FromHandle(m.HWnd);

                    // Se o clique NÃO ocorreu dentro de um CustomControl, limpa o foco do CustomControl ativo (se houver).
                    if (!_isInsideAnyCustomControl(clicked))
                    {
                        _clearFocused();
                    }
                }
                // Não consome a mensagem; deixa seguir o processamento normal do WinForms.
                return false;
            }
           
        }
    }
}
