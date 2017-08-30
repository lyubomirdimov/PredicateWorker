using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ALEWebApp.Models;
using Parser;

namespace ALEWebApp.Controllers
{
    public class ParsingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Parse(AutomatonViewModel viewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View("Index",viewModel);
            }

            // Remove White spaces
            viewModel.AsciiLogicString.RemoveWhiteSpaces();
            // Check if string is valid


            return null;
        }

      
    }
}