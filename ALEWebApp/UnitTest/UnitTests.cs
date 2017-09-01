using System;
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
            string input = "&(>(A,B),=(C,D))";
            string connective = input.GetConnective();
            Assert.AreEqual(connective,"&");
        }
    }
}
