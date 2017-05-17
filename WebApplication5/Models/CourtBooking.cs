using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class CourtBooking
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string startAt { get; set; }
        public string endAt { get; set; }
        public decimal Amount { get; set; }
        public string payID { get; set; }
        public string paymentType { get; set; }
    }
}



