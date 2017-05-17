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
    public class CricketResultsController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: CricketResults
        public ActionResult Index()
        {
            return View(db.CricketResults.ToList());
        }

        // GET: CricketResults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketResults cricketResults = db.CricketResults.Find(id);
            if (cricketResults == null)
            {
                return HttpNotFound();
            }
            return View(cricketResults);
        }

        // GET: CricketResults/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CricketResults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mId,HomeTeam,AwayTeam,HomeRuns,AwayRuns,HOversFaced,AOversFaced,HomeWickets,AwayWickets,date")] CricketResults cricketResults)
        {
            if (ModelState.IsValid)
            {
                db.CricketResults.Add(cricketResults);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cricketResults);
        }

        // GET: CricketResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketResults cricketResults = db.CricketResults.Find(id);
            if (cricketResults == null)
            {
                return HttpNotFound();
            }
            return View(cricketResults);
        }

        // POST: CricketResults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mId,HomeTeam,AwayTeam,HomeRuns,AwayRuns,HOversFaced,AOversFaced,HomeWickets,AwayWickets,date")] CricketResults cricketResults)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cricketResults).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cricketResults);
        }

        // GET: CricketResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketResults cricketResults = db.CricketResults.Find(id);
            if (cricketResults == null)
            {
                return HttpNotFound();
            }
            return View(cricketResults);
        }

        // POST: CricketResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CricketResults cricketResults = db.CricketResults.Find(id);
            db.CricketResults.Remove(cricketResults);
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

        //==================================================================================================

        public ActionResult Results()
        {
            if (User.IsInRole("Employee"))
            {
                ViewBag.ddld = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the fixtures DDL
                ViewBag.ddlf = new SelectList(db.CricketFixtures, "Id", "fixture");//Creates the division DDL
                Session["away"] = " ";
                Session["home"] = " ";
                Session["check"] = " ";
                Session["validate2"] = " ";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult Results(string HomeTeam, string AwayTeam, string HRuns, string ARuns, string HOversFaced, string AOversFaced, string HWickets, string AWickets, string ddlf, string ddld)
        {
            try
            {
                ViewBag.ddld = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the fixtures DDL
                var ss = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddld);
                ViewBag.ddlf = new SelectList(db.CricketFixtures.ToList().FindAll(x => x.division == ss.leagueTitle && x.IsComplete == false), "Id", "fixture");//Creates the division DDL

                if (ddld != null)
                {
                    Session["validate2"] = " ";
                    Session["check"] = "True";

                    if (ddlf != null)
                    {
                        var dd = db.CricketFixtures.ToList().Find(x => x.Id.ToString() == ddlf);
                        var away = dd.fixture;
                        var newaway = away.Remove(0, away.IndexOf(' ') + 3).Trim();
                        Session["away"] = newaway;
                        Session["validate2"] = " ";
                        var home = dd.fixture;
                        var newhome = home.Remove(home.IndexOf(' ') + 0);
                        Session["home"] = newhome;

                        if (HRuns != "" && ARuns != "" && HOversFaced != "" && AOversFaced != "" && HWickets != "" && AWickets != "")
                        {
                            CricketResults rs = new CricketResults();
                            rs.HomeTeam = Session["home"].ToString();
                            rs.AwayTeam = Session["away"].ToString();

                            rs.HRuns = Convert.ToInt32(HRuns);
                            rs.ARuns = Convert.ToInt32(ARuns);

                            rs.HOversFaced = Convert.ToInt32(HOversFaced);
                            rs.AOversFaced = Convert.ToInt32(AOversFaced);


                            rs.HWickets = Convert.ToInt32(HWickets);
                            rs.AWickets = Convert.ToInt32(AWickets);

                            rs.date = DateTime.Now;

                            var d = db.CricketFixtures.ToList().FindAll(x => x.Id.ToString() == ddlf);
                            var reff = db.CricketFixtures.ToList().Find(x => x.Id.ToString() == ddlf);
                            var findref = db.umpire.ToList().Find(x => x.refID == reff.refID);
                            var reflvl = db.cricketL.ToList().Find(x => x.leagueTitle == ss.leagueTitle);
                            var rateref = db.umpirerate.ToList().Find(x => x.fixture == reff.fixture);
                            Session["validate2"] = " ";
                            if (d != null)
                            {
                                foreach (var c in d)
                                {
                                    c.IsComplete = true;
                                    c.refcheck = false;
                                    c.refreass = false;
                                    rateref.played = false;
                                    rateref.done = true;
                                    Session["validate2"] = " ";
                                    CricketFixtures fx = new CricketFixtures();
                                    fx.IsComplete = c.IsComplete;
                                    fx.refcheck = c.refcheck;
                                    fx.refreass = c.refreass;
                                    findref.matchref = false;
                                    rateref.done = true;
                                    Umpirerate r = new Umpirerate();
                                    r.played = rateref.played;
                                    db.SaveChanges();

                                    foreach (var ac in db.umpire.ToList().FindAll(x => x.Experience == reflvl.refLevel))

                                    {
                                        if (ac.matchref == true)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            foreach (var ab in db.CricketFixtures.ToList().FindAll(x => x.division == ss.leagueTitle))
                                            {
                                                if (ab.refcheck == true || ac.matchref == true || ab.refreass == true && ab.IsComplete != true || ab.refID != null)
                                                {

                                                    continue;
                                                }
                                                else
                                                {
                                                    foreach (var rr in db.umpirerate.ToList())
                                                    {
                                                        if (rr.played == true | rr.done == true)
                                                        {

                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            ab.refID = ac.refID;
                                                            ac.matchref = true;
                                                            ab.refcheck = true;
                                                            ab.refreass = true;
                                                            rr.refID = ab.refID;
                                                            rr.played = true;
                                                            Session["validate2"] = " ";
                                                            db.SaveChanges();
                                                            break;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                db.CricketResults.Add(rs);
                                db.SaveChanges();
                                Session["validate2"] = " ";
                                return Redirect("/CricketResults/Results");
                            }

                        }
                        return View();
                    }

                    else
                    {
                        Session["check2"] = " ";
                    }
                }
                Session["validate2"] = " ";
                return View();
            }

            catch (Exception e)
            {
                ViewBag.ddld = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the fixtures DDL
                ViewBag.ddlf = new SelectList(db.CricketFixtures, "Id", "fixture");//Creates the division DDL
                Session["validate2"] = "PLEASE FILL ALL INFORMATION BEFORE PROCEEDING";

                return View();
            }
        }
    }
}

  