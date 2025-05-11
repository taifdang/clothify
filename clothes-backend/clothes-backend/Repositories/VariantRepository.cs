using clothes_backend.Data;
using clothes_backend.DTO.Product;
using clothes_backend.DTO.VARIANT;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace clothes_backend.Repositories
{
    public class VariantRepository : BaseRepository<ProductVariants>, IVariantRepository
    {
        public VariantRepository(DatabaseContext db) : base(db)
        {
        }
        public async Task<ProductVariants> AddVariant(ProductVariants product_variant,Products products, Dictionary<string, List<variableDTO>> dictionary)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var SKU = string.Join("-", dictionary.SelectMany(x => x.Value).Select(v => v.label).ToList());
                product_variant.sku = string.Join("-", products.categories.label, products.id, SKU);
                product_variant.title = string.Join(" / ", dictionary.SelectMany(x => x.Value).Select(v => v.label).ToList());

                if (product_variant.price == 0 || product_variant.old_price == 0)
                {
                    product_variant.price = products.price;
                    product_variant.old_price = products.old_price;
                    product_variant.percent = (decimal)Math.Ceiling((product_variant.old_price - product_variant.price) / product_variant.old_price * 100);
                }
                //_db.product_variants.Add(product_variant);
                //await _db.SaveChangesAsync();
                //variants          
                var variants = dictionary
                    .SelectMany(x => x.Value)
                        .Select(v => 
                        new Variants 
                        {
                            //product_variant_id = product_variant.id,
                            product_variants = product_variant,//EF generate
                            option_value_id = v.id ,
                           
                            
                        }).ToList();
                _db.product_variants.Add(product_variant);
                _db.variants.AddRange(variants);
                //
                await _db.SaveChangesAsync();
                //
                transaction.Commit();
                return product_variant;
            }
            catch
            {
                
                transaction.Rollback();
                return null!;
            }
            throw new NotImplementedException();
        }

        

        public async Task<Products> FindProductsAsync(int id)
        {
            var products = await _db.products
                 .AsNoTracking()         
                 .Include(x => x.categories)
                 .Include(x => x.product_option_images)
                 .Include(x => x.product_options)
                 .FirstOrDefaultAsync(x => x.id == id);
                 ;             
            return products;
        }

        public List<int> GetOptionValueId(int id) => _db.product_option_images.Select(x => x.option_value_id).ToList();
    
        public Dictionary<int, OptionValues> GetProductOptions(int id)
        {
            var productOption = _db.product_options.Where(x=>x.product_id == id).Select(g => g.option_id).ToList();
            var optionValue = _db.option_values.AsNoTracking().Where(x => productOption.Contains(x.option_id)).Distinct().ToDictionary(x=>x.id,x=>x);
            return optionValue;
        }
        public List<List<int>> GetVariantOption(int Id)
        {
            var IsVariant = _db.product_variants
                   .Include(x => x.variants)
                   .Where(x => x.product_id == Id)
                   .SelectMany(x => x.variants)
                   .GroupBy(v => v.product_variant_id)
                   .Select(g => g.Select(x => x.option_value_id).ToList()).ToList();

            return IsVariant;
        }
       
    }
}
