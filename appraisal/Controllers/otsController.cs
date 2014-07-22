using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using appraisal.Models;

namespace appraisal.Controllers
{
    public class otsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ots
        public ActionResult Index()
        {
            return View(db.ots.ToList());
        }

        // GET: ots/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(id);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // GET: ots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ots/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
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
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(id);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // POST: ots/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
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
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ots ots = db.ots.Find(id);
            if (ots == null)
            {
                return HttpNotFound();
            }
            return View(ots);
        }

        // POST: ots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ots ots = db.ots.Find(id);
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
