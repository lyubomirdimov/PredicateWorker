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

            Node tree = new Node("Root")
            {
                new Node("Category 1")
                {
                    new Node("Item 1"),
                    new Node("Item 2"),
                },
                new Node("Category 2")
                {
                    new Node("Item 3"),
                    new Node("Item 4"),
                }
            };
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Parse(LogicStatementViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index", viewModel);
            }

            // Remove White spaces
            viewModel.AsciiLogicString.RemoveWhiteSpaces();
            // Check if string is valid


            return null;
        }


    }
}