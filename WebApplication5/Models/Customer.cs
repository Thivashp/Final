using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Customer
    {
       

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int Id { get; set; }

       
        [Display(Name = "FIRST NAME:")]
        public string firstName { get; set; }

     
        [Display(Name = "LAST NAME:")]
        public string lastName { get; set; }

      
        [Display(Name = "DATE OF BIRTH:")]
        [DataType(DataType.Date)]
        public string Sdate { get; set; }

        [EmailAddress]
        [Display(Name = "EMAIL ADDRESS")]
        public string EmailAdd { get; set; }

        
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Required(ErrorMessage = "MOBILE NO. IS REQUIRED")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "PLEASE ENTER A VALID NUMBER.")]
        [Display(Name = "PHONE NUMBER")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER YOUR ADDRESS")]
        [Display(Name = "ADDRESS:")]
        public string Address { get; set; }

        [Display(Name = "TEAM NAME:")]
        public string teamName { get; set; }

        public string roles { get; set; }
    }
}