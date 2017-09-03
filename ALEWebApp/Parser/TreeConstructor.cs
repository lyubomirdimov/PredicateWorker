using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Symbols;

namespace Common
{
    public static class TreeConstructor
    {
        public static Node ConstructTree(List<Symbol> parsedString)
        {
            Node parentNode = null;
            foreach (Symbol currentSymbol in parsedString)
            {
                if (currentSymbol.IsConnective)
                {
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
                }
                else if (currentSymbol.IsPredicate)
                {
                    Node newNode = new Node(Guid.NewGuid(), currentSymbol);
                    if (parentNode == null)
                    {
                        parentNode = newNode;
                    }
                    else
                    {
                        parentNode.Add(newNode);
                    }
                }
                else if (currentSymbol.IsClosingParanthesis)
                {
                    if (parentNode?.Parent != null) parentNode = parentNode.Parent;
                }
            }
            return parentNode;
        }
    }
}
