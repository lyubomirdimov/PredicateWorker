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
        //public static List<string> Validate(string input)
        //{
        //    List<string> validationResult = new List<string>();
        //    List<Token> parsedTokens = input.ParseLogicalProposition();

        //    // ^[A-Z0-1]{1}$ - Check if string is a single predicate
        //    Regex singlePredicatePattern = new Regex("^[A-Z0-1]{1}$");
        //    if (singlePredicatePattern.IsMatch(input))
        //    // Valid String
        //    {
        //        return validationResult;
        //    }

        //    CheckNumberOfParanthesis(parsedTokens, validationResult); 
        //    CheckNumberOfCommas(parsedTokens, validationResult);      

        //    // Check if two or more consacutive commas appear in the string
        //    Regex twoOrMoreConsacutiveCommas = new Regex("^.*?,,+.*$");
        //    if (twoOrMoreConsacutiveCommas.IsMatch(input))
        //    {
        //        validationResult.Add("Two or more consacutive commas");
        //    }

        //    bool isValid = true;
        //    Stack<Token> tokenStack = new Stack<Token>();
        //    for (var i = 0; i < parsedTokens.Count; i++)
        //    {
        //        Token token = parsedTokens[i];
        //        switch (token.Type)
        //        {

        //            case TokenType.Negation:
        //                // Check if next token is openinng parenthesis
        //                if (parsedTokens[i + 1]?.IsOpeningParanthesis == false)
        //                {
        //                    isValid = false;
        //                    break;
        //                }
        //                // Check if it has a single propostion afterwards
        //                break;

        //            case TokenType.And:
        //            case TokenType.Or:
        //            case TokenType.Implication:
        //            case TokenType.BiImplication:
        //                // Check if next token is openinng parenthesis
        //                if (parsedTokens[i + 1]?.IsOpeningParanthesis == false)
        //                {
        //                    isValid = false;
        //                    break;
        //                }
        //                // 

        //                break;
        //            case TokenType.Separator:
        //                break;
        //            case TokenType.Predicate:
        //            case TokenType.True:
        //            case TokenType.False:
        //            case TokenType.OpeningParanthesis:
        //                break;
        //            case TokenType.ClosingParanthesis:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }


        //    return validationResult;
        //}
        public static bool Validate(string input)
        {
            Regex r = new Regex(
                @"^                             # Start of line
                  (                             # Either
                      [01A-Z](?![01A-Z])  |     # A predicate
                      (?<dyadic>[|&>=]\((?!,))| # Start of dyadic
                      (?<comma-dyadic>,(?!\)))| # Looks for comma followed by dyadic. Pops off the dyadic stack.
                      (?<dBracket-comma>\))|    # Looks for ending bracket following comma. pops off comma stack. 
                      (?<monadic>~\((?!\)))|    # Start of monadic function.
                      (?<uBracket-monadic>\)))  # Looks for ending bracket for unary. Pops off the monadic stack. 
                  +                             # Any number of times.
                  (?(dyadic)(?!))               # Assert dyadic stack is empty. All have a comma.
                  (?(comma)(?!))                # Assert comma stack is empty. All dyadic commas followed by parenthesis.
                  (?(monadic)(?!))              # Assert monadic stack is empty. All monadic expressions have closing brackets.
                  $"
                , RegexOptions.IgnorePatternWhitespace);

            return r.Match(input).Success;

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
