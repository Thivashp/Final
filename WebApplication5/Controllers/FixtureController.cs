using PagedList;
using PagedList.Mvc;
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
    public class FixtureController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: Fixture
        
        public ActionResult Index(int? page, string teamType, string ddlS)
        {
            if (User.IsInRole("Employee"))
            {
                ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the fixtures DDL
                var ss = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);
                Session["check45"] = " ";
                Session["check54"] = " ";
                Session["SelectTeam"] = " ";
                Session["SelectDiv"] = " ";
                if (ddlS == "")
                {
                    Session["SelectDiv"] = "PLEASE SELECT DIVISION";
                }
                else if (ddlS != null)
                {
                    Session["check45"] = "True";
                    if (teamType != null)
                    {
                        Session["check54"] = "True";
                        if (teamType == "va")
                        {
                            Session["check54"] = "True";
                            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                            var de = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);
                            Session["check64"] = teamType;
                            return View(db.fixtures.ToList().Where(x => x.division == de.leagueTitle));

                        }

                        else if (teamType == "vp")
                        {
                            Session["check54"] = "True";
                            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                            var df = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);
                            Session["check64"] = teamType;
                            return View(db.fixtures.ToList().FindAll(x => x.division == df.leagueTitle && x.IsComplete == true));
                        }
                        else if (teamType == "vu")
                        {
                            Session["check54"] = "True";
                            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                            var dg = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);
                            Session["check64"] = teamType;
                            return View(db.fixtures.ToList().FindAll(x => x.division == dg.leagueTitle && x.IsComplete == false));
                        }
                        else
                        {
                            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the fixtures DDL
                           Session["check54"] = "False";
                                Session["SelectTeam"] = "PLEASE SELECT A TEAM";
                            return View();
                        }
                        

                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        
        // GET: Fixture/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixtures fixtures = db.fixtures.Find(id);
            if (fixtures == null)
            {
                return HttpNotFound();
            }
            return View(fixtures);
        }

        // GET: Fixture/Create
        public ActionResult Create()
        {
            if (User.IsInRole("asd"))
            {
                return View();
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Fixture/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fixture")] Fixtures fixtures)
        {
            if (ModelState.IsValid)
            {
                db.fixtures.Add(fixtures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fixtures);
        }

        // GET: Fixture/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixtures fixtures = db.fixtures.ToList().Find(x=>x.Id==id);
            if (fixtures == null)
            {
                return HttpNotFound();
            }
            var d = db.soccerL.ToList().Find(x => x.leagueTitle== fixtures.division);
            ViewBag.selectRef = new SelectList(db.referee.ToList().FindAll(x => x.Experience==d.refLevel), "Id", "refID");

            return View(fixtures);
        }

        // POST: Fixture/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fixture,division")] Fixtures fixtures,string selectRef)
        {
            //var fix = db.fixtures.ToList().Find(x=>x.Id==fixtures.Id);
            var be = db.referee.ToList().Find(x => x.Id == Convert.ToInt16(selectRef));
            //fixtures.division = fix.division;

            fixtures.refID = be.refID;
            fixtures.refcheck = true;
            fixtures.refreass = true;


            if (ModelState.IsValid)
            {
                db.Entry(fixtures).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fixtures);
        }

        // GET: Fixture/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixtures fixtures = db.fixtures.Find(id);
            if (fixtures == null)
            {
                return HttpNotFound();
            }
            return View(fixtures);
        }

        // POST: Fixture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fixtures fixtures = db.fixtures.Find(id);
            db.fixtures.Remove(fixtures);
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

        public ActionResult FixtureSelect()
        {
            if (User.IsInRole("Employee"))
            {
        
        Session["error"] = " ";
            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            ViewBag.selectT = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            Session["val"] = " ";
            Session["fix"] = " ";
            return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
        [HttpPost]
        public ActionResult FixtureSelect(string ddlS, string selectT,string searchfix,int? page)
        {
            //Populate DDL
            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            var dd = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);
            ViewBag.selectT = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            var df = db.soccerL.ToList().Find(x => x.IdV.ToString() == selectT);
            
            RefRating r = new RefRating();
            Fixtures fx = new Fixtures();
          
            List<Fixtures> fix = new List<Fixtures>();
            var d = db.soccerLeague.ToList();

            if (dd ==null)
            {
                
            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
          Session["error"] = "PLEASE SELECT ATLEAST ONE FIXTURE";
                Session["fix"] = "false";
                return View();
            }

            else if (dd != null )
            {
                var v = db.fixtures.ToList().Find(x => x.division == dd.leagueTitle);
                if (v != null)
                {
                    Session["error"] = "FIXTURE ALREADY CREATED";
                    Session["fix"] = "true";
                    return View(db.fixtures.ToList().FindAll(x => x.division == dd.leagueTitle));
                }
                else
                {
                    foreach (var t1 in d)
                    {
                        foreach (var t2 in d)
                        {

                            if (t1 == t2)
                            {
                                continue;
                            }
                            else
                            {
                                fx.fixture = t1.teamName.Replace(" ", "_") + " VS " + t2.teamName.Replace(" ", "_");
                               fx.division = dd.leagueTitle;
                                fx.IsComplete = false;
                                r.home = t1.teamName;
                                r.away = t2.teamName;
                                r.fixture = fx.fixture;
                                db.refrating.Add(r);
                                db.fixtures.Add(fx);
                                db.SaveChanges();
                                Session["val"] = "true";
                                Session["fix"] = "true";
                            }
                            Session["error"] = "FIXTURE'S CREATED";
                        }
                    }
                        

                    if (Session["val"].ToString() == "true")
                    {
                        var reflvl = db.soccerL.ToList().Find(x => x.leagueTitle == dd.leagueTitle);
                        // var g = db.referee.ToList().FindAll(x => x.Experience == reflvl.refLevel && x.matchref == false).First();
                        var f = db.referee.ToList().FindAll(x => x.Experience == reflvl.refLevel && x.matchref == false).First();
                        var e = db.fixtures.ToList().FindAll(x => x.refID != f.refID && x.refcheck == false).First();
                        var g = db.refrating.First();
                        g.refID = f.refID;
                        g.played = true;
                        e.refID = f.refID;
                        
                        f.matchref = true;
                        e.refcheck = true;
                        e.refreass = true;
                        db.SaveChanges();
                        foreach (var ac in db.referee.ToList().FindAll(x => x.Experience == reflvl.refLevel))

                        {
                            if (ac.matchref == true)
                            {
                                continue;
                            }
                            else
                            {
                                foreach (var ab in db.fixtures.ToList().FindAll(x => x.division == dd.leagueTitle))
                                {
                                    if (ab.refcheck == true || ac.matchref == true)
                                    {

                                        continue;
                                    }
                                    else
                                    {
                                        foreach (var rr in db.refrating.ToList())
                                        {
                                            if (rr.played == true )
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
                                                db.SaveChanges();
                                                break;

                                            }
                                        }
                                    }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            
            Session["fix"]="true";
            return View(db.fixtures.ToList().FindAll(x => x.division == dd.leagueTitle));
        }

       
    }
}

 