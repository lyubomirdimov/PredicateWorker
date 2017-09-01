using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class Node : IEnumerable<Node>
    {
        private readonly Dictionary<string, Node> _children =
            new Dictionary<string, Node>();

        public readonly string ID;
        public Node Parent { get; private set; }

        public Node(string id)
        {
            this.ID = id;
        }

        public Node GetChild(string id)
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

        public IEnumerator<Node> GetEnumerator()
        {
            return this._children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this._children.Count;
    }
}
