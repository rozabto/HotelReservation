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
            var names = new List<string>(images.Count);
            foreach (var image in images)
            {
                var name = Guid.NewGuid().ToString("N") + '.'
                    + image.ContentType.Replace("image/", "");

                using var file = File.Create("wwwroot/" + name);
                await image.CopyToAsync(file);

                names.Add(name);
            }
            return names;
        }
    }
}
