using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common;
using Common.Helpers;
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
        public void CheckValidLogicalProposition()
        {
            Regex r = LogicPropositionValidator.PropositionalRegex;

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
