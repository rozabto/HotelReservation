using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FromBeforeToAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var from = (DateTime)value;

            var to = (DateTime)validationContext
                .ObjectInstance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttribute(typeof(ToAfterFromAttribute)) != null)
                .Select(f => f.GetValue(validationContext.ObjectInstance))
                .FirstOrDefault();

            return from.Date > to.Date
                ? new ValidationResult("'From' must be before 'To'")
                : ValidationResult.Success;
        }
    }
}
