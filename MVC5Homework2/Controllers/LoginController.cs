using MVC5Homework2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Homework2.Controllers
{
    public class LoginController : Controller
    {
        [Authorize]
        // GET: Login
        public ActionResult Index()
        {

            //if (User.Identity.IsAuthenticated)
            //{
            //    var userData = GetUserDate();
            //    string strLoginID = User.Identity.Name;
            //    Response.Write("Hello: " + strLoginID);
            //}

            return RedirectToAction("Index", "Home");
        }

      
        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(LoginViewModel collection)
        {
            if (collection.Account == "patrick" && collection.Password == "12345")
            {
                string userData = "Master";
                bool isPersistent = true;
                string strUsername = collection.Account;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  strUsername,
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  isPersistent,
                  userData,
                  FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Logout()
        {
            //清除Session中的資料
            Session.Abandon();
            //登出表單驗證
            FormsAuthentication.SignOut();
            //導至登入頁
            return RedirectToAction("Index", "Home");
        }

        public string GetUserDate()
        {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            
            var userDate = ticket.UserData;
            //TempData["cookiePath"] = ticket.CookiePath;
            //TempData["expireDate"] = ticket.Expiration.ToString();
            //TempData["expired"] = ticket.Expired.ToString();
            //TempData["isPersistent"] = ticket.IsPersistent.ToString();
            //TempData["issueDate"] = ticket.IssueDate.ToString();
            //TempData["name"] = ticket.Name;
            //TempData["userData"] = ticket.UserData;
            //TempData["version"] = ticket.Version.ToString();
            return userDate;
        }

    }
}
