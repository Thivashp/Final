using iTextSharp.text;
using iTextSharp.text.pdf;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class PaymentController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: Payment
        public ActionResult Index()
        {
            return View(db.Payments.ToList());
        }

        // GET: Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,leagueTitle,teamName,sportType,Amount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,leagueTitle,teamName,sportType,Amount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        // GET: Payment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomString1()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult afterPay()
        {
            if (Session["payment"].ToString() == "league")
            {
               
                    if (Session["cricket"].ToString() == "True")
                    {
                        CricketLeague sl = new CricketLeague();
                        sl.teamName = Session["teamName"].ToString();
                        sl.division = Session["Division"].ToString();
                        db.CricketLeague.Add(sl);
                        db.SaveChanges();
                    }
                    else
                    {
                        SoccerLeague sl = new SoccerLeague();
                        sl.teamName = Session["teamName"].ToString();
                        sl.division = Session["Division"].ToString();
                        db.soccerLeague.Add(sl);
                        db.SaveChanges();
                    
                }

                Payment p = new Payment();
                string a = RandomString() + RandomString1();
                var d = db.LeagueReg.ToList().Find(x => x.EmailAdd == Session["email2"].ToString());
                var ct = db.cricketT.ToList().Find(x => x.sportType == "Cricket" && x.TeamN == Session["teamName"].ToString());
                var st = db.soccerT.ToList().Find(x => x.sportType == "Soccer" && x.TeamN == Session["teamName"].ToString());
                var sp = db.soccerPlayers.ToList().FindAll(x => x.TeamN == Session["teamName"].ToString()).Where(x => x.PlayerRole != "CAPTAIN");
                var cp = db.cricketPlayers.ToList().FindAll(x => x.TeamN == Session["teamName"].ToString()).Where(x => x.PlayerRole != "CAPTAIN");


                if (sp != null)
                {
                    foreach (var w in sp)
                    {
                        string apiKey = "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";

                        dynamic sendGridClient = new SendGridAPIClient(apiKey);
                        string Body = "PLease Click to join team : </br>" + Url.Action("TeamConfirmed", "SoccerPlayers", new { Tokenb = Session["teamName"].ToString(), Tokenc = w.EmailAdd }, Request.Url.Scheme);

                        SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                        SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(w.EmailAdd);
                        Content content = new Content("text/plain", Body);
                        SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, " Join Team", toEmail, content);

                        dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());
                    }
                }
                else if (cp != null)
                {
                    foreach (var w in cp)
                    {
                        string apiKey = "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";

                        dynamic sendGridClient = new SendGridAPIClient(apiKey);
                        string Body = "PLease Click to join team : </br>" + Url.Action("TeamConfirmed", "CricketPlayers", new { Tokenb = Session["teamName"].ToString(), Tokenc = w.EmailAdd }, Request.Url.Scheme);

                        SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                        SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(w.EmailAdd);
                        Content content = new Content("text/plain", Body);
                        SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, " Join Team", toEmail, content);

                        dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());
                    }
                }
                else if (st != null)
                {
                    var z = db.soccerT.ToList().Find(x => x.TeamN == Session["teamName"].ToString());
                    var e = db.soccerL.ToList().Find(x => x.leagueTitle == z.Div);
                    var f = from s in db.soccerT.ToList() select s;
                    f = f.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

                    Session["Division"] = f.First().Div;
                    Session["SportType"] = f.First().sportType;


                    var lt = db.soccerL.ToList().Find(x => x.leagueTitle == Session["Division"].ToString());
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


                    p.teamName = Session["teamName"].ToString();
                    p.leagueTitle = Session["LeagueTitle"].ToString();
                    p.sportType = "Soccer";
                    p.Amount = Convert.ToDecimal(e.regFee);
                    p.payID = a;
                    db.Payments.Add(p);
                    db.SaveChanges();
                    return View(db.soccerPlayers.ToList().Where(x => x.TeamN == Session["teamName"].ToString()));
                }

                else if (ct != null)

                {
                    var z = db.cricketT.ToList().Find(x => x.TeamN == Session["teamName"].ToString());
                    var e = db.cricketL.ToList().Find(x => x.leagueTitle == z.Div);
                    var f = from s in db.cricketT.ToList() select s;
                    f = f.Where(s => s.TeamN.ToUpper() == Session["teamName"].ToString().ToUpper());

                    Session["Division"] = f.First().Div;
                    Session["SportType"] = f.First().sportType;


                    var lt = db.cricketL.ToList().Find(x => x.leagueTitle == Session["Division"].ToString());
                    Session["Amount1"] = lt.regFee;
                    Session["teamPlayer1"] = " ";
                    Session["fn1"] = "First Name: " + d.firstName;
                    Session["ln1"] = "Last Name :" + d.lastName;
                    Session["dob1"] = "Date Of Birth: " + d.Sdate;
                    Session["mail1"] = "Email: " + d.EmailAdd;
                    Session["pn1"] = "Phone Number: " + d.PhoneNum;
                    Session["add1"] = "Address: " + d.Address;
                    Session["tn1"] = "Team Name: " + d.teamName;



                    Session["LeagueTitle1"] = "League Title: " + e.leagueTitle;
                    Session["LeagueCategory1"] = "Category: " + e.category;
                    Session["LeagueSDte1"] = "Start Date: " + e.Sdate;
                    Session["Regfee1"] = "Registration Fee: " + e.regFee;
                    Session["costPR1"] = "Cost Per Round " + e.CostPR;


                    p.teamName = Session["teamName"].ToString();
                    p.leagueTitle = Session["LeagueTitle"].ToString();
                    p.sportType = "Cricket";
                    p.Amount = Convert.ToDecimal(e.regFee);
                    p.payID = a;
                    p.paymentType = "league";
                    db.Payments.Add(p);
                    db.SaveChanges();
                    return View(db.cricketPlayers.ToList().Where(x => x.TeamN == Session["teamName"].ToString()));
                }
                return View();
            }



            else if (Session["payment"].ToString() == "court")
            {

                CourtBooking p = new CourtBooking();

                Court1 c1 = new Court1();


                Court2 c2 = new Court2();

                Court3 c3 = new Court3();


                if (Session["c1"].ToString() == "true")
                {
                    p.title = Session["title"].ToString();
                    p.startAt = Session["start"].ToString();
                    p.endAt = Session["end"].ToString();
                    p.Amount = Convert.ToDecimal(Session["Amount"]);
                    p.paymentType = "court";


                    c1.Title = Session["title"].ToString();
                    c1.StartAt = Convert.ToDateTime(Session["start"]);
                    c1.EndAt = Convert.ToDateTime(Session["end"].ToString());
                    c1.Amount = Convert.ToInt16(Session["Amount"]);
                    c1.paymentType = "court1";
                    c1.paymentId = p.paymentType;
                    db.court1.Add(c1);
                    db.SaveChanges();


                    var dd = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);
                    Session["fn1"] = "First Name: " + dd.firstName;
                    Session["ln1"] = "Last Name: " + dd.lastName;
                    Session["mail1"] = "Email Address: " + dd.EmailAdd;
                    Session["pn1"] = "Phone Number: " + dd.PhoneNum;

                    Session["title1"] = Session["title"].ToString();
                    Session["start1"] = Session["start"].ToString();
                    Session["end1"] = Session["end"].ToString();
                    Session["Amount1"] = Session["Amount"].ToString();
                }
                else if (Session["c2"].ToString() == "true")
                {

                    p.title = Session["title"].ToString();
                    p.startAt = Session["start"].ToString();
                    p.endAt = Session["start"].ToString();
                    p.Amount = Convert.ToDecimal(Session["Amount"]);
                    p.paymentType = "court";

                    c2.Title = Session["title"].ToString();
                    c2.StartAt = Convert.ToDateTime(Session["start"]);
                    c2.EndAt = Convert.ToDateTime(Session["end"].ToString());
                    c2.Amount = Convert.ToInt16(Session["Amount"]);
                    c2.paymentType = "court2";
                    c2.paymentId = p.paymentType;
                    db.court2.Add(c2);
                    db.SaveChanges();


                    var dd = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);
                    Session["fn1"] = "First Name:" + dd.firstName;
                    Session["ln1"] = "Last Name" + dd.lastName;
                    Session["mail1"] = "Email " + dd.EmailAdd;
                    Session["pn1"] = "Phone Number: " + dd.PhoneNum;

                    Session["title1"] = Session["title"].ToString();
                    Session["start1"] = Session["start"].ToString();
                    Session["end1"] = Session["end"].ToString();
                    Session["Amount1"] = Session["Amount"].ToString();
                }
                else if (Session["c3"].ToString() == "true")
                {
                    p.title = Session["title"].ToString();
                    p.startAt = Session["start"].ToString();
                    p.endAt = Session["start"].ToString();
                    p.Amount = Convert.ToDecimal(Session["Amount"]);
                    p.paymentType = "court";

                    c3.Title = Session["title"].ToString();
                    c3.StartAt = Convert.ToDateTime(Session["start"]);
                    c3.EndAt = Convert.ToDateTime(Session["end"].ToString());
                    c3.Amount = Convert.ToInt16(Session["Amount"]);
                    c3.paymentType = "court3";
                    c3.paymentId = p.paymentType;
                    db.court3.Add(c3);
                    db.SaveChanges();


                    var dd = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);
                    Session["fn1"] = "First Name:" + dd.firstName;
                    Session["ln1"] = "Last Name" + dd.lastName;
                    Session["mail1"] = "Email " + dd.EmailAdd;
                    Session["pn1"] = "Phone Number: " + dd.PhoneNum;

                    Session["title1"] = Session["title"].ToString();
                    Session["start1"] = Session["start"].ToString();
                    Session["end1"] = Session["end"].ToString();
                    Session["Amount1"] = Session["Amount"].ToString();

                }

            }
            return View();
        }

