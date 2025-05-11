using clothes_backend.DTO.General;
using clothes_backend.DTO.Product;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace clothes_backend.Interfaces.Service
{
    public interface IProductService
    {
        Task<Result<List<productListDTO>>> GetId(int id);
        Task<Result<List<productListDTO>>> GetAllProductAsync();
        Task<Result<Products>> AddAsync([FromForm] ProductDTO product);
        Task<Result<Products>> UpdateAsync(int id,[FromForm]ProductDTO product);
        Task<Result<Products>> DeleteAsync(int id);
        Task<Result<List<OptionDTO>>> GetSizeByColor(int id,[FromQuery] string color);
    }
}
