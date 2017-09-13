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
            Table table = new Table();

            // Construct header row
            List<Token> headerTokens = parsedString.Where(x => x.IsPredicate && x.Type != TokenType.True && x.Type != TokenType.False).ToList();
            TableRow headerRow = new TableRow();
            foreach (var headerToken in headerTokens)
            {
                headerRow.Cells.Add(new TableCell() { Text = headerToken.ToString() });
            }
            // Add result cell at the end of header row
            headerRow.Cells.Add(new TableCell() { Text = "Result" });
            table.Rows.Add(headerRow); // Add header row

            // Data construction row by row
            double numberOfRows = Math.Pow(2, headerTokens.Count);
            for (int i = 0; i < numberOfRows; i++)
            {
                
            }

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
            return Json("Success");
        }
    }

}