using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace appraisal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "國光生物科技(股)公司2014年員工績效考核表";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "公司信箱：biz@adimmune.com.tw";

            return View();
        }
    }
}