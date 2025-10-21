using Application.Common.Interface;
using Shared.Extensions;
using System.Linq.Expressions;

namespace Application.Services;
//ref: https://blog.elmah.io/expression-trees-in-c-building-dynamic-linq-queries-at-runtime/
//ref: https://www.codeproject.com/articles/Combining-expressions-to-dynamically-append-criter#comments-section
//ref: https://coding.abel.nu/2013/01/merging-expression-trees-to-reuse-in-linq-queries/

public abstract class BaseFilterService<TEntity> : IBaseFilterService<TEntity>
{
    protected readonly List<Expression<Func<TEntity, bool>>> _filters = new();

    public void AddFilter(Expression<Func<TEntity, bool>> filter)
        => _filters.Add(filter);

    public void ClearFilters() => _filters.Clear();

    public Expression<Func<TEntity, bool>> BuildCombinedFilter()
    {
        if (_filters == null || !_filters.Any())
            return x => true;

        var combined = _filters[0];

        for (int i = 1; i < _filters.Count; i++)
        {
            combined = combined.AndAlso(_filters[i]);
        }

        return combined;
    }
}
