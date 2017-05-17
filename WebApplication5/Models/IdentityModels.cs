using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebApplication5;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string firstN { get; set; }
        public string lastN { get; set; }
        public string DateOfBirth { get; set; }
        public override string Email { get; set; }
        public bool ConfirmedEmail { get; set; }

        public string phoneNum{ get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string TeamName { get; set; }

        
       

        internal Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager userManager)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
    }
}