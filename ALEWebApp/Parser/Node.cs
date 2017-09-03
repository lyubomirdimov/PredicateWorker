using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class Node 
    {
        private readonly List<Node> _children =
            new List<Node>();

        public readonly Guid Id;
        public Symbol Symbol { get; }  
        public Node Parent { get; private set; }

        public Node(Guid id,Symbol symbol)
        {
            this.Id = id;
            Symbol = symbol;
        }

        public Node GetChild(Guid id)
        {
            return this._children.FirstOrDefault(x=>x.Id == id);
        }

        public void Add(Node item)
        {
            if (item.Parent != null)
            {
                item.Parent._children.Remove(item);
            }

            item.Parent = this;
            this._children.Add(item);
        }

        public List<Node> Children => _children;
      

        public int Count => this._children.Count;
    }
}
