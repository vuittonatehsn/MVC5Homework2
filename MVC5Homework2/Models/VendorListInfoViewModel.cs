using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC5Homework2.Models
{
    public class VendorListInfoViewModel
    {
        [DisplayName("客戶名稱")]
        public string 客戶名稱 { get; set; }
        [DisplayName("聯絡人數量")]
        public int 聯絡人數量 { get; set; }
        [DisplayName("銀行帳戶數量")]
        public int 銀行帳戶數量 { get; set; }
    }
}