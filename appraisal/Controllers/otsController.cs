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

namespace appraisal.Controllers
{
    //[Authorize(Users = @"administrator, K1336")]
    [AuthorizeAD(Groups = "webAdmin01,webHr01")]
    public class otsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ots    
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "瀏覽")]
        public ActionResult Index()
        {
            return View(db.ots.ToList().OrderBy(model => model.Vl));
        }

        // GET: ots/Details/5
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "明細")]
        public ActionResult Details(string sid)
        {
            if (sid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(sid);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // GET: ots/Create
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "新增")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ots/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "新增完成")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Skey,Vl")] ots ots)
        {
            if (ModelState.IsValid)
            {
                db.ots.Add(ots);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ots);
        }

        // GET: ots/Edit/5
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "編輯")]
        public ActionResult Edit(string sid)
        {
            if (sid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(sid);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // POST: ots/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
         [LogActionFilter(ControllerName = "評核時間管理", ActionName = "編輯完成")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Skey,Vl")] ots ots)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ots).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ots);
        }

        // GET: ots/Delete/5
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "刪除")]
        public ActionResult Delete(string sid)
        {
            if (sid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(sid);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // POST: ots/Delete/5
        [LogActionFilter(ControllerName = "評核時間管理", ActionName = "刪除完成")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string sid)
        {
            ots ots = db.ots.Find(sid);
            db.ots.Remove(ots);
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
