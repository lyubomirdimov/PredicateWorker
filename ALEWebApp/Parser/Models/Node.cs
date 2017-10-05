﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{
    public class Node
    {
        private readonly List<Node> _children =
            new List<Node>();

        public readonly Guid Id;
        public Token Token { get; }
        public Node Parent { get; private set; }

        public Node(Guid id, Token token)
        {
            this.Id = id;
            Token = token;
        }

        public Node GetChild(Guid id)
        {
            return this._children.FirstOrDefault(x => x.Id == id);
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

        public string ToInfixNotation()
        {
            switch (Token.Type)
            {
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Implication:
                case TokenType.BiImplication:
                case TokenType.Nand:
                    StringBuilder str = new StringBuilder();
                    str.Append("(");
                    str.Append(Children[0].ToInfixNotation());
                    str.Append($" {Token.ToInfixString()} ");
                    str.Append(Children[1].ToInfixNotation());
                    str.Append(")");
                    return str.ToString();
                case TokenType.Negation:
                    return $"{Token.ToInfixString()}({Children[0].ToInfixNotation()})";
                case TokenType.Predicate:
                case TokenType.True:
                case TokenType.False:
                    return Token.ToInfixString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string ToPrefixNotation()
        {
            switch (Token.Type)
            {
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Implication:
                case TokenType.BiImplication:
                case TokenType.Nand:
                    StringBuilder str = new StringBuilder();
                    str.Append($"{Token}");
                    str.Append("(");
                    str.Append(Children[0].ToPrefixNotation());
                    str.Append(",");
                    str.Append(Children[1].ToPrefixNotation());
                    str.Append(")");
                    return str.ToString();
                case TokenType.Negation:
                    return $"{Token}({Children[0].ToPrefixNotation()})";
                case TokenType.Predicate:
                case TokenType.True:
                case TokenType.False:
                    return Token.ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
