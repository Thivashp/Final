using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class SoccerResults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int mId { get; set; }

        [Display(Name = "HOME TEAM")]
        public string HomeTeam { get; set; }

        [Display(Name = "AWAY TEAM")]
        public string AwayTeam { get; set; }

        [Display(Name = "HOME GOAL")]
        [Required(ErrorMessage = "PLEASE ENTER HOME GOAL(S)")]
        public int HomeGoals { get; set; }

        [Display(Name = "AWAY GOAL")]
        [Required(ErrorMessage = "PLEASE ENTER AWAY GOAL(S)")]
        public int AwayGoals { get; set; }

        [Display(Name = "DATE TIME")]
       
        public DateTime date { get; set; }
    }
}