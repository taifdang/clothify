using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Pipelines;

namespace clothes_backend.Services
{
    public class ImageHandler : IImageHandler, IGetFile
    {      
        public ImageHandler() { }
       
        public async Task DeleteImage(string file_path)
        {
            if (File.Exists(file_path))
            {
                await Task.Run(() => File.Delete(file_path));
            }
        }

        public string getFileName(string file_name, string? attribute = null)
        {
            if (!string.IsNullOrEmpty(attribute)) //split => extention
            {
                string[] parts = file_name.Split('.');
                file_name = string.Format("{0}({1}).{2}", parts[0], attribute, parts[1]);
            }
            return file_name;
        }

        public string GetFilePath(string file_path)
        {
            return Path.Combine(Directory.GetCurrentDirectory(),"Images", file_path);
        }
      
        public async Task<List<ProductOptionImages>> loopImage([FromForm] imageUploadDTO DTO, string file_name)
        {
            var lists = new List<ProductOptionImages>();
            foreach(var file in DTO.files)//save image => loop|iterator.//new image can same old image name       
            {
                int idx = 1;
                string file_path = string.Join("", file_name, Path.GetExtension(file.FileName));
                while (File.Exists(GetFilePath(file_path)))
                {
                    file_path = string.Join("-", file_name, Path.GetExtension(file.FileName));//reset
                    file_path = getFileName(file_path, idx.ToString());
                    idx++;
                }
                try
                {
                    await SaveImage(file, GetFilePath(file_path));
                }
                catch
                {
                    return null!;
                }
                lists.Add(new ProductOptionImages() {product_id = DTO.product_id,option_value_id = DTO.option_value_id ,src = file_path });
            }
            return lists;
        }
        
        public async Task SaveImage(IFormFile file, string file_path)
        {
            using (var stream = new FileStream(file_path,FileMode.Create,FileAccess.Write))
            {
                 await file.CopyToAsync(stream);
            }
        }
    }
}
