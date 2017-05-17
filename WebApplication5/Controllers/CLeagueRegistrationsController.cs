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
    public class CLeagueRegistrationsController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        public CLeagueRegistrationsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public CLeagueRegistrationsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: CLeagueRegistrations
        public ActionResult Index()
        {
            return View(db.CLeagueReg.ToList());
        }

        // GET: CLeagueRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLeagueRegistration cLeagueRegistration = db.CLeagueReg.Find(id);
            if (cLeagueRegistration == null)
            {
                return HttpNotFound();
            }
            return View(cLeagueRegistration);
        }

        // GET: CLeagueRegistrations/Create
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
          ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
            return View();
        }

        // POST: CLeagueRegistrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address,teamName,Password,ConfirmPassword")]  CLeagueRegistration cLeagueRegistration, string add2, string post, string teamType, string ddlS, bool TsCs)
        {
            ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
            if (ModelState.IsValid)
            {
                var team = db.cricketT.ToList().Find(x => x.TeamN == cLeagueRegistration.teamName);
                var email1 = db.Customers.ToList().Find(x => x.EmailAdd == cLeagueRegistration.EmailAdd);
                 if (email1 != null)
                {
                    Session["email"] = "EMAIL ALREADY EXISTS  PLEASE LOGIN TO JOIN LEAGUE";
                    return View();
                }
              else  if (team != null)
                {
                    ViewBag.ddlS = new SelectList(db.cricketL, "leagueTitle", "leagueTitle");//Creates the division DDL

                    Session["teamName"] = "TEAM ALREADY EXISTS ";
                    return View();

                }

                else if (TsCs == false)
                {
                    ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["team"] = "click TsCs";
                    return View();
                }
                else if (ddlS == "" || ddlS == "SELECT DIVISION:")
                {
                    ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["ddlS"] = "SELECT A DIVISION FROM THE LIST PROVIDED";
                    return View();
                }
                else if (teamType == "noselect")
                {
                    ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL

                    Session["ddlT"] = "SELECT A TEAM TYPE FROM THE LIST PROVIDED";
                    return View();
                }


                else
                {
                
                    var user = new ApplicationUser { UserName = cLeagueRegistration.EmailAdd, Email = cLeagueRegistration.EmailAdd };
                    Customer c = new Customer();
                    user.firstN = cLeagueRegistration.firstName;
                    user.lastN = cLeagueRegistration.lastName;
                    user.Address = cLeagueRegistration.Address + " " + add2 + " " + post;
                    user.phoneNum = cLeagueRegistration.PhoneNum;
                    user.Email = cLeagueRegistration.EmailAdd;
                    user.ConfirmedEmail = true;
                    user.DateOfBirth = cLeagueRegistration.Sdate;
                    user.Gender = teamType;
                    user.TeamName = "Individual";
                    c.firstName = cLeagueRegistration.firstName;
                    c.lastName= cLeagueRegistration.lastName;
                   c.Sdate = cLeagueRegistration.Sdate;
                    c.EmailAdd = cLeagueRegistration.EmailAdd;
                    c.Password = cLeagueRegistration.Password;
                    c.PhoneNum = cLeagueRegistration.PhoneNum;
                    c.Address = cLeagueRegistration.Address + " " + add2 + " " + post;
                    c.teamName = cLeagueRegistration.teamName;
                    db.Customers.Add(c);
                    db.SaveChanges();
                    
                    var result = await UserManager.CreateAsync(user, cLeagueRegistration.Password);


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
                        ViewBag.ddlS = new SelectList(db.cricketL, "IdV", "leagueTitle");//Creates the division DDL
                        var dd = db.cricketL.ToList().Find(x => x.IdV.ToString() == ddlS);

                        string address = " " + add2 + " " + post;//1)
                        cLeagueRegistration.Address += address;//1)Adds the address line 1 and 2 together 
                        CricketTeams sT = new CricketTeams();
                        sT.TeamN = cLeagueRegistration.teamName;
                        sT.TeamT = teamType;
                        sT.Div = dd.leagueTitle;
                        sT.Email = cLeagueRegistration.EmailAdd;
                        sT.sportType = "Cricket";
                        db.cricketT.Add(sT);
                        CricketPlayers sp = new CricketPlayers();
                        sp.firstName = cLeagueRegistration.firstName;
                        sp.lastName = cLeagueRegistration.lastName;
                        sp.EmailAdd = cLeagueRegistration.EmailAdd;
                        Session["email2"] = cLeagueRegistration.EmailAdd;
                        sp.TeamN = cLeagueRegistration.teamName;
                        Session["teamName"] = cLeagueRegistration.teamName;
                        sp.PlayerRole = "CAPTAIN";
                        sp.JoinT = true;
                        Session["qwe"] = sp.PlayerRole;
                        db.cricketPlayers.Add(sp);
                        db.CLeagueReg.Add(cLeagueRegistration);
                        db.SaveChanges();
                        return RedirectToAction("CricketP", "CricketPlayers");
                    }
                }

            }
            return View(cLeagueRegistration);
        }

        // GET: CLeagueRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLeagueRegistration cLeagueRegistration = db.CLeagueReg.Find(id);
            if (cLeagueRegistration == null)
            {
                return HttpNotFound();
            }
            return View(cLeagueRegistration);
        }

        // POST: CLeagueRegistrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address,teamName")] CLeagueRegistration cLeagueRegistration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLeagueRegistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cLeagueRegistration);
        }

        // GET: CLeagueRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLeagueRegistration cLeagueRegistration = db.CLeagueReg.Find(id);
            if (cLeagueRegistration == null)
            {
                return HttpNotFound();
            }
            return View(cLeagueRegistration);
        }

        // POST: CLeagueRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLeagueRegistration cLeagueRegistration = db.CLeagueReg.Find(id);
            db.CLeagueReg.Remove(cLeagueRegistration);
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

        public ActionResult TsCs()
        {
            
            return View();
        }
    }
}
