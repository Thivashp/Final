using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class CricketResults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int mId { get; set; }

        [Display(Name = "HOME TEAM")]
        public string HomeTeam { get; set; }

        [Display(Name = "AWAY TEAM")]
        public string AwayTeam { get; set; }

        [Display(Name = "HOME RUNS")]
        public int HRuns { get; set; }

        [Display(Name = "AWAY RUNS")]
        public int ARuns { get; set; }

        [Display(Name = "HOME TEAM OVERS FACED")]
        public int HOversFaced { get; set; }

        [Display(Name = "AWAY TEAM OVERS FACED")]
        public int AOversFaced { get; set; }

        [Display(Name = "HOME WICKETS")]
        public int HWickets { get; set; }

        [Display(Name = "AWAY WICKETS")]
        public int AWickets { get; set; }

        [Display(Name = "DATE TIME")]
        public DateTime date { get; set; }
    }
}