using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NhegazCustomControls
{
    public partial class InnerTextBox
    {
        private void CopyText()
        {
            if (HasSelection)
            {
                int start = Math.Min(selectionStartIndex, selectionEndIndex);
                int length = Math.Max(selectionStartIndex, selectionEndIndex) - start;
                if (length > 0)
                {
                    string selectedText = Text.Substring(start, length);
                    if (!string.IsNullOrEmpty(selectedText)) Clipboard.SetText(selectedText);
                }
            }
        }

        private void CutText()
        {
            if (HasSelection)
            {
                int start = Math.Min(selectionStartIndex, selectionEndIndex);
                int length = Math.Max(selectionStartIndex, selectionEndIndex) - start;
                if (length > 0)
                {
                    string selectedText = Text.Substring(start, length);
                    if (!string.IsNullOrEmpty(selectedText)) Clipboard.SetText(selectedText);
                    DeleteSelection();            //já faz PushUndoState internamente
                }
            }
        }

        private void PasteText()
        {
            string clipText = Clipboard.ContainsText() ? Clipboard.GetText() ?? string.Empty : string.Empty;
            if (clipText.Length > 0)
            {
                // Aplica CharFilter (se houver) e remove caracteres de controle (quebra de linha etc.)
                var sb = new StringBuilder();
                foreach (char c in clipText)
                {
                    if (char.IsControl(c)) continue;
                    if (CharFilter == null || CharFilter(c)) sb.Append(c);
                }
                clipText = sb.ToString();

                if (clipText.Length > 0)
                {
                    int selLen = HasSelection ? Math.Abs(selectionEndIndex - selectionStartIndex) : 0;
                    int currentLen = Text.Length;
                    int remaining = MaxLength > 0 ? MaxLength - (currentLen - selLen) : int.MaxValue;

                    if (remaining > 0)
                    {
                        if (MaxLength > 0 && clipText.Length > remaining)
                        { clipText = clipText.Substring(0, remaining); }

                        if (clipText.Length > 0)
                        {
                            if (HasSelection) DeleteSelection();    //PushUndoState já chamado aqui
                            else PushUndoState();

                            Text = Text.Insert(CaretIndex, clipText);
                            CaretIndex += clipText.Length;
                        }
                    }
                }
            }
        }
    }
}
