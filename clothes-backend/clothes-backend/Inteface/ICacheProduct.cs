using clothes_backend.Models;

namespace clothes_backend.Inteface
{
    public interface ICacheProduct
    {
        Task<Dictionary<int, Products>> getCacheProduct(string cacheKey);
    }
}
