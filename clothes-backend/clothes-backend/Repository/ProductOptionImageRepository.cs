using clothes_backend.DTO;
using clothes_backend.Inteface;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;
namespace clothes_backend.Repository
{
    public class ProductOptionImageRepository : GenericRepository<ProductOptionImages>,IImageProcessing
    {
        public ProductOptionImageRepository(DatabaseContext db) : base(db)
        {
        }
        public async Task<object> add([FromForm]imageDTO DTO)
        {          
            try
            {
                //check product id
                var is_product = _db.products.FirstOrDefault(x => x.id == DTO.product_id);             
                if (is_product == null) return null!;
                //explicit loading: product_options
                _db.Entry(is_product).Collection(opt => opt.product_options).Load();
                //has product => list option
                var product_option = is_product.product_options.Select(opt=>opt.option_id).ToList();
                var option_value = _db.option_values.FirstOrDefault(p => p.id == DTO.option_value_id);  
                //not include
                if (!product_option.Contains(option_value!.option_id)) return null!;
                //file_name = sku = category.label + product.id + file extension
                _db.Entry(is_product).Reference(cate => cate.categories).Load();
                string file_name = String.Join('-',"MAU", is_product.categories.label , is_product.id);
                
                //save image => loop
                //new image can same old image name             
                foreach (var item in DTO.files)
                {
                    int index = 1;
                    //merge file_name = file_name + extension;
                    //file_name = String.Join("", file_name,Path.GetExtension(item.FileName));
                    string full_path = String.Join("", file_name,Path.GetExtension(item.FileName));                  
                    while(File.Exists(getFilePath(full_path)))
                    {
                        //reset
                        full_path = String.Join("", file_name, Path.GetExtension(item.FileName));
                        full_path = getFileName(full_path, index.ToString());
                        index++;
                    }                            
                    await saveImage(item, getFilePath(full_path));
                    ProductOptionImages entity = new ProductOptionImages()
                    {
                        product_id = DTO.product_id,
                        option_value_id = DTO.option_value_id,
                        src = String.Join("/", "", full_path)
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
        public string getFileName(string file_name,string? attribute = null)
        {
            //split => extention
            if(!string.IsNullOrEmpty(attribute))
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
