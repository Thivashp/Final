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
    public class VolleyballLeagueController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: VolleyballLeague
        public ActionResult Index()
        {
            return View(db.volleyL.ToList());
        }

        // GET: VolleyballLeague/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolleyballLeagues volleyballLeagues = db.volleyL.Find(id);
            if (volleyballLeagues == null)
            {
                return HttpNotFound();
            }
            return View(volleyballLeagues);
        }

        // GET: VolleyballLeague/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VolleyballLeague/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] VolleyballLeagues volleyballLeagues)
        {
            if (ModelState.IsValid)
            {
                db.volleyL.Add(volleyballLeagues);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(volleyballLeagues);
        }

        // GET: VolleyballLeague/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolleyballLeagues volleyballLeagues = db.volleyL.Find(id);
            if (volleyballLeagues == null)
            {
                return HttpNotFound();
            }
            return View(volleyballLeagues);
        }

        // POST: VolleyballLeague/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] VolleyballLeagues volleyballLeagues)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volleyballLeagues).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(volleyballLeagues);
        }

        // GET: VolleyballLeague/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VolleyballLeagues volleyballLeagues = db.volleyL.Find(id);
            if (volleyballLeagues == null)
            {
                return HttpNotFound();
            }
            return View(volleyballLeagues);
        }

        // POST: VolleyballLeague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VolleyballLeagues volleyballLeagues = db.volleyL.Find(id);
            db.volleyL.Remove(volleyballLeagues);
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
