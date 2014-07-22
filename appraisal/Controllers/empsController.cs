using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appraisal.Models;
using PagedList;

namespace appraisal.Controllers
{
    public class empsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: emps
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
        public ActionResult Details(int? id)
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
            return View(emp);
        }

        // GET: emps/Create
        public ActionResult Create()
        {
            ViewBag.DropDownList = new SelectList(db.deps, "Id", "title");
            return View();
        }

        // POST: emps/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,eid,cname,dept,title")] emp emp)
        {
            if (ModelState.IsValid)
            {
                db.emps.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        // GET: emps/Edit/5
        public ActionResult Edit(int? id)
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
                     depList = selectList
                };
            return View(viewModel);
        }

        // POST: emps/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emp1")] empEditViewModels emp)
        {
            if (ModelState.IsValid)
            {
                var empa = emp.emp1;
                db.Entry(empa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        // GET: emps/Delete/5
        public ActionResult Delete(int? id)
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
            return View(emp);
        }

        // POST: emps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            emp emp = db.emps.Find(id);
            db.emps.Remove(emp);
            db.SaveChanges();
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
