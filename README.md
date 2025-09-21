# NhegazCustomControls

Biblioteca de **controles customizados para Windows Forms (.NET)** com foco em aparência moderna, arquitetura extensível e desenho vetorial (GDI+) para alto desempenho sem dependências externas.

> Projetada para cenários em que você quer padronizar **borda, cores, paddings, cabeçalhos e ícones** em todos os controles da sua aplicação.

---

## ✨ Principais recursos

- **Base unificada (`CustomControl`)** com propriedades de tema: `BorderRadius`, `BorderWidth`, `BorderColor`, `BackgroundColor`, `ForeColor`, `HoverColor`, `OnFocusBorderColor`, `HeaderBackgroundColor`, `PaddingMode`, `HorizontalPadding` e `VerticalPadding`.
- **Sistema de _Inner Controls_**: elementos leves (labels, botões, etc.) renderizados e gerenciados pelo controle pai, com *hit testing* e eventos (click, double click, hover, focus) já roteados.
- **Cabeçalho integrado** via `CustomControlWithHeader`, com área dedicada (`HeaderBounds`) e coleção própria `HeaderControls`.
- **Desenho vetorial** (GDI+): paths para bordas/fundos arredondados e ícones (drop‑down, forward, backward, add, etc.).
- **Padding responsivo**: `PaddingMode.Absolute` ou `PaddingMode.RelativeToFont` (escala automática pela fonte).
- **Controles prontos**: `CustomDatePicker` (com `DropDownDay`, `DropDownMonth`, `DropDownYear`) e `CustomComboBox`.

---

## 🧭 Estrutura do projeto (visão geral)

```
NhegazCustomControls/
├─ Core de desenho e medidas
│  ├─ NhegazDrawingMethods*.cs        # Paths e ícones (GDI+)
│  └─ NhegazSizeMethods.cs            # Medidas de fonte/texto
│
├─ Base de controles
│  ├─ CustomControl.cs                # Pipeline de desenho e input
│  ├─ CustomControlWithHeader.cs      # Suporte a cabeçalho
│  ├─ CustomControlPadding.cs         # Padding do controle (experimental)
│  ├─ InnerControlsBase.cs            # InnerControl + roteamento de eventos
│  ├─ InnerPadding.cs                 # Padding do InnerControl
│  ├─ InnerControlsEnums.cs           # Enums (shape, alinhamentos, etc.)
│  └─ InnerButton.cs                  # Botão vetorial (ícones)
│
├─ DatePicker e Dropdowns
│  ├─ CustomDatePicker.Core.cs / .Layout.cs
│  ├─ DropDownDay.cs                  # Com cabeçalho (navegação mês)
│  ├─ DropDownMonth.cs
│  └─ DropDownYear.cs
│
└─ ComboBox
   ├─ CustomComboBox.Core.cs
   └─ CustomComboBox.Layout.cs
```

---

## 🚀 Requisitos e instalação

- **.NET 6+ (Windows)** ou **.NET Framework 4.8** com Windows Forms.
- Adicione o projeto **NhegazCustomControls** à sua solução e crie uma **Project Reference**.
- (Opcional) Ative *High DPI* e estilos padrão da sua aplicação Windows Forms.

---

## 🧩 Como usar

### 1) `CustomDatePicker`

```csharp
using System.Collections.Specialized;
using NhegazCustomControls;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        var datePicker = new CustomDatePicker
        {
            Location = new Point(20, 20),
            PaddingMode = CustomControl.PaddingModeEnum.RelativeToFont,
            HeaderBackgroundColor = Color.SteelBlue,
            OnFocusBorderColor = Color.SteelBlue,
            BorderRadius = 8,
            BorderWidth = 1
        };

        Controls.Add(datePicker);
    }
}
```

### 2) `CustomComboBox`

```csharp
using System.Collections.Specialized;
using NhegazCustomControls;
// Ajuste o namespace do CustomComboBox se necessário no seu projeto.

var combo = new CustomComboBox
{
    Location = new Point(20, 70),
    PaddingMode = CustomControl.PaddingModeEnum.RelativeToFont,
    HeaderBackgroundColor = Color.MediumSlateBlue, // cor do hover no ícone
    BorderRadius = 8
};

var items = new StringCollection();
items.AddRange(new[] { "Item 1", "Item 2", "Item 3" });
combo.ItemList = items;
combo.SelectIndexText = "Item 1";

Controls.Add(combo);
```

> Dica: os *drop‑downs* são anexados dinamicamente ao `Form` pai e herdam tema, fonte e cores do controle principal, garantindo consistência visual.

---

## 🛠️ Customização rápida

