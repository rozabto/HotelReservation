using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly bool isEnvDev;

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
            isEnvDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        }

        public async Task<IEnumerable<string>> SaveImages(IReadOnlyList<IFormFile> images)
        {
            var names = new List<string>(images.Count);

            if (!isEnvDev)
            {
                foreach (var image in images)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                        PublicId = Guid.NewGuid().ToString("N"),
                        Format = image.ContentType.Replace("image/", "")
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    names.Add(uploadResult.SecureUri.ToString());
                }
            }
            else
            {
                if (!Directory.Exists("wwwroot/img/products"))
                    Directory.CreateDirectory("wwwroot/img/products");

                foreach (var image in images)
                {
                    var name = "img/products/" + Guid.NewGuid().ToString("N") + '.'
                        + image.ContentType.Replace("image/", "");

                    using var file = File.Create("wwwroot/" + name);
                    await image.CopyToAsync(file);

                    names.Add(name);
                }
            }

            return names;
        }
    }
}