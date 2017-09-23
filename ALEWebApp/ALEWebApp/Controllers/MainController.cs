using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ALEWebApp.Models;
using Common;
using Common.Helpers;
using Common.Models;
using Microsoft.Ajax.Utilities;

namespace ALEWebApp.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View(new LogicStatementViewModel());
        }



        [ValidateAntiForgeryToken]
        public ActionResult Construct(LogicStatementViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index", viewModel);
            }

            // Construct tree
            List<Token> parsedString = viewModel.InputProposition.ParseLogicalProposition();
            Node propositionTree = TreeConstructor.ConstructTree(parsedString);
            viewModel.Tree = propositionTree;

            if (parsedString[0].IsTrueOrFalse) return View("Index", viewModel);

            // Table schemes
            viewModel.TableScheme = ConstructTableScheme(parsedString, propositionTree);
            viewModel.TableSchemeSimplified = SimplifyTableScheme(parsedString, propositionTree);

            // Hash code
            string hashValue = HashValue(viewModel);
            viewModel.TableSchemeHashCode = hashValue;

            return View("Index", viewModel);
        }

        /// <summary>
        /// Get hashvalue from result of logical proposition truth table
        /// </summary>
        private static string HashValue(LogicStatementViewModel viewModel)
        {
            string binaryValue = string.Empty;
            binaryValue = viewModel.TableScheme.DataRows.Aggregate(binaryValue, (current, row) => current + (row.Result ? "1" : "0"));
            string hashValue = binaryValue.BinaryStringToHexString();
            return hashValue;
        }


        /// <summary>
        /// Table Scheme construction from parsed logical proposition and constructed node tree for the proposition
        /// </summary>
        /// <param name="parsedString">  a list of tokens; the parsed logical proposition</param>
        /// <param name="propositionTree"> the tree constructed from the logical proposition</param>
        private TableScheme ConstructTableScheme(List<Token> parsedString, Node propositionTree)
        {
            // Construct Table
            TableScheme tableScheme = new TableScheme();

            // Construct header row
            List<Token> predicateTokens = parsedString.Where(x => x.IsPredicate).GroupBy(x => x.Char).Select(x => x.First()).ToList();
            predicateTokens = predicateTokens.OrderBy(x => x.ToString()).ToList();
            foreach (Token headerToken in predicateTokens)
            {
                tableScheme.TableHeaders.Add(headerToken.ToString());
            }
            // Add result cell at the end of header row
            tableScheme.TableHeaders.Add("Result");

            // Data construction row by row
            int binaryLength = predicateTokens.Count;
            double numberOfRows = Math.Pow(2, predicateTokens.Count);

            for (int rowNum = 0; rowNum < numberOfRows; rowNum++)
            {
                DataRow row = new DataRow
                {
                    RowNum = rowNum // Give rownumber to each row
                };

                List<Tuple<Token, bool>> subsitutionTuples = new List<Tuple<Token, bool>>();

                // Transform the rownumber to Binary, ex. rownum - 1, with 3 predicates, results in 001
                // The transformed rownumber is casted to array of chars
                char[] rowNumIntoBinary = ToBin(rowNum, binaryLength).ToCharArray();

                for (var j = 0; j < rowNumIntoBinary.Length; j++)
                // for each binary number
                {
                    // Asign value to Row values and create a tuple for replacing tokens
                    Token predicateToken = predicateTokens[j];
                    bool predicateValue = rowNumIntoBinary[j] == '1';
                    Tuple<Token, bool> replacingToken = new Tuple<Token, bool>(predicateToken, predicateValue);
                    subsitutionTuples.Add(replacingToken);

                    row.Values.Add(predicateValue ? "1" : "0");
                }
                // calculate the result of the row
                bool resultVal = CalculateRowResultRecur(propositionTree, subsitutionTuples);
                row.Result = resultVal;

                // add row to the table
                tableScheme.DataRows.Add(row);
            }

            return tableScheme;
        }


        /// <summary>
        /// Table Scheme construction and then simplification
        /// </summary>
        /// <param name="parsedString">  a list of tokens; the parsed logical proposition</param>
        /// <param name="propositionTree"> the tree constructed from the logical proposition</param>
        private TableScheme SimplifyTableScheme(List<Token> parsedString, Node propositionTree)
        {
            TableScheme tblScheme = ConstructTableScheme(parsedString, propositionTree);
            bool completed = false;

            // Loop until there is no further simplification
            while (completed == false)
            {
                completed = true; // Assume completion (if simplification is applied, completed becomes false)
                List<DataRow> simplifiedDataRows = new List<DataRow>(); // Collection of newly created simplified Rows
                int rowCount = 0;
                List<int> alreadySimplifiedRowsIndeces = new List<int>();

                for (int i = 0; i < tblScheme.NrOfDataRows; i++)
                // Loop through each row
                {
                    DataRow currentRow = tblScheme.DataRows[i];
                    bool isRowSimplified = false;

                    for (int j = i + 1; j < tblScheme.NrOfDataRows; j++)
                    // Find possible simplifications, by looping through all rows below the currentRow
                    {
                        DataRow rowToCompare = tblScheme.DataRows[j];

                        // If the row have different result, then don't compare
                        if (currentRow.Result != rowToCompare.Result) continue;

                        // Count the number of differences in between the predicate columns
                        List<int> indecesForChange = new List<int>();
                        for (int columnNr = 0; columnNr < tblScheme.NrOfPredicates; columnNr++)
                        {
                            if (currentRow.Values[columnNr] != rowToCompare.Values[columnNr])
                            {
                                indecesForChange.Add(columnNr);
                            }
                        }

                        // If there are more than one column which differs, then continue iterations
                        if (indecesForChange.Count != 1) continue;

                        // Simplify
                        DataRow newRow = new DataRow
                        {
                            Result = currentRow.Result,
                            RowNum = rowCount,
                            Values = currentRow.Values.ToList()
                        };
                        newRow.Values[indecesForChange[0]] = "*";
                        if (simplifiedDataRows.Any(x => x.Values.SequenceEqual(newRow.Values)) == false)
                        {
                            simplifiedDataRows.Add(newRow);
                        }

                        // Those two rows participated in simplification, hence not visible any further; Store their indexes
                        alreadySimplifiedRowsIndeces.Add(i);
                        alreadySimplifiedRowsIndeces.Add(j);

                        isRowSimplified = true;
                        completed = false;
                    }

                    if (isRowSimplified == false && alreadySimplifiedRowsIndeces.Contains(i) == false)
                    // No simplification was done to this row, hence it remains visible
                    {
                        simplifiedDataRows.Add(currentRow);
                    }
                }
                tblScheme.DataRows = simplifiedDataRows;
            }
            return tblScheme;
        }


        /// <summary>
        /// Recursively calculates the value of a row in a table
        /// </summary>
        /// <param name="tree"> Tree constructed from the logical proposition</param>
        /// <param name="substitutionTokens"> Tuple list, which replaces predicates in the tree with appropriate true/false values</param>
        /// <returns></returns>
        public bool CalculateRowResultRecur(Node tree, List<Tuple<Token, bool>> substitutionTokens)
        {
            bool value = false;
            if (tree.Token.IsConnective)
            {
                // Recusively get boolean values of the children
                List<bool> booleanResults = new List<bool>();
                foreach (Node child in tree.Children)
                {
                    booleanResults.Add(CalculateRowResultRecur(child, substitutionTokens)); // Recursive call
                }

                // Calculate value based on the conective node
                switch (tree.Token.Type)
                {
                    case TokenType.And:
                        value = booleanResults[0] & booleanResults[1];  // P And Q
                        break;
                    case TokenType.Or:
                        value = booleanResults[0] | booleanResults[1];  // P or Q 
                        break;
                    case TokenType.Negation:
                        value = !booleanResults[0];                     // Not P
                        break;
                    case TokenType.Implication:
                        value = !booleanResults[0] | booleanResults[1]; // not P or Q
                        break;
                    case TokenType.BiImplication:
                        value = booleanResults[0] == booleanResults[1]; // P <=> Q
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (tree.Token.IsTrueOrFalse)
            {
                if (tree.Token.Type == TokenType.True) value = true;
                if (tree.Token.Type == TokenType.False) value = false;
            }
            if (tree.Token.IsPredicate)
            {
                // Replace a predicate from the tree with a boolean
                Tuple<Token, bool> replacementTuple = substitutionTokens.FirstOrDefault(x => x.Item1.Char == tree.Token.Char);

                value = replacementTuple?.Item2 ?? throw new ArgumentNullException();
            }
            return value;
        }

        /// <summary>
        /// Convert integer value to Binary, with certain length
        /// </summary>
        /// <param name="value"> the integer value to be transformed</param>
        /// <param name="len"> length of the binary </param>
        /// <returns></returns>
        public static string ToBin(int value, int len)
        {
            return (len > 1 ? ToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }


        /// <summary>
        /// Validates weather or not input is a valid Logical proposition
        /// </summary>
        /// <param name="inputProposition">Logical proposition as ASCII string</param>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ValidateProposition(string inputProposition)
        {
            return Json(LogicPropositionValidator.Validate(inputProposition));
        }


        public ActionResult GenerateRandomProposition()
        {
            // TODO: Random props
            return Json("Success");
        }
    }

}