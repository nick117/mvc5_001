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
    public class exmsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: exms
        public ActionResult Index()
        {
            return View(db.exms.ToList());
        }

        // GET: exms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exm exm = db.exms.Find(id);
            if (exm == null)
            {
                return HttpNotFound();
            }
            return View(exm);
        }

        // GET: exms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: exms/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,subject,sn")] exm exm)
        {
            if (ModelState.IsValid)
            {
                db.exms.Add(exm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exm);
        }

        // GET: exms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exm exm = db.exms.Find(id);
            if (exm == null)
            {
                return HttpNotFound();
            }
            return View(exm);
        }

        // POST: exms/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,subject,sn")] exm exm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exm);
        }

        // GET: exms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            exm exm = db.exms.Find(id);
            if (exm == null)
            {
                return HttpNotFound();
            }
            return View(exm);
        }

        // POST: exms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            exm exm = db.exms.Find(id);
            db.exms.Remove(exm);
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
