using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using WebApplication5.Models;


namespace WebApplication5.Controllers
{
    public class court2Controller : Controller
    {
        // GET: court1
        public ActionResult Admin() //admin and employee form
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
        public ActionResult Customer() //customer form
        {
            if (User.IsInRole("Customer"))
            {


                Session["title"] = "";
                Session["desc"] = "";
                Session["start"] = "";
                Session["end"] = "";
                Session["Amount"] = "";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        public ActionResult Customer(string one) //customer form
        {
            if (Session["bool"].ToString() == "true")
            {
                return View();
            }
            return View();

        }
        public ActionResult Public() // user's that are not logged in 
        {
            return View();
        }

        public ActionResult IndexAdmin() //admin and employee form
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

        public ActionResult IndexCustomer() //admin and employee form
        {
            if (User.IsInRole("Customer"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult IndexPublic() //admin and employee form
        {
            return View();
        }

        public JsonResult GetEvents()
        {

           
            {
                WebApplication5Context bk = new WebApplication5Context();
                var v = bk.court2.OrderBy(a => a.StartAt).ToList();
                return new JsonResult { Data = v, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [HttpPost]
        public JsonResult SaveEvent(Court2 evt)
        {

            bool status = false;
            using (WebApplication5Context bk = new WebApplication5Context())
            {
                if (evt.EndAt != null && evt.StartAt.TimeOfDay == new TimeSpan(0, 0, 0) &&
                    evt.EndAt.TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    evt.IsFullDay = true;
                }
                else
                {
                    evt.IsFullDay = false;
                }
                if (evt.EventID == 0)
                {
                    double amount = 0;
                    //var v = bk.court1.Where(a => a.EventID.Equals(evt.EventID)).FirstOrDefault();
                    //if (v != null)
                    //{

                    if (evt.Title == "Soccer")
                    {
                        amount = 350;
                    }
                    else if (evt.Title == "Cricket")
                    {
                        amount = 960;
                    }


                    Session["title"] = evt.Title;
                    Session["start"] = evt.StartAt;
                    Session["end"] = evt.EndAt;
                    Session["Amount"] = amount;
                    Session["bool"] = "true";
                    Session["IsFullDay"] = evt.IsFullDay;


                }
                else
                {
                    bk.court2.Add(evt);

                }
                //bk.SaveChanges();// will not make it save to the database
                status = true;//will not display the event but will still save it to the database
            }

            return new JsonResult { Data = new { status = status } };

        }


        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            bool status = false;
            using (WebApplication5Context bk = new WebApplication5Context())
            {
                var v = bk.court2.Where(a => a.EventID.Equals(eventID)).FirstOrDefault();
                if (v != null)
                {
                    bk.court2.Remove(v);
                    bk.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        public ActionResult Confirm()
        {
            Session["payment"] = "court";
            Session["court2"] = "true";
            return View();
        }

        [HttpPost]
        public JsonResult SaveEvent1(Court2 evt)
        {

            bool status = false;
            using (WebApplication5Context bk = new WebApplication5Context())
            {
                if (evt.EndAt != null && evt.StartAt.TimeOfDay == new TimeSpan(0, 0, 0) &&
                    evt.EndAt.TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    evt.IsFullDay = true;
                }
                else
                {
                    evt.IsFullDay = false;
                }
                if (evt.EventID == 0)
                {
                    var v = bk.court2.Where(a => a.EventID.Equals(evt.EventID)).FirstOrDefault();
                    if (v != null)
                    {


                        v.Title = evt.Title;
                        v.Description = evt.Description;
                        v.StartAt = evt.StartAt;
                        v.EndAt = evt.EndAt;

                        Session["IsFullDay"] = evt.IsFullDay;




                    }
                    else
                    {
                        bk.court2.Add(evt);

                    }
                    bk.SaveChanges();// will not make it save to the database
                    status = true;//will not display the event but will still save it to the database
                }

                return new JsonResult { Data = new { status = status } };

            }


        }

    }

}