using clothes_backend.Data;
using clothes_backend.DTO.General;
using clothes_backend.DTO.Image;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace clothes_backend.Repositories
{
    public class ImageRepository : BaseRepository<ProductOptionImages>, IImageRepository
    {
        public ImageRepository(DatabaseContext db) : base(db)
        {
        }

        public async Task addRange(List<ProductOptionImages> image_list)
        {
            _db.product_option_images.AddRange(image_list);
            await _db.SaveChangesAsync();
        }

        public async Task<ImageInfoDTO> findImage([FromForm] imageUploadDTO DTO)
        {
            var product = await _db.products
                .Include(p => p.categories)
                .Include(q => q.product_options)
                .FirstOrDefaultAsync(x => x.id == DTO.product_id);//check product id                                                                                      //
            if (product is null) return null!;
            var product_option = product.product_options.Select(opt => opt.option_id).ToList();  //has product => list option          
            var option_value = _db.option_values.FirstOrDefault(p => p.id == DTO.option_value_id);// chi them anh 1 option

            if (!product_option.Contains(option_value!.option_id)) return null!;  //not include

            var data = new ImageInfoDTO()
            {
                option_value = option_value,
                product = product
            };          
            return data;
        }        
    }
}
