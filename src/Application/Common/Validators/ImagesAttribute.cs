using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ImagesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var image in (IReadOnlyList<IFormFile>)value)
            {
                if (!image.ContentType.StartsWith("image"))
                    return new ValidationResult("All files must be images");
            }
            return ValidationResult.Success;
        }
    }
}