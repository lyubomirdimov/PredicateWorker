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
            viewModel.TableScheme = TableConstructionHelper.ConstructTableScheme(parsedString, propositionTree);
            viewModel.TableSchemeSimplified = TableConstructionHelper.SimplifyTableScheme(parsedString, propositionTree);

            // Hash code
            string binaryValue = string.Empty;
            binaryValue = viewModel.TableScheme.DataRows.Aggregate(binaryValue, (current, row) => current + (row.Result ? "1" : "0"));
            binaryValue = GeneralHelper.Reverse(binaryValue);
            viewModel.TableSchemeHashCode = binaryValue.BinaryStringToHexString();

            // DNF
            viewModel.DNFTableScheme = TableConstructionHelper.GetDnf(viewModel.TableScheme);
            viewModel.DNFTableSchemeSimplified = TableConstructionHelper.GetDnf(viewModel.TableSchemeSimplified);

            // CNF
            viewModel.CNFTableScheme = TableConstructionHelper.GetCnf(viewModel.TableScheme);
            viewModel.CNFTableSchemeSimplified = TableConstructionHelper.GetCnf(viewModel.TableSchemeSimplified);

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
            // TODO: Random props
            return Json("Success");
        }
    }

}