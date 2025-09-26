using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhegazCustomControls
{
    public static class NhegazCultureMethods
    {
        private static readonly string[] DefaultWeekDayLetters = { "D", "S", "T", "Q", "Q", "S", "S" };

        /// <summary>
        /// Retorna abreviações de dias da semana com base na cultura atual do sistema.
        /// Caso a cultura não seja acessível, retorna padrão brasileiro "D S T Q Q S S".
        /// </summary>
        public static string[] GetCultureWeekdayLettersOrDefault()
        {
            try
            {
                // Ex.: "dom.", "seg.", "ter.", "qua.", "qui.", "sex.", "sáb."
                var abbr = CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedDayNames;

                if (abbr?.Length == 7)
                {
                    var oneLetter = new string[7];
                    for (int i = 0; i < 7; i++)
                    {
                        // Pega 1ª letra (maiúscula). Mantém fallback do default se vazio/nulo
                        var s = abbr[i];
                        oneLetter[i] = string.IsNullOrWhiteSpace(s)
                            ? DefaultWeekDayLetters[i]
                            : s.Substring(0, 1).ToUpperInvariant();
                    }
                    return oneLetter;
                }
            }
            catch { /* ignora e cai no default */}

            return DefaultWeekDayLetters;
        }
    }
}