public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("MotionInc" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(5);
            PdfPTable tableLayout1 = new PdfPTable(7);
            PdfPTable tableLayout2 = new PdfPTable(5);
            PdfPTable tableLayout3 = new PdfPTable(5);
            PdfPTable tableLayout4 = new PdfPTable(4);
            PdfPTable tableLayout5 = new PdfPTable(4);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            if (Session["payment"].ToString() == "league")
            {
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                //Add Content to PDF 
                doc.Add(Add_Content_To_PDF1(tableLayout1));
                doc.Add(Add_Content_To_PDF(tableLayout));
                doc.Add(Add_Content_To_PDF3(tableLayout3));
          

            }
            else if (Session["payment"].ToString() == "court")
            {
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                //Add Content to PDF 
              
                doc.Add(Add_Content_To_PDF5(tableLayout4));
                doc.Add(Add_Content_To_PDF4(tableLayout5));

            }
            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)

        {

            float[] headers = { 50, 24, 45, 35, 50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top





            tableLayout.AddCell(new PdfPCell(new Phrase("LEAGUE INFORMATION", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            AddCellToHeader(tableLayout, "League Title");
            AddCellToHeader(tableLayout, "League Category");
            AddCellToHeader(tableLayout, "League Start Date");
            AddCellToHeader(tableLayout, "Team Name");
            AddCellToHeader(tableLayout, "Registeration Fee");



            ////Add body

            AddCellToBody(tableLayout, Session["LeagueTitle"].ToString());
            AddCellToBody(tableLayout, Session["LeagueCategory"].ToString());
            AddCellToBody(tableLayout, Session["LeagueSDte"].ToString());
            AddCellToBody(tableLayout, Session["teamName"].ToString());
            AddCellToBody(tableLayout, Session["Regfee"].ToString());



            return tableLayout;
        }
        protected PdfPTable Add_Content_To_PDF1(PdfPTable tableLayout1)

        {


            float[] headers1 = { 50, 50, 50, 50, 50, 50, 50 };  //Header Widths
            tableLayout1.SetWidths(headers1);        //Set the pdf headers
            tableLayout1.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout1.HeaderRows = 1;




            tableLayout1.AddCell(new PdfPCell(new Phrase("YOUR INFORMATION", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });


            AddCellToHeader(tableLayout1, "First Name");
            AddCellToHeader(tableLayout1, "Last Name");
            AddCellToHeader(tableLayout1, "Date Of Birth");
            AddCellToHeader(tableLayout1, "Email Address");
            AddCellToHeader(tableLayout1, "Phone Number");
            AddCellToHeader(tableLayout1, "Address");
            AddCellToHeader(tableLayout1, "Team Name");

            ////Add body

            AddCellToBody(tableLayout1, Session["fn"].ToString());
            AddCellToBody(tableLayout1, Session["ln"].ToString());
            AddCellToBody(tableLayout1, Session["dob"].ToString());
            AddCellToBody(tableLayout1, Session["mail"].ToString());
            AddCellToBody(tableLayout1, Session["pn"].ToString());
            AddCellToBody(tableLayout1, Session["add"].ToString());
            AddCellToBody(tableLayout1, Session["tn"].ToString());
            return tableLayout1;
        }



        protected PdfPTable Add_Content_To_PDF3(PdfPTable tableLayout3)

        {


            float[] headers3 = { 80, 50, 50, 50, 90 };  //Header Widths
            tableLayout3.SetWidths(headers3);        //Set the pdf headers
            tableLayout3.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout3.HeaderRows = 1;


            tableLayout3.AddCell(new PdfPCell(new Phrase("PAYMENT INFORMATION", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
            var d = db.Payments.ToList().Find(x => x.teamName == Session["teamName"].ToString() && x.sportType == "Soccer");
            var f = db.Payments.ToList().Find(x => x.teamName == Session["teamName"].ToString() && x.sportType == "Cricket");
            if (d != null)
            {
                ////Add header
                AddCellToHeader(tableLayout3, "League Title");
                AddCellToHeader(tableLayout3, "Sport Type");
                AddCellToHeader(tableLayout3, "Team Name");
                AddCellToHeader(tableLayout3, "Amount Paid");
                AddCellToHeader(tableLayout3, "Payment ID");


                AddCellToBody(tableLayout3, d.leagueTitle);
                AddCellToBody(tableLayout3, d.sportType);
                AddCellToBody(tableLayout3, d.teamName);
                AddCellToBody(tableLayout3, d.Amount.ToString());
                AddCellToBody(tableLayout3, d.payID);

                return tableLayout3;
            }
            if (f != null)
            {
                ////Add header
                AddCellToHeader(tableLayout3, "League Title");
                AddCellToHeader(tableLayout3, "Sport Type");
                AddCellToHeader(tableLayout3, "Team Name");
                AddCellToHeader(tableLayout3, "Amount Paid");
                AddCellToHeader(tableLayout3, "Payment ID");


                AddCellToBody(tableLayout3, f.leagueTitle);
                AddCellToBody(tableLayout3, f.sportType);
                AddCellToBody(tableLayout3, f.teamName);
                AddCellToBody(tableLayout3, f.Amount.ToString());
                AddCellToBody(tableLayout3, f.payID);

                return tableLayout3;
            }
            return tableLayout3;

        }

        protected PdfPTable Add_Content_To_PDF5(PdfPTable tableLayout5)

        {


            float[] headers5 = { 80, 50, 50, 50};  //Header Widths
            tableLayout5.SetWidths(headers5);        //Set the pdf headers
            tableLayout5.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout5.HeaderRows = 1;


            tableLayout5.AddCell(new PdfPCell(new Phrase("PAYEE INFORMATION", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            var dd = db.Customers.ToList().Find(x => x.EmailAdd == User.Identity.Name);
                ////Add header
                AddCellToHeader(tableLayout5, "FIRST NAME");
                AddCellToHeader(tableLayout5, "LAST NAME");
                AddCellToHeader(tableLayout5, "EMAIL ADDRESS");
                AddCellToHeader(tableLayout5, "CONTACT NUMBER");
             


                AddCellToBody(tableLayout5, dd.firstName);
                AddCellToBody(tableLayout5, dd.lastName);
                AddCellToBody(tableLayout5, dd.EmailAdd);
                AddCellToBody(tableLayout5, dd.PhoneNum);
                

                return tableLayout5;
       
        }
        protected PdfPTable Add_Content_To_PDF4(PdfPTable tableLayout4)

        {


            float[] headers4 = { 80, 50, 50, 50};  //Header Widths
            tableLayout4.SetWidths(headers4);        //Set the pdf headers
            tableLayout4.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout4.HeaderRows = 1;


            tableLayout4.AddCell(new PdfPCell(new Phrase("PAYMENT INFORMATION", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });
        

            ////Add header
            AddCellToHeader(tableLayout4, "TITLE");
            AddCellToHeader(tableLayout4, "START TIME");
            AddCellToHeader(tableLayout4, "END TIME");
            AddCellToHeader(tableLayout4, "AMOUNT");

          
        
   

            AddCellToBody(tableLayout4, Session["title"].ToString() );
            AddCellToBody(tableLayout4, Session["start"].ToString());
            AddCellToBody(tableLayout4, Session["end"].ToString());
            AddCellToBody(tableLayout4, Session["Amount"].ToString());
           

            return tableLayout4;

        }


        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255) });
        }


        public ActionResult SendEmail()
        {
            string apiKey = "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";

            dynamic sendGridClient = new SendGridAPIClient(apiKey);
            string Body = CreatePdf().ToString();

            SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
            SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(Session["email2"].ToString());
            Content content = new Content("text/plain", Body);
            SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, " Join Team", toEmail, content);

            dynamic response = sendGridClient.client.mail.send.post(requestBody: mail.Get());
            return View();
        }





        public ActionResult TeamConfirmed(string Tokenb, string Tokenc)
        {
            var lt = db.soccerPlayers.ToList().Find(x => x.TeamN == Tokenb && x.EmailAdd == Tokenc);

            if (lt != null)
            {
                lt.JoinT = true;
                db.soccerPlayers.Add(lt);
                db.SaveChanges();
                Session["team"] = Tokenb;

            }
            else
            {
                Session["team"] = "Individual";
            }

            return View();

        }

    }
}
