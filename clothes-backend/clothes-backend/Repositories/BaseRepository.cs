using clothes_backend.Data;
using clothes_backend.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseContext _db;
        protected readonly DbSet<T> _dbSet;
        public BaseRepository(DatabaseContext db)
        {
            this._db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual async Task add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task delete(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if(item != null)
            {
               _dbSet.Remove(item);
               await _db.SaveChangesAsync();             
            }
        }
        public virtual async Task<IEnumerable<T>> get()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> getId(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IEnumerable<T> pagination(IEnumerable<T> entity, int currentPage, int limit)
        {       
            int skip = (currentPage - 1) * limit;
            entity =  entity.Skip(skip).Take(limit);
            return entity;
        }

        public virtual async Task<T> update(int id, T entity)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null) throw new NotImplementedException();
            _db.Entry(item).CurrentValues.SetValues(entity);//same value,type
            return entity;
        }
        
    }
}
