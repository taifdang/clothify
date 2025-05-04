using clothes_backend.DTO.General;
using clothes_backend.DTO.IMAGE;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IImageService
    {
        Task<Result<ProductOptionImages>> AddImage([FromForm] imageUploadDTO DTO);
    }
}
