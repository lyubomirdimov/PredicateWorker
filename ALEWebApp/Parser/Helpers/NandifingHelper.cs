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
            return "";
        }
    }
}
