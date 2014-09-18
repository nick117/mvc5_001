using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using appraisal.Filters;
using appraisal.Models;

namespace appraisal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [LogActionFilter(ControllerName = "首頁", ActionName = "績效評核")]
        public ActionResult Index()
        {
            //取出考核時間區段
            var dict = new Dictionary<string, string>();
            var itemots = from item in db.ots select item;
            foreach (var it1 in itemots) 
            {
                dict[it1.Skey] = it1.Vl;
            };  
            //判斷可撈取考核紀錄,如有紀錄者才會顯示頁籤
            int r1 = 0;
            int r2 = 0;
            int r3 = 0;
            DateTime dt = Convert.ToDateTime(System.DateTime.Now.ToString("D"));
            //初評
            if (dt.CompareTo(Convert.ToDateTime(dict["startDate"])) >= 0 && dt.CompareTo(Convert.ToDateTime(dict["endDate1"])) <= 0)
            {
                r1 = 1;
                r2 = 3;
                r3 = 5;
            };
            //複評
            if (dt.CompareTo(Convert.ToDateTime(dict["startDate2"])) >= 0 && dt.CompareTo(Convert.ToDateTime(dict["endDate2"])) <= 0)
            {
                r1 = 2;
                r2 = 4;
                r3 = 6;
            };
            var items = from item in db.ts select item;
            var itemA = items.Where(s => s.exm.Equals(r1)
                                  & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                                      );
            if (itemA.Count() == 0)
            { ViewBag.tabA = 0 ;}
            else
            { ViewBag.tabA = 1 ;};
            itemA = items.Where(s => s.exm.Equals(r2)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            if (itemA.Count() == 0)
            { ViewBag.tabB = 0; }
            else
            { ViewBag.tabB = 1; };
            itemA = items.Where(s => s.exm.Equals(r3)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            if (itemA.Count() == 0)
            { ViewBag.tabC = 0; }
            else
            { ViewBag.tabC = 1; };
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