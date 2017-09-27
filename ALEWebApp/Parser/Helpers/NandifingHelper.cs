using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Helpers
{
    public static class NandifingHelper
    {
        public static string Nandify(this string logProp)
        {
            List<Token> parsedLogProp = logProp.ParseLogicalProposition();
            Node tree = TreeConstructor.ConstructTree(parsedLogProp);
            // Substitute Implications and BiImplications

            // Recursively Nandify
            return "";
        }

        private static Node SubstituteImplications(Node tree)
        {
            
            
            return tree;
        }
    }
}
