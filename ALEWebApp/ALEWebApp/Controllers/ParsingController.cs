using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            List<Token> parsedString = viewModel.InputProposition.ParseLogicalProposition();
            viewModel.Tree = TreeConstructor.ConstructTree(parsedString);

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
    }

}