using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class GeneralHelper
    {
      

        /// <summary>
        /// Convert integer value to Binary, with certain length
        /// </summary>
        /// <param name="value"> the integer value to be transformed</param>
        /// <param name="len"> length of the binary </param>
        /// <returns></returns>
        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }

    }
}
