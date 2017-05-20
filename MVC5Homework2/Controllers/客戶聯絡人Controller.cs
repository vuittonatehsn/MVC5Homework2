using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Homework2.Models;
using System.Linq.Dynamic;
using PagedList;

namespace MVC5Homework2.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        [OutputCache(Duration = 5, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        // GET: 客戶聯絡人
        public ActionResult Index(int page = 1, string sorting = "Id")
        {
            int currentPage = page < 1 ? 1 : page;
            var data = repo.GetAllWith客戶資料().AsQueryable().OrderBy(sorting);
            var final = data.ToPagedList(currentPage, 10);

            ViewBag.Occupation = myOccupationSelectList;
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(w => w.IsDeleted != true);
            return View(final);
        }

        public ActionResult Include(int id)
        {
   
            var data = repo.FindBy(o => o.客戶Id == id, "客戶資料");
            return View(data.ToList());
        }
        [HttpPost]
        public ActionResult Include(int id, BatchUpdateViewModel[] testList)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in testList)
                {
                    var final = repo.GetById(item.Id);
                    final.職稱 = item.職稱;
                    final.電話 = item.電話;
                    final.手機 = item.手機;
                }
                repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;
                repo.UnitOfWork.Commit();
                TempData["success"] = "更新成功";
                //return RedirectToAction("Include", id);
                //var result = repo.FindBy(o => o.客戶Id == id, "客戶資料");
                //return PartialView(result.ToList());
            }

            var data = repo.FindBy(o => o.客戶Id == id, "客戶資料");
            ViewData.Model = data;
           
            return PartialView();


        }





        [HttpPost]
        public ActionResult Index(string Occupation, string sorting = "Id")
        {
            var data = repo.GetAllWith客戶資料ByOccupation(Occupation).OrderBy(sorting);
            ViewBag.Occupation = myOccupationSelectList;
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.職稱 == occupation).Where(w => w.IsDeleted != true);
            return View(data.ToList());
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.GetById(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.GetById(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(客戶聯絡人 客戶聯絡人) //[Bind(Exclude = "客戶資料")] 
        {
            if (ModelState.IsValid)
            {
                repo.Update(客戶聯絡人);    
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repo.GetById(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repo.GetById(id);
            repo.Delete(客戶聯絡人);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        
    }
}
