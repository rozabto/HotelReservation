using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PriceRequiredAttribute : ValidationAttribute
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
                    .Where(f => f.GetCustomAttribute(typeof(PriceRequiredAttribute)) != null)
                    .Select(f => f.GetValue(validationContext.ObjectInstance));

                if (fields.All(f => ((decimal?)f).HasValue))
                    return new ValidationResult("You can't have both fields with a price");
            }
            return ValidationResult.Success;
        }
    }
}
