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

            viewModel.AsciiLogicString = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";
            viewModel.AsciiLogicString = viewModel.AsciiLogicString.RemoveWhiteSpaces();
            

            // Validate correctness of Logic statement
            List<string> validationResult = LogicPropositionValidator.Validate(viewModel.AsciiLogicString);

            List<Token> parsedString = viewModel.AsciiLogicString.ParseLogicalProposition();
            //if (validationResult.Count > 0)
            //{
            //    foreach (var error in validationResult)
            //    {
            //        ModelState.AddModelError("", error);
            //    }
            //    return View("Index", viewModel);
            //}

            

            viewModel.Tree = TreeConstructor.ConstructTree(parsedString);

            return View("Index", viewModel);
        }


    }
}