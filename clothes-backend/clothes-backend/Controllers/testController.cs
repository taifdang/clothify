using AutoMapper;
using clothes_backend.Data;
using clothes_backend.DTO.Product;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.DTO.Test;
using clothes_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;
        public testController(DatabaseContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("test")]
        public async Task<IActionResult> getTest()
        {   
            //var data = await getImage();
            var products = _db.products
                .Where(x=>x.id == 15)
                .Include(p => p.product_option_images)
                .Include(v=>v.product_variants)
                    .ThenInclude(vr=>vr.variants)
                        .ThenInclude(vr => vr.option_values)
                            .ThenInclude(vr => vr.options)               
                .AsSplitQuery()
                .ToList();
            //.FirstOrDefault(p => p.id == 15);
            var dtoList = products.Select(product =>
            {
                //images
                var images = product.product_option_images
                    .GroupBy(s => s.option_value_id)
                    .OrderBy(x => x.Key)
                    .SelectMany(g => g
                        .Where(c => c.src!.StartsWith("MAU"))
                        .OrderBy(c => c.id)
                        .Take(1)
                        .Select(x => new ImageDTO
                        {
                            id = x.id,
                            src = x.src!
                        })
                    ).ToList();
                //variant 
                var variant = product.product_variants
                             .Select(x => new VariantDTO
                             {
                                 id = x.id,
                                 title = x.title,
                                 price = x.price,
                                 old_price = x.old_price,
                                 sku = x.sku,
                                 percent = x.percent,
                                 quantity = x.quantity
                             })
                             .ToList();
                //options_value
                var options_value = product.product_option_images
                                    .GroupBy(x => x.options_values.options.title)
                                    .Select(group_option => new OptionImageDTO
                                    {
                                        title = group_option.Key,
                                        option_id = group_option.Select(k => k.options_values.option_id).FirstOrDefault() ?? null!,
                                        options = group_option
                                            .GroupBy(v => v.options_values.value)
                                            .Select(group_option_value => new optionValueDTO
                                            {
                                                //image =  valueGroup.Select(i => i.src).ToList()
                                                title = group_option_value.Key,
                                                image = group_option_value.Select(i => new ImageDTO { id = i.id, src = i.src }).ToList()
                                            })
                                            .ToList()
                                    })
                                    .ToList();
                //options
                var options = product.product_variants
                            .SelectMany(v => v.variants)
                            .GroupBy(ovid => ovid.option_values.option_id)
                            .Select(x => new OptionDTO
                            {
                                option_id = x.Key,
                                title = x.Select(x => x.option_values.options.title).FirstOrDefault(),
                                values = x.Select(a => a.option_values.value).Distinct().ToList()
                            })
                            .ToList();
               

                var dto = _mapper.Map<ListPDTO>(product);
                dto.variants = variant;
                dto.images = images;
                dto.options_value = options_value;
                dto.options = options;
                return dto;
            }).ToList();
            //


            return Ok(dtoList);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getTestOptions(int id, [FromQuery] string color)
        {
            //var data = await getImageTest();
            //var data = await getTest1();

            //var IsVariant = _db.product_variants
            //       .Include(x => x.variants)
            //       .Where(x => x.product_id == 15)
            //       .SelectMany(x => x.variants)
            //       .GroupBy(v => v.product_variant_id)
            //       .Select(g => g.Select(x => x.option_value_id)).ToList();

            //load option cua product_varint duoc click
            //list options => filter kich thuoc theo mau sac


            //.SelectMany(x => x.variants.Where(g => g.option_values.option_id == "size"))
            //.ToList();
            //.GroupBy(v => v.product_variant_id == id)

            //.Select(g => g.Select(x => x.option_value_id)).ToList();
            //option_list
            var IsVariant = _db.product_variants
               .Include(x => x.variants)
               .Where(x => x.product_id == id && x.title.StartsWith(color))
               .SelectMany(x => x.variants.Where(g => g.option_values.option_id == "size")
               .Select(x => x.option_values.value).ToList()).ToList();
            //image
            var imageVarinat = _db.product_option_images
                .Include(x => x.options_values)
                .Where(x => x.product_id == id && x.options_values.value.StartsWith(color))
                .Select(item => new { item.id, item.src }).ToList();

            
            var options = _db.product_variants
                           .Where(x => x.product_id == id && x.title.StartsWith(color))
                           .SelectMany(v => v.variants)
                           .GroupBy(ovid => ovid.option_values.option_id == "size")
                           .Select(x => new OptionDTO
                           {
                              // option_id = x.,
                               
                               title = x.Select(x => x.option_values.options.title).FirstOrDefault(),
                               option_id = x.Select(x => x.option_values.option_id).FirstOrDefault(),
                               values = x.Select(a => a.option_values.value).Distinct().ToList()
                           })
                           .ToList();
            return Ok(options);
        }
        [NonAction]
        public async Task<object> getImage()
        {
            var image = _db.product_option_images
                   //.Where(p => p.product_id == 15)
                   .GroupBy(s => s.option_value_id)
                   .OrderBy(x => x.Key)
                   .Select(v=> v.OrderBy(x => x.id).Where(c => c.src!.StartsWith("MAU")).Select(x => new ImageDTO
                   {
                      id = x.id,
                      src = x.src!
                   }).Take(2)).ToList()
                   .SelectMany(g=>g)               
                   .ToList();

            

            return image;
        }
        [NonAction]
        public async Task<object> getTest1()
        {
            var variants = await _db.product_variants
                      .Include(v => v.variants)
                          .ThenInclude(ov => ov.option_values)
                              .ThenInclude(os => os.options)
                      .Where(p => p.product_id == 15)
                      .SelectMany(vs => vs.variants)
                      .GroupBy(ovid => ovid.option_values.option_id)
                      .Select(x => new 
                      {
                          option_id = x.Key,
                          title = x.Select(x => x.option_values.options.title).FirstOrDefault(),
                          values = x.Select(a => a.option_values.value).Distinct().ToList()
                      })
                      .ToListAsync();



            return variants;
        }
        [NonAction]
        public async Task<object> get()
        {
            var data = _db.option_values.AsEnumerable().Select((x, index) => new
            {
                id = index,
                value = x.value
            }).ToList();
            //
            var list11 = _db.product_option_images
                .Where(x=>x.product_id == 15)
                .AsEnumerable()
                .GroupBy(p => p.option_value_id)         
                .SelectMany(x => x.Select(g => new { g.id, g.src }).Skip(1).Take(1))
                .ToList();
            //
            


            return list11;
        }
        [NonAction]
        public async Task<object> getOptions()
        {
            //var data = _db.variants
            //    .Where(x => x.product_variants.product_id == 15)    
            //    .Include(y=>y.option_values)
            //    .ThenInclude(c=>c.options)
            //    .AsEnumerable()
            //    .GroupBy(z=>z.option_values.options.title)
            //    .Select(r => new {
            //        title = r.Key,
            //        option_id = r.Select(ro=>ro.option_values.option_id).FirstOrDefault() ?? null!,
            //        value = r.GroupBy(b => b.option_values.value)
            //                .SelectMany(x=>x.Select(y=>y.option_values.value)).Distinct().ToList(),
            //        values_map = r.GroupBy(b=>b.option_values.value)                     
            //                .Select((group, index) => new
            //                {
            //                    id = index,
            //                    value = group.Key
            //                }).ToList()
            //    })
            //    .ToList();
            var data = _db.variants
                 
                .Include(y=>y.option_values)
                .ThenInclude(c=>c.options)

                .AsEnumerable()
                .GroupBy(z=>z.option_values.options.title)
                .Select(r => new {
                    title = r.Key,
                    option_id = r.Select(ro=>ro.option_values.option_id).FirstOrDefault() ?? null!,
                    value = r.GroupBy(b => b.option_values.value)
                            .SelectMany(x=>x.Select(y=>y.option_values.value)).Distinct().ToList(),
                    values_map = r.GroupBy(b=>b.option_values.value)                     
                            .Select((group, index) => new
                            {
                                id = index,
                                value = group.Key
                            }).ToList()
                })
                .ToList();
            return data;
        }
        
        //public class OptionDTO
        //{
        //    public int id { get; set; }
        //    public string value { get; set; }
        //}
        [NonAction]
        public async Task<object> getImageTest()//images
        {
            //var product = _db.products
            //     .Include(x => x.product_option_images)
            //     .Select(
            //         y => new
            //         {
            //             y.title,
            //             image = y.product_option_images.AsEnumerable()
            //                        .GroupBy(k => k.option_value_id)
            //                        .Select(a => new
            //                        {

            //                            value = a.Select(x => new
            //                            {
            //                                x.id,
            //                                x.src
            //                            }).Take(2)
            //                        }).ToList()
            //         }
            //     ).ToList();
           
            //var list11 = _db.product_option_images               
            //    .GroupBy(p => p.option_value_id)
            //    .AsEnumerable()
            //    .SelectMany(x => x.Select(g => new { g.id, g.src }).Take(2))
            //    .ToList();        
            var product_list = _db.products
                 .Include(x => x.product_option_images)                    
                 .Select(
                     y => new
                     {
                         y.title,
                         image = y.product_option_images                                  
                                    .GroupBy(k => k.options_values.value)
                                    .Select(x => new {x.Key,image = x.Select(y => new {y.id,y.src})}).ToList()
                     }
                 ).ToList();
            return product_list;
        }
        [NonAction]
        public async Task<object> getTopN()
        {
            var images = _db.product_option_images
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
    }
}
