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

            // Infix notation
            viewModel.InfixNotation = propositionTree.ToInfixNotation();

            if (parsedString[0].IsTrueOrFalse) return View("Index", viewModel);

            // Table schemes
            viewModel.TableScheme = TableSchemeUtil.ConstructTableScheme(parsedString);
            viewModel.TableSchemeSimplified = TableSchemeUtil.SimplifyTableScheme(parsedString);

            // Hash code
            viewModel.TableSchemeHashCode = viewModel.TableScheme.TableSchemeToHashCode();

            // DNF
            Tuple<string,string> dnfTuple = TableSchemeUtil.GetDnf(viewModel.TableScheme);
            viewModel.DNFTableScheme = dnfTuple.Item1;
            viewModel.DNFTableSchemeHashCode = dnfTuple.Item2;
            Tuple<string, string> dnfSimplifiedTuple = TableSchemeUtil.GetDnf(viewModel.TableSchemeSimplified);
            viewModel.DNFTableSchemeSimplified = dnfSimplifiedTuple.Item1;

            // CNF
            Tuple<string, string> cnfTuple = TableSchemeUtil.GetCnf(viewModel.TableScheme);
            viewModel.CNFTableScheme = cnfTuple.Item1;
            viewModel.CNFTableSchemeHashCode = cnfTuple.Item2;
            Tuple<string, string> cnfSimplifiedTuple = TableSchemeUtil.GetCnf(viewModel.TableSchemeSimplified);
            viewModel.CNFTableSchemeSimplified = cnfSimplifiedTuple.Item1;

            // Nandify
            viewModel.Nandified = viewModel.InputProposition.Nandify();

            return View("Index", viewModel);
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
            Node tree = TreeConstructor.ConstructRandomTree();
            string prefixTree = tree.ToPrefixNotation();
            return Json(prefixTree,JsonRequestBehavior.AllowGet);
        }
    }

}