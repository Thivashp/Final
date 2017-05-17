using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class CricketFixtures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "MATCH")]
        public string fixture { get; set; }
        [Display(Name = "DIVISION")]
        public string division { get; set; }
        [Display(Name = "REFEREE ID")]
        public string refID { get; set; }
        public bool IsComplete { get; set; }
        public bool refcheck { get; set; }
        public bool refreass { get; set; }
    }
}