using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int Id { get; set; }
        public string leagueTitle { get; set; }
        public string teamName { get; set; }
        public string sportType { get; set; }
        public decimal Amount { get; set; }
        public string payID { get; set; }
public string paymentType { get; set; }




    }
}