using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class CustomerVController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();
        // GET: CustomerV
        public ActionResult Index()
        {
            return View();
        }
        /////////////////////////////////////////////////////////////////////SOCCER TEAM VIEW//////////////////////////////////////////////////////////////////
        public ActionResult CustSocT()
        {
            Session["teamFN"] = " ";
            Session["teamLN"] = " ";
            Session["teamEA"] = " ";
            Session["email3"] = " ";
            Session["emailResult"] = " ";
            var d = db.soccerT.ToList().FindAll(x => x.Email.ToUpper() == User.Identity.Name.ToUpper());
            Session["selectT"] = "    ";
            ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper()), "Id", "TeamN");
            return View(d.ToList());
        }
        [HttpPost]
        public ActionResult CustSocT(string selectT, string subject, string message)
        {
            var dd = db.soccerPlayers.ToList().Find(x => x.Id.ToString() == selectT);//searches for string players from index 
            var f = db.soccerPlayers.ToList().FindAll(x => x.TeamN == dd.TeamN);//searches for players using string
            var email = db.soccerPlayers.ToList().Where(x => x.EmailAdd == User.Identity.Name && x.PlayerRole == "CAPTAIN" || x.EmailAdd == User.Identity.Name && x.PlayerRole == "VICE-CAPTAIN");
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
                Session["emailResult"] = "EMAIL SENT";

            }

            var d = db.soccerT.ToList().FindAll(x => x.Email == User.Identity.Name);
            ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
            Session["emailResult"] = " ";
            return View(d.ToList());

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
            var d = db.cricketT.ToList().FindAll(x => x.Email.ToUpper() == User.Identity.Name.ToUpper());
            Session["selectT"] = "    ";
            ViewBag.selectT = new SelectList(db.cricketPlayers.ToList().FindAll(x => x.EmailAdd.ToUpper() == User.Identity.Name.ToUpper()), "Id", "TeamN");
            return View(d.ToList());
        }
        [HttpPost]
        public ActionResult CustCrkT(string selectT, string subject, string message)
        {
            var dd = db.cricketPlayers.ToList().Find(x => x.Id.ToString() == selectT);//searches for string players from index 
            var f = db.cricketPlayers.ToList().FindAll(x => x.TeamN == dd.TeamN);//searches for players using string
            var email = db.cricketPlayers.ToList().Where(x => x.EmailAdd == User.Identity.Name && x.PlayerRole == "CAPTAIN" || x.EmailAdd == User.Identity.Name && x.PlayerRole == "VICE-CAPTAIN");
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
                Session["emailResult"] = "EMAIL SENT";

            }

            var d = db.cricketT.ToList().FindAll(x => x.Email == User.Identity.Name);
            ViewBag.selectT = new SelectList(db.soccerPlayers.ToList().FindAll(x => x.EmailAdd == User.Identity.Name), "Id", "TeamN");
            return View(d.ToList());
        }
 ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}