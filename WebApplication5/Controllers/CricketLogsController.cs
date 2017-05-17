using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Models
{
    public class CricketLogsController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: CricketLogs
        public ActionResult Index()
        {
            return View(db.CricketLogs.ToList());
        }

        // GET: CricketLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLog cricketLog = db.CricketLogs.Find(id);
            if (cricketLog == null)
            {
                return HttpNotFound();
            }
            return View(cricketLog);
        }

        // GET: CricketLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CricketLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TeamName,totalm,Wins,Draws,Losses,NetRunRate,Points")] CricketLog cricketLog)
        {
            if (ModelState.IsValid)
            {
                db.CricketLogs.Add(cricketLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cricketLog);
        }

        // GET: CricketLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLog cricketLog = db.CricketLogs.Find(id);
            if (cricketLog == null)
            {
                return HttpNotFound();
            }
            return View(cricketLog);
        }

        // POST: CricketLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeamName,totalm,Wins,Draws,Losses,NetRunRate,Points")] CricketLog cricketLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cricketLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cricketLog);
        }

        // GET: CricketLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketLog cricketLog = db.CricketLogs.Find(id);
            if (cricketLog == null)
            {
                return HttpNotFound();
            }
            return View(cricketLog);
        }

        // POST: CricketLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CricketLog cricketLog = db.CricketLogs.Find(id);
            db.CricketLogs.Remove(cricketLog);
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

        //====================================================================================================
        public ActionResult cricketLog()
        {
            ViewBag.ddld = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

            return View();
        }
        [HttpPost]
        public ActionResult cricketLog(string ddld)
        {
            ViewBag.ddld = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
            var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddld);//Finds team based on division ddl

            List<CricketLog> results = new List<CricketLog>();

            var s = db.CricketLeague.ToList();

            var sr = db.CricketResults.ToList();



            if (s.Count() > 0)
            {
                foreach (CricketLeague t in s)
                {
                    CricketLog res = new CricketLog();
                    //res.Id = t.Id;
                    res.TeamName = t.teamName;

                    //cal num of wins
                    int wins = 0;
                    int losses = 0;
                    int draws = 0;
                    double nrr =0.0;
                    int points = 0;
                    double RunRateFor = 0.0;
                    double RunRateAgainst = 0.0;
                    

                    // var allteammatches = sr.Where(i => i.HomeTeam == t.teamName || i.AwayTeam == t.teamName);
                    var allteamhomematches = db.CricketResults.ToList().FindAll(i => i.HomeTeam == t.teamName);
                    int homec = allteamhomematches.Count();
                    var allteamawaymatches = db.CricketResults.ToList().FindAll(i => i.AwayTeam == t.teamName);
                    int awayc = allteamawaymatches.Count();

                    foreach (var m in allteamhomematches)
                    {
                        if (m.HRuns > m.ARuns)
                        {
                            wins++;
                            points = points + 2;
                        }
                        if (m.HRuns < m.ARuns)
                        {
                            losses++;
                            points = points + 0;
                        }
                        if (m.HRuns == m.ARuns)
                        {
                            draws++;
                            points = points + 1;
                        }
                        RunRateFor = m.HRuns / m.HOversFaced;
                        RunRateAgainst = m.ARuns / m.AOversFaced;
                        nrr += RunRateFor - RunRateAgainst;
                    }

                    foreach (var m in allteamawaymatches)
                    {
                        if (m.ARuns > m.HRuns)
                        {
                            wins++;
                            points = points + 2;
                        }
                        if (m.ARuns < m.HRuns)
                        {
                            losses++;
                            points = points + 0;
                        }
                        if (m.ARuns == m.HRuns)
                        {
                            draws++;
                            points = points + 1;
                        }
                        RunRateFor = m.HRuns / m.HOversFaced;
                        RunRateAgainst = m.ARuns / m.AOversFaced;
                        nrr += RunRateAgainst - RunRateFor;
                    }
                    res.totalm = homec + awayc;
                    res.Wins = wins;
                    res.Losses = losses;
                    res.Draws = draws;
                    //res.GoalsFor = goalsfor;
                    //res.GoalsAgainst = goalsagainst;
                    //res.GoalsDifference = goalsfor - goalsagainst;
                    res.Points = points;
                    res.NetRunRate = Math.Round(nrr, 2);

                    results.Add(res);
                }
            }
            ViewData["Results"] = results.OrderByDescending(i => i.Points).OrderByDescending(i => i.NetRunRate);
            return View();
        }
    }
}
