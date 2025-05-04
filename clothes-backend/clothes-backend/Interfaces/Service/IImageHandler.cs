using clothes_backend.DTO.IMAGE;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IImageHandler
    {
        Task saveImage(IFormFile file, string file_path);
        string getFileName(string file_name,string attribute);
        string getFilePath(string file_path);
        Task<List<ProductOptionImages>>  loopImage([FromForm] imageUploadDTO DTO,string file_name);
       
    }
}
