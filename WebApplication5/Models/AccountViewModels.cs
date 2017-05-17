using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication5.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "USER NAME")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CURRENT PASSWORD")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "THE {0} {2} CHARACTERS LONG.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NEW PASSWORD")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "CONFIRM NEW PASSWORD")]
        [Compare("NewPassword", ErrorMessage = "THE NEW PASSWORD AND EXISTING PASSWORD DO NOT MATCH.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "EMAIL ADDRESS")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [Display(Name = "REMEMBER ME?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "ENTER NAME")]
        [Display(Name = "NAME")]
        public string firstN { get; set; }

        [Required(ErrorMessage = "ENTER SURNAME")]
        [Display(Name = "SURNAME")]
        public string lastN { get; set; }

        [Required(ErrorMessage = "ENTER DATE OF BIRTH")]
        [Display(Name = "DATE OF BIRTH")]
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "ENTER EMAIL ADDRESS")]
        [EmailAddress]
        [Display(Name = "EMAIL ADDRESS")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "THE {0} MUST BE ATLEAST {2} CHARACTERS LONG.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "CONFIRM PASSWORD")]
        [Compare("Password", ErrorMessage = "THE PASSWORD AND CONFIRMATION PASSWORD DO NOT MATCH.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "ENTER CONTACT NUMBER")]
        [StringLength(10, ErrorMessage = "PHONE NUMBER MUST BE 10 DIGITS LONG")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "CONTACT NUMBER")]
        public string phoneNum { get; set; }

        [Required(ErrorMessage = "ENTER PHYSICAL ADDRESS")]
        [Display(Name = "ADDRESS")]
        public string Address { get; set; }

       [Display(Name = "GENDER")]
        public string Gender { get; set; }

        public string TeamName { get; set; }

    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "CODE")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "REMEMBER THIS?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "EMAIL ADDRESS")]
        public string Email { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "EMAIL ADDRESS")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "THE {0} MUST BE ATLEAST {2} CHARACTERS LONG.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "CONFIRM PASSWORD")]
        [Compare("Password", ErrorMessage = "THE PASSWORD AND CONFIRMATION PASSWORD DO NOT MATCH.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "PLEASE ENTER YOUR USERNAME")]
        [Display(Name = "USER NAME")]
        public string UserName { get; set; }
    }
    }
