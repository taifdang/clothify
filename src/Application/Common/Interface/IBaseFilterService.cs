using System.Linq.Expressions;

namespace Application.Common.Interface;

public interface IBaseFilterService<TEntity>
{
    public void AddFilter(Expression<Func<TEntity, bool>> filter);
    public Expression<Func<TEntity, bool>> BuildCombinedFilter();
    void ClearFilters();

}
