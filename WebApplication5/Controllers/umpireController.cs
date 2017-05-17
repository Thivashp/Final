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
    public class umpireController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();

        // GET: umpire
        public ActionResult Index()
        {
            if (User.IsInRole("Employee"))
            {

                foreach (var f in db.umpire.ToList())
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
                return View(db.umpire.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: umpire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            umpire umpire = db.umpire.Find(id);
            if (umpire == null)
            {
                return HttpNotFound();
            }
            return View(umpire);
        }

        // GET: umpire/Create
        public ActionResult Create()
        {
            Session["validate1"] = " ";
            if (User.IsInRole("Employee"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

           
        }

        // POST: umpire/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
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
        public ActionResult Create([Bind(Include = "Id,Name,Surname,emailAdd,Sdate,yearsOXP,Gender,Experience,startDate,updateY,refID,matchref")] umpire umpire,string gender)
        {

            var d = db.umpire.ToList().Find(x => x.emailAdd.ToUpper() == umpire.emailAdd.ToUpper());
            try
            {
                if (d != null)
                {
                    Session["validate1"] = "EMAIL ALREADY EXISTS";
                }
                else
                {
                    if (umpire.yearsOXP <= 3)
            {
                umpire.Experience = "Amateur";
            }
            else if (umpire.yearsOXP > 3 && umpire.yearsOXP <= 7)
            {
                umpire.Experience = "Intermediate";
            }
            else if (umpire.yearsOXP >= 8)
            {
                umpire.Experience = "Professional";
            }

            umpire.Gender = gender;
            umpire.startDate = DateTime.Now.Date;
            umpire.updateY = umpire.startDate.Date;
            umpire.refID = "MI" + RandomString() + RandomString();

            db.umpire.Add(umpire);
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







        // GET: umpire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            umpire umpire = db.umpire.Find(id);
            if (umpire == null)
            {
                return HttpNotFound();
            }
            return View(umpire);
        }

        // POST: umpire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,emailAdd,Sdate,yearsOXP,Gender,Experience,startDate,updateY,refID,matchref")] umpire umpire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umpire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(umpire);
        }

        // GET: umpire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            umpire umpire = db.umpire.Find(id);
            if (umpire == null)
            {
                return HttpNotFound();
            }
            return View(umpire);
        }

        // POST: umpire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            umpire umpire = db.umpire.Find(id);
            db.umpire.Remove(umpire);
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

        public ActionResult SearchUmpire(string refereeId, string eAdd, int? page)
        {
            if (User.IsInRole("Employee"))
            {
                var sr = from s in db.umpire select s;

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
