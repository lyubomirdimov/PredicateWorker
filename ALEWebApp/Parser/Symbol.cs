using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Symbols
{
    public enum SymbolType
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
    public class Symbol
    {
        public SymbolType Type { get; set; }
        public char Char { get; }
        public bool IsConnective { get; } = false;
        public bool IsParanthesis { get; } = false;
        public bool IsPredicate { get; } = false;
        public bool IsValid { get; set; } = true;
        public bool IsNegation => Type == SymbolType.Negation;
        public bool IsClosingParanthesis => Type == SymbolType.ClosingParanthesis;

        public Symbol(char? c)
        {
            if (c == null)
            {
                IsValid = false;
            }
            else
            {
                switch (c)
                {
                    case '~':
                        Type = SymbolType.Negation;
                        IsConnective = true;
                        break;
                    case '&':
                        Type = SymbolType.And;
                        IsConnective = true;
                        break;
                    case '|':
                        Type = SymbolType.Or;
                        IsConnective = true;
                        break;
                    case '>':
                        Type = SymbolType.Implication;
                        IsConnective = true;
                        break;
                    case '=':
                        Type = SymbolType.BiImplication;
                        IsConnective = true;
                        break;
                    case ',':
                        Type = SymbolType.Separator;
                        break;
                    case '(':
                        Type = SymbolType.OpeningParanthesis;
                        IsParanthesis = true;
                        break;
                    case ')':
                        Type = SymbolType.ClosingParanthesis;
                        IsParanthesis = true;
                        break;
                    case '0':
                        Type = SymbolType.False;
                        IsPredicate = true;
                        break;
                    case '1':
                        Type = SymbolType.True;
                        IsPredicate = true;
                        break;
                    default:
                        Type = SymbolType.Predicate;
                        IsPredicate = true;
                        break;
                }
                Char = (char) c;
            }
        }

      
        public override string ToString() => Char.ToString();
    }
}
