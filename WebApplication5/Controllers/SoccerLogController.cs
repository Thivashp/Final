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
    public class SoccerLogController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: SoccerLog
        public ActionResult Index()
        {
            return View(db.SoccerLogs.ToList());
        }

        // GET: SoccerLog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLog soccerLog = db.SoccerLogs.Find(id);
            if (soccerLog == null)
            {
                return HttpNotFound();
            }
            return View(soccerLog);
        }

        // GET: SoccerLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SoccerLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TeamName,Wins,Draws,Losses,GoalsFor,GoalsAgainst,GoalsDifference,Points")] SoccerLog soccerLog)
        {
            if (ModelState.IsValid)
            {
                db.SoccerLogs.Add(soccerLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(soccerLog);
        }

        // GET: SoccerLog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLog soccerLog = db.SoccerLogs.Find(id);
            if (soccerLog == null)
            {
                return HttpNotFound();
            }
            return View(soccerLog);
        }

        // POST: SoccerLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TeamName,Wins,Draws,Losses,GoalsFor,GoalsAgainst,GoalsDifference,Points")] SoccerLog soccerLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soccerLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(soccerLog);
        }

        // GET: SoccerLog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoccerLog soccerLog = db.SoccerLogs.Find(id);
            if (soccerLog == null)
            {
                return HttpNotFound();
            }
            return View(soccerLog);
        }

        // POST: SoccerLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoccerLog soccerLog = db.SoccerLogs.Find(id);
            db.SoccerLogs.Remove(soccerLog);
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

        public ActionResult soccerLog()
        {
            ViewBag.ddld = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
          
            return View();
        }
        [HttpPost]
        public ActionResult soccerLog(string ddld)
        {
            ViewBag.ddld = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            var dd = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddld);//Finds team based on division ddl

            List<SoccerLog> results = new List<SoccerLog>();

            var s = db.soccerLeague.ToList();

            var sr = db.soccerResults.ToList();

            

            if (s.Count() > 0)
            {
                foreach (SoccerLeague t in s)
                {
                    SoccerLog res = new SoccerLog();
                    //res.Id = t.Id;
                    res.TeamName = t.teamName;

                    //cal num of wins
                    int wins = 0;
                    int losses = 0;
                    int draws = 0;
                    int goalsfor = 0;
                    int goalsagainst = 0;
                    int points = 0;

                   // var allteammatches = sr.Where(i => i.HomeTeam == t.teamName || i.AwayTeam == t.teamName);
                    var allteamhomematches = db.soccerResults.ToList().FindAll(i => i.HomeTeam == t.teamName);
                    int homec = allteamhomematches.Count();
                    var allteamawaymatches = db.soccerResults.ToList().FindAll(i => i.AwayTeam == t.teamName);
                    int awayc = allteamawaymatches.Count();
                    foreach (var m in allteamhomematches)
                    {
                        if (m.HomeGoals > m.AwayGoals)
                        {
                            wins++;
                            points = points + 3;
                        }
                        if (m.HomeGoals < m.AwayGoals)
                        {
                            losses++;
                            points = points + 0;
                        }
                        if (m.HomeGoals == m.AwayGoals)
                        {
                            draws++;
                            points = points + 1;
                        }
                        goalsfor += (int)m.HomeGoals;
                        goalsagainst += (int)m.AwayGoals;
                    }

                    foreach (var m in allteamawaymatches)
                    {
                        if (m.AwayGoals > m.HomeGoals)
                        {
                            wins++;
                            points = points + 3;
                        }
                        if (m.AwayGoals < m.HomeGoals)
                        {
                            losses++;
                            points = points + 0;
                        }
                        if (m.AwayGoals == m.HomeGoals)
                        {
                            draws++;
                            points = points + 1;
                        }
                        goalsfor += (int)m.AwayGoals;
                        goalsagainst += (int)m.HomeGoals;
                    }
                    res.totalm = homec + awayc;
                    res.Wins = wins;
                    res.Losses = losses;
                    res.Draws = draws;
                    res.GoalsFor = goalsfor;
                    res.GoalsAgainst = goalsagainst;
                    res.GoalsDifference = goalsfor-goalsagainst;
                    res.Points = points;

                    results.Add(res);
                }
            }
            ViewData["Results"] = results.OrderByDescending(i => i.Points).ThenByDescending(i => i.GoalsDifference);
            return View();
        }

        //public ActionResult View(int Id)
        //{
           
        //    var allteams = db.soccerLeague.ToList().Where(i => i.Id != Id).OrderBy(i => i.teamName);
        //    if (allteams.Count() > 0)
        //    {
        //        ViewData["AllTeams"] = allteams;
        //    }
        //    var team = db.soccerLeague.ToList().Where(i => i.Id == Id).FirstOrDefault();
        //    if (team != null)
        //    {
                
        //        var allreports = db.soccerResults.ToList().Where(i => i.HomeTeam == Id || i.Matches.AwayTeamId == Id).OrderByDescending(i => i.Matches.Timestamp);

        //        if (Request.QueryString["opponent"] != null)
        //        {
        //            var opponent = allteams.Where(i => i.Id.ToString() == Request.QueryString["opponent"].ToString()).FirstOrDefault();
        //            if (opponent != null)
        //            {
        //                ViewData["opponentTeamName"] = opponent.TeamName;
        //                allreports = allreports.Where(i => i.Matches.HomeTeamId == opponent.Id || i.Matches.AwayTeamId == opponent.Id).OrderByDescending(i => i.Matches.Timestamp);
        //            }
        //        }

        //        var allhomegames = allreports.Where(i => i.Matches.HomeTeamId == Id);
        //        var allawaygames = allreports.Where(i => i.Matches.AwayTeamId == Id);

        //        if (allhomegames.Count() > 0)
        //        {
        //            ViewData["HomeGames"] = allhomegames;
        //        }
        //        if (allawaygames.Count() > 0)
        //        {
        //            ViewData["AwayGames"] = allawaygames;
        //        }

        //        return View(team);
        //    }
        //    else
        //    {
        //        return Redirect("/");
        //    }

        //}
    }
}
