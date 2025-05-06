using clothes_backend.DTO.General;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Services
{
    public class ImageSerivce : IImageService
    {
        private readonly IImageRepository _Repository;
        private readonly IImageHandler _ImageHanlder;
        private readonly IBackgroundJobClient _backgroundJob;
        public ImageSerivce(IImageRepository Repository, IImageHandler ImageHandler,IBackgroundJobClient backgroundJob)
        {
            _Repository = Repository;
            _ImageHanlder = ImageHandler;
            _backgroundJob = backgroundJob;
        }
        public async Task<Result<ProductOptionImages>> AddImage([FromForm] imageUploadDTO DTO)
        {
            try
            {
                var result = await _Repository.findImage(DTO);
                if (result is null) return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.NotFound);
                string file_name = String.Join('-', "MAU", result.product.categories.label, result.product.id, result.option_value.label);
                //var entity = await loopImage(DTO, file_name);
                //_Repository?.addRange(entity);
                var entity = await _ImageHanlder.loopImage(DTO, file_name);
                if(entity is null) Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.Isvalid);
                await _Repository.addRange(entity!);
                return Result<ProductOptionImages>.Success();
            }
            catch
            {
                return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }

        //public string getFileName(string file_name, string attribute)
        //{
        //    if (!string.IsNullOrEmpty(attribute)) //split => extention
        //    {
        //        string[] parts = file_name.Split('.');
        //        file_name = string.Format("{0}({1}).{2}", parts[0], attribute, parts[1]);
        //    }
        //    return file_name;
        //}

        //public string getFilePath(string file_path) => Path.Combine(Directory.GetCurrentDirectory(), "Images", file_path);
      
        //public async Task SaveImage(IFormFile file, string file_path)
        //{
        //    using (var stream = new FileStream(file_path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //}
        //public async Task<List<ProductOptionImages>> loopImage(imageUploadDTO DTO, string file_name)
        //{
        //    var lists = new List<ProductOptionImages>();
        //    foreach (var item in DTO.files)   //save image => loop.//new image can same old image name        
        //    {
        //        int index = 1;
        //        string full_path = String.Join("", file_name, Path.GetExtension(item.FileName));   //merge file_name = file_name + extension;               
        //        while (File.Exists(getFilePath(full_path)))
        //        {
        //            full_path = String.Join("", file_name, Path.GetExtension(item.FileName));//reset
        //            full_path = getFileName(full_path, index.ToString());
        //            index++;
        //        }
        //        await SaveImage(item, getFilePath(full_path));
        //        //=>background_job
        //        ProductOptionImages entity = new ProductOptionImages()
        //        {
        //            product_id = DTO.product_id,
        //            option_value_id = DTO.option_value_id,
        //            src = full_path
        //        };
        //        lists.Add(entity);
        //    }
        //    return lists;
        //}

        public async Task<Result<ProductOptionImages>> DeleteImage(int id)
        {
            var image = await _Repository.FindBase(x=>x.id == id);
            if (image is null) return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.NotFound);
            try
            {
                //image processing
                var file_name = image.src;
                var full_path = Path.Combine(Directory.GetCurrentDirectory(), "Images", file_name);
                //enqueue
                _backgroundJob.Enqueue(() => _ImageHanlder.DeleteImage(full_path));
                _Repository?.DeleteBase(image);
                return Result<ProductOptionImages>.Success();
            }
            catch
            {
                return Result<ProductOptionImages>.Failure(Utils.Enum.StatusCode.Isvalid);
            }
        }
    }
      
}
