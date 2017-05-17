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
    public class UmpireratesController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: Umpirerates
        public ActionResult Index()
        {
            return View(db.umpirerate.ToList());
        }

        // GET: Umpirerates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umpirerate umpirerate = db.umpirerate.Find(id);
            if (umpirerate == null)
            {
                return HttpNotFound();
            }
            return View(umpirerate);
        }

        // GET: Umpirerates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Umpirerates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,homerating,awayrating,refID,fixture,home,away,played,done,homedone,awaydone,hrating,arating")] Umpirerate umpirerate)
        {
            if (ModelState.IsValid)
            {
                db.umpirerate.Add(umpirerate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(umpirerate);
        }

        // GET: Umpirerates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umpirerate umpirerate = db.umpirerate.Find(id);
            if (umpirerate == null)
            {
                return HttpNotFound();
            }
            return View(umpirerate);
        }

        // POST: Umpirerates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,homerating,awayrating,refID,fixture,home,away,played,done,homedone,awaydone,hrating,arating")] Umpirerate umpirerate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umpirerate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(umpirerate);
        }

        // GET: Umpirerates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umpirerate umpirerate = db.umpirerate.Find(id);
            if (umpirerate == null)
            {
                return HttpNotFound();
            }
            return View(umpirerate);
        }

        // POST: Umpirerates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Umpirerate umpirerate = db.umpirerate.Find(id);
            db.umpirerate.Remove(umpirerate);
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

        public ActionResult umpirerate()
        {
            if (User.IsInRole("Customer"))
            {
            Session["test09"] = " ";
            Session["test10"] = " ";
            ViewBag.ddlS = new SelectList(db.cricketT.ToList().Where(x => x.Email.ToUpper() == User.Identity.Name.ToUpper()), "IdV", "TeamN");//Creates the division DDL
               return View();
            }
            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public ActionResult umpirerate(string ddlS, string ddlT, string fb, string comment)
        {
            try
            {
                Session["test10"] = " ";
                ViewBag.ddlS = new SelectList(db.cricketT.ToList().Where(x => x.Email.ToUpper() == User.Identity.Name.ToUpper()), "IdV", "TeamN");//Creates the division DDL
                var team = db.cricketT.ToList().Find(x => x.IdV == Convert.ToInt16(ddlS));
                var findTeam = db.CricketLeague.ToList().Find(x => x.teamName == team.TeamN);


                if (findTeam != null)
                {
                    Session["test09"] = "true";
                    var allTeams = db.umpirerate.ToList().FindAll(x => x.fixture.Contains(findTeam.teamName));
                    ViewBag.ddlT = new SelectList(allTeams.ToList().Where(x => x.home == team.TeamN && x.homedone == false || x.away == team.TeamN && x.awaydone == false), "Id", "fixture");//Creates the division DDL

                    if (ddlT != null)
                    {

                        var d = db.umpirerate.ToList().Find(x => x.Id == Convert.ToInt16(ddlT));
                        if (findTeam.teamName == d.home)
                        {
                            d.homerating = comment;
                            d.hrating = fb;
                            d.homedone = true;

                        }
                        else
                        {
                            d.awayrating = comment;
                            d.arating = fb;
                            d.awaydone = true;
                        }
                        db.SaveChanges();
                        return RedirectToAction("umpirerate", "Umpirerates");

                    }

                }

                return View();

            }
            catch (Exception e)
            {
                Session["test10"] = "PLEASE ENTER ALL THE FIELDS TO RATE REFEREE";
                return View();
            }
        }
}
}
