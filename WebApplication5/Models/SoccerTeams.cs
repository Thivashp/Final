using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class SoccerTeams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int IdV { get; set; }

        [Display(Name = "TEAM NAME")]
        public string TeamN { get; set; }

        [Display(Name = "TEAM TYPE")]
        public string TeamT{ get; set; }

        [Display(Name = "DIVISION")]
        public string Div { get; set; }

        
        public string sportType { get; set; }

        [Display(Name = "EMAIL")]
        public string Email { get; set; }
    }
}