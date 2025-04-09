using AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Repository
{
    public class ProductRepository : GenericRepository<Products>
    {
        private readonly IMapper _mapper;
        public ProductRepository(DatabaseContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }    
        public override async Task<IEnumerable<Products>> get()
        {          
            var products = await _db.products.ToListAsync();
            return products;
        }
        public override async Task<Products?> getId(int id)
        {
            var result = await _db.products.FirstOrDefaultAsync(x => x.id == id);
            if (result == null) return null;
            return result;
        }
        public override Task add(Products entity)
        {
            return base.add(entity);
        }
        //overload
        public async Task<Products?> add([FromForm] productDTO DTO)
        {
            using var tracsaction = _db.Database.BeginTransaction();
            try
            {           
                var products = _mapper.Map<Products>(DTO);
                
                await base.add(products);
                await _db.SaveChangesAsync();
                foreach (var item in DTO.options)
                {
                    _db.product_options.AddRange(new ProductOptions
                    {
                        product_id = products.id,
                        option_id = item
                    });
                }
                await _db.SaveChangesAsync();
                tracsaction.Commit();
                return products;
            }
            catch
            {
                tracsaction.Rollback();
                return null;
            }         
        }
        public async Task<Products?> update(int id, [FromForm]productDTO DTO)
        {
            try
            {
                if (id != DTO.id) return null;
                var product = await _db.products.FindAsync(id);
                if (product == null) return null;

                _mapper.Map(DTO, product);
                
                await _db.SaveChangesAsync();
                return product;
            }
            catch
            {
                return null;
            }
        }
        public override async Task<bool> delete(int id)
        {
            var data = await _db.products.FindAsync(id);
            if (data == null) return false;
            _db.products.Remove(data);
            await _db.SaveChangesAsync();
            return true;

            //remove image?
        }
        public override IEnumerable<Products> pagination(IEnumerable<Products> entity, int currentPage, int limit)
        {
            return base.pagination(entity, currentPage, limit);
        }
    }
}

