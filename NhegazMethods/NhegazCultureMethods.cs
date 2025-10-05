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
        private static readonly string[] DefaultMonthAbbr3 =
        {
            "Jan","Fev","Mar","Abr","Mai","Jun","Jul","Ago","Set","Out","Nov","Dez"
        };

        /// <summary>
        /// Retorna abreviações (3 letras) dos meses conforme a cultura atual.
        /// Se não for possível, retorna "Jan..Dez" em PT-BR.
        /// </summary>
        public static string[] GetCultureMonthAbbr3OrDefault()
        {
            try
            {
                var dtf = CultureInfo.CurrentUICulture.DateTimeFormat;
                var abbr = dtf.AbbreviatedMonthNames;

                if (abbr?.Length >= 12)
                {
                    var months = new string[12];
                    var ti = CultureInfo.CurrentUICulture.TextInfo;

                    for (int i = 0; i < 12; i++)
                    {
                        var s = abbr[i];
                        if (string.IsNullOrWhiteSpace(s))
                            s = dtf.MonthNames != null && dtf.MonthNames.Length > i ? dtf.MonthNames[i] : null;

                        if (string.IsNullOrWhiteSpace(s))
                        {
                            months[i] = DefaultMonthAbbr3[i];
                            continue;
                        }

                        s = s.Replace(".", "").Trim();
                        s = s.Length >= 3 ? s[..3] : s;
                        months[i] = ti.ToTitleCase(s.ToLower());
                    }
                    return months;
                }
            }
            catch { /* cai no default */ }

            return DefaultMonthAbbr3;
        }
    }
}
