using System;
using Common;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Common
{
    public class MemoryService : IMemoryService
    {
        private readonly MemoryCache _cache;

        public MemoryService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(1)
            });
        }

        public T GetValue<T>(string key) =>
            _cache.TryGetValue(key, out var result) ? (T)result : default;

        public void SetValue<T>(string key, T value) =>
            _cache.Set(key, value, DateTimeOffset.Now.AddMinutes(10));
    }
}