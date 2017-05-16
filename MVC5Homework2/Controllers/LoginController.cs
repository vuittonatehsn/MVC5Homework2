using MVC5Homework2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC5Homework2.Controllers
{
    public class LoginController : BaseController
    {
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

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
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
            byte[] source = Encoding.Default.GetBytes(collection.Password);//將字串轉為Byte[]
            byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
            string result = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串
            //Response.Write("SHA256加密:  " + result);//輸出結果

            var data = db.客戶資料.Where(e => e.Account == collection.Account && e.Password == result).SingleOrDefault();

            if (data != null)
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

        public ActionResult Manage()
        {
            var byRole = User.IsInRole("Master");

            客戶資料 客戶資料 = repo.GetByAccount(User.Identity.Name);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

     

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(int id, FormCollection form)
        {
            var 客戶資料 = repo.GetById(id);
            if (TryUpdateModel(客戶資料, new string[] { "客戶名稱", "統一編號", "電話", "傳真", "地址", "Email" }))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            } 
            return View(客戶資料); 
        }

    }
}
