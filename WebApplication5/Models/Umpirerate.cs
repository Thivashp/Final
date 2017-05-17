using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Umpirerate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "MATCH RATING")]
        public string homerating { get; set; }

        [Display(Name = "MATCH RATING")]
        public string awayrating { get; set; }

        [Display(Name = "REFEREE ID")]
        public string refID { get; set; }


        [Display(Name = "FIXTURES")]
        public string fixture { get; set; }

        [Display(Name = "HOME TEAM")]
        public string home { get; set; }

        [Display(Name = "AWAY TEAM")]
        public string away { get; set; }


        [Display(Name = "MATCHES PLAYED")]
        public bool played { get; set; }

        [Display(Name = "MATCHES PLAYED")]
        public bool done { get; set; }

        public bool homedone { get; set; }
        public bool awaydone { get; set; }

        public string hrating { get; set; }
        public string arating { get; set; }


    }
}