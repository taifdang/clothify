using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IProductService
    {
        Task<Result<productListDTO>> GetId(int id);
        Task<Result<List<productListDTO>?>> GetAllProductAsync();
        Task<Result<Products>> AddAsync([FromForm] ProductDTO product);
        Task<Result<Products>> UpdateAsync(int id,[FromForm]ProductDTO product);
        Task<Result<Products>> DeleteAsync(int id);
    }
}
