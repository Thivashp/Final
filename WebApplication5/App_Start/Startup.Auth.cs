using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using WebApplication5.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication5
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            ApplicationDbContext context = new ApplicationDbContext();



            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));





            // In Startup I am creating first Admin Role and creating a default Admin User    

            if (!roleManager.RoleExists("Admin"))

            {



                // first we create Admin role   

                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();

                role.Name = "Admin";

                roleManager.Create(role);



                //Here we create a Admin super user who will maintain the website                  



                var user = new ApplicationUser();

                user.UserName = "Calmin23m@gmail.com";

                user.Email = "Calmin23m@gmail.com";

                user.ConfirmedEmail = true;

                user.EmailConfirmed = true;

                string userPWD = "scab23";



                var chkUser = UserManager.Create(user, userPWD);



                //Add default User to Role Admin   

                if (chkUser.Succeeded)

                {

                    var result1 = UserManager.AddToRole(user.Id, "Admin");



                }

            }
            //////////////////////////////////////////////////////////////////////////
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();  }
        }
    }
}