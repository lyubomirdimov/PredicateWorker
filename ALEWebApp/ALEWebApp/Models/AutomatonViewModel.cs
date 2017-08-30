using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ALEWebApp.Models
{
    public class AutomatonViewModel
    {
        
        [Required(ErrorMessage = "Logical Predicate is Required")]
        public string AsciiLogicString { get; set; }
    }
}