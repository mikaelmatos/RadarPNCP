using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RadarLicitacoes.Util
{
    public class Textos
    {
        public static string RemoverTags(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, "<.*?>", string.Empty);
        }
        public static string UnificaSimbolosRepetidos(string simbolo, string texto)
        {
            while (texto.Contains(simbolo + "" + simbolo))
            {
                texto = texto.Replace(simbolo + "" + simbolo, simbolo);
            }

            return texto;
        }

    }
}
