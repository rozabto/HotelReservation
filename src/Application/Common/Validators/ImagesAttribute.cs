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
            if (!(value is IReadOnlyList<IFormFile> images))
                return new ValidationResult("Images Is Required");

            if (images.Count < 2)
                return new ValidationResult("Upload at least 2 images");

            foreach (var image in images)
            {
                if (!image.ContentType.StartsWith("image"))
                    return new ValidationResult("All files must be images");
            }
            return ValidationResult.Success;
        }
    }
}