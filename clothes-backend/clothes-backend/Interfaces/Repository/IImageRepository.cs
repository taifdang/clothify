using clothes_backend.DTO.Image;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Repository
{
    public interface IImageRepository:IBaseRepository<ProductOptionImages>
    {
        Task addRange(List<ProductOptionImages> image_list);
        Task<ImageInfoDTO> findImage([FromForm] imageUploadDTO DTO);
    }
}
