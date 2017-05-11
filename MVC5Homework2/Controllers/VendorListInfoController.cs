using MVC5Homework2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC5Homework2.Controllers
{
    public class VendorListInfoController : Controller
    {
        客戶資料Entities vendorDB = new 客戶資料Entities();
        // GET: VendorListInfo
        public ActionResult VendorList()
        {
            var vendorList = vendorDB.MemberView.Take(20).Select(r => new VendorListInfoViewModel
            {
                客戶名稱 = r.客戶名稱,
                聯絡人數量 =  r.聯絡人數量.Value,
                銀行帳戶數量 = r.銀行帳戶數量.Value

            });
            return View(vendorList);
        }

        // GET: VendorListInfo/Details/5
        public ActionResult Details(string 客戶名稱)
        {
            if (string.IsNullOrEmpty(客戶名稱))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var memberView = vendorDB.Database.SqlQuery<VendorListInfoViewModel>("SELECT * FROM MemberView WHERE 客戶名稱 = @p0", 客戶名稱).FirstOrDefault();
            if (memberView == null)
            {
                return HttpNotFound();
            }
            return View(memberView);
        }

        [HttpPost]
        public ActionResult Search(string searchKey)
        {
            var result = vendorDB.usp_SearchVendor(searchKey).ToList();
            return View(result);
        }


    }
}
