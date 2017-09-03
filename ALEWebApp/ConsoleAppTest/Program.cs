using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            LogicStatementValidator.CheckInitialValidSymbol(input,validResults);


            //List<Symbol> parsedString = input.ParseLogicalProposition();

            //Node parentNode = TreeConstructor.ConstructTree(parsedString);

            //if (parentNode != null)
            //{
            //    BFS(parentNode);
            //}


            Console.ReadLine();
        }

       

        private static void BFS(Node parentNode)
        {
            Queue q = new Queue();
            q.Enqueue(parentNode);
            while (q.Count > 0)
            {
                Node n = (Node) q.Dequeue();
                Console.WriteLine(n.Symbol.ToString());
                foreach (var nChild in n.Children)
                {
                    q.Enqueue(nChild);
                }
            }
        }
    }
}
