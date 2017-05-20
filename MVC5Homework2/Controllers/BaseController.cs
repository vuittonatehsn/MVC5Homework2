using ClosedXML.Excel;
using MVC5Homework2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public IEnumerable<SelectListItem> myOccupationSelectList
        {
            get
            {
                var data     = new SelectList(db.客戶聯絡人.Select(w => new { Value = w.職稱, Text = w.職稱 }), "Value", "Text");
                return data;
            }
        }

        //[HttpPost]
        public ActionResult HasData()
        {
            JObject jo = new JObject();
            bool result = !db.客戶資料.Count().Equals(0);
            jo.Add("Msg", result.ToString());
            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }


        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "customer",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");
            
            ExportExcelEventHandler( dt, exportFileName, exportFileName);
            //return File(ControllerContext.HttpContext.Response.OutputStream, "application/vnd.ms-excel", "exportFileName");
            
            return File("~/App_Data/"+ exportFileName, "application/vnd.ms-excel");
        }

        private JArray GetExportData()
        {
            var query = db.客戶資料
                          .OrderBy(x => x.Id);
                          //.ThenBy(x => x.)
                          //.ThenBy(x => x.Zip);

            JArray jObjects = new JArray();

            foreach (var item in query)
            {
                var jo = new JObject();
                jo.Add("Id", item.Id);
                jo.Add("客戶名稱", item.客戶名稱);
                jo.Add("地址", item.地址);
                jo.Add("電話", item.電話);
                jObjects.Add(jo);
            }
            return jObjects;
        }

        private void ExportExcelEventHandler( DataTable ExportData, string FileName, string SheetName)
        {
            try
            {
                var workbook = new XLWorkbook();

                if (ExportData != null)
                {
                    this.ControllerContext.HttpContext.Response.Clear();

                    // 編碼
                    this.ControllerContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;

                    // 設定網頁ContentType
                    this.ControllerContext.HttpContext.Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // 匯出檔名
                    var browser = this.ControllerContext.HttpContext.Request.Browser.Browser;
                    var exportFileName = browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase)
                        ? FileName
                        : HttpUtility.UrlEncode(FileName, Encoding.UTF8);

                    this.ControllerContext.HttpContext.Response.AddHeader(
                        "Content-Disposition",
                        string.Format("attachment;filename={0}", exportFileName));

                    // Add all DataTables in the DataSet as a worksheets
                    workbook.Worksheets.Add(ExportData, SheetName);
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    workbook.SaveAs(memoryStream);
                    //    memoryStream.WriteTo(this.ControllerContext.HttpContext.Response.OutputStream);
                    //    memoryStream.Close();
                    //}
                    workbook.SaveAs(@"~/App_Data/" + exportFileName);

            }
            workbook.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


       

    }
}