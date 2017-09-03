using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public static class LogicStatementValidator
    {
        private static readonly List<char> Connectives = new List<char>() { '&', '|', '>', '=', '~' };

        public static List<string> Validate(string asciiString)
        {
            List<string> validationResult = new List<string>();
            List<Symbol> parsedSymbols = asciiString.ParseLogicalProposition();

            CheckNumberOfParanthesis(parsedSymbols, validationResult);
            CheckInitialValidSymbol(asciiString, validationResult);

            Regex twoOrMoreConsacutiveCommas = new Regex("^.*?,,+.*$");
            if (twoOrMoreConsacutiveCommas.IsMatch(asciiString))
            {
                validationResult.Add("Two or more consacutive commas");
            }

            

            return validationResult;
        }


        /// <summary>
        /// Check if Number of opening Paranthesis is equal to the number of closing paranthesis
        /// </summary>
        public static void CheckNumberOfParanthesis(List<Symbol> parsedSymbols, List<string> validationResult)
        {
            
            if (parsedSymbols.Select(x => x.IsOpeningParanthesis).ToList().Count != parsedSymbols.Select(x => x.IsClosingParanthesis).ToList().Count)
            {
                validationResult.Add("There is a problem with number of paranthesis");
            }
        }

        /// <summary>
        /// Check if it starts with valid Symbol
        /// </summary>
        public static void CheckInitialValidSymbol(string input, List<string> validationResult)
        {
            Regex pattern = new Regex("[0-1A-Z]|^[&|>=~][(]");
            if (pattern.IsMatch(input) == false)
            {
                validationResult.Add("Invalid initial character");
            }
            //if (parsedSymbols[0].IsPredicate == false && parsedSymbols[0].IsConnective == false)
            //{
            //    validationResult.Add("Starting character is not a correct character");
            //}
        }

        private static void CheckValidityBetweenParanthesis(List<Symbol> parsedSymbols, List<string> validationResult)
        {
            Stack<Symbol> stackOfParenthesis = new Stack<Symbol>();
            foreach (Symbol parsedSymbol in parsedSymbols)
            {
                if (parsedSymbol.IsOpeningParanthesis)
                {
                    stackOfParenthesis.Push(parsedSymbol);
                }
                if (parsedSymbol.IsClosingParanthesis)
                {
                    Symbol openingParenthis = stackOfParenthesis.Pop();

                    List<Symbol> symbolsBetweenParenthesis =
                        parsedSymbols.Skip(parsedSymbols.IndexOf(openingParenthis) - 1)
                        .Take(parsedSymbols.IndexOf(parsedSymbol) - parsedSymbols.IndexOf(openingParenthis) + 2)
                        .ToList();

                    if (parsedSymbols[0].IsConnective == false)
                    {
                        validationResult.Add("Character before Paranthesis is invalid");
                    }
                    else
                    {
                        
                        if (parsedSymbols[0].IsNegation)
                            // There should be only one Proposition after the negation
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    // 
                }
            }
        }

    }
}
