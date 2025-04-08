namespace clothes_backend.Inteface
{
    public interface IGenericRepository<T> where T:class
    {
        Task<IEnumerable<T>> get();
        Task<T> getId(int id);
        Task add(T entity);
        Task update(int id,T entity);
        Task delete(int id);
    }
}
