using AutoMapper;
using AutoMapper.QueryableExtensions;
using clothes_backend.Data;
using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Utils;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace clothes_backend.Repository
{
    public class ProductRepository : BaseRepository<Products>,IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductRepository> _logger;    
        public static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private static readonly string cache_key = "product_cachev1";
        public ProductRepository(
            DatabaseContext db,
            IMapper mapper,          
            IDistributedCache distributedCache,
            ILogger<ProductRepository> logger,
            IConfiguration configuration
            ) : base(db)
        {
            _cache = distributedCache;
            _logger = logger;         
            _mapper = mapper;
        }           
        //public async Task<object> getTest()
        // {          
        //    var image =  _db.product_option_images
        //          .Where(p => p.product_id == 15)
        //          .OrderBy(x => x.id)
        //          .GroupBy(op => op.option_value_id)
        //          .Select(g => g.First())
        //          .AsEnumerable()
        //          .Select(x => new
        //          {
        //              x.id,
        //              x.src
        //          })
        //          .ToList();
        //    var images =  _db.product_option_images
        //    .Where(p => p.product_id == 15)
        //    .OrderBy(x => x.id)
        //    .AsEnumerable()
        //    .GroupBy(p => p.option_value_id) 
        //    .SelectMany(g => g.OrderBy(x => x.id).Take(1)) 
        //    .Select(x => new
        //    {
        //        id = x.id,
        //        src = x.src
        //    })
        //    .ToList();
      
        //    return images;
        //}   
        
        public override IEnumerable<Products> PaginationBase(IEnumerable<Products> entity, int currentPage, int limit)
        {
            return base.PaginationBase(entity, currentPage, limit);
        }
        public async Task<List<productListDTO>?> GetProductAllAsync()
        {
            _logger.Log(LogLevel.Information, "Trying get cache from database");
            if (_cache.TryGetValue(cache_key, out List<productListDTO>? products))
            {
                _logger.Log(LogLevel.Information, "Found cache");
            }
            else
            {
                try
                {
                    await _semaphore.WaitAsync();
                    if (_cache.TryGetValue(cache_key, out products))
                    {
                        _logger.Log(LogLevel.Information, "Found cache v2");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "Not found cache from database");
                        products = await _db.products
                               .AsNoTracking()
                               .Where(p => p.id == 15)
                               .ProjectTo<productListDTO>(_mapper.ConfigurationProvider)
                               .AsSingleQuery()
                               .ToListAsync();

                        var cacheOptions = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(90));
                        await _cache.SetAsync(cache_key, products, cacheOptions);

                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return products;
        }
        
        public async Task<Products?> UpdataeAsync(int id, [FromForm] ProductDTO DTO)
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
        public async Task<List<string>> DeleteAsync(int id)
        {
            var data = await _db.products.Include(x => x.product_option_images).FirstOrDefaultAsync(x => x.id == id);
            if (data == null) return null!;                          
            var files = data.product_option_images.Select(x => x.src).ToList();                                                                             
            _db.products.Remove(data);
            await _db.SaveChangesAsync();
            return files!;
        }

        public async Task<Products> AddAsync([FromForm] ProductDTO DTO)
        {
            try
            {
                var products = _mapper.Map<Products>(DTO);
                foreach (var item in DTO.options)
                {
                    _db.product_options.AddRange(new ProductOptions
                    {
                        product_id = products.id,
                        option_id = item
                    });
                }
                _db.Add(products);
                await _db.SaveChangesAsync();             
                return products;
            }
            catch
            {             
                return null!;
            }
        }
        
        public async Task<Result<Products>> UpdateAsync(int id, [FromForm] ProductDTO DTO)
        {
            try
            {
                if (id != DTO.id) return Result<Products>.Failure(StatusCode.NotFound);
                var product = await _db.products.FindAsync(id);
                if (product == null) return Result<Products>.Failure(StatusCode.NotFound);
                _mapper.Map(DTO, product);
                await _db.SaveChangesAsync();
                return Result<Products>.Success();
            }
            catch
            {
                return Result<Products>.Failure(StatusCode.Isvalid);
            }
        }

        public async Task<productListDTO> GetIdq(int id)
        {
            try
            {
                var products = await _db.products
                    .AsNoTracking()
                    .Where(p => p.id == id)
                    .ProjectTo<productListDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();                           
                return products!;
            }
            catch
            {
                return null!;
            }
        }

        public async Task<Result<productListDTO>> GetId(int id)
        {
            try
            {
                var isHas = _db.products.FirstOrDefault(x => x.id == id);
                if(isHas is null) return Result<productListDTO>.Failure(StatusCode.NotFound);
                var products = await _db.products
                    .AsNoTracking()                 
                    .ProjectTo<productListDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x=>x.id == id);
                return Result<productListDTO>.Success(products!);
            }
            catch
            {
                return Result<productListDTO>.Failure();
            }
        }
    }
}

