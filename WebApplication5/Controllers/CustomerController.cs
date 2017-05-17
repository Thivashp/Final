using SendGrid;
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
    public class CustomerController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: Customer
        public ActionResult Index()
        {
            //return View(db.Customers.ToList());
            if (User.IsInRole("Admin"))
            {
                return View(db.Customers.ToList().Where(i => i.roles == "Employee"));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,Password,PhoneNum,Address,teamName")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            var d = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);
            var e = db.Customers.ToList().Find(x => x.Id == d.Id);

            //if (e.Id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Customer customer = db.Customers.Find(e.Id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
        /////////////////////////////////////////////////////////////////////SOCCER TEAM VIEW//////////////////////////////////////////////////////////////////
        public ActionResult CustSocT()
        {
            Session["teamFN"] = " ";
            Session["teamLN"] = " ";
            Session["teamEA"] = " ";
            Session["email3"] = " ";
            Session["emailResult"] = " ";
            Session["Validation"] = " ";
            Session["EValidation"] = " ";
            var d = db.soccerT.ToList().FindAll(x => x.Email.ToUpper() == User.Identity.Name.ToUpper());
            Session["selectT"] = "    ";
            ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper()), "Id", "TeamN");
            return View(d.ToList());
        }
        [HttpPost]
        public ActionResult CustSocT(string selectT, string subject, string message)
        {
            var dd = db.soccerPlayers.ToList().Find(x => x.Id.ToString() == selectT);//searches for string players from index 

            var email = db.soccerPlayers.ToList().Where(x => x.EmailAdd == User.Identity.Name && x.PlayerRole == "CAPTAIN" || x.EmailAdd == User.Identity.Name && x.PlayerRole == "VICE-CAPTAIN");

            if (dd == null)
            {
                Session["Validation"] = "PLEASE SELECT A TEAM FOR MORE INFORMATION";
            }
            else
            {
                Session["Validation"] = " ";
                var f = db.soccerPlayers.ToList().FindAll(x => x.TeamN == dd.TeamN);//searches for players using string
                List<SoccerPlayers> listP = db.soccerPlayers.Where(item => item.TeamN == dd.TeamN && item.JoinT == true).ToList();
                if (f != null)
                {

                    ViewBag.List = listP;
                    foreach (var x in f)
                    {
                        if (x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper() && x.PlayerRole == "CAPTAIN" || x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper() && x.PlayerRole == "CAPTAIN")
                        {
                            Session["email3"] = "True";
                        }

                    }

                }

                foreach (var w in listP)
                {
                    string apiKey = "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";

                    dynamic sendGridClient = new SendGridAPIClient(apiKey);
                    string Body = message;
                    string subj = subject;

                    SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                    SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(w.EmailAdd);
                    Content content = new Content("text/plain", Body);
                    SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, subj, toEmail, content);

                    dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());


                }
                Session["emailResult"] = "EMAIL SENT";
                var d = db.soccerT.ToList().FindAll(x => x.Email == User.Identity.Name);
                ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
                Session["emailResult"] = " ";
                return View(d.ToList());


            }
            var q = db.soccerT.ToList().FindAll(x => x.Email == User.Identity.Name);
            ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
            Session["emailResult"] = " ";
            return View(q.ToList());
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////CRICKET TEAM VIEW//////////////////////////////////////////////////////////////////
        public ActionResult CustCrkT()
        {
            Session["teamFN"] = " ";
            Session["teamLN"] = " ";
            Session["teamEA"] = " ";
            Session["email3"] = " ";
            Session["emailResult"] = " ";
            Session["Validation"] = " ";
            Session["EValidation"] = " ";
            var d = db.cricketT.ToList().FindAll(x => x.Email.ToUpper() == User.Identity.Name.ToUpper());
            Session["selectT"] = "    ";
            ViewBag.selectT = new SelectList(db.cricketPlayers.ToList().FindAll(x => x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper()), "Id", "TeamN");
            return View(d.ToList());
        }
        [HttpPost]
        public ActionResult CustCrkT(string selectT, string subject, string message)
        {
            var dd = db.cricketPlayers.ToList().Find(x => x.Id.ToString() == selectT);//searches for string players from index 

            var email = db.cricketPlayers.ToList().Where(x => x.EmailAdd == User.Identity.Name && x.PlayerRole == "CAPTAIN" || x.EmailAdd == User.Identity.Name && x.PlayerRole == "VICE-CAPTAIN");

            if (dd == null)
            {
                Session["Validation"] = "PLEASE SELECT A TEAM FOR MORE INFORMATION";
            }
            else
            {
                Session["Validation"] = " ";
                var f = db.cricketPlayers.ToList().FindAll(x => x.TeamN == dd.TeamN);//searches for players using string
                List<CricketPlayers> listP = db.cricketPlayers.Where(item => item.TeamN == dd.TeamN && item.JoinT == true).ToList();
                if (f != null)
                {

                    ViewBag.List = listP;
                    foreach (var x in f)
                    {
                        if (x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper() && x.PlayerRole == "CAPTAIN" || x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper() && x.PlayerRole == "CAPTAIN")
                        {
                            Session["email3"] = "True";
                        }

                    }

                }

                foreach (var w in listP)
                {
                    string apiKey = "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";

                    dynamic sendGridClient = new SendGridAPIClient(apiKey);
                    string Body = message;
                    string subj = subject;

                    SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                    SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(w.EmailAdd);
                    Content content = new Content("text/plain", Body);
                    SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, subj, toEmail, content);

                    dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());


                }
                Session["emailResult"] = "EMAIL SENT";
                var d = db.cricketT.ToList().FindAll(x => x.Email == User.Identity.Name);
                ViewBag.selectT = new SelectList(db.cricketPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
                Session["emailResult"] = " ";
                return View(d.ToList());


            }
            var q = db.cricketT.ToList().FindAll(x => x.Email == User.Identity.Name);
            ViewBag.selectT = new SelectList(db.cricketPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
            Session["emailResult"] = " ";
            return View(q.ToList());
        }

        public ActionResult Edit2(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);

            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "Id,firstName,lastName,Sdate,EmailAdd,PhoneNum,Address")] Customer customer)
        {

            if (ModelState.IsValid)
            {
                customer.roles = "Employee";
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(customer);
        }

        public ActionResult Index1()
        {
            if (User.IsInRole("Employee"))
            {
                return View(db.Customers.ToList().Where(i => i.roles == ""));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

    }
}
