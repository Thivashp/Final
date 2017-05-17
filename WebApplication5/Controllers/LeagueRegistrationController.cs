using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class LeagueRegistrationController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        public LeagueRegistrationController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public LeagueRegistrationController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: LeagueRegistration
        public ActionResult Index()
        {
            return View(db.LeagueReg.ToList());
        }

        // GET: LeagueRegistration/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueRegisteration leagueRegisteration = db.LeagueReg.Find(id);
            if (leagueRegisteration == null)
            {
                return HttpNotFound();
            }
            return View(leagueRegisteration);
        }

        // GET: LeagueRegistration/Create
        public ActionResult Create()
        {
            Session["team"] = "    ";
            Session["teamName"] = "    ";
            Session["ddlS"] = "    ";
            Session["ddlT"] = "    ";
            Session["qwe"] = "   ";
            Session["email"] = "   ";
            Session["email2"] = "    ";
            Session["payment"] = " ";
            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            return View();
        }

        // POST: LeagueRegistration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address,TeamName,Password,ConfirmPassword")] LeagueRegisteration leagueRegisteration, string add2, string post, string teamType, string ddlS, bool TsCs)
        {
            ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
            if (ModelState.IsValid)
            {
                var team = db.soccerT.ToList().Find(x => x.TeamN == leagueRegisteration.teamName);
                var email1 = db.Customers.ToList().Find(x => x.EmailAdd == leagueRegisteration.EmailAdd);
                if (email1 != null)
                {
                    Session["email"] = "EMAIL ALREADY EXISTS  PLEASE LOGIN TO JOIN LEAGUE";
                    return View();
                }
                else if (team != null)
                {
                    ViewBag.ddlS = new SelectList(db.soccerL, "leagueTitle", "leagueTitle");//Creates the division DDL

                    Session["teamName"] = "TEAM ALREADY EXISTS ";
                    return View();

                }

                else if (TsCs == false)
                {
                    ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["team"] = "click TsCs";
                    return View();
                }
                else if (ddlS == "" || ddlS == "SELECT DIVISION:")
                {
                    ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["ddlS"] = "SELECT A DIVISION FROM THE LIST PROVIDED";
                    return View();
                }
                else if (teamType == "noselect")
                {
                    ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["ddlT"] = "SELECT A TEAM TYPE FROM THE LIST PROVIDED";
                    return View();
                }


                else
                {
                    Customer c = new Customer();
                    var user = new ApplicationUser { UserName = leagueRegisteration.EmailAdd, Email = leagueRegisteration.EmailAdd };

                    user.firstN = leagueRegisteration.firstName;
                    user.lastN = leagueRegisteration.lastName;
                    user.Address = leagueRegisteration.Address + " " + add2 + " " + post;
                    user.phoneNum = leagueRegisteration.PhoneNum;
                    user.Email = leagueRegisteration.EmailAdd;
                    user.ConfirmedEmail = true;
                    user.DateOfBirth = leagueRegisteration.Sdate;
                    user.Gender = teamType;
                    user.TeamName = "Individual";
                    c.firstName = leagueRegisteration.firstName;
                    c.lastName = leagueRegisteration.lastName;
                    c.Sdate = leagueRegisteration.Sdate;
                    c.EmailAdd = leagueRegisteration.EmailAdd;
                    c.Password = leagueRegisteration.Password;
                    c.PhoneNum = leagueRegisteration.PhoneNum;
                    c.Address = leagueRegisteration.Address + " " + add2 + " " + post;
                    c.teamName = leagueRegisteration.teamName;
                    db.Customers.Add(c);
                    var result = await UserManager.CreateAsync(user, leagueRegisteration.Password);


                    if (User.IsInRole("Admin"))
                    {
                        if (result.Succeeded)
                        {




                            UserManager.AddToRole(user.Id, "Employee");
                            Session["username"] = User.Identity.GetUserId();

                            user.Id = "emp" + user.Id;

                            return RedirectToAction("Index", "Home", new { Email = user.Email });
                        }





                    }


                    if (result.Succeeded)
                    {



                        user.ConfirmedEmail = false;
                        UserManager.AddToRole(user.Id, "Customer");
                        Session["username"] = User.Identity.GetUserId();
                        string apiKey = "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";

                        dynamic sendGridClient = new SendGridAPIClient(apiKey);

                        string Body = string.Format("Dear customer Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" </a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                        SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                        SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(user.Email);
                        Content content = new Content("text/plain", Body);
                        SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, "Registration", toEmail, content);

                        dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());

                        //////////////////////////////////////////////////////////////////////////////////////////////

                        ViewBag.ddlS = new SelectList(db.soccerL, "IdV", "leagueTitle");//Creates the division DDL
                        var dd = db.soccerL.ToList().Find(x => x.IdV.ToString() == ddlS);

                        string address = " " + add2 + " " + post;//1)
                        leagueRegisteration.Address += address;//1)Adds the address line 1 and 2 together 
                        SoccerTeams sT = new SoccerTeams();
                        sT.TeamN = leagueRegisteration.teamName;
                        sT.TeamT = teamType;
                        sT.Div = dd.leagueTitle;
                        sT.Email = leagueRegisteration.EmailAdd;
                        sT.sportType = "Soccer";
                        
                        db.soccerT.Add(sT);
                        SoccerPlayers sp = new SoccerPlayers();
                        sp.firstName = leagueRegisteration.firstName;
                        sp.lastName = leagueRegisteration.lastName;
                        sp.EmailAdd = leagueRegisteration.EmailAdd;
                        Session["email2"] = leagueRegisteration.EmailAdd;
                        sp.TeamN = leagueRegisteration.teamName;
                        Session["teamName"] = leagueRegisteration.teamName;
                        sp.PlayerRole = "CAPTAIN";
                        sp.JoinT = true;
                        Session["qwe"] = sp.PlayerRole;
                        db.soccerPlayers.Add(sp);
                        db.LeagueReg.Add(leagueRegisteration);
                        db.SaveChanges();
                        return RedirectToAction("SoccerP", "SoccerPlayers");

                    }
                }

            }
            return View(leagueRegisteration);
        }

        // GET: LeagueRegistration/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueRegisteration leagueRegisteration = db.LeagueReg.Find(id);
            if (leagueRegisteration == null)
            {
                return HttpNotFound();
            }
            return View(leagueRegisteration);
        }

        // POST: LeagueRegistration/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address,TeamName,TeamType,TermCon")] LeagueRegisteration leagueRegisteration)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(leagueRegisteration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leagueRegisteration);
        }

        // GET: LeagueRegistration/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeagueRegisteration leagueRegisteration = db.LeagueReg.Find(id);
            if (leagueRegisteration == null)
            {
                return HttpNotFound();
            }
            return View(leagueRegisteration);
        }

        // POST: LeagueRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeagueRegisteration leagueRegisteration = db.LeagueReg.Find(id);
            db.LeagueReg.Remove(leagueRegisteration);
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

       public ActionResult testddl()
        {
            
            return View();
        }

        public ActionResult TsCs()
        {
            return View();
        }
    }
}
