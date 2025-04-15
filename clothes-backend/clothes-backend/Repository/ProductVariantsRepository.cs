using AutoMapper;
using clothes_backend.AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Inteface;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace clothes_backend.Repository
{
    public class ProductVariantsRepository : GenericRepository<ProductVariants>
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public ProductVariantsRepository(DatabaseContext db, IMapper mapper, ICacheService cache) : base(db)
        {
            _mapper = mapper;
            _cache = cache;
        }
        public override async Task<ProductVariants> getId(int id)
        {
            var product_variant = await _db.product_variants.FirstOrDefaultAsync(x => x.id == id);
            if (product_variant is null) return null;
            return product_variant;
        }     
        public async Task<int?> delete(int id)
        {
            var product_variant = await _db.product_variants.FirstOrDefaultAsync(x => x.id == id);
            if (product_variant is null) return null;
            try
            {
                _db.product_variants.Remove(product_variant);
                await _db.SaveChangesAsync();
                return 200;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return 400;
            }
           
        }          
        public async Task<object?> add([FromForm] productVariantsDTO DTO)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var product = await getCache();
                if (!product.TryGetValue(DTO.product_id, out var value)) return null;
                //image 
                var image_key = value.product_option_images.Select(x => x.option_value_id).ToHashSet();
                if (!DTO.options.Any(x => image_key.Contains(x))) return null;
                //set todictionary =>
                var product_options = value.product_options.Select(g => g.option_id).ToList();
                var option_value = _db.option_values.AsNoTracking().Where(x => product_options.Contains(x.option_id)).Distinct().ToDictionary(x=>x.id,x=>x);
                //check
                var dictionary = new Dictionary<string, List<variableDTO>>();          
                foreach (var option in DTO.options)
                {
                    var check = option_value.TryGetValue(option, out var option_detail);
                    if (!check) return null;
                    dictionary.Add(option_detail.option_id, new List<variableDTO>
                    {
                         new variableDTO { id = option_detail.id, label = option_detail.label }
                    });

                }
                //check variants
                var exist_variants = _db.product_variants
                    .Include(x => x.variants)
                    .Where(x => x.product_id == value.id)
                    .SelectMany(x => x.variants)
                    .GroupBy(v => v.product_variant_id)
                    .Select(g => g.Select(x => x.option_value_id).ToList()).ToList();
                
                bool matchingGroups = exist_variants.Any(g => g.Intersect(DTO.options).Count() == DTO.options.Count());
                if (matchingGroups) return null;//not exist => new
                var product_variant = _mapper.Map<ProductVariants>(DTO);
                var SKU = string.Join("-", dictionary.SelectMany(x => x.Value).Select(v => v.label).ToList());
            
                product_variant.sku = string.Join("-", value.categories.label, value.id, SKU);
                product_variant.title = string.Join(" / ", dictionary.SelectMany(x => x.Value).Select(v => v.label).ToList());

                if (product_variant.price == 0 || product_variant.old_price == 0)
                {
                    product_variant.price = value.price;
                    product_variant.old_price = value.old_price;
                    product_variant.percent = (decimal)Math.Ceiling((product_variant.old_price - product_variant.price) / product_variant.old_price * 100);
                }
                _db.product_variants.Add(product_variant);
                await _db.SaveChangesAsync();
                //variants          
                var variants = dictionary.SelectMany(x => x.Value).Select(v => new Variants { product_variant_id = product_variant.id, option_value_id = v.id }).ToList();
                _db.variants.AddRange(variants);
                await _db.SaveChangesAsync();
                transaction.Commit();
                
                return product_variant;
   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                return null;
            }
        }
        //
      
        public async Task<Dictionary<int,Products>> getCache()
        {         
            if (_cache.isCached(CacheKeys.products_cacheKey))
            {
                return _cache.Get<Dictionary<int, Products>>(CacheKeys.products_cacheKey);
            }
            //??
            var products = await _db.products.AsNoTracking().Include(x=>x.categories).Include(x=>x.product_option_images).Include(x=>x.product_options).ToDictionaryAsync(p => p.id, p => p);
            _cache.Set(CacheKeys.products_cacheKey, products, TimeSpan.FromMinutes(10));
            return products;
        }
    }
}
