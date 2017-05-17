using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class RefereeMatch
    {
        [Key]
        public int ID { get; set; }

        public string fixtures { get; set; }

        public string division { get; set; }

        public string refereeID { get; set; }




    }
}