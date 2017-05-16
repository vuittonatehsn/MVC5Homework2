using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Homework2.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public IQueryable<客戶聯絡人> GetAllWith客戶資料()
        {
            return this.FindBy(w => w.IsDeleted != true , "客戶資料");
        }

        public IQueryable<客戶聯絡人> GetAllWith客戶資料ByOccupation(string occupation)
        {
            return this.FindBy(w => w.IsDeleted != true, "客戶資料").Where(客 => 客.職稱 == occupation);
        }
        public 客戶聯絡人 GetById(int id)
        {
            return All().FirstOrDefault(p => p.Id == id);
        }
        public override void Delete(客戶聯絡人 entity)
        {
            this.UnitOfWork.Context.Configuration.ValidateOnSaveEnabled = false;

            entity.IsDeleted = true;
        }

        public void Update(客戶聯絡人 客戶聯絡人)
        {
             this.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
        }


}

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}