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
            var input = "1";
            Regex regex = new Regex("(^[A-Z0-1]{1}$)");
            Assert.IsTrue(regex.IsMatch(input));



        }

    }
}
