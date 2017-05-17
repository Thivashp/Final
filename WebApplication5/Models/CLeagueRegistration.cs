using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class CLeagueRegistration
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER YOUR NAME")]
        [Display(Name = "FIRST NAME:")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER YOUR SURNAME")]
        [Display(Name = "LAST NAME:")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "ENTER DATE OF BIRTH")]
        [Display(Name = "DATE OF BIRTH:")]
        [DataType(DataType.Date)]
        public string Sdate { get; set; }

        [Required(ErrorMessage = "ENTER EMAIL ADDRESS")]
        [EmailAddress]
        [Display(Name = "EMAIL ADDRESS")]
        public string EmailAdd { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "THE {0} MUST BE ATLEAST {2} CHARACTERS LONG.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "THE PASSWORD AND CONFIRMATION PASSWORD DO NOT MATCH")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "MOBILE NO. IS REQUIRED")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "PLEASE ENTER A VALID NUMBER.")]
        [Display(Name = "PHONE NUMBER")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "PLEASE ENTER YOUR ADDRESS")]
        [Display(Name = "ADDRESS:")]
        public string Address { get; set; }

        [Display(Name = "TEAM NAME:")]
        public string teamName { get; set; }


    }
}