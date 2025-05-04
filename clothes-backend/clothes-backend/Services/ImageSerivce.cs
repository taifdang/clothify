using clothes_backend.DTO.General;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Services
{
    public class ImageSerivce : IImageService, IImageHandler
    {
        private readonly IImageRepository _repository;
        public ImageSerivce(IImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ProductOptionImages>> AddImage([FromForm] imageUploadDTO DTO)
        {
            try
            {
                var result = await _repository.findImage(DTO);
                if (result is null) return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.NotFound);
                string file_name = String.Join('-', "MAU", result.product.categories.label, result.product.id, result.option_value.label);
                var entity = await loopImage(DTO, file_name);
                //=>background_job
                _repository?.addRange(entity);
                return Result<ProductOptionImages>.Success();
            }
            catch
            {
                return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        public string getFileName(string file_name, string attribute)
        {
            if (!string.IsNullOrEmpty(attribute)) //split => extention
            {
                string[] parts = file_name.Split('.');
                file_name = string.Format("{0}({1}).{2}", parts[0], attribute, parts[1]);
            }
            return file_name;
        }

        public string getFilePath(string file_path) => Path.Combine(Directory.GetCurrentDirectory(), "Images", file_path);
      
        public async Task saveImage(IFormFile file, string file_path)
        {
            using (var stream = new FileStream(file_path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task<List<ProductOptionImages>> loopImage(imageUploadDTO DTO, string file_name)
        {
            var lists = new List<ProductOptionImages>();
            foreach (var item in DTO.files)   //save image => loop.//new image can same old image name        
            {
                int index = 1;
                string full_path = String.Join("", file_name, Path.GetExtension(item.FileName));   //merge file_name = file_name + extension;               
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
                lists.Add(entity);
            }
            return lists;
        }
    }
      
}
