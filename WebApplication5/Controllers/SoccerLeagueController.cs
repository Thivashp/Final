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
    public class SoccerLeagueController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: SoccerLeague
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {

                return View(db.soccerL.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
        }

        // GET: SoccerLeague/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLeagues soccerLeagues = db.soccerL.Find(id);
            if (soccerLeagues == null)
            {
                return HttpNotFound();
            }
            return View(soccerLeagues);
        }

        // GET: SoccerLeague/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Employee"))
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        // POST: SoccerLeague/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] SoccerLeagues soccerLeagues, string teamType)
        {
            if (ModelState.IsValid)
            {
                soccerLeagues.refLevel = teamType;
                db.soccerL.Add(soccerLeagues);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(soccerLeagues);
        }

        // GET: SoccerLeague/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLeagues soccerLeagues = db.soccerL.Find(id);
            if (soccerLeagues == null)
            {
                return HttpNotFound();
            }
            return View(soccerLeagues);
        }

        // POST: SoccerLeague/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] SoccerLeagues soccerLeagues)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soccerLeagues).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(soccerLeagues);
        }

        // GET: SoccerLeague/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLeagues soccerLeagues = db.soccerL.Find(id);
            if (soccerLeagues == null)
            {
                return HttpNotFound();
            }
            return View(soccerLeagues);
        }

        // POST: SoccerLeague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoccerLeagues soccerLeagues = db.soccerL.Find(id);
            db.soccerL.Remove(soccerLeagues);
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
