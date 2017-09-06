using System;
using System.Text.RegularExpressions;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void RemoveWhiteSpaces()
        {
            string input = " Hello  Worlds  test ttt";
            input = input.RemoveWhiteSpaces();
            Assert.AreEqual(input,"HelloWorldstestttt");
        }

        [TestMethod]
        public void GetConnectives()
        {
            //string input = "&(>(A,B),=(C,D))";
            //string connective = input.GetConnective();
            //Assert.AreEqual(connective,"&");
        }

        /// <summary>
        /// Check if Regex accepts only single predicate strings such as 1,0, A, B
        /// </summary>
        [TestMethod]
        public void RegexCheckOnlyPredicate()
        {
            var input = "1";
            Regex regex = new Regex("(^[A-Z0-1]{1}$)");
            Assert.IsTrue(regex.IsMatch(input));

            input = "AA";
            Assert.IsFalse(regex.IsMatch(input));

            input = "1";
            Assert.IsTrue(regex.IsMatch(input));

            input = "&";
            Assert.IsFalse(regex.IsMatch(input));

            input = "1&";
            Assert.IsFalse(regex.IsMatch(input));

            input = "11";
            Assert.IsFalse(regex.IsMatch(input));

            input = "Z";
            Assert.IsTrue(regex.IsMatch(input));


        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void RegexCheck()
        {
            string input = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";
            Regex r = new Regex(
                @"^([01A-Z]|
                (?<dyadic>[|&>=]\()|
                (?<comma-dyadic>\,)|
                (?<dBracket-comma>\))
                |(?<unary>~\()|
                (?<uBracket-unary>\)))+
                (?(dyadic)(?!))(?(comma)(?!))(?(unary)(?!))$"
                , RegexOptions.IgnorePatternWhitespace);
            Assert.IsTrue(r.IsMatch(input));



        }

        public void CheckRegex()
        {
            Regex r = new Regex(
                @"^([01A-Z]|
                (?<dyadic>[|&>=]\()|
                (?<comma-dyadic>\,)|
                (?<dBracket-comma>\))
                |(?<unary>~\()|
                (?<uBracket-unary>\)))+
                (?(dyadic)(?!))(?(comma)(?!))(?(unary)(?!))$"
                , RegexOptions.IgnorePatternWhitespace);

            string target = @"&(A,|(B,C)";
            string[] passList = {
                @"&(A,B)",
                @"~(0)",
                @"&(A,~(B))",
                @">(~(=(D,A)),~(B))",
                @"T"};
            for (int i = 0; i < passList.Length; i++)
            {
                if (r.Match(passList[i]).Success)
                {
                    Console.WriteLine("String: " + passList[i] + " Matches.");
                }
                else
                {
                    Console.WriteLine("Match expected but failed");
                }
            }

            string[] failList = {
                @"&(A,B",
                @"~(A,B)",
                @"&(A,|(B))",
                @">(~(=(D,A),~(B))",
                @">"};

            for (int i = 0; i < failList.Length; i++)
            {
                if (r.Match(failList[i]).Success)
                {
                    Console.WriteLine("String: " + failList[i] + " matches but should not.");
                }
                else
                {
                    Console.WriteLine("No match as expected");
                }
            }
        }

        public void regexCheck()
        {
   
        }

    }
}
