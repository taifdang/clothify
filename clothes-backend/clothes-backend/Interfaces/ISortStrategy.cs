namespace clothes_backend.Interfaces
{
    public interface ISortStrategy<T>
    {
        IEnumerable<T> Sort(IEnumerable<T> values);
    }
}
