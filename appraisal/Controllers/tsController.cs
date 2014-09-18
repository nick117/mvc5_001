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
    public class tsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();       
        // GET: ts
        [AuthorizeAD(Groups = "webAdmin01,webHr01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "瀏覽")]
        public ActionResult Index(int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.CurrentItemsPerPage = itemsPerPage;
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
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.emp1.cname.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp1.eid.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp1.dep.title.ToUpper().Contains(searchString.ToUpper())
                                        || s.exm1.subject.ToUpper().Contains(searchString.ToUpper())
                                        || s.emp2.cname.ToUpper().Contains(searchString.ToUpper()));
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

        // GET: ts/Details/5
        [AuthorizeAD(Groups = "webAdmin01,webHr01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "明細")]
        public ActionResult Details(int? id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ts ts = db.ts.Find(id);
            if (ts == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.OldSortParm = sortOrder;
            ViewBag.CurrentFilter = searchString;
            return View(ts);
        }

        // GET: ts/Create
        [AuthorizeAD(Groups = "webAdmin01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "新增")]
        public ActionResult Create(int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.DropDownList1 = new SelectList(db.exms, "id", "subject");
            List<emp> reasonList = db.emps.OrderBy(m => m.cname).ToList();
            ViewBag.DropDownList2 = new SelectList(reasonList, "id", "cname");
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.OldSortParm = sortOrder;
            ViewBag.CurrentFilter = searchString;
            return View();
        }

        // POST: ts/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AuthorizeAD(Groups = "webAdmin01")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "新增完成")]
        public ActionResult Create([Bind(Include = "id,emp,exm,boss,vl,suggest,lockAudit")] ts ts, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.DropDownList1 = new SelectList(db.exms, "id", "subject");
            List<emp> reasonList = db.emps.OrderBy(m => m.cname).ToList();
            ViewBag.DropDownList2 = new SelectList(reasonList, "id", "cname");
            if (ModelState.IsValid)
            {
                db.ts.Add(ts);
                db.SaveChanges();
                return RedirectToAction("Index", new { page = page, itemsPerPage = itemsPerPage, sortOrder = sortOrder, searchString = searchString });
            }

            return View(ts);
        }

        // GET: ts/Edit/5
        [AuthorizeAD(Groups = "webAdmin01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "編輯")]
        public ActionResult Edit(int? id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ts ts = db.ts.Find(id);
            if (ts == null)
            {
                return HttpNotFound();
            }
            var empl = db.emps.OrderBy(m => m.cname).ToList();
            SelectList selectList1 = new SelectList(empl, "id", "cname", ts.emp);
            SelectList selectList2 = new SelectList(empl, "id", "cname", ts.boss);
            var exml = db.exms.ToList();
            SelectList selectList3 = new SelectList(exml, "id", "subject", ts.exm);
            int ipp = itemsPerPage == null ? 10 : (int)itemsPerPage;
            tsEditViewModels viewModel = new tsEditViewModels()
            {
                tsa = ts,
                empList = selectList1,
                bossList = selectList2,
                exmList = selectList3,
                pageno = (int)page,
                v_itemsPerPage = ipp,
                v_currentFilter = searchString,
                v_sortOrder = sortOrder
            };
            return View(viewModel);
        }

        // POST: ts/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AuthorizeAD(Groups = "webAdmin01")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "編輯完成")]
        public ActionResult Edit([Bind(Include = "tsa,pageno,v_itemsPerPage,v_currentFilter,v_sortOrder")] tsEditViewModels tsv)
        {
            if (ModelState.IsValid)
            {
                var tsb = tsv.tsa;
                db.Entry(tsb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { page = tsv.pageno, itemsPerPage = tsv.v_itemsPerPage, sortOrder = tsv.v_sortOrder, searchString = tsv.v_currentFilter });
            }
            return View(tsv);
        }

        // GET: ts/Delete/5
        [AuthorizeAD(Groups = "webAdmin01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "刪除")]
        public ActionResult Delete(int? id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.CurrentFilter = searchString;
            ViewBag.OldSortParm = sortOrder;
            ts ts = db.ts.Find(id);
            if (ts == null)
            {
                return HttpNotFound();
            }
            return View(ts);
        }

        // POST: ts/Delete/5
        [AuthorizeAD(Groups = "webAdmin01")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "刪除完成")]
        public ActionResult DeleteConfirmed(int id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ts ts = db.ts.Find(id);
            db.ts.Remove(ts);
            db.SaveChanges();
            return RedirectToAction("Index", new { page = page, itemsPerPage = itemsPerPage, sortOrder = sortOrder, searchString = searchString });
        }

        [AuthorizeAD(Groups = "webAdmin01")]
        public ActionResult RenewProgress()
        {
            return View();
        }

        // TODO: Add progress bar here.
        [AuthorizeAD(Groups = "webAdmin01")]
        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "全部刪除並更新")]
        public ActionResult Renew(int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.OldSortParm = sortOrder;
            ViewBag.CurrentFilter = searchString;
            return View();
        }

        [LogActionFilter(ControllerName = "考核紀錄管理", ActionName = "全部刪除並更新完成")]
        [HttpPost, ActionName("Renew")]
        public ActionResult RenewConfirmed()
        {
            db.Database.ExecuteSqlCommandAsync(@"DELETE FROM ts");
            Task.Run(() =>ImportAsync());
            return RedirectToAction("RenewProgress");
        }

        private async void ImportAsync()
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            int t1, empid;
            char[] delimiterChars = { '/' };
            string[] CardNos;
            var find = from data in db1.importtss
                       where data.CardNo1 != null
                       select data;
            foreach (var item in find)
            {
                switch (item.Type)
                {
                    case "組級":
                        t1 = 3;
                        break;
                    case "部級":
                        t1 = 5;
                        break;
                    default:
                        t1 = 1;
                        break;
                }
                var empq = db1.emps.Where(s1 => s1.eid.ToUpper().Equals(item.CardNo.ToUpper())).FirstOrDefault();
                empid = (empq != null) ? empq.id : 1;
                CardNos = item.CardNo1.Split(delimiterChars);
                foreach (string s in CardNos)
                {
                    ts tsa = new ts();
                    tsa.emp = empid;
                    empq = db1.emps.Where(s1 => s1.eid.ToUpper().Equals(s.ToUpper())).FirstOrDefault();
                    tsa.boss = (empq != null) ? empq.id : 1;
                    tsa.exm = t1;
                    db1.ts.Add(tsa);
                }
                CardNos = item.CardNo2.Split(delimiterChars);
                foreach (string s in CardNos)
                {
                    ts tsa = new ts();
                    tsa.emp = empid;
                    empq = db1.emps.Where(s1 => s1.eid.ToUpper().Equals(s.ToUpper())).FirstOrDefault();
                    tsa.boss = (empq != null) ? empq.id : 1;
                    tsa.exm = t1 + 1;
                    db1.ts.Add(tsa);
                }
                db1.SaveChanges();
            }
            db1.Dispose();
        }

        [HttpPost]
        public ActionResult HasData()
        {
            JObject jo = new JObject();
            bool result = !db.ts.Count().Equals(0);
            jo.Add("Msg", result.ToString());
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }

        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "Appraisal_",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "績效考核結果",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData()
        {
            var query = db.ts
                          .OrderBy(x => x.exm1.subject)
                          .ThenBy(x => x.emp1.dept)
                          .ThenBy(x => x.emp1.eid);

            JArray jObjects = new JArray();

            foreach (var item in query)
            {
                var jo = new JObject();
                jo.Add("部門", item.emp1.dep.title);               
                jo.Add("姓名", item.emp1.cname);
                jo.Add("卡號", item.emp1.eid);
                jo.Add("職稱", item.emp1.title);
                jo.Add("主管", item.emp2.cname);
                jo.Add("主管卡號", item.emp2.eid);
                jo.Add("得分", item.vl);
                jo.Add("評語", item.suggest);
                jo.Add("評核類別", item.exm1.subject);
                jObjects.Add(jo);
            }
            return jObjects;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
