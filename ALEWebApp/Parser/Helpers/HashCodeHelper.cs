using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Helpers
{
    public static class HashCodeHelper
    {
        public static string BinaryStringToHexString(this string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static string TableSchemeToHashCode(this TableScheme tblScheme)
        {

            string binaryValue = string.Empty;
            binaryValue = tblScheme.DataRows.Aggregate(binaryValue, (current, row) => current + (row.Result ? "1" : "0"));
            binaryValue = binaryValue.Reverse();
            
            return binaryValue.BinaryStringToHexString();
        }
    }
}
