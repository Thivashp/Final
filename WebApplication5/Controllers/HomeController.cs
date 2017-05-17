using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
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
    public class HomeController : Controller
    {
        private WebApplication5Context db = new WebApplication5Context();
        public ActionResult Index()
        {
            ViewBag.Message = "Put your message here bro";
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {
            WebApplication5Context db = new WebApplication5Context();

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                var toBeResized = (from ev in db.Eventss where ev.id == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var toBeResized = (from ev in db.Eventss where ev.id == Convert.ToInt32(e.Id) select ev).First();
                toBeResized.eventstart = e.NewStart;
                toBeResized.eventend = e.NewEnd;
                db.SaveChanges();
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                var toBeCreated = new Event { eventstart = e.Start, eventend = e.End, text = (string)e.Data["name"] };
                db.Eventss.Add(toBeCreated);
                db.SaveChanges();
                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = from ev in db.Eventss select ev;

                DataIdField = "id";
                DataTextField = "text";
                DataStartField = "eventstart";
                DataEndField = "eventend";
            }

        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult TsCs()
        {
            return View();
        }
        public ActionResult SelectReg()
        {
            return View();
        }

   
    }

}