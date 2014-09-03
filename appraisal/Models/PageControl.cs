using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appraisal.Models
{
    public class PageControl
    {
        public int itemsPerPage { get; set; }
        public string sortOrder { get; set; }
        public string searchString { get; set; }
    }
}