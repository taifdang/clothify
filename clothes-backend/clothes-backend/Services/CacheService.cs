using clothes_backend.Interfaces.Service;
using Microsoft.Extensions.Caching.Memory;

namespace clothes_backend.Service
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }     
        public virtual T? Get<T>(string cacheKey)
        {
            _logger.LogInformation($"[CACHE] Get key: {cacheKey}");
            return _memoryCache.TryGetValue(cacheKey, out T value) ? value : default;
        }

        public virtual bool isCached(string cacheKey)
        {
            var cache = _memoryCache.TryGetValue(cacheKey, out _);
            _logger.LogInformation("[CACHE] Check key: {0} | status: {1} ", cacheKey, cache);
            return cache;
        }

        public virtual void Remove(string cacheKey)
        {
            _logger.LogWarning($"[CACHE] Remove key: {cacheKey}");
            _memoryCache.Remove(cacheKey);
        }

        public virtual void Set<T>(string cacheKey, T value, TimeSpan? duration = null)
        {
            //absoluteExp = time expired
            //slidingExp = last accessed => restart
            _logger.LogInformation($"[CACHE] Set key: {cacheKey}");
            _memoryCache.Set(cacheKey,value,new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(duration ?? TimeSpan.FromMinutes(30))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                );
        }
    }
}
