using clothes_backend.Interfaces;
using clothes_backend.Models;

namespace clothes_backend.Service
{
    public class SortByAsc : ISortStrategy<Products>
    {
        public IEnumerable<Products> Sort(IEnumerable<Products> values)
        {
            return values.OrderBy(p => p.price).ToList();
        }
    }
}

