namespace MVC5Homework2.Models
{
    using Attribute;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        // public customer
        public 客戶資料 資料
        {
            get
            {
                return 客戶資料;

            }
        }
        //private 客戶資料Entities db = new 客戶資料Entities();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if (客戶資料.Email == Email)
            {
                yield return new ValidationResult("error", new[] { "Email", "重複了" }); 

            }
          
        }
        
    }
 
    
    public partial class 客戶聯絡人MetaData
    {

        public int Id { get; set; }

        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression(@"\w.+\@\w.+")]
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        public string Email { get; set; }
        //[CellPhoneDataTypeAttribute]
        [CellPhoneValidationAttribute]
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
