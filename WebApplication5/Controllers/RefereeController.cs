using PagedList;
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
    public class RefereeController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: Referee
        [Authorize]

        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {
          
            foreach (var f in db.referee.ToList())
            {
                if (f.startDate.Date.Day == DateTime.Now.Day && f.startDate.Date.Month == DateTime.Now.Month && f.startDate.Date.Year < DateTime.Now.Year && f.updateY.Year < DateTime.Now.Year)

                // if(r.startDate.Date.Year != DateTime.Now.Year)
                {
                    f.yearsOXP = f.yearsOXP + 1;
                    f.updateY = DateTime.Now.Date;

                    if (f.yearsOXP <= 3)
                    {
                        f.Experience = "Amateur";
                    }
                    else if (f.yearsOXP > 3 || f.yearsOXP <= 7)
                    {
                        f.Experience = "Intermediate";
                    }
                    else if (f.yearsOXP >= 8)
                    {
                        f.Experience = "Professional";
                    }

                    db.SaveChanges();

                }
                
            }
            return View(db.referee.ToList());
        }
            else
            {
                return RedirectToAction("Login", "Account");
    }
}

    // GET: Referee/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referee referee = db.referee.Find(id);
            if (referee == null)
            {
                return HttpNotFound();
            }
            return View(referee);
        }

        // GET: Referee/Create
        [Authorize]
        public ActionResult Create()
        {
            Session["validate1"] = " ";
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Employee"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
                
            }
            return View();
        }
        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomString1()
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // POST: Referee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Sdate,yearsOXP,Gender,startDate,emailAdd")] Referee referee, string gender)
        {
            var d = db.referee.ToList().Find(x => x.emailAdd.ToUpper() == referee.emailAdd.ToUpper());
            try
            {
                if (d != null)
                {
                    Session["validate1"] = "EMAIL ALREADY EXISTS";
                }
                else
                { 
                    if (referee.yearsOXP <= 3)
                    {
                        referee.Experience = "Amateur";
                    }
                    else if (referee.yearsOXP > 3 && referee.yearsOXP <= 7)
                    {
                        referee.Experience = "Intermediate";
                    }
                    else if (referee.yearsOXP >= 8)
                    {
                        referee.Experience = "Professional";
                    }

                    referee.Gender = gender;
                    referee.startDate = DateTime.Now.Date;
                    referee.updateY = referee.startDate.Date;
                    referee.refID = "MI" + RandomString() + RandomString();

                    db.referee.Add(referee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Session["validate1"] = "PLEASE ENTER INFORMATION  INTO ALL FIELDS";
            }
            return View();
        }
                

              
            
        
           
            

    // GET: Referee/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referee referee = db.referee.Find(id);
            if (referee == null)
            {
                return HttpNotFound();
            }
            return View(referee);
        }

        // POST: Referee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Sdate,yearsOXP,Gender,Experience")] Referee referee)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(referee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referee);
        }

        // GET: Referee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referee referee = db.referee.Find(id);
            if (referee == null)
            {
                return HttpNotFound();
            }
            return View(referee);
        }

        // POST: Referee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Referee referee = db.referee.Find(id);
            db.referee.Remove(referee);
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
        public ActionResult SearchReferee(string refereeId, string eAdd, int? page)
        {
            if (User.IsInRole("Employee"))
            {
                var sr = from s in db.referee select s;

                if (!String.IsNullOrEmpty(refereeId))
                {
                    sr = sr.Where(x => x.refID.Contains(refereeId));
                }

                if (!String.IsNullOrEmpty(eAdd))
                {
                    sr = sr.Where(x => x.emailAdd.Contains(eAdd));
                }
                return View(sr.ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}
