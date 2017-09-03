using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";

            List<string> validResults = new List<string>();

            LogicPropositionValidator.Validate(input);

            Regex checkExpression = new Regex(@"(^[&|>=]\(([A-Z0-1]{1}|\(.*\)),([A-Z0-1]{1}|\(*\))\)$)|(^[~]\(([A-Z0-1]{1}|\(*\))\))");
            Regex singlePredicatePattern = new Regex("^[A-Z0-1]{1}$");
            List<string> blocks = new List<string>();
            List<Token> parsedTokens = input.ParseLogicalProposition();

            Stack<Token> stackOfParenthesis = new Stack<Token>();
            foreach (Token token in parsedTokens)
            {
                if (token.IsOpeningParanthesis)
                {
                    stackOfParenthesis.Push(token);
                }
                if (token.IsClosingParanthesis)
                {
                    Token openingParenthesis = stackOfParenthesis.Pop();

                    int openingIndex = parsedTokens.IndexOf(openingParenthesis);
                    int closingIndex = parsedTokens.IndexOf(token) - openingIndex;
                    List<Token> symbolsBlock = parsedTokens.Skip(openingIndex - 1).Take(closingIndex + 2).ToList();
                    string stringBlock = input.Substring(openingIndex - 1, closingIndex + 2);
                    blocks.Add(stringBlock);

                    if (symbolsBlock[0].IsConnective == false)
                    {
                        break;
                    }
                    else
                    {

                        if (symbolsBlock[0].IsNegation)
                            // There should be only one Proposition after the negation
                        {
                            string withoutParanthesis = stringBlock.Substring(2, 1);
                            if (singlePredicatePattern.IsMatch(input))
                            {
                                continue;
                            }



                        }
                        else
                        {

                        }
                    }
                    // 
                }
            }
            foreach (var block in blocks)
            {
                Console.WriteLine(block);
            }

            Console.ReadLine();
        }

       

        private static void BFS(Node parentNode)
        {
            Queue q = new Queue();
            q.Enqueue(parentNode);
            while (q.Count > 0)
            {
                Node n = (Node) q.Dequeue();
                Console.WriteLine(n.Token.ToString());
                foreach (var nChild in n.Children)
                {
                    q.Enqueue(nChild);
                }
            }
        }
    }
}
