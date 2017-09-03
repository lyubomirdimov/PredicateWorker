using System;
using System.Collections;
using System.Collections.Generic;
using Common.Symbols;

namespace Common
{
    public class Node 
    {
        private readonly Dictionary<Guid, Node> _children =
            new Dictionary<Guid, Node>();

        public readonly Guid ID;
        public Symbol Symbol { get; }  
        public Node Parent { get; private set; }

        public Node(Guid id,Symbol symbol)
        {
            this.ID = id;
            Symbol = symbol;
        }

        public Node GetChild(Guid id)
        {
            return this._children[id];
        }

        public void Add(Node item)
        {
            if (item.Parent != null)
            {
                item.Parent._children.Remove(item.ID);
            }

            item.Parent = this;
            this._children.Add(item.ID, item);
        }

        public Dictionary<Guid, Node> Children => _children;
      

        public int Count => this._children.Count;
    }
}
