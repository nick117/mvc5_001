using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appraisal.Models;
using appraisal.Filters;
using PagedList;

namespace appraisal.Controllers
{
 //   [Authorize(Users = @"administrator, K1336")]
    [AuthorizeAD(Groups = "webAdmin01")]
    public class empsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: emps     
        [LogActionFilter(ControllerName="員工資料管理",ActionName="瀏覽")]
        public ActionResult Index(int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (itemsPerPage != null)
            { ViewBag.CurrentItemsPerPage = itemsPerPage; }
            else
            { ViewBag.CurrentItemsPerPage = 10; };           
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.OldSortParm = sortOrder;
            if (searchString == "*")
            { searchString = ""; }
            ViewBag.CurrentFilter = searchString;
            var emp = db.emps.OrderBy(x => x.id); ;
            var items = from item in emp select item;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.eid.ToUpper().Contains(searchString.ToUpper())
                                        || s.cname.ToUpper().Contains(searchString.ToUpper())
                                        || s.dep.title.ToUpper().Contains(searchString.ToUpper())
                                        || s.title.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderBy(x => x.eid);
                    break;
                case "Date":
                    items = items.OrderBy(x => x.dep.title);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => x.dep.title);
                    break;
                default:
                    items = items.OrderByDescending(x => x.eid);
                    break;
            }
            return View(items.ToPagedList(pageNumber: page ?? 1, pageSize: itemsPerPage ?? 10));
        }

        // GET: emps/Details/5
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "明細")]
        public ActionResult Details(int? id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emp emp = db.emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.OldSortParm = sortOrder;
            ViewBag.CurrentFilter = searchString;
            return View(emp);
        }

        // GET: emps/Create
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "新增")]
        public ActionResult Create(int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.DropDownList = new SelectList(db.deps, "Id", "title");
            ViewBag.CurrentPage = page;
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.OldSortParm = sortOrder;
            ViewBag.CurrentFilter = searchString;
            return View();
        }

        // POST: emps/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "新增完成")]
        public ActionResult Create([Bind(Include = "id,eid,cname,dept,title")] emp emp, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            ViewBag.DropDownList = new SelectList(db.deps, "Id", "title");
            if (ModelState.IsValid)
            {
                db.emps.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index", new { page = page, itemsPerPage = itemsPerPage, sortOrder = sortOrder, searchString = searchString });
            }
            return View(emp);
        }

        // GET: emps/Edit/5
       [LogActionFilter(ControllerName = "員工資料管理", ActionName = "編輯")]
        public ActionResult Edit(int? id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            emp emp = db.emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            var depl = db.deps.ToList();
            SelectList selectList = new SelectList(depl, "id", "title", emp.dept);
            empEditViewModels viewModel = new empEditViewModels()
                {
                    emp1 = emp,
                     depList = selectList,
                     pageno = (int)page,
                     v_itemsPerPage = (int)itemsPerPage,
                    v_currentFilter = searchString,
                     v_sortOrder = sortOrder
                };
            return View(viewModel);
        }

        // POST: emps/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "編輯完成")]
        public ActionResult Edit([Bind(Include = "emp1,pageno,v_itemsPerPage,v_currentFilter,v_sortOrder")] empEditViewModels empv)
        {
            if (ModelState.IsValid)
            {
                var empa = empv.emp1;
                db.Entry(empa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { page = empv.pageno, itemsPerPage = empv.v_itemsPerPage, sortOrder = empv.v_sortOrder, searchString = empv.v_currentFilter });
            }
            return View(empv);
        }

        // GET: emps/Delete/5
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "刪除")]
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
            emp emp = db.emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: emps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LogActionFilter(ControllerName = "員工資料管理", ActionName = "刪除完成")]
        public ActionResult DeleteConfirmed(int id, int? page, int? itemsPerPage, string sortOrder, string searchString)
        {
            emp emp = db.emps.Find(id);
            db.emps.Remove(emp);
            db.SaveChanges();
            return RedirectToAction("Index", new { page = page, itemsPerPage = itemsPerPage, sortOrder = sortOrder, searchString = searchString });
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
