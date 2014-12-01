using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appraisal.Models
{
    public class HomeIndexViewModels
    {
        public PagedList.IPagedList<ts> tsA { get; set; }
        public PagedList.IPagedList<ts> tsB { get; set; }
        public PagedList.IPagedList<ts> tsC { get; set; }

        public PagedList.IPagedList<ts> ts1A { get; set; }
        public PagedList.IPagedList<ts> ts1B { get; set; }
        public PagedList.IPagedList<ts> ts1C { get; set; }

        public ts tsAD { get; set; }
        public ts tsBD { get; set; }
        public ts tsCD { get; set; }


        public int pageA { get; set; }
        public int pageB { get; set; }
        public int pageC { get; set; }

        public string v_NameFilterA { get; set; }
        public string v_NameFilterB { get; set; }
        public string v_NameFilterC { get; set; }

        public string v_SignFilterA { get; set; }
        public string v_SignFilterB { get; set; }
        public string v_SignFilterC { get; set; }

        public string v_DeptFilterA { get; set; }
        public string v_DeptFilterB { get; set; }
        public string v_DeptFilterC { get; set; }

        public int v_IDA { get; set; }
        public int v_IDB { get; set; }
        public int v_IDC { get; set; }
    }
}