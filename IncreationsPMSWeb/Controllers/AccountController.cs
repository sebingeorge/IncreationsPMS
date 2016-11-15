using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using Microsoft.AspNet.Identity;


using IncreationsPMSDomain;
using System.Web.Security;
using System.Configuration;
using IncreationsPMSDAL;

using System.Security.Cryptography;
using System.Text;

namespace IncreationsPMSWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string saltpassword = String.Concat(salt, model.Password);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltpassword, "sha1");

            var user = (new UserRepository()).GetUserByUserNameAndPassword(model.Username, hashedPassword);

            if (user == null || user.UserName == null)
            {
                ModelState.AddModelError("", "Invalid User name or Password. Please try again.");
                return View(model);
            }
            else
            {
                SignIn(user, model.OrganizationId, model.RememberMe, HttpContext.Response.Cookies);
                return RedirectToLocal(returnUrl);
            }
        }

        private void SignIn(User user, int OrganizationId, bool isPersistent, HttpCookieCollection cookiecollection)
        {
            var userData = String.Format("{0}|{1}|{2}|{3}",
                user.UserId, user.UserName, user.UserPassword, user.UserEmail);
            var ticket = new FormsAuthenticationTicket(1, userData, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30), isPersistent, userData, FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            cookiecollection.Add(authCookie);

            HttpCookie userCookie = new HttpCookie("userCookie") { HttpOnly = true };
            userCookie.Values.Add("UserId", user.UserId.ToString());
            userCookie.Values.Add("UserName", user.UserName.ToString());
            userCookie.Values.Add("publicKey", ConvertPasswordToPublicKey(user.UserPassword));
            userCookie.Values.Add("UserEmail", user.UserEmail.ToString());
            cookiecollection.Add(userCookie);
            Session.Add("user", userCookie);

            UserRepository repo = new UserRepository();
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            repo.InsertLoginHistory(user, Session.SessionID.ToString(), ip, OrganizationId.ToString());
            //return userCookie;
        }
        public string ConvertPasswordToPublicKey(string encrytedpwd)
        {
            return GetMD5CryptoString(encrytedpwd);
        }
        protected string GetMD5CryptoString(string original)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(original);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
        //

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}