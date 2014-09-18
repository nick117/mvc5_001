using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using appraisal.Models;
using appraisal.Filters;
using appraisal.Infrastructure.CustomResults;
using PagedList;

namespace appraisal.Controllers
{
    public class tsVController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: tsV
        [LogActionFilter(ControllerName = "考核管理", ActionName = "瀏覽")]
        public ActionResult Index(int idv, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.cIdv = idv;
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID_Desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_Desc" : "Name";
            ViewBag.DeptSortParm = sortOrder == "Dept" ? "Dept_Desc" : "Dept";
            ViewBag.SubjSortParm = sortOrder == "Subj" ? "Subj_Desc" : "Subj";
            ViewBag.MangSortParm = sortOrder == "Mang" ? "Mang_Desc" : "Mang";
            ViewBag.VLSortParm = sortOrder == "VL" ? "VL_Desc" : "VL";
            ViewBag.OldSortParm = sortOrder;
            if (searchString == "*")
            { searchString = ""; }
            ViewBag.CurrentFilter = searchString;
            var emp = db.ts.OrderBy(x => x.id); ;
            var items = from item in emp select item;
            items = items.Where(s =>  s.exm.Equals(idv)
 //                                  && s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                                      );
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => (s.emp1.cname.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp1.eid.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp1.dep.title.ToUpper().Contains(searchString.ToUpper())
                                        || s.exm1.subject.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp2.cname.ToUpper().Contains(searchString.ToUpper()))
                                        );
            }
            switch (sortOrder)
            {
                case "ID_Desc":
                    items = items.OrderBy(x => x.emp1.eid);
                    break;
                case "Name":
                    items = items.OrderBy(x => x.emp1.cname);
                    break;
                case "Name_Desc":
                    items = items.OrderByDescending(x => x.emp1.cname);
                    break;
                case "Dept":
                    items = items.OrderBy(x => x.emp1.dep.title);
                    break;
                case "Dept_Desc":
                    items = items.OrderByDescending(x => x.emp1.dep.title);
                    break;
                case "Subj":
                    items = items.OrderBy(x => x.exm1.subject);
                    break;
                case "Subj_Desc":
                    items = items.OrderByDescending(x => x.exm1.subject);
                    break;
                case "Mang":
                    items = items.OrderBy(x => x.emp2.cname);
                    break;
                case "Mang_Desc":
                    items = items.OrderByDescending(x => x.emp2.cname);
                    break;
                case "VL":
                    items = items.OrderBy(x => x.vl);
                    break;
                case "VL_Desc":
                    items = items.OrderByDescending(x => x.vl);
                    break;
                default:
                    items = items.OrderByDescending(x => x.emp1.eid);
                    break;
            }
            return View(items.ToPagedList(pageNumber: page ?? 1, pageSize: itemsPerPage ?? 10));
        }
    }
}