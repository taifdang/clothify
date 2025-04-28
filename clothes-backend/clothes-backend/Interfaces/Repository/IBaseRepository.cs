namespace clothes_backend.Interfaces.Repository
{
    public interface IBaseRepository<T> where T:class
    {
        Task<List<T>> GetAllBase();
        Task<T> GetIdBase(int id);
        Task AddBase(T entity);
        Task<T> UpdateBase(int id,T entity);
        Task DeleteBase(int id);
        IEnumerable<T> PaginationBase(IEnumerable<T> entity, int currentPage, int limit);
        
    }
}
