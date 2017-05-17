using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class pass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int PromoId { get; set; }


        [Display(Name = "USERNAME")]
        public string Username { get; set; }


        [Display(Name = "PASSWORD")]
        public string Password { get; set; }
        [Display(Name = "EMAIL ADDRESS")]
        public string Email { get; set; }

       
    }
}


       