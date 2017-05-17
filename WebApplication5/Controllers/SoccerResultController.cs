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
    public class SoccerResultController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: SoccerResult
        public ActionResult Index()
        {
            return View(db.soccerResults.ToList());
        }

        // GET: SoccerResult/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerResults soccerResults = db.soccerResults.Find(id);
            if (soccerResults == null)
            {
                return HttpNotFound();
            }
            return View(soccerResults);
        }

        // GET: SoccerResult/Create
        public ActionResult Create()
        {
            ViewBag.ddlS = new SelectList(db.fixtures, "Id", "fixture");//Creates the division DDL
            Session["test"] = " ";
            return View();
        }

        // POST: SoccerResult/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mId,HomeTeamId,AwayTeamId,HomeGoals,AwayGoals,date")] SoccerResults soccerResults, string ddlS)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ddlS = new SelectList(db.soccerL, "Id", "fixture");//Creates the division DDL
                var dd = db.fixtures.ToList().Find(x => x.Id.ToString() == ddlS);
                var home = dd.fixture;
                var newhome = home.Remove(0, home.IndexOf(' ') + 1);
                Session["test"] = newhome;
                db.soccerResults.Add(soccerResults);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(soccerResults);
        }

        // GET: SoccerResult/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerResults soccerResults = db.soccerResults.Find(id);
            if (soccerResults == null)
            {
                return HttpNotFound();
            }
            return View(soccerResults);
        }

        // POST: SoccerResult/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mId,HomeTeamId,AwayTeamId,HomeGoals,AwayGoals,date")] SoccerResults soccerResults)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soccerResults).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(soccerResults);
        }

        // GET: SoccerResult/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerResults soccerResults = db.soccerResults.Find(id);
            if (soccerResults == null)
            {
                return HttpNotFound();
            }
            return View(soccerResults);
        }

        // POST: SoccerResult/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoccerResults soccerResults = db.soccerResults.Find(id);
            db.soccerResults.Remove(soccerResults);
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
        public ActionResult Results()
        {
            if (User.IsInRole("Employee"))
            {

                ViewBag.ddld = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the fixtures DDL
                ViewBag.ddlf = new SelectList(db.fixtures, "Id", "fixture");//Creates the division DDL
                Session["away"] = " ";
                Session["home"] = " ";
                Session["check"] = " ";
                Session["goals"] = " ";
                Session["goals1"] = " ";
                Session["validate2"] = " ";

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult Results(string HomeTeam, string AwayTeam, string HomeGoals, string AwayGoals, string ddlf, string ddld)
        {
            try {
                ViewBag.ddld = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the fixtures DDL
                var ss = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddld);
                ViewBag.ddlf = new SelectList(db.fixtures.ToList().FindAll(x => x.division == ss.leagueTitle && x.IsComplete == false), "Id", "fixture");//Creates the division DDL

                if (ddld != null)
                {
                    Session["validate2"] = " ";
                    Session["check"] = "True";

                    if (ddlf != null)
                    {
                        var dd = db.fixtures.ToList().Find(x => x.Id.ToString() == ddlf);
                        var away = dd.fixture;
                        var newaway = away.Remove(0, away.IndexOf(' ') + 3).Trim();
                        Session["away"] = newaway;
                        Session["validate2"] = " ";
                        var home = dd.fixture;
                        var newhome = home.Remove(home.IndexOf(' ') + 0);
                        Session["home"] = newhome;

                        if (AwayGoals != "" && HomeGoals != "")
                        {
                            SoccerResults rs = new SoccerResults();
                            rs.HomeTeam = Session["home"].ToString();
                            rs.AwayTeam = Session["away"].ToString();
                            rs.HomeGoals = Convert.ToInt32(HomeGoals);
                            rs.AwayGoals = Convert.ToInt32(AwayGoals);
                            rs.date = DateTime.Now;
                            Session["validate2"] = " ";
                            var d = db.fixtures.ToList().FindAll(x => x.Id.ToString() == ddlf);
                            var reff = db.fixtures.ToList().Find(x => x.Id.ToString() == ddlf);
                            var findref = db.referee.ToList().Find(x => x.refID == reff.refID);
                            var reflvl = db.soccerL.ToList().Find(x => x.leagueTitle == ss.leagueTitle);
                            var rateref = db.refrating.ToList().Find(x => x.fixture == reff.fixture);

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
                                    Fixtures fx = new Fixtures();
                                    fx.IsComplete = c.IsComplete;
                                    fx.refcheck = c.refcheck;
                                    fx.refreass = c.refreass;
                                    findref.matchref = false;
                                    rateref.done = true;
                                    RefRating r = new RefRating();
                                    r.played = rateref.played;
                                    db.SaveChanges();



                                    foreach (var ac in db.referee.ToList().FindAll(x => x.Experience == reflvl.refLevel))

                                    {
                                        if (ac.matchref == true)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            foreach (var ab in db.fixtures.ToList().FindAll(x => x.division == ss.leagueTitle))
                                            {
                                                if (ab.refcheck == true || ac.matchref == true || ab.refreass == true && ab.IsComplete != true || ab.refID != null)
                                                {

                                                    continue;
                                                }
                                                else
                                                {
                                                    foreach (var rr in db.refrating.ToList())
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
                                db.soccerResults.Add(rs);
                                db.SaveChanges();
                                Session["validate2"] = " ";
                                return Redirect("/SoccerResult/Results");
                            }

                        }
                        else if (HomeGoals == "" || AwayGoals == "")
                        {
                            if (HomeGoals == "" && AwayGoals == "")
                            {
                                Session["goals"] = "PLEASE ENTER HOME GOAL(S)";
                                Session["goals1"] = "PLEASE ENTER AWAY GOAL(S)";

                            }
                            else if (HomeGoals == "")
                            {
                                Session["goals"] = "PLEASE ENTER HOME GOAL(S)";
                            }
                            else if (AwayGoals == "")
                            {
                                Session["goals1"] = "PLEASE ENTER AWAY GOAL(S)";
                            }

                        }
                        Session["validate2"] = " ";
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
            catch(Exception e)
            {
                ViewBag.ddld = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the fixtures DDL
                ViewBag.ddlf = new SelectList(db.fixtures, "Id", "fixture");//Creates the division DDL
                Session["validate2"] = "PLEASE FILL ALL INFORMATION BEFORE PROCEEDING";
             
                return View();
            }
            }
    }
}
   
               
               

            

               


