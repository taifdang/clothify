using Microsoft.AspNetCore.Http;
using Shared.Models.Media;

namespace Application.Common.Interface;

public interface IFileService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<FileUploadResult> AddFileAsync(IFormFile file);
    string GetFileUrl(string fileName);
}
