using clothes_backend.DTO;
using clothes_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repository
{
    public class ProductRepository : GenericRepository<Products>
    {
        public ProductRepository(DatabaseContext db) : base(db)
        {
        }
        public override async Task<IEnumerable<Products>> get()
        {          
            var products = await _db.products.ToListAsync();
            return products;
        }
        public override async Task<Products> getId(int id)
        {
            return await _db.products.FirstOrDefaultAsync(x => x.id == id);
        }
        public override Task add(Products entity)
        {
            return base.add(entity);
        }
        //overload
        public async Task add(productDTO entity)
        {
            return;
        }
    }
}

