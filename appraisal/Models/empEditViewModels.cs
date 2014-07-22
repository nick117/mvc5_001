using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appraisal.Models
{
    public class empEditViewModels
    {
        public emp emp1 { get; set;}

        public IEnumerable<SelectListItem> depList { get; set; }
    }
}