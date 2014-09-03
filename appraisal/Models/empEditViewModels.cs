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

        public int pageno { get; set; }

        public int v_itemsPerPage { get; set; }

        public string v_sortOrder { get; set; }

        public string v_currentFilter { get; set; }
    }
}