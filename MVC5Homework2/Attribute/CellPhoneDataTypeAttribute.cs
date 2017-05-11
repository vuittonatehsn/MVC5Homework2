using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5Homework2.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public sealed class CellPhoneDataTypeAttribute: DataTypeAttribute
    {
     
        private static Regex _regex = new Regex(@"\d{4}-\d{6}", RegexOptions.IgnoreCase);
        public CellPhoneDataTypeAttribute() : base(DataType.Text)
        {
            ErrorMessage = "客製化沒錯!!! 格式: ( e.g. 0911-111111 )";

        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string result = value as string;
            return result != null && _regex.Match(result).Length > 0;
        }
    }
}