using PagedList;
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
    public class CricketLeagueController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: CricketLeague
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {

                return View(db.cricketL.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // GET: CricketLeague/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLeagues cricketLeagues = db.cricketL.Find(id);
            if (cricketLeagues == null)
            {
                return HttpNotFound();
            }
            return View(cricketLeagues);
        }

        // GET: CricketLeague/Create
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

        // POST: CricketLeague/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] CricketLeagues cricketLeagues)
        {
            if (ModelState.IsValid)
            {
                db.cricketL.Add(cricketLeagues);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cricketLeagues);
        }

        // GET: CricketLeague/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLeagues cricketLeagues = db.cricketL.Find(id);
            if (cricketLeagues == null)
            {
                return HttpNotFound();
            }
            return View(cricketLeagues);
        }

        // POST: CricketLeague/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdV,leagueTitle,category,Sdate,regFee,CostPR")] CricketLeagues cricketLeagues)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cricketLeagues).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cricketLeagues);
        }

        // GET: CricketLeague/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLeagues cricketLeagues = db.cricketL.Find(id);
            if (cricketLeagues == null)
            {
                return HttpNotFound();
            }
            return View(cricketLeagues);
        }

        // POST: CricketLeague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CricketLeagues cricketLeagues = db.cricketL.Find(id);
            db.cricketL.Remove(cricketLeagues);
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
