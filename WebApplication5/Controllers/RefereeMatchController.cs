using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class RefereeMatchController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: RefereeMatch
        public ActionResult Index()
        {
            return View(db.refereeMatch.ToList());
        }

        // GET: RefereeMatch/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefereeMatch refereeMatch = db.refereeMatch.Find(id);
            if (refereeMatch == null)
            {
                return HttpNotFound();
            }
            return View(refereeMatch);
        }

        // GET: RefereeMatch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RefereeMatch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "refereeID,fixtures")] RefereeMatch refereeMatch)
        {
            if (ModelState.IsValid)
            {
                db.refereeMatch.Add(refereeMatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(refereeMatch);
        }

        // GET: RefereeMatch/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefereeMatch refereeMatch = db.refereeMatch.Find(id);
            if (refereeMatch == null)
            {
                return HttpNotFound();
            }
            return View(refereeMatch);
        }

        // POST: RefereeMatch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "refereeID,fixtures")] RefereeMatch refereeMatch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(refereeMatch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(refereeMatch);
        }

        // GET: RefereeMatch/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RefereeMatch refereeMatch = db.refereeMatch.Find(id);
            if (refereeMatch == null)
            {
                return HttpNotFound();
            }
            return View(refereeMatch);
        }

        // POST: RefereeMatch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RefereeMatch refereeMatch = db.refereeMatch.Find(id);
            db.refereeMatch.Remove(refereeMatch);
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
