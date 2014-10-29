using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
        public ActionResult Index(int? tabNo, int? pageA, int? pageB, int? pageC, int? idA, int? idB, int? idC, string searchStringA, string searchStringB, string searchStringC, string NormalSelA, string NormalSelB, string NormalSelC, bool? UnscoredA, bool? UnscoredB, bool? UnscoredC, string sDefaultA, string sDefaultB, string sDefaultC)
        {
            //決定預設顯示Tab
            ViewBag.TabValue = tabNo ?? 0;
            ViewBag.sDefaultA = sDefaultA ?? "*";
            ViewBag.ErrorMsg = "*";
            searchStringA = searchStringA ?? "";
            if (searchStringA == "*")
            { searchStringA = ""; }
            ViewBag.CurrentFilterA = searchStringA;
            NormalSelA = NormalSelA ?? "*";
            ViewBag.CurSelA = NormalSelA;
            bool UnsA = UnscoredA ?? false;
            if (UnsA)
            { ViewBag.CurScoA = true; }
            else
            { ViewBag.CurScoA = false; };
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
            //取出可憑核資料並設定tab可視
            var items = from item in db.ts select item;
            items = items.OrderBy(x => x.emp1.eid);
            var itemA = items.Where(s => s.exm.Equals(r1)
                                  & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                                      );
            var itemAB = itemA;
            //過濾名字
            if (searchStringA.Trim().Length > 0 & searchStringA != "*")
            {
                itemA = itemA.Where(s => s.emp1.cname.Contains(searchStringA));
                if (itemA.Count() == 0) { itemA = itemAB;};
            };
            itemAB = itemA;
            //過濾分數
            if (UnsA)
            {
                itemA = itemA.Where(s => s.vl.Value == null);
                if (itemA.Count() == 0) { itemA = itemAB;};
            };
            itemAB = itemA;
            //過濾部門
            if (NormalSelA.Trim().Length > 0 & NormalSelA != "*")
            {
                itemA = itemA.Where(s => s.emp1.dep.title.Equals(NormalSelA));
                if (itemA.Count() == 0)
                {
                    itemA = itemAB;
                    ViewBag.sDefaultA = "*";
                }
                else
                {
                    ViewBag.sDefaultA = NormalSelA;
                };
            }
            else
            {
                ViewBag.sDefaultA = "*";
            };
            //
            var ts1 = itemA.ToPagedList(pageNumber: pageA ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabA = 0 ; }
            else
            { ViewBag.tabA = 1 ; };
            var itemB = items.Where(s => s.exm.Equals(r2)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var ts2 = itemB.ToPagedList(pageNumber: pageB ?? 1, pageSize: 5);
            if (itemB.Count() == 0)
            { ViewBag.tabB = 0; }
            else
            { ViewBag.tabB = 1; };           
            var itemC = items.Where(s => s.exm.Equals(r3)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var ts3 = itemC.ToPagedList(pageNumber: pageC ?? 1, pageSize: 5);
            if (itemC.Count() == 0)
            { ViewBag.tabC = 0; }
            else
            { ViewBag.tabC = 1; };
            //參數傳遞
            int page1 = pageA ?? 1;
            int page2 = pageB ?? 1;
            int page3 = pageC ?? 1;
            int id1 = idA ?? 0;
            int id2 = idB ?? 0;
            int id3 = idC ?? 0;
            ts temptsa = ts1.FirstOrDefault(x => x.id == id1);
            ts temptsb = ts2.FirstOrDefault(x => x.id == id2);
            ts temptsc = ts3.FirstOrDefault(x => x.id == id3);
            if (temptsa == null) { id1 = 0; };
            if (temptsb == null) { id2 = 0; };
            if (temptsc == null) { id3 = 0; };
            HomeIndexViewModels viewModels = new HomeIndexViewModels()
            {
                tsA = ts1,
                tsB = ts2,
                tsC = ts3,
                pageA = page1,
                pageB = page2,
                pageC = page3,
                v_IDA = id1,
                v_IDB = id2,
                v_IDC = id3,
                tsAD = temptsa,
                tsBD = temptsb,
                tsCD = temptsc
            };
            return View(viewModels);
        }

        [HttpPost]
        public ActionResult Index([Bind(Exclude = "")] HomeIndexViewModels tsv, string judge, int? tabNo, int? pageA, int? pageB, int? pageC, int? idA, int? idB, int? idC, string searchStringA, string searchStringB, string searchStringC, string NormalSelA, string NormalSelB, string NormalSelC, IEnumerable<bool> UnscoredA, IEnumerable<bool> UnscoredB, IEnumerable<bool> UnscoredC)
        {
            //決定預設顯示Tab
            ViewBag.TabValue = tabNo ?? 0;
            ViewBag.ErrorMsg = "*";
            ViewBag.CurrentFilterA = searchStringA;
            ViewBag.CurSelA = NormalSelA;
            if (UnscoredA != null && UnscoredA.Count() == 2)
            { ViewBag.CurScoA = true; }
            else
            { ViewBag.CurScoA = false; };
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
            //取出可評核資料並設定tab可視
            var items = from item in db.ts select item;
            items = items.OrderBy(x => x.emp1.eid);
            var itemA = items.Where(s => s.exm.Equals(r1)
                                  & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                                      );
            var itemAB = itemA;
            //過濾名字
            if (searchStringA.Trim().Length > 0 & searchStringA !="*")
            {
                itemA = itemA.Where(s => s.emp1.cname.Contains(searchStringA));
                if (itemA.Count() == 0) { itemA = itemAB; ViewBag.ErrorMsg = "沒有任何員工姓名包含(" + searchStringA + ")"; };
            };
            itemAB = itemA;
            //過濾分數
            if (UnscoredA != null && UnscoredA.Count() == 2)
            {
                itemA = itemA.Where(s => s.vl.Value == null);
                if (itemA.Count() == 0) { itemA = itemAB; ViewBag.ErrorMsg = "此查詢條件沒有任何未評核員工"; };
            };
            itemAB = itemA;
            //過濾部門
            if (NormalSelA.Trim().Length > 0 & NormalSelA != "*")
            {
                itemA = itemA.Where(s => s.emp1.dep.title.Equals(NormalSelA));
                if (itemA.Count() == 0) { 
                    itemA = itemAB;
                    ViewBag.sDefaultA = "*";
                    ViewBag.ErrorMsg = "此查詢條件沒有任何員工";
                }
                else {
                    ViewBag.sDefaultA = NormalSelA;
                };               
            }
            else { 
                ViewBag.sDefaultA = "*"; 
            };
            //
            if (judge == "查詢一般")
            {
                pageA = 1;
            };
            var ts1 = itemA.ToPagedList(pageNumber: pageA ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabA = 0; }
            else
            { ViewBag.tabA = 1; };
            itemA = items.Where(s => s.exm.Equals(r2)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var ts2 = itemA.ToPagedList(pageNumber: pageB ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabB = 0; }
            else
            { ViewBag.tabB = 1; };
            itemA = items.Where(s => s.exm.Equals(r3)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var ts3 = itemA.ToPagedList(pageNumber: pageC ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabC = 0; }
            else
            { ViewBag.tabC = 1; };
            ts tsm;
            if (judge == "儲存")
            {
                tsm = ts1.FirstOrDefault(x => x.id == tsv.tsAD.id);
                if (tsm != null & tsv.tsAD.vl > 0 & tsv.tsAD.vl <= 100)
                {
                    tsm.vl = tsv.tsAD.vl;
                    tsm.suggest = tsv.tsAD.suggest;
                    tsv.v_IDA = 0;
                }
            }
            else if (judge == "更新")
            {
                tsm = ts2.FirstOrDefault(x => x.id == tsv.tsBD.id);
                if (tsm != null & tsv.tsBD.vl > 0 & tsv.tsBD.vl <= 100)
                {
                    tsm.vl = tsv.tsBD.vl;
                    tsm.suggest = tsv.tsBD.suggest;
                    tsv.v_IDB = 0;
                }
            }
            else if (judge == "送出")
            {
                tsm = ts3.FirstOrDefault(x => x.id == tsv.tsCD.id);
                if (tsm != null & tsv.tsCD.vl > 0 & tsv.tsCD.vl <= 100)
                {
                    tsm.vl = tsv.tsCD.vl;
                    tsm.suggest = tsv.tsCD.suggest;
                    tsv.v_IDC = 0;
                }
            }
            else if (judge == "查詢一般")
            {
                ViewBag.TabValue = 1;
                ViewBag.NormalSelA = NormalSelA;
                tsv.v_IDA = 0;
            }
            else
            {

            }       
            db.SaveChanges();
            if (tsv.v_IDA == 0) {
                tsv.tsAD = items.FirstOrDefault(x => x.id == tsv.v_IDA);
            } else {
                tsv.tsAD = ts1.FirstOrDefault(x => x.id == tsv.v_IDA);
            };
            if (tsv.v_IDB == 0)
            {
                tsv.tsBD = items.FirstOrDefault(x => x.id == tsv.v_IDB);
            }
            else
            {
                tsv.tsBD = ts2.FirstOrDefault(x => x.id == tsv.v_IDB);
            };
            if (tsv.v_IDC == 0)
            {
                tsv.tsCD = items.FirstOrDefault(x => x.id == tsv.v_IDC);
            }
            else
            {
                tsv.tsCD = ts3.FirstOrDefault(x => x.id == tsv.v_IDC);
            };
            tsv.pageA = (int)pageA;
            tsv.pageB = (int)pageB;
            tsv.pageC = (int)pageC;
            tsv.tsA = ts1;
            tsv.tsB = ts2;
            tsv.tsC = ts3;
            return View(tsv);
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

        /// <summary>
        /// This method will be called via Ajax in Views/Index.cshtml 
        /// 取回被考核人員的部門別
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetD(String Kind)
        {
            //取出考核時間區段
            var dict = new Dictionary<string, string>();
            var itemots = from item in db.ots select item;
            foreach (var it1 in itemots)
            {
                dict[it1.Skey] = it1.Vl;
            };
            //判斷可撈取考核紀錄
            int r1 = 0;
            DateTime dt = Convert.ToDateTime(System.DateTime.Now.ToString("D"));
            //初評
            if (dt.CompareTo(Convert.ToDateTime(dict["startDate"])) >= 0 && dt.CompareTo(Convert.ToDateTime(dict["endDate1"])) <= 0)
            {
                r1 = Int16.Parse(Kind);
            };
            //複評
            if (dt.CompareTo(Convert.ToDateTime(dict["startDate2"])) >= 0 && dt.CompareTo(Convert.ToDateTime(dict["endDate2"])) <= 0)
            {
                r1 = Int16.Parse(Kind)+1;
            };
            var items = from item in db.ts select item;
            var itemA = items.Where(s => s.exm.Equals(r1)
                                  & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                                      ).Select(x => new
                                      {
                                          DP = x.emp1.dep.title
                                      }).Distinct().ToList();
            var jsonData = new
            {
                itemA
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}