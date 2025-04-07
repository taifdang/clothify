namespace clothes_backend.Inteface
{
    public interface ICRUD<T>
    {
        void Add(T item);
        T GetId(T id);
        void Update(T id, T item);
        void Remove(T id);

    }
}
