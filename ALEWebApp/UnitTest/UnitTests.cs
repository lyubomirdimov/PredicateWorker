using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using Common.Helpers;
using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest
{
    [TestClass]
    public class UnitTests
    {
        public TableScheme TestTableScheme { get; set; }
        public string TestProposition { get; set; }
        public UnitTests()
        {
            Init();
        }

        private void Init()
        {
            TestProposition = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";

            TestTableScheme = new TableScheme();
            TestTableScheme.TableHeaders = new List<string> { "A", "B", "C" };
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = true,
                RowNum = 0,
                Values = new List<string> { "0", "0", "0" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = true,
                RowNum = 2,
                Values = new List<string> { "0", "0", "1" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 2,
                Values = new List<string> { "0", "1", "0" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 3,
                Values = new List<string> { "0", "1", "1" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 4,
                Values = new List<string> { "1", "0", "0" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 5,
                Values = new List<string> { "1", "0", "1" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 6,
                Values = new List<string> { "1", "1", "0" }
            });
            TestTableScheme.DataRows.Add(new DataRow
            {
                Result = false,
                RowNum = 7,
                Values = new List<string> { "1", "1", "1" }
            });
        }


        [TestMethod]
        public void RemoveWhiteSpaces()
        {
            string input = " Hello  Worlds  test ttt";
            input = input.RemoveWhiteSpaces();
            Assert.AreEqual(input, "HelloWorldstestttt");
        }


        [TestMethod]
        public void CheckValidLogicalProposition()
        {
            Regex r = LogicPropositionValidator.PropositionalRegex;

            List<string> validPropostions;
            List<string> invalidPropositions;

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/ValidProps.json");
                validPropostions = JsonConvert.DeserializeObject<List<string>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/Invalid%20Propositions.json");
                invalidPropositions = JsonConvert.DeserializeObject<List<string>>(json);
            }

            foreach (var validPropostion in validPropostions)
            {
                Assert.IsTrue(r.Match(validPropostion).Success);
            }

            foreach (var invalidProposition in invalidPropositions)
            {
                Assert.IsFalse(r.Match(invalidProposition).Success);
            }
        }


        [TestMethod]
        public void TestRandomVectors()
        {
            Regex r = LogicPropositionValidator.PropositionalRegex;
            for (int i = 0; i < 100; i++)
            {
                Node tree = TreeConstructor.ConstructRandomTree();
                string prefixTree = tree.ToPrefixNotation();
                Assert.IsTrue(r.Match(prefixTree).Success);
            }
        }

        [TestMethod]
        public void TestToBin()
        {
            int num = 1;
            string binNum = "00000001";
            string bin = GeneralHelper.ToBin(num, 8);
            Assert.AreEqual(binNum, bin);


            num = 2;
            binNum = "00000010";
            bin = GeneralHelper.ToBin(num, 8);
            Assert.AreEqual(binNum, bin);

            num = 3;
            binNum = "00000011";
            bin = GeneralHelper.ToBin(num, 8);
            Assert.AreEqual(binNum, bin);

            num = 4;
            binNum = "00000100";
            bin = GeneralHelper.ToBin(num, 8);
            Assert.AreEqual(binNum, bin);

        }

        [TestMethod]
        public void TestRevertBin()
        {

            string bin = "00110101";
            string exp = "10101100";
            bin = bin.Reverse();
            Assert.AreEqual(exp, bin);

        }

        [TestMethod]
        public void TestBinaryStringToHexString()
        {
            Dictionary<string, string> testVectors;
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/ValidHashCodeVectors.json");
                testVectors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            foreach (var keyValuePair in testVectors)
            {
                string hexString = keyValuePair.Key.BinaryStringToHexString();
                Assert.AreEqual(keyValuePair.Value, hexString);
            }
        }

        

    }
}
