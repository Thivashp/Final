using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class SoccerPlayers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "ENTER FIRST NAME")]
        [Display(Name = "FIRST NAME")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "ENTER LAST NAME")]
        [Display(Name = "LAST NAME")]
        public string lastName{ get; set; }

        [Display(Name = "TEAM NAME")]
        public string TeamN { get; set; }


        [Required(ErrorMessage = "ENTER EMAIL ADDRESS")]
        [EmailAddress]
        [Display(Name = "EMAIL ADDRESS")]
        public string EmailAdd { get; set; }

        [Display(Name = "PLAYER ROLE")]
        public string PlayerRole { get; set; }
        public bool JoinT{ get; set; }



    }
}