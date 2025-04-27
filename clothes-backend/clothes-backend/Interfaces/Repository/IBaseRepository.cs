namespace clothes_backend.Interfaces.Repository
{
    public interface IBaseRepository<T> where T:class
    {
        Task<IEnumerable<T>> get();
        Task<T> getId(int id);
        Task add(T entity);
        Task<T> update(int id,T entity);
        Task delete(int id);
        IEnumerable<T> pagination(IEnumerable<T> entity, int currentPage, int limit);
        
    }
}
