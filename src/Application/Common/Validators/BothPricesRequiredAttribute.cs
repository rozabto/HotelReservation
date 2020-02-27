using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BothPricesRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var price = (decimal?)value;

            if (price.HasValue)
            {
                var fields = validationContext
                    .ObjectInstance
                    .GetType()
                    .GetProperties()
                    .Where(f => f.GetCustomAttribute(typeof(BothPricesRequiredAttribute)) != null)
                    .Select(f => f.GetValue(validationContext.ObjectInstance));

                if (fields.Any(f => !((decimal?)f).HasValue))
                    return new ValidationResult("Both Fields require a value");
            }
            return ValidationResult.Success;
        }
    }
}
