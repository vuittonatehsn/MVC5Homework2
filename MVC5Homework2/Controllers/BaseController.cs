using MVC5Homework2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Homework2.Controllers
{
    public abstract class BaseController : Controller
    {
        protected 客戶資料Entities db = new 客戶資料Entities();

        public IEnumerable<SelectListItem> mySelectList
        {
            get {
                List<SelectListItem> myList = new List<SelectListItem>();
                var data = new[]{
                 new SelectListItem{ Value="A",Text="A"},
                 new SelectListItem{ Value="B",Text="B"},
                 new SelectListItem{ Value="C",Text="C"}

                };
                
                return data.ToList();
            }
        }

    }
}