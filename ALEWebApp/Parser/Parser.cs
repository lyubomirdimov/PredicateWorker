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



    }
}
