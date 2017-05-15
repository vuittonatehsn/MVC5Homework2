using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5Homework2.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => !p.IsDeleted).Take(20);
        }

        public 客戶資料 GetById(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<客戶資料> GetListBySearch(string 客戶分類篩選)
        {
            IQueryable<客戶資料> result = this.All().Where(w=>w.客戶分類 == 客戶分類篩選).OrderByDescending(p => p.Id);
            
            return result;
        }
        public override void Delete(客戶資料 entity)
        {
            this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            entity.IsDeleted = true;
        }

        

    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}