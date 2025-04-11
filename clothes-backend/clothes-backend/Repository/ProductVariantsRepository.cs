using AutoMapper;
using clothes_backend.AutoMapper;
using clothes_backend.DTO;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;

namespace clothes_backend.Repository
{
    public class ProductVariantsRepository : GenericRepository<ProductVariants>
    {
        private readonly IMapper _mapper;
        public ProductVariantsRepository(DatabaseContext db, IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }
        public override Task<ProductVariants> getId(int id)
        {
            return base.getId(id);
        }
        #region NOTE
        //public async Task<object?> add([FromForm] productVariantsDTO DTO)
        //{

        //    var product = await _db.products.FindAsync(DTO.product_id);
        //    if (product == null) return null;
        //    //
        //    //Thêm product_variant trước để lấy id
        //    var product_variant = _mapper.Map<ProductVariants>(DTO);
        //    if(product_variant.price == 0 || product_variant.old_price == 0)
        //    {
        //        product_variant.price = product.price; 
        //        product_variant.old_price = product.old_price;
        //        product_variant.percent = (decimal)Math.Ceiling((product_variant.old_price - product_variant.price)/ product_variant.old_price * 100);
        //    }
        //    _db.product_variants.Add(product_variant);
        //    await _db.SaveChangesAsync();
        //    //
        //    await _db.Entry(product).Collection(x => x.product_option_images).LoadAsync();
        //    var distinctPairs = product.product_option_images
        //            .Select(x => x.option_value_id)
        //            .Distinct()
        //            .ToList();
        //    bool data = DTO.options.Any(x => distinctPairs.Contains(x));
        //    //check co thuoc product_option khong
        //    await _db.Entry(product).Collection(x => x.product_options).LoadAsync();
        //    var product_option = product.product_options.Select(opt => opt.option_id).ToList();

        //    var option_value = _db.option_values.Where(x=> distinctPairs.Contains(x.id)).ToList();
        //    var matches = DTO.options.FindAll(x => distinctPairs.Contains(x));
        //    //bool check_result = _db.option_values.Where(x => DTO.options.Find(x.id)).ToList();
        //    //await _db.Entry(product).Collection(x => x.product_options).LoadAsync();

        //    ////1 option_value_id = option_value_id của product_option_images  
        //    //var product_option = product.product_options.Select(opt => opt.option_id).ToList();
        //    //var option_value = _db.option_values.FirstOrDefault(p => p.id == DTO.option_value_id);
        //    //if (!product_option.Contains(option_value!.option_id)) return null!;  //not include
        //    //foreach (var option in DTO.options)
        //    //{
        //    //    var option_item = option_value.FirstOrDefault(p => p.id == option);
        //    //    //if (!check_data.Contains(option_item.option_id)) return null;
        //    //    //
        //    //    var variants = new Variants
        //    //    {
        //    //        product_variant_id = product_variant.id,
        //    //        option_value_id = option
        //    //    };
        //    //    //
        //    //    product_variant.title = string.IsNullOrWhiteSpace(product_variant.title)
        //    //    ? option_item.value
        //    //                   : product_variant.title + " / " + option_item.value;
        //    //    product_variant.sku = product_variant.sku + "." + option_item.label;
        //    //    //
        //    //    _db.variants.Add(variants);
        //    //    await _db.SaveChangesAsync();
        //    //}
        //    return matches;

        //#########
        //using (var transactions = _db.Database.BeginTransaction())
        //{
        //    int image_seleted = 0;
        //    try
        //    {
        //        foreach (var option in DTO.options)
        //        {
        //            var option_item = option_value.FirstOrDefault(p => p.id == option);
        //            if (option_item == null) return null;
        //            if (product_option.Contains(option_item.option_id))
        //            {

        //            }
        //        }
        //        transactions.Commit();
        //    }
        //    catch
        //    {
        //        transactions.Rollback();
        //    }
        //}

        //}
        #endregion
        public override Task update(int id, ProductVariants entity)
        {
            return base.update(id, entity);
        }
        public override Task delete(int id)
        {
            return base.delete(id);
        }
        public async Task<object?> ADD([FromForm] productVariantsDTO DTO)
        {
      
            var product = await _db.products.FindAsync(DTO.product_id);
            if (product == null) return null;          
         

            await _db.Entry(product).Collection(x => x.product_options).LoadAsync();//product_option
            var product_option = product.product_options            
               .Select(g => g.option_id).ToList();
           
            await _db.Entry(product).Collection(x => x.product_option_images).LoadAsync(); //image_only_image
            var images = product.product_option_images.Select(x => x.option_value_id).Distinct().ToList();
            if (images == null) return null;
            var option_img = DTO.options.FindIndex(x => images.Contains(x));        
            var option_value = _db.option_values.ToList(); //check exist option_id
            
            using (var transactions = _db.Database.BeginTransaction())
            {           
                List<string> list = new List<string>();
                try
                {                 
                    var product_variant = _mapper.Map<ProductVariants>(DTO);//save product_variant
                    await _db.Entry(product).Reference(x => x.categories).LoadAsync();
                    product_variant.sku = string.Join("-",product.categories.label,product.id);
                    if (product_variant.price == 0 || product_variant.old_price == 0)
                    {
                       
                        product_variant.price = product.price;
                        product_variant.old_price = product.old_price;
                        product_variant.percent = (decimal)Math.Ceiling((product_variant.old_price - product_variant.price) / product_variant.old_price * 100);
                        
                    }
                    _db.product_variants.Add(product_variant);
                    await _db.SaveChangesAsync();
                    
                    foreach (var option in DTO.options)
                    {
                        var option_item = option_value.FirstOrDefault(p => p.id == option);
                        if (option_item == null) return null;
                        if (!product_option.Contains(option_item.option_id)) return null;                          
                        if (list.Contains(option_item.option_id)) return null;  
                        list.Add(option_item.option_id);
                        var variants = new Variants
                        {
                            product_variant_id = product_variant.id,
                            option_value_id = option
                        };
                        //
                        product_variant.title = string.IsNullOrWhiteSpace(product_variant.title) ? option_item.value : string.Join("/ ", product_variant.title,option_item.value);                                   
                        product_variant.sku = string.Join("-", product_variant.sku, option_item.label);
                        //
                        _db.variants.Add(variants);
                        await _db.SaveChangesAsync();
                    }
                    if (product_option.Count() != list.Count()) return null;
                    transactions.Commit();
                    return list;
                }
                catch
                {
                    if (option_img == -1) return null;
                    transactions.Rollback();
                    return null;
                }
            }
        }
    }
}
