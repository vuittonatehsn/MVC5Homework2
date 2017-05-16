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
            
            ExportExcelEventHandler(this.ControllerContext, dt, exportFileName, exportFileName);
            //return File(ControllerContext.HttpContext.Response.OutputStream, "documents.zip", "application/zip");
            return File(ControllerContext.HttpContext.Response.OutputStream, "application/vnd.ms-excel", exportFileName);
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

        private void ExportExcelEventHandler(ControllerContext context, DataTable ExportData, string FileName, string SheetName)
        {
            try
            {
                var workbook = new XLWorkbook();

                if (ExportData != null)
                {
                    context.HttpContext.Response.Clear();

                    // 編碼
                    context.HttpContext.Response.ContentEncoding = Encoding.UTF8;

                    // 設定網頁ContentType
                    context.HttpContext.Response.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // 匯出檔名
                    var browser = context.HttpContext.Request.Browser.Browser;
                    var exportFileName = browser.Equals("Firefox", StringComparison.OrdinalIgnoreCase)
                        ? FileName
                        : HttpUtility.UrlEncode(FileName, Encoding.UTF8);

                    context.HttpContext.Response.AddHeader(
                        "Content-Disposition",
                        string.Format("attachment;filename={0}", exportFileName));

                    // Add all DataTables in the DataSet as a worksheets
                    workbook.Worksheets.Add(ExportData, SheetName);
                    using (var memoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(memoryStream);
                        memoryStream.WriteTo(context.HttpContext.Response.OutputStream);
                        memoryStream.Close();
                    }
                    //workbook.SaveAs("C:\\ASPNET_MVC5\\Project\\MVC5Homework2\\MVC5Homework2\\App_Data" + exportFileName);

            }
            workbook.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        // Create a salted password given the salt value.
        private byte[] CreateSaltedPassword(byte[] saltValue, byte[] unsaltedPassword)
        {
            // Add the salt to the hash.
            byte[] rawSalted = new byte[unsaltedPassword.Length + saltValue.Length];
            unsaltedPassword.CopyTo(rawSalted, 0);
            saltValue.CopyTo(rawSalted, unsaltedPassword.Length);

            //Create the salted hash.         
            SHA1 sha1 = SHA1.Create();
            byte[] saltedPassword = sha1.ComputeHash(rawSalted);

            // Add the salt value to the salted hash.
            byte[] dbPassword = new byte[saltedPassword.Length + saltValue.Length];
            saltedPassword.CopyTo(dbPassword, 0);
            saltValue.CopyTo(dbPassword, saltedPassword.Length);

            return dbPassword;
        }

        // Compare the hashed password against the stored password.
        private bool ComparePasswords(byte[] storedPassword, byte[] hashedPassword)
        {
            if (storedPassword == null || hashedPassword == null || hashedPassword.Length != storedPassword.Length - 50)
                return false;

            // Get the saved saltValue.
            byte[] saltValue = new byte[50];
            int saltOffset = storedPassword.Length - 50;
            for (int i = 0; i < 50; i++)
                saltValue[i] = storedPassword[saltOffset + i];

            byte[] saltedPassword = CreateSaltedPassword(saltValue, hashedPassword);

            // Compare the values.
            return CompareByteArray(storedPassword, saltedPassword);
        }

        // Compare the contents of two byte arrays.
        private bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }


    }
}