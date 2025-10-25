using Infrastructure.Data;
using Infrastructure.Interface;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class, new()
{
    protected DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entity)
    {   
         await _dbSet.AddRangeAsync(entity); 
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public void Attach(TEntity entity)
    {
        _dbSet.Attach(entity);  
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        IQueryable<TEntity> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(filter);
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<TResult?> GetByIdAsync<TResult>(
         Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.Select(selector).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return await(filter == null
            ? _dbSet.AsNoTracking().ToListAsync()
            : _dbSet.AsNoTracking().Where(filter).ToListAsync());
    }

    public async Task<List<TResult>> GetListAsync<TResult>(
        Expression<Func<TEntity, bool>>? filter = null, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Expression<Func<TEntity, TResult>> selector = null)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (include != null)
        {
            query = include(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (selector != null)
            return await query.Select(selector).ToListAsync();

        return await query.Cast<TResult>().ToListAsync();
    }

    public async Task<Pagination<TResult>> ToPagination<TResult>(
        int pageIndex, 
        int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Expression<Func<TEntity, object>>? orderBy = null, 
        bool ascending = true, 
        Expression<Func<TEntity, TResult>> selector = null)
    {

        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (include != null)
        {
            query = include(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        orderBy ??= x => EF.Property<object>(x, "Id");

        query = ascending 
            ? query.OrderBy(orderBy) 
            : query.OrderByDescending(orderBy);

        var projectedQuery = query.Select(selector);

        var result = await Pagination<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);

        return result;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }
}
