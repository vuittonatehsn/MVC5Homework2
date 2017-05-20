using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Homework2.Models;
using PagedList;
using System.Reflection;
using System.Linq.Expressions;
using System.Web.Routing;

namespace MVC5Homework2.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

        [OutputCache(Duration = 5, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        // GET: 客戶資料
        public ActionResult Index(int page=1, string sorting="Id")
        {
            int currentPage = page < 1 ? 1 : page;
            var result = repo.All().OrderByProperty(sorting);
            var final =  result.ToPagedList(currentPage, 10);
            ViewBag.myList = mySelectList;

            //ViewBag.客戶分類list = new SelectList(new string[] { "A", "B", "C" }, result.Select(e=>e.客戶分類)).ToList();
            return View(final); //.Where(w => w.IsDeleted != true)
        }

        [OutputCache(Duration = 5, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        [HttpPost]
        public ActionResult Index(string myList, int page = 1)
        {
            int currentPage = page < 1 ? 1 : page;
            var result = repo.GetListBySearch(myList);
            ViewBag.myList = mySelectList;
            return View(result.ToPagedList(currentPage, 10));
        }

        public ActionResult TestIndex(string myList, int page = 1, string sorting = "Id")
        {
            Expression<Func<客戶資料, string>> c = x => x.傳真;
            
            int currentPage = page < 1 ? 1 : page;

            var result = repo.All();
            var dataSort = result.OrderByProperty("傳真");
            ViewBag.myList = mySelectList;
            return View(result.ToPagedList(currentPage, 10));
        }

        




        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.GetById(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.GetById(id.Value);
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
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶資料 = repo.GetById(id);
            if(TryUpdateModel(客戶資料, new string[] { "客戶名稱", "統一編號", "電話", "傳真", "地址" , "Email"})){
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.GetById(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 customerInfo = repo.GetById(id);
            //客戶資料 客戶資料 = repo.客戶資料.Find(id);
            //IQueryable<客戶聯絡人> 聯絡人列表 = repo.客戶聯絡人.Where(r=>r.客戶Id == id);
            //IQueryable<客戶銀行資訊> 銀行資訊列表 = repo.客戶銀行資訊.Where(r => r.客戶Id == id);
            //聯絡人列表.ToList().ForEach(w => w.IsDeleted = true);
            //銀行資訊列表.ToList().ForEach(w => w.IsDeleted = true);
            repo.Delete(customerInfo);
            //db.客戶銀行資訊.Where(r => r.客戶Id == id).ToList().ForEach(w => w.IsDeleted = true);
            //db.客戶資料.Find(id).IsDeleted = true;
            //db.客戶聯絡人.RemoveRange(聯絡人列表);
            //db.客戶銀行資訊.RemoveRange(銀行資訊列表);
            //db.客戶資料.Remove(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult GetJson()
        {
            repo.UnitOfWork.LazyLoadingEnabled = false;
            return Json(repo.All(), JsonRequestBehavior.AllowGet);
        }

        
    }
}
