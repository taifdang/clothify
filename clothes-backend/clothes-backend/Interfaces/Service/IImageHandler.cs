using clothes_backend.DTO.IMAGE;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IImageHandler
    {
        Task SaveImage(IFormFile file, string file_path);
        Task DeleteImage(string file_path);      
        Task<List<ProductOptionImages>> loopImage([FromForm] imageUploadDTO DTO, string file_name);
    }
}
