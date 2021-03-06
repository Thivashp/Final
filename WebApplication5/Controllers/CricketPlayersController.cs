﻿using SendGrid;
using SendGrid.Helpers.Mail;
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
    public class CricketPlayersController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: CricketPlayers
        public ActionResult Index()
        {
            return View(db.cricketPlayers.ToList());
        }

        // GET: CricketPlayers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketPlayers cricketPlayers = db.cricketPlayers.Find(id);
            if (cricketPlayers == null)
            {
                return HttpNotFound();
            }
            return View(cricketPlayers);
        }

        // GET: CricketPlayers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CricketPlayers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,firstName,lastName,TeamN,EmailAdd,PlayerRole,JoinT")] CricketPlayers cricketPlayers)
        {
            if (ModelState.IsValid)
            {
                db.cricketPlayers.Add(cricketPlayers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cricketPlayers);
        }

        // GET: CricketPlayers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketPlayers cricketPlayers = db.cricketPlayers.Find(id);
            if (cricketPlayers == null)
            {
                return HttpNotFound();
            }
            return View(cricketPlayers);
        }

        // POST: CricketPlayers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName,TeamN,EmailAdd,PlayerRole,JoinT")] CricketPlayers cricketPlayers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cricketPlayers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cricketPlayers);
        }

        // GET: CricketPlayers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CricketPlayers cricketPlayers = db.cricketPlayers.Find(id);
            if (cricketPlayers == null)
            {
                return HttpNotFound();
            }
            return View(cricketPlayers);
        }

        // POST: CricketPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CricketPlayers cricketPlayers = db.cricketPlayers.Find(id);
            db.cricketPlayers.Remove(cricketPlayers);
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

        public ActionResult CricketP()
        {
            Session["ddlr"] = "   ";
            Session["Division"] = " ";
            Session["SportType"] = " ";
            Session["Amount"] = " ";
            Session["email1"] = " ";
            Session["name"] = " ";
            Session["lastName"] = "  ";
            Session["roles"] = " ";
            Session["teamPlayer"] = " ";
            Session["roles3"] = "  ";
            Session["cap"] = "  ";
            var d = from s in db.cricketPlayers.ToList() select s;
            d = d.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());
            return View(d.ToList());

        }

        [HttpPost]
        public ActionResult CricketP(string firstN, string lastN, string Email, string roles)
        {

            var c = db.cricketPlayers.ToList().Find(x => x.EmailAdd == Email);
            var l = db.cricketPlayers.ToList().Find(x => x.TeamN == Session["teamName"].ToString() && x.EmailAdd == Email);
            var ddd = db.cricketPlayers.ToList().Find(x => x.PlayerRole == "CAPTAIN" && x.TeamN == Session["teamName"].ToString());
            var eee = db.cricketPlayers.ToList().Find(x => x.PlayerRole == "VICE-CAPTAIN" && x.TeamN == Session["teamName"].ToString());
            var k = from s in db.soccerPlayers.ToList() select s;
            k = k.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

            if (l != null)
            {
                Session["teamPlayer"] = "PLAYER ALREADY EXISTS WITHIN TEAM " + l.EmailAdd.ToUpper();
                var q = from s in db.cricketPlayers.ToList() select s;
                q = q.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());
                return View(q.ToList());
            }
            else if (roles == "CAPTAIN" && ddd != null)
            {
                var d = from s in db.cricketPlayers.ToList() select s;
                d = d.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());
                Session["cap"] = "CAPTAIN ALREADY EXISTS";
                return View(d.ToList());

            }

            else if (roles == "VICE-CAPTAIN" && eee != null)
            {
                var d = from s in db.cricketPlayers.ToList() select s;
                d = d.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());
                Session["cap"] = "VICE-CAPTAIN ALREADY EXISTS";
                return View(d.ToList());

            }


            else if (c != null)
            {
                Session["email1"] = Email;
                Session["name"] = firstN;
                Session["lastName"] = lastN;
                Session["roles"] = roles;
                // Session["JoinT"] = false;
                return RedirectToAction("ExistingPlayer");
            }

            else if (roles == "")
            {
                Session["ddlr"] = "SELECT A PLAYER ROLE";
                return View();
            }
            else if (k.Count() > 13)
            {
                Session["capP"] = "MAXIMUM PlAYERS ADDED";
                var d = from s in db.soccerPlayers.ToList() select s;
                d = d.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

                return View(d.ToList());
            }

            else
            {

                CricketPlayers sp = new CricketPlayers();
                sp.firstName = firstN;
                sp.lastName = lastN;
                sp.EmailAdd = Email;
                sp.TeamN = Session["teamName"].ToString();
                sp.PlayerRole = roles;
                sp.JoinT = false;
                db.cricketPlayers.Add(sp);
                db.SaveChanges();
                var d = from s in db.cricketPlayers.ToList() select s;
                d = d.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

                //Session["teamName"]
                //Session["Division"]
                //Session["SportType"] 
                // Session["Amount"]
                var f = from s in db.cricketT.ToList() select s;
                f = f.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());


                Session["Division"] = f.First().Div;
                Session["SportType"] = f.First().sportType;


                var lt = db.cricketL.ToList().Find(x => x.leagueTitle == Session["Division"].ToString());
                Session["Amount"] = lt.regFee;
                Session["teamPlayer"] = " ";
                Session["roles3"] = "  ";
                Session["cap"] = "  ";
                return View(d.ToList());
            }
        }
        public ActionResult ExistingPlayer()
        {

            return View();
        }


        public ActionResult ExistingPlayer1()
        {
            CricketPlayers sp = new CricketPlayers();

            sp.EmailAdd = Session["email1"].ToString();
            sp.firstName = Session["name"].ToString();
            sp.lastName = Session["lastName"].ToString();
            sp.PlayerRole = Session["roles"].ToString();
            sp.TeamN = Session["teamName"].ToString();
            sp.JoinT = false;
            db.cricketPlayers.Add(sp);
            db.SaveChanges();

            string apiKey = "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";

            dynamic sendGridClient = new SendGridAPIClient(apiKey);
            string Body = "PLease Click to join team : </br>" + Url.Action("TeamConfirmed", "CricketPlayers", new { Tokenb = Session["teamName"].ToString(), Tokenc = Session["email1"].ToString() }, Request.Url.Scheme);

            SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
            SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(Session["email1"].ToString());
            Content content = new Content("text/plain", Body);
            SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, " Join Team", toEmail, content);

            dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());


            return RedirectToAction("CricketP", "CricketPlayers");

        }
        public ActionResult TeamConfirmed(string Tokenb, string Tokenc)
        {
            var lt = db.cricketPlayers.ToList().Find(x => x.TeamN == Tokenb && x.EmailAdd == Tokenc);

            if (lt != null)
            {
                lt.JoinT = true;
                db.cricketPlayers.Add(lt);
                db.SaveChanges();
                Session["team"] = Tokenb;

            }
            else
            {
                Session["team"] = "Individual";
            }

            return View();

        }

        public ActionResult VerifyS()
        {
            var q = from s in db.cricketPlayers.ToList() select s;
            q = q.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

            List<string> m = new List<string>();
            List<string> n = new List<string>();
            List<string> o = new List<string>();
            var d = db.CLeagueReg.ToList().Find(x => x.EmailAdd == Session["email2"].ToString());
            var z = db.cricketT.ToList().Find(x => x.TeamN == Session["teamName"].ToString());
            var e = db.cricketL.ToList().Find(x => x.leagueTitle == z.Div);
            var f = from s in db.cricketT.ToList() select s;
            f = f.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());


            Session["Division"] = f.First().Div;
            Session["SportType"] = f.First().sportType;


            var lt = db.cricketL.ToList().Find(x => x.leagueTitle == Session["Division"].ToString());
            Session["Amount"] = lt.regFee;
            Session["teamPlayer"] = " ";
            Session["fn"] = "First Name: " + d.firstName;
            Session["ln"] = "Last Name :" + d.lastName;
            Session["dob"] = "Date Of Birth: " + d.Sdate;
            Session["mail"] = "Email: " + d.EmailAdd;
            Session["pn"] = "Phone Number: " + d.PhoneNum;
            Session["add"] = "Address: " + d.Address;
            Session["tn"] = "Team Name: " + d.teamName;



            Session["LeagueTitle"] = "League Title: " + e.leagueTitle;
            Session["LeagueCategory"] = "Category: " + e.category;
            Session["LeagueSDte"] = "Start Date: " + e.Sdate;
            Session["Regfee"] = "Registration Fee: " + e.regFee;
            Session["costPR"] = "Cost Per Round " + e.CostPR;
            Session["payment"] = "league";
            Session["cricket"] = "True";
            Session["cricket"] = "True";
            return View(db.cricketPlayers.ToList().Where(x => x.TeamN == Session["teamName"].ToString()));
        }


        public ActionResult JoinCLeague()
        {
            Session["ValidateTeam"] = " ";
            Session["bool"] = " ";
            Session["ddlS"] = " ";
            Session["ddlT"] = " ";
            ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

            return View();
        }



        [HttpPost]
        public ActionResult JoinCLeague(string ddlS, string teamN, string teamType, string firstN, string lastN, string Email, string roles)
        {
            var TeamValidate = db.cricketT.ToList().Find(x => x.TeamN == teamN);
            if (teamN == "")
            {
                Session["ValidateTeam"] = "PLEASE ENTER A TEAM ";
                ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
                var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddlS);

                if (teamType != "")
                {
                    Session["ddlT"] = "";
                    //ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL

                }
                if (ddlS != "")
                {
                    Session["ddlS"] = "";
                    // ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                }
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////
            if (teamType == "" || teamType == null)
            {
                Session["ddlT"] = "PLEASE PICK A TYPE";
                ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

                if (teamN != "")
                {
                    Session["ValidateTeam"] = "";
                }
                if (ddlS != "")
                {
                    Session["ddlS"] = "";
                    // ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                    // return View();
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            if (ddlS == "")
            {
                Session["ddlS"] = "PLEASE CHOOSE A LEAGUE";
                ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
                var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddlS);
                if (teamN != "")
                {
                    Session["ValidateTeam"] = "";
                }
                if (teamType != "")
                {
                    Session["ddlT"] = "";
                    //ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL

                }
            }
            if (TeamValidate != null)
            {
                Session["ValidateTeam"] = "TEAM NAME AREADY TAKEN ";
                ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
                var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddlS);
            }

            else if (TeamValidate == null && teamN != "" && teamType != "")
            {
                Session["ddlT"] = "";
                ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
                var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddlS);
                Session["teamN"] = teamN;
                Session["bool"] = "true";
                Session["ValidateTeam"] = " ";

                if (User.Identity.Name != null)
                {
                    var captain = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);

                    CricketTeams sT = new CricketTeams();
                    sT.TeamN = teamN;
                    sT.TeamT = teamType;
                    sT.Div = dd.leagueTitle;
                    sT.Email = User.Identity.Name;
                    sT.sportType = "Cricket";
                    db.cricketT.Add(sT);

                    CricketPlayers sp = new CricketPlayers();
                    sp.firstName = captain.firstName;
                    sp.lastName = captain.lastName;
                    sp.EmailAdd = captain.EmailAdd;
                    sp.TeamN = teamN;
                    sp.PlayerRole = "CAPTAIN";
                    sp.JoinT = true;
                    Session["email2"] = captain.EmailAdd;
                    Session["teamName"] = teamN;
                    //Session["qwe"] = sp.PlayerRole;
                    db.cricketPlayers.Add(sp);
                    db.SaveChanges();
                    return Redirect("/CricketPlayers/CricketP");
                }
            }


            return View();
        }
    }
}
