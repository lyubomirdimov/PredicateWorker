using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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

            input = ">(&(A,B),~(C))";
            List<Token> parsedString = input.ParseLogicalProposition();
            


            Table table = new Table();

            // Construct header row
            List<Token> predicateTokens = parsedString.Where(x => x.IsPredicate && x.Type != TokenType.True && x.Type != TokenType.False).ToList();
            predicateTokens = predicateTokens.OrderBy(x => x.ToString()).ToList();
            TableRow headerRow = new TableRow();
            foreach (var headerToken in predicateTokens)
            {
                headerRow.Cells.Add(new TableCell() { Text = headerToken.ToString() });
            }
            // Add result cell at the end of header row
            headerRow.Cells.Add(new TableCell() { Text = "Result" });
            table.Rows.Add(headerRow); // Add header row

            // Data construction row by row
            int binaryLength = predicateTokens.Count;
            double numberOfRows = Math.Pow(2, predicateTokens.Count);
            for (int i = 0; i < numberOfRows; i++)
            {
                TableRow dataRow = new TableRow();
                Char[] binaryValue = ToBin(i, binaryLength).ToCharArray();
                foreach (var predValue in binaryValue)
                {
                    TableCell newTableCell = new TableCell(){Text = predValue.ToString()};
                    dataRow.Cells.Add(newTableCell);
                }
                // calculalte result
                Node propositionTree = TreeConstructor.ConstructTree(parsedString);
                foreach (var predicateToken in predicateTokens)
                {
                    // Replace
                    //BFS(propositionTree,predicateToken,);
                }
            }




            Console.ReadLine();

        }

        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }

        private static void BFS(Node parentNode, string predicate, TokenType replacement)
        {
            Queue q = new Queue();
            q.Enqueue(parentNode);
            while (q.Count > 0)
            {
                Node n = (Node)q.Dequeue();
                if (n.Token.ToString() == predicate)
                {
                    n.Token.Type = replacement;
                }
                foreach (var nChild in n.Children)
                {
                    q.Enqueue(nChild);
                }
            }
        }
    }
}
