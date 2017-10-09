using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Helpers;
using Common.Models;

namespace ALEWebApp.Models
{
    public class LogicStatementViewModel
    {
        
        [Display(Name = "Logical Propositon")]
        [Required(ErrorMessage = "Logical Proposition is Required")]
        [Remote("ValidateProposition","Main",HttpMethod = "POST",ErrorMessage = "Invalid Logical Proposition")]
        public string InputProposition { get; set; }

        public Node Tree { get; set; }
        public string InfixNotation { get; set; }
        public TableScheme TableScheme { get; set; }
        public TableScheme TableSchemeSimplified { get; set; }
        public string TableSchemeHashCode { get; set; }
        public string DNFTableScheme { get; set; }
        public string DNFTableSchemeHashCode { get; set; }
        public string DNFTableSchemeSimplified { get; set; }
        public string CNFTableScheme { get; set; }
        public string CNFTableSchemeHashCode { get; set; }
        public string CNFTableSchemeSimplified { get; set; }
        public string Nandified { get; set; }
        public string NANDTableSchemeHashCode { get; set; }

        public List<string> ExampleValidPropositions => new List<string>
        {
            "&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B))))",
            "|(|(P,Q),&(~(P),~(Q)))",
            "&(|(P,Q),&(~(P),~(Q)))",
            "%(P,Q)",
            "|(&(|(>(>(>(B,B),C),B),~(=(=(B,B),~(=(B,>(>(&(C,B),&(B,&(B,=(=(C,&(C,C)),B)))),C)))))),=(=(&(=(|(A,C),|(B,C)),=(|(&(C,|(&(C,C),B)),B),&(>(C,B),>(|(A,C),=(B,B))))),|(B,C)),|(=(=(B,A),A),&(|(~(&(C,B)),A),B)))),|(B,~(&(C,|(B,&(C,|(B,&(>(A,~(A)),A))))))))",
            ">(&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))),&(&(=(A,B),>(&(A,B),~(C))),>(A,~(&(A,B)))))",
            "|(&(|(%(=(B,D),B),~(%(=(B,&(B,%(D,C))),C))),~(~(C))),=(|(>(&(D,D),|(A,B)),=(D,D)),|(D,D)))"

        };
    }
   
}