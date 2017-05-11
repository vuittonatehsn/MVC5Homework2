using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5Homework2.Attribute
{
    public class CellPhoneValidationAttribute: ValidationAttribute
    {
        public CellPhoneValidationAttribute(): base("Validation客製化格式: ( e.g. 0911-111111 )")
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                var result = value.ToString();
                Regex regex = new Regex(@"\d{4}-\d{6}");
                Match match = regex.Match(result);
                if (!match.Success)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;  
        }
    }
}