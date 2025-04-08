using clothes_backend.Inteface;
using clothes_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DatabaseContext _db;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(DatabaseContext db)
        {
            this._db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual Task add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task delete(int id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> get()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual Task<T> getId(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task update(int id, T entity)
        {
            throw new NotImplementedException();
        }
    }
}
