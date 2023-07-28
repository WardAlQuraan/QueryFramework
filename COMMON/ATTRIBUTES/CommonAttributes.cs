using COMMON.EXCEPTION_RESPONSE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.ATTRIBUTES
{
    [AttributeUsage(AttributeTargets.Class ,Inherited = true)]
    public class SoftDeleteAttribute : Attribute
    {

    }

    public class RequiredFamily :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is null || (value is string && string.IsNullOrEmpty(value.ToString()))) 
            {
                throw new FamilyValidationException($"{validationContext.DisplayName} is required");
            }

            var result = ValidationResult.Success;

            // call overridden method.
            if (!IsValid(value))
            {
                string[]? memberNames = validationContext.MemberName is { } memberName
                    ? new[] { memberName }
                    : null;
                result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return result;
        }


    }


    public class IntBoolValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int || value is long)
            {
                if(Convert.ToInt64(value) != 0  && Convert.ToInt64(value) != 1)
                    throw new FamilyValidationException($"{validationContext.DisplayName} must be 0 or 1");
            }else if(value is not null)
            {
                throw new FamilyValidationException($"{validationContext.DisplayName} must be 0 or 1");
            }

            return ValidationResult.Success;

            // call overridden method.
        }


    }
}
