using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class SoccerLeague
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }
       
        [Display(Name = "TEAM NAME")]
        public string teamName { get; set; }
        [Display(Name = "DIVISION")]
        public string division { get; set; }
    }
}