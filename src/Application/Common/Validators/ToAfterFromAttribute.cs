using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ToAfterFromAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var to = (DateTime)value;

            var from = (DateTime)validationContext
                .ObjectInstance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttribute(typeof(ToAfterFromAttribute)) != null)
                .Select(f => f.GetValue(validationContext.ObjectInstance))
                .FirstOrDefault();

            return from.Date > to.Date
                ? new ValidationResult("'To' must be after 'From'")
                : ValidationResult.Success;
        }
    }
}
