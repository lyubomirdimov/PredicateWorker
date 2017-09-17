using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ALEWebApp.Models;
using Common;

namespace ALEWebApp.Controllers
{
    public class ParsingController : Controller
    {
        public ActionResult Index()
        {
            return View(new LogicStatementViewModel());
        }



        [ValidateAntiForgeryToken]
        public ActionResult Parse(LogicStatementViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index", viewModel);
            }

            // Construct tree
            List<Token> parsedString = viewModel.InputProposition.ParseLogicalProposition();
            Node propositionTree = TreeConstructor.ConstructTree(parsedString);
            viewModel.Tree = propositionTree;


            // Construct Table
            TableScheme tableScheme = new TableScheme();

            //            Table table = new Table();

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
                    RowNum = rowNum
                };

                List<Tuple<Token, bool>> subsitutionTuples = new List<Tuple<Token, bool>>();
                char[] rowNumIntoBinary = ToBin(rowNum, binaryLength).ToCharArray();

                for (var j = 0; j < rowNumIntoBinary.Length; j++)
                {
                    // Asign value to Row values and create a tuple for replacing tokens
                    Token predicateToken = predicateTokens[j];
                    bool predicateValue = rowNumIntoBinary[j] == '1';
                    Tuple<Token, bool> replacingToken = new Tuple<Token, bool>(predicateToken, predicateValue);

                    subsitutionTuples.Add(replacingToken);
                    row.Values.Add(predicateValue);
                }
                bool resultVal = RecusivelyCalculate(propositionTree, subsitutionTuples);
                row.Values.Add(resultVal);
                tableScheme.DataRows.Add(row);
            }

            viewModel.TableScheme = tableScheme;

            return View("Index", viewModel);
        }


        public bool RecusivelyCalculate(Node tree, List<Tuple<Token, bool>> substitutionTokens)
        {
            bool value = false;
            if (tree.Token.IsConnective)
            {
                List<bool> booleanResults = new List<bool>();
                foreach (Node child in tree.Children)
                {
                    booleanResults.Add(RecusivelyCalculate(child, substitutionTokens));
                }

                switch (tree.Token.Type)
                {
                    case TokenType.And:                    
                        value = booleanResults[0] & booleanResults[1]; // P And Q
                        break;
                    case TokenType.Or:
                        value = booleanResults[0] | booleanResults[1]; // P or Q 
                        break;
                    case TokenType.Negation:
                        value = !booleanResults[0]; // Not P
                        break;
                    case TokenType.Implication:
                        value = !booleanResults[0] | booleanResults[1]; // not P or Q
                        break;
                    case TokenType.BiImplication:
                        value = booleanResults[0] == booleanResults[1];
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
                Tuple<Token, bool> replacementTuple = substitutionTokens.FirstOrDefault(x => x.Item1.Char == tree.Token.Char);

                value = replacementTuple?.Item2 ?? throw new ArgumentNullException();
            }
            return value;

        }
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
            return Json("Success");
        }
    }

}