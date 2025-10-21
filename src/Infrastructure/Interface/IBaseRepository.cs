using Infrastructure.Models;
using System.Linq.Expressions;

namespace Infrastructure.Interface;

public interface IBaseRepository<TEnitty> where TEnitty : class, new()
{
    Task<TEnitty?> FirstOrDefaultAsync(
      Expression<Func<TEnitty, bool>> filter,
      Func<IQueryable<TEnitty>, IQueryable<TEnitty>>? include = null);
    Task<bool> AnyAsync(Expression<Func<TEnitty, bool>> filter);

    Task<TResult?> GetByIdAsync<TResult>(Expression<Func<TEnitty, TResult>> selector, Expression<Func<TEnitty, bool>>? filter = null);
    Task<TEnitty> GetAsync(Expression<Func<TEnitty, bool>>? filter = null, CancellationToken cancellationToken = default);
    
   
    Task AddAsync(TEnitty entity);
    Task AddRangeAsync(IEnumerable<TEnitty> entity);

    void Update(TEnitty entity);
    void UpdateRange(IEnumerable<TEnitty> entities);

    void Delete(TEnitty entity);
    void DeleteRange(IEnumerable<TEnitty> entities);

    Task<Pagination<TResult>> ToPagination<TResult>(
        int pageIndex,
        int pageSize,
        Expression<Func<TEnitty, bool>>? filter = null,
        Func<IQueryable<TEnitty>, IQueryable<TEnitty>>? include = null,
        Expression<Func<TEnitty, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<TEnitty, TResult>> selector = null);

    Task<List<TResult>> GetListAsync<TResult>(
      Expression<Func<TEnitty, bool>>? filter = null,
      Func<IQueryable<TEnitty>, IQueryable<TEnitty>>? include = null,
      Expression<Func<TEnitty, TResult>> selector = null);
}
