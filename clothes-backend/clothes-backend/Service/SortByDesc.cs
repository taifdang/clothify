using clothes_backend.Inteface;
using clothes_backend.Models;

namespace clothes_backend.Service
{
    public class SortByDesc : ISortStrategy<Products>
    {
        public IEnumerable<Products> Sort(IEnumerable<Products> values)
        {
            return values.OrderByDescending(p => p.price).ToList();
        }
    }
}
