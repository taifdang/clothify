using System.Linq.Expressions;

namespace clothes_backend.Interfaces.Repository
{
    public interface IBaseRepository<T> where T:class
    {
        Task<List<T>> GetAllBase();
        Task<T> GetIdBase(int id);
        Task AddBase(T entity);
        Task<T> UpdateBase(int id,T entity);
        Task DeleteBase(T item);
        IEnumerable<T> PaginationBase(IEnumerable<T> entity, int currentPage, int limit);
        Task<T> FindBase(Expression<Func<T,bool>> condition);
       


    }
}
