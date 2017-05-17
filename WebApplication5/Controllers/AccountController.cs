using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using WebApplication5.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Data.Entity;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace WebApplication5.Controllers
{
   
    public class AccountController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        WebApplication5Context db = new WebApplication5Context();
        string a;
        public ActionResult ViewAll()
        {

            var all = context.Users.ToList();
            return View(all);
        }
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult Use()
        {

            return View();
        }


        [AllowAnonymous]
        public ActionResult ResPas()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResPas(string email)
        {
            ApplicationDbContext c = new ApplicationDbContext();

            var s = db.passs.ToList();





            foreach (var ss in s)

            {
                if (ss.Email == email)
                {
                    string sss = "Dear user your password is:" + ss.Password;



                    string apiKey = "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";


                    dynamic sendGridClient = new SendGridAPIClient(apiKey);


                    SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                    SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(email);
                    Content content = new Content("text/plain", sss);
                    SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, "Forgotten Password", toEmail, content);

                    dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());


                    return View("ForgotPasswordConfirmation");




                }
                else if (ss.Email == null)
                {
                    ViewBag.s = "Email does not exist";
                    return View("ResPas");
                }



            }
            return View("ResPas");
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, string email)
        {
            ApplicationDbContext c = new ApplicationDbContext();

            int incrementalDelay;
            if (HttpContext.Application[Request.UserHostAddress] != null)
            {

                incrementalDelay = (int)HttpContext.Application[Request.UserHostAddress];
                await Task.Delay(incrementalDelay * 1000);
            }

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);






                if (user != null)
                {

                    if (user.ConfirmedEmail == true)
                    {
                        if (HttpContext.Application[Request.UserHostAddress] != null)
                        {
                            HttpContext.Application.Remove(Request.UserHostAddress);
                        }

                        await SignInAsync(user, model.RememberMe);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {

                        ModelState.AddModelError("", "Confirm Email Address.");
                    }
                }
                else
                {
                    if (HttpContext.Application[Request.UserHostAddress] == null)
                    {
                        incrementalDelay = 1;
                    }
                    else
                    {
                        incrementalDelay = (int)HttpContext.Application[Request.UserHostAddress] * 2;
                    }
                    HttpContext.Application[Request.UserHostAddress] = incrementalDelay;

                    ModelState.AddModelError("", "Invalid username or password.");

                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string gender)
        {
            if (ModelState.IsValid)
            {

                pass p = new pass();
                var user = new ApplicationUser() { Email = model.Email };

                user.firstN = model.firstN;
                user.lastN = model.lastN;
                user.Address = model.Address;
                p.Password = model.Password;
                p.Email = model.Email;
                db.passs.Add(p);
                db.SaveChanges();
                user.phoneNum = model.phoneNum;
                user.Email = model.Email;
                user.ConfirmedEmail = true;
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = gender;


                if (Session["team"].ToString() != null)
                {
                    user.TeamName = Session["team"].ToString();
                }
                else
                {
                    user.TeamName = "Individual";
                }
                var result = await UserManager.CreateAsync(user, model.Password);


                if (User.IsInRole("Admin"))
                {
                    if (result.Succeeded)
                    {




                        UserManager.AddToRole(user.Id, "Employee");
                        Session["username"] = User.Identity.GetUserId();

                        user.Id = "emp" + user.Id;

                        return RedirectToAction("Index", "Customer", new { Email = user.Email });
                    }





                }


                if (result.Succeeded)
                {



                    user.ConfirmedEmail = false;
                    UserManager.AddToRole(user.Id, "Customer");
                    Session["username"] = User.Identity.GetUserId();




                    string apiKey = "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";

                    dynamic sendGridClient = new SendGridAPIClient(apiKey);

                    string Body = string.Format("Dear customer Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" </a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                    SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                    SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(user.Email);
                    Content content = new Content("text/plain", Body);
                    SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, "Registration", toEmail, content);

                    dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());









                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
                else
                {
                    AddErrors(result);
                }



            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);

            if (user != null)
            {

                if (user.Email == Email)
                {
                    user.ConfirmedEmail = true;
                    await UserManager.UpdateAsync(user);
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });

                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }

        }
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Use", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
        [AllowAnonymous]
        public ActionResult RegistrationType()
        {

            return View();
        }


        [AllowAnonymous]
        public ActionResult TeamConfirmed(string Tokenb)
        {
            if (Tokenb != null)
            {
                Session["team"] = Tokenb;
            }
            else
            {
                Session["team"] = "Individual";
            }

            return View();

        }




        [Authorize]
        public ActionResult Register2()
        {
            if (User.IsInRole("Admin"))
            {
                Session["email"] = "    ";
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
     
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register2(RegisterViewModel model, string gender, string add2, string post)
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var email1 = db.Customers.ToList().Find(x => x.EmailAdd == model.Email);
                if (email1 != null)
                {
                    Session["email"] = "EMAIL ALREADY EXISTS  PLEASE LOGIN";
                    return View();
                }
                else
                {
                    user.firstN = model.firstN;
                    user.lastN = model.lastN;
                    user.Address = model.Address + " " + add2 + " " + post;
                    user.phoneNum = model.phoneNum;
                    user.Email = model.Email;
                    user.ConfirmedEmail = true;
                    user.DateOfBirth = model.DateOfBirth;
                    user.Gender = gender;
                    user.TeamName = "";
                    c.firstName = model.firstN;
                    c.lastName = model.lastN;
                    c.Sdate = model.DateOfBirth;
                    c.EmailAdd = model.Email;
                    c.Password = model.Password;
                    c.PhoneNum = model.phoneNum;
                    c.Address = model.Address + " " + add2 + " " + post;
                    c.teamName = "";
                    c.roles = "Employee";
                    db.Customers.Add(c);
                    db.SaveChanges();
                    var result = await UserManager.CreateAsync(user, model.Password);


                    if (User.IsInRole("Admin"))
                    {
                        if (result.Succeeded)
                        {




                            UserManager.AddToRole(user.Id, "Employee");
                            Session["username"] = User.Identity.GetUserId();

                            user.Id = "emp" + user.Id;

                            return RedirectToAction("Index", "Home", new { Email = user.Email });
                        }





                    }


                    if (result.Succeeded)
                    {



                        user.ConfirmedEmail = false;
                        UserManager.AddToRole(user.Id, "Customer");
                        Session["username"] = User.Identity.GetUserId();




                        string apiKey = "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";

                        dynamic sendGridClient = new SendGridAPIClient(apiKey);

                        string Body = string.Format("Dear customer Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" </a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                        SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                        SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(user.Email);
                        Content content = new Content("text/plain", Body);
                        SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, "Registration", toEmail, content);

                        dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());









                        return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                    }
                }




            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Register3()
        {
            Session["email"] = "    ";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register3(RegisterViewModel model, string gender, string add2, string post)
        {
            if (ModelState.IsValid)
            {
                Customer c = new Customer();
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var email1 = db.Customers.ToList().Find(x => x.EmailAdd == model.Email);
                if (email1 != null)
                {
                    Session["email"] = "EMAIL ALREADY EXISTS  PLEASE LOGIN";
                    return View();
                }
                else
                {
                    user.firstN = model.firstN;
                    user.lastN = model.lastN;
                    user.Address = model.Address + " " + add2 + " " + post;
                    user.phoneNum = model.phoneNum;
                    user.Email = model.Email;
                    user.ConfirmedEmail = true;
                    user.DateOfBirth = model.DateOfBirth;
                    user.Gender = gender;
                    user.TeamName = "";
                    c.firstName = model.firstN;
                    c.lastName = model.lastN;
                    c.Sdate = model.DateOfBirth;
                    c.EmailAdd = model.Email;
                    c.Password = model.Password;
                    c.PhoneNum = model.phoneNum;
                    c.Address = model.Address + " " + add2 + " " + post;
                    c.teamName = "";
                    db.Customers.Add(c);
                    db.SaveChanges();
                    var result = await UserManager.CreateAsync(user, model.Password);


                    if (User.IsInRole("Admin"))
                    {
                        if (result.Succeeded)
                        {




                            UserManager.AddToRole(user.Id, "Employee");
                            Session["username"] = User.Identity.GetUserId();

                            user.Id = "emp" + user.Id;

                            return RedirectToAction("Index", "Home", new { Email = user.Email });
                        }





                    }


                    if (result.Succeeded)
                    {



                        user.ConfirmedEmail = false;
                        UserManager.AddToRole(user.Id, "Customer");
                        Session["username"] = User.Identity.GetUserId();




                        string apiKey = "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";

                        dynamic sendGridClient = new SendGridAPIClient(apiKey);

                        string Body = string.Format("Dear customer Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" </a>", user.UserName, Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                        SendGrid.Helpers.Mail.Email fromEmail = new SendGrid.Helpers.Mail.Email("Practice071@gmail.com");
                        SendGrid.Helpers.Mail.Email toEmail = new SendGrid.Helpers.Mail.Email(user.Email);
                        Content content = new Content("text/plain", Body);
                        SendGrid.Helpers.Mail.Mail mail = new Mail(fromEmail, "Registration", toEmail, content);

                        dynamic response = await sendGridClient.client.mail.send.post(requestBody: mail.Get());









                        return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                    }
                }




            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Use", "Account");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }






       
    }
}
#endregion