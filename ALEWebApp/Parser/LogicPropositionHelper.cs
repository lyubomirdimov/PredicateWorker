using System.Text.RegularExpressions;
using Fare;


namespace Common
{
    public static class LogicPropositionHelper
    {
        public static Regex PropositionalRegex => new Regex(


            @"^                                 # Start of line
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

        

      
    }
}