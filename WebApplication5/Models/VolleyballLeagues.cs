using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class VolleyballLeagues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int IdV { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER A LEAGUE TITLE")]
        [Display(Name = "LEAGUE TITLE:")]
        public string leagueTitle { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER A CATEGORY")]
        [Display(Name = "CATEGORY:")]
        public string category { get; set; }

        [Required(ErrorMessage = "ENTER LEAGUE START DATE")]
        [Display(Name = "START DATE:")]
        [DataType(DataType.Date)]
        public string Sdate { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER A REGISTRATION FEE")]
        [Display(Name = "REGISTRATION FEE:R")]
        public double regFee { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER COST PER ROUND")]
        [Display(Name = "COST PER ROUND:R")]
        public double CostPR { get; set; }

    }
}