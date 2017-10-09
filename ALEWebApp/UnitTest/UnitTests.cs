using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<string> ValidTestVectors { get; set; }
        public List<string> InvalidTestVectors { get; set; }
        public Dictionary<string, string> BinToHCVectors { get; set; }
        public Dictionary<string, string> PropositionToHashCodeVectors { get; set; }

        public UnitTests()
        {
            Init();
        }

        private void Init()
        {
            // Some hardcoded test data
            TestProposition = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";

            TestTableScheme = new TableScheme
            {
                TableHeaders = new List<string> { "A", "B", "C", "Result" },
                Predicates = new List<string> { "A", "B", "C" }
            };

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
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/ValidPropositions.json");
                ValidTestVectors = JsonConvert.DeserializeObject<List<string>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/InvalidPropositions.json");
                InvalidTestVectors = JsonConvert.DeserializeObject<List<string>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/BinToHashcode.json");
                BinToHCVectors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps/master/PropositionToHashcode.json");
                PropositionToHashCodeVectors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                
            }

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


            foreach (var validPropostion in ValidTestVectors)
            {
                Assert.IsTrue(LogicPropositionValidator.Validate(validPropostion));
            }

            foreach (var invalidProposition in InvalidTestVectors)
            {
                Assert.IsFalse(LogicPropositionValidator.Validate(invalidProposition));
            }
        }


        [TestMethod]
        public void TestRandomVectors()
        {
            for (int i = 0; i < 1000; i++)
            {
                Node tree = TreeConstructor.ConstructRandomTree();
                string prefixTree = tree.ToPrefixNotation();
                Assert.IsTrue(LogicPropositionValidator.Validate(prefixTree));
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
            foreach (var keyValuePair in BinToHCVectors)
            {
                string hexString = keyValuePair.Key.BinaryStringToHexString();
                Assert.AreEqual(keyValuePair.Value, hexString);
            }
        }

        [TestMethod]
        public void TestTreeConstruction()
        {
            foreach (var testVector in ValidTestVectors)
            {
                List<Token> proposition = testVector.ParseLogicalProposition();
                Node tree = TreeConstructor.ConstructTree(proposition);
                string treeToProposition = tree.ToPrefixNotation();

                Assert.AreEqual(testVector, treeToProposition);
            }
        }

        [TestMethod]
        public void TestTableSchemeConstruction()
        {
            // Parse
            List<Token> logicalProposition = TestProposition.ParseLogicalProposition();

            // Construct Table Scheme
            TableScheme scheme = TableSchemeUtil.ConstructTableScheme(logicalProposition);

            // Compare
            CollectionAssert.AreEqual(scheme.TableHeaders, TestTableScheme.TableHeaders);
            CollectionAssert.AreEqual(scheme.Predicates, TestTableScheme.Predicates);
            CollectionAssert.AreEqual(scheme.DataRows[0].Values, TestTableScheme.DataRows[0].Values);
            Assert.AreEqual(scheme.NrOfDataRows, TestTableScheme.NrOfDataRows);
            Assert.AreEqual(scheme.DataRows[0].Result, TestTableScheme.DataRows[0].Result);
            CollectionAssert.AreEqual(scheme.DataRows.Last().Values, TestTableScheme.DataRows.Last().Values);
            Assert.AreEqual(scheme.DataRows.Last().Result, TestTableScheme.DataRows.Last().Result);

        }

        [TestMethod]
        public void TestTableSchemeToHashCode()
        {
            foreach (var keyValuePair in PropositionToHashCodeVectors)
            {
                // Construct Table
                TableScheme tableScheme = TableSchemeUtil.ConstructTableScheme(keyValuePair.Key.ParseLogicalProposition());
                string hashCode = tableScheme.TableSchemeToHashCode();

                Assert.AreEqual(keyValuePair.Value, hashCode);
            }
        }

        [TestMethod]
        public void TestSimplification()
        {

            foreach (var testVector in ValidTestVectors)
            {
                List<Token> proposition = testVector.ParseLogicalProposition();
                TableScheme scheme = TableSchemeUtil.ConstructTableScheme(proposition);
                string tshashCode = scheme.TableSchemeToHashCode();

                // Simplify
                TableScheme simplifyTableScheme = TableSchemeUtil.SimplifyTableScheme(proposition);
                // Get DNF and CNF of simplified
                //Tuple<string, string> cnf = TableSchemeUtil.GetCnf(simplifyTableScheme);
                Tuple<string, string> dnf = TableSchemeUtil.GetDnf(simplifyTableScheme);

                //Assert.AreEqual(tshashCode,cnf.Item2);
                Assert.AreEqual(tshashCode, dnf.Item2);

            }
        }

        [TestMethod]
        public void TestDNF()
        {
            foreach (var testVector in ValidTestVectors)
            {
                List<Token> proposition = testVector.ParseLogicalProposition();
                TableScheme scheme = TableSchemeUtil.ConstructTableScheme(proposition);
                string tableSchemeToHashCode = scheme.TableSchemeToHashCode();
                Tuple<string, string> dnf = TableSchemeUtil.GetDnf(scheme);
                string dnfHashCode = dnf.Item2;

                Assert.AreEqual(tableSchemeToHashCode, dnfHashCode);
            }
        }

        [TestMethod]
        public void TestCNF()
        {
            foreach (var testVector in ValidTestVectors)
            {
                List<Token> proposition = testVector.ParseLogicalProposition();
                TableScheme scheme = TableSchemeUtil.ConstructTableScheme(proposition);
                string tableSchemeToHashCode = scheme.TableSchemeToHashCode();

                Tuple<string, string> cnf = TableSchemeUtil.GetCnf(scheme);
                string cnfHashcode = cnf.Item2;

                Assert.AreEqual(tableSchemeToHashCode, cnfHashcode);
            }
        }

        [TestMethod]
        public void TestNandify()
        {
            foreach (var testVector in ValidTestVectors)
            {
                if (testVector.Length > 30) continue; // Trying to avoid SystemOutOfMemory exception

                List<Token> tokens = testVector.ParseLogicalProposition();
                TableScheme tableScheme = TableSchemeUtil.ConstructTableScheme(tokens);
                string hashCode = tableScheme.TableSchemeToHashCode();

                Node tree = TreeConstructor.ConstructTree(tokens);
                string nandifiedTree = tree.Nandify();
                List<Token> nandTokens = nandifiedTree.ParseLogicalProposition();
                TableScheme nandTblScheme = TableSchemeUtil.ConstructTableScheme(nandTokens);
                string nandHashCode = nandTblScheme.TableSchemeToHashCode();

                Assert.AreEqual(hashCode, nandHashCode);
            }

            for (int i = 0; i < 100; i++)
            {
                Node randomTree = TreeConstructor.ConstructRandomTree();
                string proposition = randomTree.ToPrefixNotation();

                if (proposition.Length > 30) continue; // Trying to avoid SystemOutOfMemory exception


                List<Token> tokens = proposition.ParseLogicalProposition();
                TableScheme tableScheme = TableSchemeUtil.ConstructTableScheme(tokens);
                string hashCode = tableScheme.TableSchemeToHashCode();

                Node tree = TreeConstructor.ConstructTree(tokens);
                string nandifiedTree = tree.Nandify();
                List<Token> nandTokens = nandifiedTree.ParseLogicalProposition();
                TableScheme nandTblScheme = TableSchemeUtil.ConstructTableScheme(nandTokens);
                string nandHashCode = nandTblScheme.TableSchemeToHashCode();

                Assert.AreEqual(hashCode, nandHashCode);

            }
        }
    }
}

