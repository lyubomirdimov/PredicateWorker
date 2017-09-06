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


            //List<string> validResults = new List<string>();

            //LogicPropositionValidator.Validate(input);

            //Regex checkExpression = new Regex(@"(^[&|>=]\(([A-Z0-1]{1}|\(.*\)),([A-Z0-1]{1}|\(*\))\)$)|(^[~]\(([A-Z0-1]{1}|\(*\))\))");
            //Regex singlePredicatePattern = new Regex("^[A-Z0-1]{1}$");
            //List<string> blocks = new List<string>();
            //List<Token> parsedTokens = input.ParseLogicalProposition();

            //Stack<Token> stackOfParenthesis = new Stack<Token>();
            //foreach (Token token in parsedTokens)
            //{
            //    if (token.IsOpeningParanthesis)
            //    {
            //        stackOfParenthesis.Push(token);
            //    }
            //    if (token.IsClosingParanthesis)
            //    {
            //        Token openingParenthesis = stackOfParenthesis.Pop();

            //        int openingIndex = parsedTokens.IndexOf(openingParenthesis);
            //        int closingIndex = parsedTokens.IndexOf(token) - openingIndex;
            //        List<Token> symbolsBlock = parsedTokens.Skip(openingIndex - 1).Take(closingIndex + 2).ToList();
            //        string stringBlock = input.Substring(openingIndex - 1, closingIndex + 2);
            //        blocks.Add(stringBlock);

            //        if (symbolsBlock[0].IsConnective == false)
            //        {
            //            break;
            //        }
            //        else
            //        {

            //            if (symbolsBlock[0].IsNegation)
            //                // There should be only one Proposition after the negation
            //            {
            //                string withoutParanthesis = stringBlock.Substring(2, 1);
            //                if (singlePredicatePattern.IsMatch(input))
            //                {
            //                    continue;
            //                }



            //            }
            //            else
            //            {

            //            }
            //        }
            //        // 
            //    }
            //}
            //foreach (var block in blocks)
            //{
            //    Console.WriteLine(block);
            //}

            //Console.ReadLine();

            Regex r = new Regex(
                @"^                             # Start of line
                  (                             # Either
                      [01A-Z]|                  # A Predicate
                      (?<dyadic>[|&>=]\((?!,))| # Start of dyadic
                      (?<comma-dyadic>,(?!\)))| # Looks for comma followed by dyadic. Pops off the dyadic stack.
                      (?<dBracket-comma>\))|    # Looks for ending bracket following comma. pops off comma stack. 
                      (?<monadic>~\((?!\)))|    # Start of monadic function.
                      (?<uBracket-monadic>\)))  # Looks for ending bracket for unary. Pops off the monadic stack. 
                  +                             # Any number of times.
                  (?(dyadic)(?!))               # Assert dyadic stack is empty. All have a comma.
                  (?(comma)(?!))                # Assert comma stack is empty. All dyadic commas followed by brackets.
                  (?(monadic)(?!))              # Assert monadic stack is empty. All monadic expressions have closing brackets.
                  $ "
                , RegexOptions.IgnorePatternWhitespace);

            // ^([01A-Z]|(?<dyadic>[|&>=]\()|(?<comma-dyadic>\,)|(?<dBracket-comma>\))|(?<unary>~\()|(?<uBracket-unary>\)))+(?(dyadic)(?!))(?(comma)(?!))(?(unary)(?!))$
            string target = @"&(A,|(B,C)";
            string[] passList = {
                @"&(A,B)",
                @"~(0)",
                @"&(A,~(B))",
                @">(~(=(D,A)),~(B))",
                @"T",
                @"&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
                @"1",
                @"0",
                @"~(A)"
            };
            for (int i = 0; i < passList.Length; i++)
            {
                if (r.Match(passList[i]).Success)
                {
                    Console.WriteLine("String: " + passList[i] + " Matches.");
                }
                else
                {
                    Console.WriteLine("Match expected but failed");
                }
            }

            string[] failList = {
                @"&(A,B",
                @"~(A,B)",
                @"&(A,|(B))",
                @">(~(=(D,A),~(B))",
                @">",
                @"~",
                @"=",

            };

            for (int i = 0; i < failList.Length; i++)
            {
                if (r.Match(failList[i]).Success)
                {
                    Console.WriteLine("String: " + failList[i] + " matches but should not.");
                }
                else
                {
                    Console.WriteLine("No match as expected");
                }
            }
            Console.ReadLine();
        }



        private static void BFS(Node parentNode)
        {
            Queue q = new Queue();
            q.Enqueue(parentNode);
            while (q.Count > 0)
            {
                Node n = (Node)q.Dequeue();
                Console.WriteLine(n.Token.ToString());
                foreach (var nChild in n.Children)
                {
                    q.Enqueue(nChild);
                }
            }
        }
    }
}
