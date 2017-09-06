using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Common;

namespace ALEWebApp.Models
{
    public class LogicStatementViewModel
    {
        
        [Required(ErrorMessage = "Logical Predicate is Required")]
        public string AsciiLogicString { get; set; }

        public Node Tree { get; set; }

        public List<string> ExampleValidPropositions => new List<string>
        {
            "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
            "&(=(A,B),>(&(A,B),~(C)))",
            "C",
            ">(~(>(A,B)),C)",
            ">(&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))),&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))))"

        };
    }
}