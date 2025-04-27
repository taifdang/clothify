using Microsoft.Extensions.Caching.Memory;

namespace clothes_backend.Interfaces.Service
{
    public interface ICacheService
    {   
        bool isCached(string cacheKey);
        T? Get<T>(string cacheKey);
        void Set<T>(string cacheKey, T value, TimeSpan? duration = null);
        void Remove(string cacheKey);
    }
}
