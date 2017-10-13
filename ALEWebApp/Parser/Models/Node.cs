using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Helpers;

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



        /// <summary>
        /// Recursively calculates the value of a row in a table
        /// </summary>
        /// <param name="tree"> Tree constructed from the logical proposition</param>
        /// <param name="substitutionTokens"> Tuple list, which replaces predicates in the tree with appropriate true/false values</param>
        /// <returns></returns>
        public bool CalculatePropositionBySubstitution(List<Tuple<Token, bool>> substitutionTokens)
        {
            bool value = false;
            if (Token.IsConnective)
            {
                // Recusively get boolean values of the children
                List<bool> booleanResults = new List<bool>();
                foreach (Node child in Children)
                {
                    booleanResults.Add(child.CalculatePropositionBySubstitution(substitutionTokens)); // Recursive call
                }

                // Calculate value based on the conective node
                switch (Token.Type)
                {
                    case TokenType.And:
                        value = booleanResults[0] & booleanResults[1];    // P And Q
                        break;
                    case TokenType.Or:
                        value = booleanResults[0] | booleanResults[1];    // P or Q 
                        break;
                    case TokenType.Negation:
                        value = !booleanResults[0];                       // Not P
                        break;
                    case TokenType.Implication:
                        value = !booleanResults[0] | booleanResults[1];   // not P or Q
                        break;
                    case TokenType.BiImplication:
                        value = booleanResults[0] == booleanResults[1];   // P <=> Q
                        break;
                    case TokenType.Nand:
                        value = !(booleanResults[0] & booleanResults[1]); // Not P And Q
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (Token.IsTrueOrFalse)
            {
                if (Token.Type == TokenType.True) value = true;
                if (Token.Type == TokenType.False) value = false;
            }
            if (Token.IsPredicate)
            {
                // Replace a predicate from the tree with a boolean
                Tuple<Token, bool> replacementTuple = substitutionTokens.FirstOrDefault(x => x.Item1.Char == Token.Char);

                value = replacementTuple?.Item2 ?? throw new ArgumentNullException();
            }
            return value;
        }


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



        public string Nandify()
        {
            try
            {
                string result;
                Node A = null;
                Node B = null;
                switch (Token.Type)
                {
                    case TokenType.And:
                        A = Children[0];
                        B = Children[1];
                        result = $"%(%({A.Nandify()},{B.Nandify()}),%({A.Nandify()},{B.Nandify()}))"; // ( A ⊼ B ) ⊼ ( A ⊼ B )
                        break;
                    case TokenType.Or:
                        A = Children[0];
                        B = Children[1];
                        result = $"%(%({A.Nandify()},{A.Nandify()}),%({B.Nandify()},{B.Nandify()}))"; //( A ⊼ A ) ⊼ ( B ⊼ B )
                        break;
                    case TokenType.Negation:
                        A = Children[0];
                        result = $"%({A.Nandify()},{A.Nandify()})"; // A ⊼ A
                        break;
                    case TokenType.Implication:
                        A = Children[0];
                        B = Children[1];
                        result = $"%({A.Nandify()},%({B.Nandify()},{B.Nandify()}))"; 
                        break;
                    case TokenType.BiImplication:
                        A = Children[0];
                        B = Children[1];
                        result = $"%(" +
                                 $"%(%(%({A.Nandify()},{B.Nandify()}),%({A.Nandify()},{B.Nandify()})),%(%({A.Nandify()},{B.Nandify()}),%({A.Nandify()},{B.Nandify()})))" +
                                 $"," +
                                 $"%(" +
                                 $"%(%(%({A.Nandify()},{A.Nandify()}),%({B.Nandify()},{B.Nandify()})),%(%({A.Nandify()},{A.Nandify()}),%({B.Nandify()},{B.Nandify()})))" +
                                 $"," +
                                 $"%(%(%({A.Nandify()},{A.Nandify()}),%({B.Nandify()},{B.Nandify()})),%(%({A.Nandify()},{A.Nandify()}),%({B.Nandify()},{B.Nandify()})))" +
                                 $"))";
                        // (((A⊼A)⊼(B⊼B))⊼((A⊼A)⊼(B⊼B)))*(((A⊼A)⊼(B⊼B))
                        // ⊼
                        // ((A⊼A)⊼(B⊼B)))⊼(((A⊼B)⊼(A⊼B))*((A⊼B)⊼(A⊼B)))
                        break;
                    case TokenType.Nand:
                        A = Children[0];
                        B = Children[1];
                        result = $"%({A.Nandify()},{B.Nandify()})";
                        break;
                    case TokenType.Predicate:
                    case TokenType.True:
                    case TokenType.False:
                        return Token.ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }
            catch (OutOfMemoryException ex)
            {
                throw;
            }

        }
    }
}
