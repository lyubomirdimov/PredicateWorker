using System;

namespace Common.Models
{

    /// <summary>
    /// Token type represents the equivalent logical proposition statement for certain token
    /// Token Type accepts Connectives, Predicates, Separator, and Paranthesis
    /// </summary>
    public enum TokenType
    {
        // Connective - &
        And,

        // Connective - |
        Or,

        // Connective - ~
        Negation,

        // Connective - >
        Implication,

        // Connective - =
        BiImplication,

        // NAND
        Nand,

        // Comma - ,
        Separator,

        // Any English language letter
        Predicate,

        // 1
        True,

        // 0
        False,

        // (
        OpeningParanthesis,

        // )
        ClosingParanthesis,

    }

    /// <summary>
    /// Used for parsing a Ascii string   
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }
        public char Char { get; private set; }
        public bool IsConnective { get; } = false;
        public bool IsParanthesis { get; } = false;
        public bool IsPredicate { get; } = false;
        public bool IsTrueOrFalse { get; set; } = false;
        public bool IsValid { get; set; } = true;
        public bool IsNegation => Type == TokenType.Negation;
        public bool IsOpeningParanthesis => Type == TokenType.OpeningParanthesis;
        public bool IsClosingParanthesis => Type == TokenType.ClosingParanthesis;
        public bool IsSeparator => Type == TokenType.Separator;

        public Token(char? c)
        {
            if (c == null)
            {
                IsValid = false;
            }
            else
            {
                switch (c)
                // Define the type of the Character
                {
                    case '~':
                        Type = TokenType.Negation;
                        IsConnective = true;
                        break;
                    case '&':
                        Type = TokenType.And;
                        IsConnective = true;
                        break;
                    case '|':
                        Type = TokenType.Or;
                        IsConnective = true;
                        break;
                    case '>':
                        Type = TokenType.Implication;
                        IsConnective = true;
                        break;
                    case '=':
                        Type = TokenType.BiImplication;
                        IsConnective = true;
                        break;
                    case '%':
                        Type = TokenType.Nand;
                        IsConnective = true;
                        break;
                    case ',':
                        Type = TokenType.Separator;
                        break;
                    case '(':
                        Type = TokenType.OpeningParanthesis;
                        IsParanthesis = true;
                        break;
                    case ')':
                        Type = TokenType.ClosingParanthesis;
                        IsParanthesis = true;
                        break;
                    case '0':
                        Type = TokenType.False;
                        IsTrueOrFalse = true;
                        break;
                    case '1':
                        Type = TokenType.True;
                        IsTrueOrFalse = true;
                        break;
                    default:
                        Type = TokenType.Predicate;
                        IsPredicate = true;
                        break;
                }
                Char = (char)c;
            }
        }


        public override string ToString() => Char.ToString();

        public string ToInfixString()
        {
            switch (Type)
            {
                case TokenType.And:
                    return "∧";
                case TokenType.Or:
                    return "∨";
                case TokenType.Negation:
                    return "¬";
                case TokenType.Implication:
                    return "⇒";
                case TokenType.BiImplication:
                    return "⇔";
                case TokenType.Nand:
                    return "⊼";
                case TokenType.Predicate:
                    return Char.ToString();
                case TokenType.True:
                    return "True";
                case TokenType.False:
                    return "False";
                case TokenType.Separator:
                case TokenType.OpeningParanthesis:
                case TokenType.ClosingParanthesis:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
