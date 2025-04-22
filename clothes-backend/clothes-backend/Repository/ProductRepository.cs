using AutoMapper;
using AutoMapper.QueryableExtensions;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Inteface;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace clothes_backend.Repository
{
    public class ProductRepository : GenericRepository<Products>,ICacheProduct
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public ProductRepository(DatabaseContext db, IMapper mapper, ICacheService _cache) : base(db)
        {
            _mapper = mapper;
        }       
        //custom list variants = product => product_variants => variants => option_values => options
         public async Task<object?> getTest()
         {
            //var variants = await _db.product_variants                 
            //          .Include(v => v.variants)
            //              .ThenInclude(ov => ov.option_values)
            //                  .ThenInclude(os => os.options)
            //          .Where(p => p.product_id == 15)
            //          .SelectMany(vs => vs.variants)
            //          .GroupBy(ovid => ovid.option_values.option_id)
            //          .Select(x => new listVariantDTO
            //          {
            //              option_id = x.Key,
            //              title = x.Select(x => x.option_values.options.title).FirstOrDefault(),
            //              values = x.Select(a => a.option_values.value).Distinct().ToList()
            //          })
            //          .ToListAsync();
            //    
            var image =  _db.product_option_images
                  .Where(p => p.product_id == 15)
                  .OrderBy(x => x.id)
                  .GroupBy(op => op.option_value_id)
                  .Select(g => g.First())
                  .AsEnumerable()
                  .Select(x => new
                  {
                      x.id,
                      x.src
                  })
                  .ToList();
            var images =  _db.product_option_images
            .Where(p => p.product_id == 15)
            .OrderBy(x => x.id)
            .AsEnumerable()
            .GroupBy(p => p.option_value_id) 
            .SelectMany(g => g.OrderBy(x => x.id).Take(1)) 
            .Select(x => new
            {
                id = x.id,
                src = x.src
            })
            .ToList();
      
            return images;
        }
        //custom list images = products => product_options_image => options_value => options 
        public async Task<object?> getAll()
        {           
            var product = await _db.products
                .AsNoTracking()
                .Where(p => p.id == 15)
                .ProjectTo<listProductDTO>(_mapper.ConfigurationProvider)       
                .AsSingleQuery()
                .ToListAsync();    
          
            if (product == null) return null;

            return product;
        }
        public override Task add(Products entity)
        {
            return base.add(entity);
        }
        //overload
        public async Task<Products?> add([FromForm] productsDTO DTO)
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
        public async Task<Products?> update(int id, [FromForm]productsDTO DTO)
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
            //var match = Regex.Match(file_name.src, @"[a-zA-Z]+-[0-9]"); //regex => "S-S"
            //remove image?          
            await _db.Entry(data).Collection(img => img.product_option_images).LoadAsync();
            var files = data.product_option_images.ToList();
            if(files != null)
            {
                var full_path = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                foreach(var file in files)
                {
                    var path = Path.Combine(full_path, file.src);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }           
            _db.products.Remove(data);
            await _db.SaveChangesAsync();
            return true;       
        }
        public override IEnumerable<Products> pagination(IEnumerable<Products> entity, int currentPage, int limit)
        {
            return base.pagination(entity, currentPage, limit);
        }
        public async Task<Dictionary<int, Products>> getCacheProduct(string cacheKey)
        {
            if (_cache.isCached(CacheKeys.products_cacheKey))
            {
                return _cache.Get<Dictionary<int, Products>>(CacheKeys.products_cacheKey);
            }
            //??
            var products = await _db.products.AsNoTracking().Include(x => x.categories).Include(x => x.product_option_images).Include(x => x.product_options).ToDictionaryAsync(p => p.id, p => p);
            _cache.Set(CacheKeys.products_cacheKey, products, TimeSpan.FromMinutes(10));
            return products;
        }
    }
}

