using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Common;

namespace ALEWebApp.Models
{
    public class LogicStatementViewModel
    {
        
        [Display(Name = "Logical Propositon")]
        [Required(ErrorMessage = "Logical Proposition is Required")]
        [Remote("ValidateProposition","Parsing",HttpMethod = "POST",ErrorMessage = "Invalid Logical Proposition")]
        public string InputProposition { get; set; }

        public Node Tree { get; set; }

        public List<string> ExampleValidPropositions => new List<string>
        {
            "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
            "&(=(A,B),>(&(A,B),~(C)))",
            "C",
            ">(~(>(A,B)),C)"
            //">(&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))),&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))))"

        };
    }
}