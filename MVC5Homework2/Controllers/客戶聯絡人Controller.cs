using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Homework2.Models;

namespace MVC5Homework2.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        [OutputCache(Duration = 5, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        // GET: 客戶聯絡人
        public ActionResult Index()
        {
            var data = repo.GetAllWith客戶資料();
            ViewBag.Occupation = myOccupationSelectList;
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(w => w.IsDeleted != true);
            return View(data);
        }

        public ActionResult Include(int id)
        {
   
            var data = repo.FindBy(o => o.客戶Id == id, "客戶資料");
            return View(data.ToList());
        }
        [HttpPost]
        public ActionResult Include(int id, 客戶聯絡人[] items)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in items)
                {
                    var final = repo.FindBy(e => e.Id == item.Id, "").FirstOrDefault();
                    final.職稱 = item.職稱;
                    final.電話 = item.電話;
                    final.手機 = item.手機;
                }
                repo.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;
                repo.UnitOfWork.Commit();
                
                return RedirectToAction("Include", id);

            }

            var data = repo.FindBy(o => o.客戶Id == id, "客戶資料");
            ViewData.Model = data;
            return View("Include");
        }





        [HttpPost]
        public ActionResult Index(string Occupation)
        {
            var data = repo.GetAllWith客戶資料ByOccupation(Occupation);
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