- **Cores**: `BackgroundColor`, `ForeColor`, `HeaderBackgroundColor`, `BorderColor`, `OnFocusBorderColor`, `HoverColor`.
- **Bordas**: `BorderRadius`, `BorderWidth`.
- **Padding**: `PaddingMode` (`Absolute` ou `RelativeToFont`), `HorizontalPadding`, `VerticalPadding`.
- **Formatação**: use `NhegazSizeMethods.TextExactSize`, `TextProportionalSize` e `FontUnitSize` para dimensionar elementos pela fonte.
- **Ícones** (`InnerButton`): `ButtonIcon` (DropDown, Forward, Backward, Add…), `IconSizeMode` e `IconSizePercent`.

---

## 🔎 Como funciona (arquitetura)

**Pipeline de desenho**

1. `DrawBackGround(e)` — fundo com raio e recorte (*clip*).
2. `DrawHeader(e)` / `DrawHeaderElements(e)` — quando o controle possui cabeçalho.
3. `DrawInnerControls(e)` — pinta todos os `InnerControls`.
4. `DrawBorder(e)` — contorno, com destaque quando o controle está em foco.

**Roteamento de eventos**

- `OnMouseClick/DoubleClick/Move/GotFocus/LostFocus` do controle pai repassam para `InnerControls` (e para `HeaderControls`, quando houver cabeçalho). Cada `InnerControl` possui `HitBox`, estados de hover e _raise_ de eventos que disparam *handlers* registrados no controle pai.

**Desenho vetorial**

- Paths para borda e fundo são gerados por `NhegazDrawingMethods` (arcos, retângulos arredondados) e ícones (triângulos, “+”, etc.). Isso garante pixel‑perfection e anti‑aliasing quando necessário.

---

## 🧪 Exemplos prontos (idéias)

- **Calendário com navegação mensal**: `DropDownDay` utiliza `HeaderControls` para *Backward*/*Forward* e uma grade 6×7 de dias.
- **Seleção por década**: `DropDownYear` exibe década atual e seta *Backward*/*Forward* para trocar a faixa de anos.
- **ComboBox minimalista**: rótulo do item selecionado + botão *DropDown* com hover padronizado.

---

## ➕ Estendendo a biblioteca

1. **Herde** de `CustomControl` (ou `CustomControlWithHeader` se precisar de cabeçalho).
2. **Crie e registre** elementos via `InnerControls.Add(new InnerLabel/InnerButton/...)`.
3. **Override**:
   - `AdjustPadding()` (se usar `RelativeToFont`).
   - `AdjustInnerSizes()` / `AdjustInnerLocations()` (e suas variantes por índice ou grade) para layout.
   - `SetMinimumSize()` para garantir tamanho mínimo coerente.
4. **Desenho**: reutilize `NhegazDrawingMethods` para paths de fundo/borda/ícones.
5. **Eventos**: associe `Click`, `MouseEnter`, `MouseLeave` dos `InnerControls` para hover/ação sem *UserControl* aninhado.

> Padrão sugerido: mantenha **layout** (sizes/locations), **pintura** (desenho) e **interações** (eventos) bem separados e com nomes consistentes.

---

## 🗺️ Roadmap

- Finalizar `CustomDataGridView` com cabeçalho e colunas configuráveis.
- Unificar padding interno/externo (`CustomControlPadding` e `InnerControlPadding`).
- Acessibilidade (navegação por teclado/announce) e *focus visuals*.
- Localização (labels de meses/dias personalizados) e culturas.
- Empacotar como **NuGet** e melhorar o *designer support* no Visual Studio.

---

## 🤝 Contribuindo

- Use *issues* para bugs/ideias e *pull requests* com mudanças focadas.
- Siga o estilo de código atual (nomes claros, comentários XML em membros públicos, separação `*.Core.cs`/`*.Layout.cs` quando fizer sentido).

---

## 📄 Licença

Defina a licença do projeto (por exemplo, MIT). Se ainda não houver, inclua um arquivo `LICENSE`.

---

## 📚 Referências rápidas de API

- **Base**: `CustomControl` — propriedades de tema, `Adjust*` e pipeline de desenho.
- **Cabeçalho**: `CustomControlWithHeader` — `HeaderBounds`, `HeaderControls` e `DrawHeader*`.
- **Inner Controls**: `InnerControl`, `InnerControls`, `InnerButton`, `InnerControlPadding`, `BackGroundShape`.
- **Desenho**: `NhegazDrawingMethods` (paths, ícones) e `NhegazSizeMethods` (medidas).
- **Controles prontos**: `CustomDatePicker` (+ `DropDownDay`/`Month`/`Year`) e `CustomComboBox`.

---

> **Dica de manutenção:** Sempre que uma mudança afetar geometria (fonte, paddings, tamanho, colunas/linhas), concentre o *re‑layout* em `AdjustControlSize()` e nos `AdjustInner*` derivados, evitando recalcular em *setters* de propriedades.
