﻿using System;
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
    public class depsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: deps
        public ActionResult Index()
        {
            return View(db.deps.ToList());
        }

        // GET: deps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dep dep = db.deps.Find(id);
            if (dep == null)
            {
                return HttpNotFound();
            }
            return View(dep);
        }

        // GET: deps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: deps/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title")] dep dep)
        {
            if (ModelState.IsValid)
            {
                db.deps.Add(dep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dep);
        }

        // GET: deps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dep dep = db.deps.Find(id);
            if (dep == null)
            {
                return HttpNotFound();
            }
            return View(dep);
        }

        // POST: deps/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title")] dep dep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dep);
        }

        // GET: deps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dep dep = db.deps.Find(id);
            if (dep == null)
            {
                return HttpNotFound();
            }
            return View(dep);
        }

        // POST: deps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dep dep = db.deps.Find(id);
            db.deps.Remove(dep);
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
