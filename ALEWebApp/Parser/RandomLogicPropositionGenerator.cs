using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Class for generating random logic propositions
    /// </summary>
    public static class RandomLogicPropositionGenerator
    {
        private enum Connectives
        {
            And,
            Or,
            Implication,
            BiImplication,
            Negation
        }
        public static string GenerateRandomProposition()
        {
            Random random = new Random();
            int size = random.Next(20);
            Array values = Enum.GetValues(typeof(Connectives));
            Connectives randomConnective = (Connectives)values.GetValue(random.Next(values.Length));

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                randomConnective = (Connectives) values.GetValue(random.Next(values.Length));
                //str.Append(pl)
            }



            return string.Empty;
        }

     
    }
}
