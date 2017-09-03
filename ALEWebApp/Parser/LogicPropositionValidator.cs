using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Static class for validation of Ascii string logical propositions
    /// </summary>
    public static class LogicPropositionValidator
    {
        /// <summary>
        /// Determines validity of ascii logical proposition string
        /// </summary>
        /// <param name="input">Ascii Logical Proposition string for validation</param>
        /// <returns>List of validation results, if the list count is 0, then no errors occured</returns>
        public static List<string> Validate(string input)
        {
            List<string> validationResult = new List<string>();
            List<Token> parsedTokens = input.ParseLogicalProposition();

            // ^[A-Z0-1]{1}$ - Check if string is a single predicate
            Regex singlePredicatePattern = new Regex("^[A-Z0-1]{1}$");
            if (singlePredicatePattern.IsMatch(input))
            // Valid String
            {
                return validationResult;
            }
            
            CheckNumberOfParanthesis(parsedTokens, validationResult); 
            CheckNumberOfCommas(parsedTokens, validationResult);      

            // Check if two or more consacutive commas appear in the string
            Regex twoOrMoreConsacutiveCommas = new Regex("^.*?,,+.*$");
            if (twoOrMoreConsacutiveCommas.IsMatch(input))
            {
                validationResult.Add("Two or more consacutive commas");
            }

            bool isExpressionValid = true;
            List<string> blocks = new List<string>();
            Stack<Token> stackOfParenthesis = new Stack<Token>();
            foreach (Token token in parsedTokens)
            {
                if (token.IsOpeningParanthesis)
                {
                    stackOfParenthesis.Push(token);
                }
                if (token.IsClosingParanthesis)
                {
                    Token openingParenthesis = stackOfParenthesis.Pop();

                    int openingIndex = parsedTokens.IndexOf(openingParenthesis);
                    int closingIndex = parsedTokens.IndexOf(token) - openingIndex;
                    List<Token> symbolsBlock = parsedTokens.Skip(openingIndex - 1).Take(closingIndex + 2).ToList();
                    string stringBlock = input.Substring(openingIndex - 1, closingIndex + 2);
                    blocks.Add(stringBlock);

                    if (symbolsBlock[0].IsConnective == false)
                    {
                        isExpressionValid = false;
                        break;
                    }
                    else
                    {

                        if (symbolsBlock[0].IsNegation)
                        // There should be only one Proposition after the negation
                        {
                            string withoutParanthesis = stringBlock.Substring(2, 1);
                            if (singlePredicatePattern.IsMatch(withoutParanthesis))
                            {
                                continue;
                            }

                            

                        }
                        else
                        {

                        }
                    }
                    // 
                }
            }

            if (isExpressionValid == false)
            {
                validationResult.Add("Expression is not valid");
            }

            return validationResult;
        }


        /// <summary>
        /// Check if Number of opening Paranthesis is equal to the number of closing paranthesis
        /// </summary>
        public static void CheckNumberOfParanthesis(List<Token> parsedSymbols, List<string> validationResult)
        {
            if (parsedSymbols.Select(x => x.IsOpeningParanthesis).ToList().Count != parsedSymbols.Select(x => x.IsClosingParanthesis).ToList().Count)
            {
                validationResult.Add("There is a problem with number of paranthesis");
            }
        }

        public static void CheckNumberOfCommas(List<Token> parsedSymbols, List<string> validationResult)
        {
            if ((parsedSymbols.Select(x => x.IsOpeningParanthesis).ToList().Count - parsedSymbols.Select(x => x.IsNegation).ToList().Count) != parsedSymbols.Select(x => x.IsSeparator).ToList().Count)
            {
                validationResult.Add("Commas does not match");
            }
        }

        /// <summary>
        /// Removes White spaces from string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveWhiteSpaces(this string input)
        {
            Regex rgx = new Regex("\\s+");
            return rgx.Replace(input, "");
        }



    }
}
