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
    [AuthorizeAD(Groups = "webAdmin01")]
    public class actlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: actlogs    
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "瀏覽")]
        public ActionResult Index(int? page, int? itemsPerPage, string sortOrder, string currentFilter, string searchString)
        {
            ViewBag.CurrentItemsPerPage = itemsPerPage;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.OldSortParm = sortOrder;
            if (searchString != null)
            {
  //              page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.searchString = searchString;
            var actLogs = db.actlogs.OrderBy(x => x.id);;        
            var items = from item in actLogs select item;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Pepo.ToUpper().Contains(searchString.ToUpper())
                                        || s.App.ToUpper().Contains(searchString.ToUpper())
                                        || s.Act.ToUpper().Contains(searchString.ToUpper())
                                        || s.Ext.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderBy(x => x.id);
                    break;
                case "Date":
                    items = items.OrderBy(x => x.Pepo);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => x.Pepo);
                    break;
                default:
                    items = items.OrderByDescending(x => x.id);
                    break;
            }  
            return View(items.ToPagedList(pageNumber: page ?? 1, pageSize: itemsPerPage ?? 10));
        }

        // GET: actlogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actlog actlog = db.actlogs.Find(id);
            if (actlog == null)
            {
                return HttpNotFound();
            }
            return View(actlog);
        }

        // GET: actlogs/Create
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "新增")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: actlogs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "新增完成")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,App,Pepo,Act,Ext,Tm")] actlog actlog)
        {
            if (ModelState.IsValid)
            {
                db.actlogs.Add(actlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actlog);
        }

        // GET: actlogs/Edit/5
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "編輯")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actlog actlog = db.actlogs.Find(id);
            if (actlog == null)
            {
                return HttpNotFound();
            }
            return View(actlog);
        }

        // POST: actlogs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "編輯完成")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,App,Pepo,Act,Ext,Tm")] actlog actlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actlog);
        }

        // GET: actlogs/Delete/5
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "刪除")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            actlog actlog = db.actlogs.Find(id);
            if (actlog == null)
            {
                return HttpNotFound();
            }
            return View(actlog);
        }

        // POST: actlogs/Delete/5
        [LogActionFilter(ControllerName = "Logs管理", ActionName = "刪除完成")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            actlog actlog = db.actlogs.Find(id);
            db.actlogs.Remove(actlog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [LogActionFilter(ControllerName = "Logs管理", ActionName = "全部刪除")]
        public ActionResult DeleteAll()
        {
            return View();
        }

        [LogActionFilter(ControllerName = "Logs管理", ActionName = "全部刪除完成")]
        [HttpPost, ActionName("DeleteAll")]
        public ActionResult DeleteAllConfirmed()
        {
//            db.actlogs.RemoveRange(db.actlogs);
//            db.SaveChanges();
            db.Database.ExecuteSqlCommandAsync(@"DELETE FROM actlog");
            return RedirectToAction("Index");
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
