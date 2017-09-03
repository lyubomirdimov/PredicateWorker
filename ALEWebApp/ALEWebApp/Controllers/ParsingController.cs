using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALEWebApp.Models;
using Common;
using Common.Symbols;

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
        
           

            // Remove White spaces
            
            // Check if string is valid

            viewModel.AsciiLogicString = "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))";
            List<Symbol> parsedString = viewModel.AsciiLogicString.ParseLogicalProposition();

            viewModel.Tree = TreeConstructor.ConstructTree(parsedString);

            return View("Index", viewModel);
        }


    }
}