using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Symbols;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var properOutput = "&(&(=(A,B),>(&(A,B),~(C))),~(0))";
            //var nodes = properOutput.ParseLogicalProposition();
            var split = SplitLogicalProposition(properOutput);

            foreach (var str in split)
            {
                Console.WriteLine(str);
            }
            

            Console.ReadLine();
        }

        public static char GetConnective(List<Symbol> input) => input.First(x => x.IsConnective).Char;

        public static List<string> SplitLogicalProposition(string properOutput)
        {
            List<Symbol> input = properOutput.ParseLogicalProposition();
            Stack<Symbol> paranthesisStack = new Stack<Symbol>();
            List<Tuple<Symbol,Symbol>> paranthesisTuples = new List<Tuple<Symbol, Symbol>>();
            foreach (var symbol in input)
            {
                if (symbol.Type == SymbolType.OpeningParanthesis)
                {
                    paranthesisStack.Push(symbol);
                }
                else if (symbol.Type == SymbolType.ClosingParanthesis)
                {
                    Symbol openingParanthesis = paranthesisStack.Pop();
                    paranthesisTuples.Add(new Tuple<Symbol, Symbol>(openingParanthesis,symbol));
                }
            }
            List<string> groups = new List<string>();

            foreach (var paranthesisTuple in paranthesisTuples)
            {
                groups.Add(properOutput.Substring(input.IndexOf(paranthesisTuple.Item1) - 1,input.IndexOf(paranthesisTuple.Item2) - input.IndexOf(paranthesisTuple.Item1) + 2));
            }
            return groups;



        }

        public void BuildATree()
        {
            // Get Connective
            // new Node (Connective)

            // if (negation symbol), then no separator
            // Single node children
            // else
            // Add Node children

            // if
        }
    }
}
