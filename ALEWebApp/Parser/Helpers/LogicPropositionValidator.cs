using System.Text.RegularExpressions;

namespace Common.Helpers
{
    /// <summary>
    /// Static class for validation of Ascii string logical propositions
    /// </summary>
    public static class LogicPropositionValidator
    {
        public static Regex PropositionalRegex => new Regex(


            @"^                                 # Start of line
                  (                             # Either
                      [01A-Z](?![01A-Z])  |     # A predicate
                      (?<couple>[|&>=%]\((?!,))| # Start of couple
                      (?<comma-couple>,(?!\)))| # Looks for comma followed by couple. Pops off the couple stack.
                      (?<dBracket-comma>\))|    # Looks for ending bracket following comma. pops off comma stack. 
                      (?<single>~\((?!\)))|     # Start of single function.
                      (?<uBracket-single>\)))   # Looks for ending bracket for unary. Pops off the single stack. 
                  +                             # Any number of times.
                  (?(couple)(?!))               # Assert couple stack is empty. All have a comma.
                  (?(comma)(?!))                # Assert comma stack is empty. All couple commas followed by parenthesis.
                  (?(single)(?!))               # Assert single stack is empty. All single expressions have closing brackets.
                  $"
            , RegexOptions.IgnorePatternWhitespace);


        public static bool Validate(string input)
        {
            Regex r = PropositionalRegex;
            return r.Match(input).Success;
        }

        public static string RemoveWhiteSpaces(this string input)
        {
            Regex rgx = new Regex("\\s+");
            return rgx.Replace(input, "");
        }



    }
}
