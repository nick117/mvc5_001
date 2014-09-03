using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using appraisal.Filters;

namespace appraisal.Controllers
{
    public class HomeController : Controller
    {
        [LogActionFilter(ControllerName = "首頁", ActionName = "績效評核")]
        public ActionResult Index()
        {
            return View();
        }

        [LogActionFilter(ControllerName = "首頁", ActionName = "關於")]
        public ActionResult About()
        {
            ViewBag.Message = "國光生物科技(股)公司2014年員工績效考核表";

            return View();
        }

        [LogActionFilter(ControllerName = "首頁", ActionName = "聯絡方式")]
        public ActionResult Contact()
        {
            ViewBag.Message = "公司信箱：biz@adimmune.com.tw";

            return View();
        }

        public ActionResult KP()
        {
            return View();
        }
    }
}