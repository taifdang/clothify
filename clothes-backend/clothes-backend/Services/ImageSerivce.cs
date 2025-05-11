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
