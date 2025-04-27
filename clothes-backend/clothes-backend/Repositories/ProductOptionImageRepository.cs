using clothes_backend.Data;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Repository
{
    public class ProductOptionImageRepository : BaseRepository<ProductOptionImages>,IImageService
    {
        public ProductOptionImageRepository(DatabaseContext db) : base(db)
        {
        }
        public async Task<object> add([FromForm]imageUploadDTO DTO)
        {          
            try
            {          
                var is_product = _db.products.FirstOrDefault(x => x.id == DTO.product_id);//check product id           
                if (is_product == null) return null!;          
                _db.Entry(is_product).Collection(opt => opt.product_options).Load(); //explicit loading: product_options    
                var product_option = is_product.product_options.Select(opt=>opt.option_id).ToList();  //has product => list option
                var option_value = _db.option_values.FirstOrDefault(p => p.id == DTO.option_value_id);           
                if (!product_option.Contains(option_value!.option_id)) return null!;  //not include
                _db.Entry(is_product).Reference(cate => cate.categories).Load();  //file_name = sku = category.label + product.id + option_value.label+ file extension
                string file_name = String.Join('-',"MAU", is_product.categories.label , is_product.id, option_value.label);                                          
                foreach (var item in DTO.files)   //save image => loop.//new image can same old image name        
                {
                    int index = 1;             
                    string full_path = String.Join("", file_name,Path.GetExtension(item.FileName));   //merge file_name = file_name + extension;               
                    while (File.Exists(getFilePath(full_path)))
                    {                       
                        full_path = String.Join("", file_name, Path.GetExtension(item.FileName));//reset
                        full_path = getFileName(full_path, index.ToString());
                        index++;
                    }                            
                    await saveImage(item, getFilePath(full_path));
                    ProductOptionImages entity = new ProductOptionImages()
                    {
                        product_id = DTO.product_id,
                        option_value_id = DTO.option_value_id,
                        src = full_path
                    };
                    _db.product_option_images.AddRange(entity);
                }
                await _db.SaveChangesAsync();
                return file_name;

            }
            catch
            {
                return null!;
            }
        }
        public async Task<string> delete(int id)
        {
            var is_exist = await _db.product_option_images.FindAsync(id);
            if (is_exist == null) return null!;
            try
            {
                //image processing
                var file_name = is_exist.src;
                var full_path = Path.Combine(Directory.GetCurrentDirectory(),"Images",file_name);
                if (File.Exists(full_path))
                {
                    File.Delete(full_path);
                }
                _db.product_option_images.Remove(is_exist);
                await _db.SaveChangesAsync();
                return file_name;              
            }
            catch
            {
                return null!;
            }       
        }
        public string getFileName(string file_name,string? attribute = null)
        {        
            if(!string.IsNullOrEmpty(attribute)) //split => extention
            {
                string[] parts = file_name.Split('.');             
                file_name = string.Format("{0}({1}).{2}", parts[0], attribute, parts[1]);
            }
            return file_name;
        }

        public string getFilePath(string file_path)
        {           
            return Path.Combine(Directory.GetCurrentDirectory(), "Images", file_path);
        }

        public async Task saveImage(IFormFile file,string file_path)
        {
            using (var stream = new FileStream(file_path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
