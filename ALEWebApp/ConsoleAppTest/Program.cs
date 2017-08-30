using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "   Hellooo    W   oorl   d";
            Console.WriteLine(input);
            
            input = Parser.Parser.RemoveWhiteSpaces(input);
            Console.WriteLine(input);
            Console.ReadLine();
        }
    }
}
