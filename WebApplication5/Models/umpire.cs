using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class umpire
    { 

        [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "NAME REQUIRED")]
    [Display(Name = "NAME")]
    public string Name { get; set; }

    [Required(ErrorMessage = "SURNAME REQUIRED")]
    [Display(Name = "SURNAME")]
    public string Surname { get; set; }

    [EmailAddress]
    [Display(Name = "EMAIL ADDRESS")]
    public string emailAdd { get; set; }


    [Required(ErrorMessage = "ENTER DATE OF BIRTH")]
    [Display(Name = "DATE OF BIRTH")]
    [DataType(DataType.Date)]
    public string Sdate { get; set; }


    [Required(ErrorMessage = "ENTER YEARS OF EXPERIENCE")]
    [Display(Name = "YEARS OF EXPERIENCE")]
    public int yearsOXP { get; set; }

    [Required(ErrorMessage = "GENDER REQUIRED")]
    [Display(Name = "GENDER")]
    public string Gender { get; set; }

    public string Experience { get; set; }


    [Display(Name = "START DATE")]
    [DataType(DataType.Date)]
    public DateTime startDate { get; set; }

    public DateTime updateY { get; set; }

    public string refID { get; set; }

    public bool matchref { get; set; }
}
    }
