using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class ProductionImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ProductionImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
        }

        public async Task<IEnumerable<string>> SaveImages(IReadOnlyList<IFormFile> images)
        {
            var names = new List<string>(images.Count);

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

            return names;
        }
    }
}
