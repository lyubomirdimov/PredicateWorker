using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Common.Helpers
{

    public static class ParsingHelper
    {

        /// <summary>
        /// Parses a Logical ASCII string Proposition to a List of Tokens
        /// </summary>
        /// <param name="input">Logical Ascii string proposition, which is parsed to a List of tokens</param>
        public static List<Token> ParseLogicalProposition(this string input) => input.ToCharArray().Select(c => new Token(c)).ToList();
      
    }
}
