using System;
using System.Collections.Generic;
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

       
        [TestMethod]
        public void CheckRegex()
        {
            Regex r = LogicPropositionHelper.PropositionalRegex;

            List<string> validPropostions = new List<string>
            {
                @">(A,B)",
                @"~(A)",
                @">(=(A,B),~(C))",
                @"&(A,B)",
                @"~(0)",
                @"&(A,~(B))",
                @">(~(=(D,A)),~(B))",
                @"T",
                @"&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
                @"1",
                @"0",
                @"~(A)",
                @"A"

            };
            foreach (var validPropostion in validPropostions)
            {
                Assert.IsTrue(r.Match(validPropostion).Success);
            }


            List<string> invalidPropositions = new List<string>
            {
                @"&(A,B",
                @"~(A,B)",
                @"&(A,|(B))",
                @">",
                @"~",
                @"=",
                @"3",
                @"4",
                @"11",
                @"~(,B)",
                @">(,B)",
                @"=(A,)",
                @">(,)",
                @"=(,)",
                @"~()",
                @">(,",
                @",,",
                @" "
            };
            foreach (var invalidProposition in invalidPropositions)
            {
                Assert.IsFalse(r.Match(invalidProposition).Success);
            }
        }

       

    }
}
