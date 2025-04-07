using clothes_backend.Inteface;
using clothes_backend.Models;

namespace clothes_backend.Service
{
    public class SortByPercent : ISortStrategy<Products>
    {
        public IEnumerable<Products> Sort(IEnumerable<Products> values)
        {
            return values.OrderBy(p => p.old_price % p.price > 0).ToList();
        }
    }
}
