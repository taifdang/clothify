using clothes_backend.Inteface;
using clothes_backend.Models;

namespace clothes_backend.Service
{
    public class SortByDefault : ISortStrategy<Products>
    {
        public IEnumerable<Products> Sort(IEnumerable<Products> values)
        {
            return values;
        }
    }
}
