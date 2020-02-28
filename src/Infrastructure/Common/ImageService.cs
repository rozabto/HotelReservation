using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class ImageService : IImageService
    {
        public async Task<IEnumerable<string>> SaveImages(IReadOnlyList<IFormFile> images)
        {
            if (!Directory.Exists("wwwroot/img/products"))
                Directory.CreateDirectory("wwwroot/img/products");

            var names = new List<string>(images.Count);

            foreach (var image in images)
            {
                var name = "img/products/" + Guid.NewGuid().ToString("N") + '.'
                    + image.ContentType.Replace("image/", "");

                using var file = File.Create("wwwroot/" + name);
                await image.CopyToAsync(file);

                names.Add(name);
            }

            return names;
        }
    }
}