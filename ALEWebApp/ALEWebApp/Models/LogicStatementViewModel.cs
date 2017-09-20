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
        [Remote("ValidateProposition","Main",HttpMethod = "POST",ErrorMessage = "Invalid Logical Proposition")]
        public string InputProposition { get; set; }

        public Node Tree { get; set; }

        public TableScheme TableScheme { get; set; }
        public TableScheme TableSchemeSimplified { get; set; }
        public List<string> ExampleValidPropositions => new List<string>
        {
            "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
            "&(=(A,B),>(&(A,B),~(C)))",
            "C",
            ">(~(>(A,B)),C)"
            //">(&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))),&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))))"

        };
    }
    public class TableScheme
    {
        public List<string> TableHeaders { get; set; } = new List<string>();
        public List<DataRow> DataRows { get; set; } = new List<DataRow>();
        public int NrOfDataRows { get; set; }
    }
    public class DataRow
    {
        public int RowNum { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public bool Result { get; set; }

    }
}