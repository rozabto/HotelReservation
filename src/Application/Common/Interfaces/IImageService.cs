using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<IEnumerable<string>> SaveImages(IReadOnlyList<IFormFile> images);
    }
}