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
    public class CricketTeamController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: CricketTeam
        public ActionResult Index()
        {
            return View(db.cricketT.ToList());
        }

        // GET: CricketTeam/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketTeams cricketTeams = db.cricketT.Find(id);
            if (cricketTeams == null)
            {
                return HttpNotFound();
            }
            return View(cricketTeams);
        }

        // GET: CricketTeam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CricketTeam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdV,TeamN,TeamT,Div,sportType,Email")] CricketTeams cricketTeams)
        {
            if (ModelState.IsValid)
            {
                db.cricketT.Add(cricketTeams);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cricketTeams);
        }

        // GET: CricketTeam/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketTeams cricketTeams = db.cricketT.Find(id);
            if (cricketTeams == null)
            {
                return HttpNotFound();
            }
            return View(cricketTeams);
        }

        // POST: CricketTeam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdV,TeamN,TeamT,Div,sportType,Email")] CricketTeams cricketTeams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cricketTeams).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cricketTeams);
        }

        // GET: CricketTeam/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketTeams cricketTeams = db.cricketT.Find(id);
            if (cricketTeams == null)
            {
                return HttpNotFound();
            }
            return View(cricketTeams);
        }

        // POST: CricketTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CricketTeams cricketTeams = db.cricketT.Find(id);
            db.cricketT.Remove(cricketTeams);
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
