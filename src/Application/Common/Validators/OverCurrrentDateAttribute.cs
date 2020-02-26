using System;
using System.ComponentModel.DataAnnotations;
using Common;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class OverCurrrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var dateTime = validationContext.GetRequiredService<IDateTime>();
            return dateTime.Now > (DateTime)value ?
                new ValidationResult("Date provided must be over current date")
                : ValidationResult.Success;
        }
    }
}