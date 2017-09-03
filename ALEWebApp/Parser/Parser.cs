using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Symbols;

namespace Common
{
    

    public static class Parser
    {

        public static string RemoveWhiteSpaces(this string input)
        {
            string pattern = "\\s+";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            return rgx.Replace(input, replacement);
        }

        public static List<Symbol> ParseLogicalProposition(this string input) => input.ToCharArray().Select(c => new Symbol(c)).ToList();

        /// <summary>
        /// Gets connective of a Ascii logical proposition string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Symbol GetConnective(this string input) => HasConnective(input) ? new Symbol(input.ToCharArray(0, 1).FirstOrDefault()) : new Symbol(null);

        public static bool HasConnective(this string input) => new Symbol(input.ToCharArray(0, 1).FirstOrDefault()).IsConnective;
    }
}
