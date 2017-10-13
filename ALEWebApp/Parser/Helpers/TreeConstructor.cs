using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Common.Helpers
{
    public static class TreeConstructor
    {
        /// <summary>
        /// Constructs a binary Node tree from a parsed ASCII Logical Proposition
        /// </summary>
        /// <param name="parsedString">Parsed ASCII Logical proposition; List of tokens</param>
        /// <returns>Root Node of a binary tree</returns>
        public static Node ConstructTree(List<Token> parsedString)
        {
            Node parentNode = null; // Used as a reference in each iteration

            foreach (Token currentSymbol in parsedString)
            {
                // The loop skips Separators(commas) and opening paranthesis

                if (currentSymbol.IsConnective)
                // Connectives always create nodes with either one or two children
                {
                    Node newNode = new Node(Guid.NewGuid(), currentSymbol);
                    if (parentNode == null)
                    // If there is no existing node, then this is the Root, namely the upper most Node
                    {
                        parentNode = newNode;
                    }
                    else
                    // add the connective as a child of the upper Node (parent node),
                    // and assign the child node as the new reference to parent node.
                    {
                        parentNode.Add(newNode);
                        parentNode = newNode;
                    }
                }
                else if (currentSymbol.IsPredicate || currentSymbol.IsTrueOrFalse)
                // Predicates are always leaves, they don't have children
                {
                    Node newNode = new Node(Guid.NewGuid(), currentSymbol);
                    if (parentNode == null)
                    // If the tree is made up of only a single Predicate
                    {
                        parentNode = newNode;
                    }
                    else
                    {
                        parentNode.Add(newNode);
                    }
                }
                else if (currentSymbol.IsClosingParanthesis)
                // Closing paranthesis signals that the construction should continue in reverse order, going back to it's parent
                {
                    if (parentNode?.Parent != null) parentNode = parentNode.Parent;
                }
            }
            return parentNode;
        }



        public static Node ConstructRandomTree()
        {
            int maxLevel = GenerateRandomNumber(100);
            int counter = 0;
            Node tree = RecurConstRndTree(ref counter, maxLevel);
            return tree;
        }

        private static Node RecurConstRndTree(ref int counter, int max)
        {
            if (counter == max)
            {
                List<char> chars = "10PQRXS".ToList();
                int r = GenerateRandomNumber(chars.Count);
                return new Node(Guid.NewGuid(), new Token(chars[r]));
            }
            counter++;
            Node node = new Node(Guid.NewGuid(), RandomToken());

            if (node.Token.IsConnective)
            {
                if (node.Token.IsNegation)
                {
                    node.Add(RecurConstRndTree(ref counter,max));
                }
                else
                {
                    node.Add(RecurConstRndTree(ref counter, max));
                    node.Add(RecurConstRndTree(ref counter, max));
                }
            }
            if (node.Token.IsPredicate || node.Token.IsTrueOrFalse)
            {
                return node;
            }
            return node;
        }

        private static Token RandomToken()
        {
            List<char> chars = "~&|>=%10PQRXS~&|>=%".ToList();
            int r = GenerateRandomNumber(chars.Count);
            return new Token(chars[r]);
        }


        private static Random random;
        private static object syncObj = new object();
        private static int GenerateRandomNumber(int max)
        {
            lock (syncObj)
            {
                if (random == null)
                    random = new Random(); // Or exception...
                return random.Next(max);
            }
        }

    }
}
