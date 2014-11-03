using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Mvc;
using PagedList;
using appraisal.Filters;
using appraisal.Models;
using System.Web.UI.DataVisualization.Charting;

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
            ViewBag.sDefaultB = sDefaultB ?? "*";
            ViewBag.sDefaultC = sDefaultC ?? "*";
            ViewBag.ErrorMsg = "*";
            searchStringA = searchStringA ?? "";
            if (searchStringA == "*")
            { searchStringA = ""; };
            searchStringB = searchStringB ?? "";
            if (searchStringB == "*")
            { searchStringB = ""; };
            searchStringC = searchStringC ?? "";
            if (searchStringC == "*")
            { searchStringC = ""; };
            ViewBag.CurrentFilterA = searchStringA;
            ViewBag.CurrentFilterB = searchStringB;
            ViewBag.CurrentFilterC = searchStringC;
            NormalSelA = NormalSelA ?? "*";
            NormalSelB = NormalSelB ?? "*";
            NormalSelC = NormalSelC ?? "*";
            ViewBag.CurSelA = NormalSelA;
            ViewBag.CurSelB = NormalSelB;
            ViewBag.CurSelC = NormalSelC;
            bool UnsA = UnscoredA ?? false;
            if (UnsA)
            { ViewBag.CurScoA = true; }
            else
            { ViewBag.CurScoA = false; };
            bool UnsB = UnscoredB ?? false;
            if (UnsB)
            { ViewBag.CurScoB = true; }
            else
            { ViewBag.CurScoB = false; };
            bool UnsC = UnscoredC ?? false;
            if (UnsC)
            { ViewBag.CurScoC = true; }
            else
            { ViewBag.CurScoC = false; };
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
            Session["ChartA"] = CountGrade(itemA);
            var ts1 = itemA.ToPagedList(pageNumber: pageA ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabA = 0 ; }
            else
            { ViewBag.tabA = 1 ; };
            var itemB = items.Where(s => s.exm.Equals(r2)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var itemBB = itemB;
            //過濾名字
            if (searchStringB.Trim().Length > 0 & searchStringB != "*")
            {
                itemB = itemB.Where(s => s.emp1.cname.Contains(searchStringB));
                if (itemB.Count() == 0) { itemB = itemBB; };
            };
            itemBB = itemB;
            //過濾分數
            if (UnsB)
            {
                itemB = itemB.Where(s => s.vl.Value == null);
                if (itemB.Count() == 0) { itemB = itemBB; };
            };
            itemBB = itemB;
            //過濾部門
            if (NormalSelB.Trim().Length > 0 & NormalSelB != "*")
            {
                itemB = itemB.Where(s => s.emp1.dep.title.Equals(NormalSelB));
                if (itemB.Count() == 0)
                {
                    itemB = itemBB;
                    ViewBag.sDefaultB = "*";
                }
                else
                {
                    ViewBag.sDefaultB = NormalSelB;
                };
            }
            else
            {
                ViewBag.sDefaultB = "*";
            };
            //
            Session["ChartB"] = CountGrade(itemB);
            var ts2 = itemB.ToPagedList(pageNumber: pageB ?? 1, pageSize: 5);
            if (itemB.Count() == 0)
            { ViewBag.tabB = 0; }
            else
            { ViewBag.tabB = 1; };           
            var itemC = items.Where(s => s.exm.Equals(r3)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var itemCB = itemC;
            //過濾名字
            if (searchStringC.Trim().Length > 0 & searchStringC != "*")
            {
                itemC = itemC.Where(s => s.emp1.cname.Contains(searchStringC));
                if (itemC.Count() == 0) { itemC = itemCB; };
            };
            itemCB = itemC;
            //過濾分數
            if (UnsC)
            {
                itemC = itemC.Where(s => s.vl.Value == null);
                if (itemC.Count() == 0) { itemC = itemCB; };
            };
            itemCB = itemC;
            //過濾部門
            if (NormalSelC.Trim().Length > 0 & NormalSelC != "*")
            {
                itemC = itemC.Where(s => s.emp1.dep.title.Equals(NormalSelC));
                if (itemC.Count() == 0)
                {
                    itemC = itemCB;
                    ViewBag.sDefaultC = "*";
                }
                else
                {
                    ViewBag.sDefaultC = NormalSelC;
                };
            }
            else
            {
                ViewBag.sDefaultC = "*";
            };
            //
            Session["ChartC"] = CountGrade(itemC);
            var ts3 = itemC.ToPagedList(pageNumber: pageC ?? 1, pageSize: 5);
            if (itemC.Count() == 0)
            { ViewBag.tabC = 0; }
            else
            { ViewBag.tabC = 1; };
            //調整頁籤順序
            if (tabNo == 3) { ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB + ViewBag.tabC; };
            if (tabNo == 2) { ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB; };
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
            //ViewBag.TabValue = tabNo ?? 0;
            ViewBag.ErrorMsg = "*";
            ViewBag.CurrentFilterA = searchStringA;
            ViewBag.CurrentFilterB = searchStringB;
            ViewBag.CurrentFilterC = searchStringC;
            ViewBag.CurSelA = NormalSelA;
            ViewBag.CurSelB = NormalSelB;
            ViewBag.CurSelC = NormalSelC;
            if (UnscoredA != null && UnscoredA.Count() == 2)
            { ViewBag.CurScoA = true; }
            else
            { ViewBag.CurScoA = false; };
            if (UnscoredB != null && UnscoredB.Count() == 2)
            { ViewBag.CurScoB = true; }
            else
            { ViewBag.CurScoB = false; };
            if (UnscoredC != null && UnscoredC.Count() == 2)
            { ViewBag.CurScoC = true; }
            else
            { ViewBag.CurScoC = false; };
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
            //查詢時一律回第一頁
            if (judge == "查詢一般")
            {
                pageA = 1;
            }
            else if (judge == "查詢組級")
            {
                pageB = 1;
            }
            else if (judge == "查詢部級")
            {
                pageC = 1;
            }
            ;
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
            var ts1 = itemA.ToPagedList(pageNumber: pageA ?? 1, pageSize: 5);
            if (itemA.Count() == 0)
            { ViewBag.tabA = 0; }
            else
            { ViewBag.tabA = 1; };
            var item2A = items.Where(s => s.exm.Equals(r2)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var item2AB = item2A;
            //過濾名字
            if (searchStringB.Trim().Length > 0 & searchStringB != "*")
            {
                item2A = item2A.Where(s => s.emp1.cname.Contains(searchStringB));
                if (item2A.Count() == 0) { item2A = item2AB; ViewBag.ErrorMsg = "沒有任何員工姓名包含(" + searchStringB + ")"; };
            };
            item2AB = item2A;
            //過濾分數
            if (UnscoredB != null && UnscoredB.Count() == 2)
            {
                item2A = item2A.Where(s => s.vl.Value == null);
                if (item2A.Count() == 0) { item2A = item2AB; ViewBag.ErrorMsg = "此查詢條件沒有任何未評核員工"; };
            };
            item2AB = item2A;
            //過濾部門
            if (NormalSelB.Trim().Length > 0 & NormalSelB != "*")
            {
                item2A = item2A.Where(s => s.emp1.dep.title.Equals(NormalSelB));
                if (item2A.Count() == 0)
                {
                    item2A = item2AB;
                    ViewBag.sDefaultB = "*";
                    ViewBag.ErrorMsg = "此查詢條件沒有任何員工";
                }
                else
                {
                    ViewBag.sDefaultB = NormalSelB;
                };
            }
            else
            {
                ViewBag.sDefaultB = "*";
            };
            var ts2 = item2A.ToPagedList(pageNumber: pageB ?? 1, pageSize: 5);
            if (item2A.Count() == 0)
            { ViewBag.tabB = 0; }
            else
            { ViewBag.tabB = 1; };
            var item3A = items.Where(s => s.exm.Equals(r3)
                      & s.emp2.eid.Equals(User.Identity.Name.ToUpper())
                          );
            var item3AB = item3A;
            //過濾名字
            if (searchStringC.Trim().Length > 0 & searchStringC != "*")
            {
                item3A = item3A.Where(s => s.emp1.cname.Contains(searchStringC));
                if (item3A.Count() == 0) { item3A = item3AB; ViewBag.ErrorMsg = "沒有任何員工姓名包含(" + searchStringC + ")"; };
            };
            item3AB = item3A;
            //過濾分數
            if (UnscoredC != null && UnscoredC.Count() == 2)
            {
                item3A = item3A.Where(s => s.vl.Value == null);
                if (item3A.Count() == 0) { item3A = item3AB; ViewBag.ErrorMsg = "此查詢條件沒有任何未評核員工"; };
            };
            item3AB = item3A;
            //過濾部門
            if (NormalSelC.Trim().Length > 0 & NormalSelC != "*")
            {
                item3A = item3A.Where(s => s.emp1.dep.title.Equals(NormalSelC));
                if (item3A.Count() == 0)
                {
                    item3A = item3AB;
                    ViewBag.sDefaultC = "*";
                    ViewBag.ErrorMsg = "此查詢條件沒有任何員工";
                }
                else
                {
                    ViewBag.sDefaultC = NormalSelC;
                };
            }
            else
            {
                ViewBag.sDefaultC = "*";
            };
            var ts3 = item3A.ToPagedList(pageNumber: pageC ?? 1, pageSize: 5);
            if (item3A.Count() == 0)
            { ViewBag.tabC = 0; }
            else
            { ViewBag.tabC = 1; };
            ts tsm;
            if (judge == "儲存")
            {
                ViewBag.TabValue = 1;
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
                ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB;
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
                    ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB + ViewBag.tabC;
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
            else if (judge == "查詢組級")
            {
                ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB;
                ViewBag.NormalSelB = NormalSelB;
                tsv.v_IDB = 0;
            }
            else if (judge == "查詢部級")
            {
                ViewBag.TabValue = ViewBag.tabA + ViewBag.tabB + ViewBag.tabC;
                ViewBag.NormalSelC = NormalSelC;
                tsv.v_IDC = 0;
            }
            else
            {

            };      
            db.SaveChanges();
            Session["ChartA"] = CountGrade(itemA);
            Session["ChartB"] = CountGrade(item2A);
            Session["ChartC"] = CountGrade(item3A);
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

        public ActionResult GetPieChartA()
        {
            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            int[] yValues = (int[])Session["ChartA"];
            string[] xValues = { "A+", "A", "B", "C", "D", "未" };

            chart.Series["Data"].Points.DataBindXY(xValues, yValues);
            chart.Series[0].Label = "#VALX #PERCENT{P0}"; // THIS IS TO GET THE PERCENTAGE
            //Other chart formatting and data source omitted.

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.ToArray(), "image/png");
        }

        public ActionResult GetPieChartB()
        {
            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            int[] yValues = (int[])Session["ChartB"];
            string[] xValues = { "A+", "A", "B", "C", "D", "未" };

            chart.Series["Data"].Points.DataBindXY(xValues, yValues);
            chart.Series[0].Label = "#VALX #PERCENT{P0}"; // THIS IS TO GET THE PERCENTAGE
            //Other chart formatting and data source omitted.

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.ToArray(), "image/png");
        }

        public ActionResult GetPieChartC()
        {
            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea());

            chart.Series.Add(new Series("Data"));
            chart.Series["Data"].ChartType = SeriesChartType.Pie;
            int[] yValues = (int[])Session["ChartC"];
            string[] xValues = { "A+", "A", "B", "C", "D", "未" };

            chart.Series["Data"].Points.DataBindXY(xValues, yValues);
            chart.Series[0].Label = "#VALX #PERCENT{P0}"; // THIS IS TO GET THE PERCENTAGE
            //Other chart formatting and data source omitted.

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.ToArray(), "image/png");
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

        private int[] CountGrade(IQueryable<ts> tsList)
        {
            int[] tGrade = new int[6];
            int Total = 0;
            tGrade[0] = tsList.Where(s => s.vl >= 95).Count();
            Total += tGrade[0];
            tGrade[1] = tsList.Where(s => s.vl >= 85).Count() - Total;
            Total += tGrade[1];
            tGrade[2] = tsList.Where(s => s.vl >= 70).Count() - Total;
            Total += tGrade[2];
            tGrade[3] = tsList.Where(s => s.vl >= 60).Count() - Total;
            Total += tGrade[3];
            tGrade[4] = tsList.Where(s => s.vl >= 0).Count() - Total;
            Total += tGrade[4];
            tGrade[5] = tsList.Count() - Total;
            return tGrade;
        }

    }
}