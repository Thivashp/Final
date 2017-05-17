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
    public class SoccerTeamController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: SoccerTeam
        public ActionResult Index()
        {
            return View(db.soccerT.ToList());
        }

        // GET: SoccerTeam/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerTeams soccerTeams = db.soccerT.Find(id);
            if (soccerTeams == null)
            {
                return HttpNotFound();
            }
            return View(soccerTeams);
        }

        // GET: SoccerTeam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SoccerTeam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdV,TeamN,TeamT,Div,Email")] SoccerTeams soccerTeams)
        {

            if (ModelState.IsValid)
            {
                db.soccerT.Add(soccerTeams);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(soccerTeams);
        }

        // GET: SoccerTeam/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerTeams soccerTeams = db.soccerT.Find(id);
            if (soccerTeams == null)
            {
                return HttpNotFound();
            }
            return View(soccerTeams);
        }

        // POST: SoccerTeam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdV,TeamN,TeamT,Div,Email")] SoccerTeams soccerTeams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soccerTeams).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(soccerTeams);
        }

        // GET: SoccerTeam/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerTeams soccerTeams = db.soccerT.Find(id);
            if (soccerTeams == null)
            {
                return HttpNotFound();
            }
            return View(soccerTeams);
        }

        // POST: SoccerTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoccerTeams soccerTeams = db.soccerT.Find(id);
            db.soccerT.Remove(soccerTeams);
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
