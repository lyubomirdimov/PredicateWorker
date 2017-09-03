using System;
using System.Collections;
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
            var input = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";
            //var input = "&(A,B)";
            //var nodes = properOutput.ParseLogicalProposition();
            //var split = SplitLogicalProposition(input);


            //foreach (var str in split)
            //{
            //    Console.WriteLine(str);
            //}

            List<Symbol> parsedString = input.ParseLogicalProposition();
            List<Tuple<Symbol, Symbol>> tuples = GetTuplesBetweenParantheses(parsedString);
            Tuple<Symbol, Symbol> upperMostExpression = tuples[tuples.Count - 1];

            Node parentNode = null;
            for (int i = 0; i < parsedString.Count; i++)
            {
                Symbol currentSymbol = parsedString[i];
                switch (currentSymbol.Type)
                {
                    case SymbolType.And:
                    case SymbolType.Or:
                    case SymbolType.Negation:
                    case SymbolType.Implication:
                    case SymbolType.BiImplication:
                        Node newNode = new Node(Guid.NewGuid(), currentSymbol);
                        if (parentNode == null)
                        {
                            parentNode = newNode;
                        }
                        else
                        {
                            parentNode.Add(newNode);
                            parentNode = newNode;
                        }
                        break;
                    
                    case SymbolType.Predicate:
                    case SymbolType.True:
                    case SymbolType.False:
                        newNode = new Node(Guid.NewGuid(), currentSymbol);
                        if (parentNode == null)
                        {
                            parentNode = newNode;
                        }
                        else
                        {
                            parentNode.Add(newNode);

                        }
                        break;
                        
                    case SymbolType.OpeningParanthesis:
                    case SymbolType.Separator:
                        break;
                    case SymbolType.ClosingParanthesis:
                        if(parentNode.Parent != null) parentNode = parentNode.Parent;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (parentNode != null)
            {
                Queue q = new Queue();
                q.Enqueue(parentNode);
                while (q.Count >0)
                {
                    Node n = (Node)q.Dequeue();
                    Console.WriteLine(n.Symbol.ToString());
                    foreach (var nChild in n.Children)
                    {
                        q.Enqueue(nChild.Value);
                    }
                }
            }




            Console.ReadLine();
        }
       
        


        public static List<Tuple<Symbol, Symbol>> GetTuplesBetweenParantheses(List<Symbol> parsedString)
        {
            Stack<Symbol> paranthesisStack = new Stack<Symbol>();
            List<Tuple<Symbol, Symbol>> paranthesisTuples = new List<Tuple<Symbol, Symbol>>();
            foreach (var symbol in parsedString)
            {
                if (symbol.IsConnective)
                {
                    // new node
                }
                if (symbol.Type == SymbolType.OpeningParanthesis)
                {
                    paranthesisStack.Push(symbol);
                }
                else if (symbol.Type == SymbolType.ClosingParanthesis)
                {
                    Symbol openingParanthesis = paranthesisStack.Pop();
                    paranthesisTuples.Add(new Tuple<Symbol, Symbol>(openingParanthesis, symbol));
                }
            }
            return paranthesisTuples;
        }



        public static List<string> SplitLogicalProposition(string properOutput)
        {
            List<Symbol> input = properOutput.ParseLogicalProposition();
            Stack<Symbol> paranthesisStack = new Stack<Symbol>();
            List<Tuple<Symbol, Symbol>> paranthesisTuples = new List<Tuple<Symbol, Symbol>>();
            foreach (var symbol in input)
            {
                if (symbol.IsConnective)
                {
                    // new node
                }
                if (symbol.Type == SymbolType.OpeningParanthesis)
                {
                    paranthesisStack.Push(symbol);
                }
                else if (symbol.Type == SymbolType.ClosingParanthesis)
                {
                    Symbol openingParanthesis = paranthesisStack.Pop();
                    paranthesisTuples.Add(new Tuple<Symbol, Symbol>(openingParanthesis, symbol));
                }
            }

            Tuple<Symbol, Symbol> upperMostExpression = paranthesisTuples[paranthesisTuples.Count - 1];
            List<Symbol> symbols = input.Skip(input.IndexOf(upperMostExpression.Item1) - 1).Take(input.IndexOf(upperMostExpression.Item2) - input.IndexOf(upperMostExpression.Item1) + 2).ToList();
            Symbol conn = symbols[0];
            Node rootNode = new Node(Guid.NewGuid(), conn);
            Tuple<Symbol, Symbol> branch1 = paranthesisTuples[paranthesisTuples.Count - 2];
            Tuple<Symbol, Symbol> branch2 = paranthesisTuples[paranthesisTuples.Count - 3];

            List<Symbol> symbols1 = input.Skip(input.IndexOf(branch1.Item1) - 1).Take(input.IndexOf(branch1.Item2) - input.IndexOf(branch1.Item1) + 2).ToList();
            List<Symbol> symbols2 = input.Skip(input.IndexOf(branch2.Item1) - 1).Take(input.IndexOf(branch2.Item2) - input.IndexOf(branch2.Item1) + 2).ToList();
            if (conn.IsNegation)
            {
                // One branch only

            }
            else
            {
                //Two branches
            }
            List<string> groups = new List<string>();

            List<List<Symbol>> grrs = new List<List<Symbol>>();
            for (var index = paranthesisTuples.Count - 1; index > 0; index--)
            {
                var paranthesisTuple = paranthesisTuples[index];
                groups.Add(properOutput.Substring(input.IndexOf(paranthesisTuple.Item1) - 1, input.IndexOf(paranthesisTuple.Item2) - input.IndexOf(paranthesisTuple.Item1) + 2));
                grrs.Add(input.Skip(input.IndexOf(paranthesisTuple.Item1) - 1).Take(input.IndexOf(paranthesisTuple.Item2) - input.IndexOf(paranthesisTuple.Item1) + 2).ToList());
            }

            Node root = new Node(Guid.NewGuid(), groups[0].GetConnective());
            if (groups[0].GetConnective().IsNegation)
            {
                // only one branch

            }
            else
            {
                // two branches
            }


            Node refNode = null;
            for (int i = paranthesisTuples.Count - 1; i > 0; i--)
            {
                Tuple<Symbol, Symbol> paranthesisTuple = paranthesisTuples[i];
                // Get Connective
                Symbol connective = input[input.IndexOf(paranthesisTuple.Item1) - 1];
                Node newNode = new Node(Guid.NewGuid(), connective);
                if (refNode == null)
                {
                    refNode = newNode;
                }
                else
                {
                    if (refNode.Symbol.IsNegation)
                    {
                        // Only one child

                    }
                    else
                    {
                        // Two children
                    }
                }
            }
            // else two children
            return groups;

        }

        //public static Node RecursiveTreeBuild(string input)
        //{
        //    List<Symbol> parsedString = input.ParseLogicalProposition();
        //    if (parsedString[0].IsConnective)
        //    {
        //        // new node and recurs
        //    }
        //    if (parsedString[0].IsPredicate)
        //    {
        //        // return
        //    }

        //}


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
