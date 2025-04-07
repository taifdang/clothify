using clothes_backend.Inteface;

namespace clothes_backend.Service
{
    public class SortService<T>
    {
        private readonly ISortStrategy<T> _sortStrategy;
        public SortService(ISortStrategy<T> sortStrategy)
        {
            _sortStrategy = sortStrategy;
        }
        public IEnumerable<T> GetList(IEnumerable<T> values)
        {
            return _sortStrategy.Sort(values);
        }
    }
}
