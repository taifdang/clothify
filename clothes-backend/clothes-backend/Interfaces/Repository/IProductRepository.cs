using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Tnef;

namespace clothes_backend.Interfaces.Repository
{
    public interface IProductRepository:IBaseRepository<Products>
    {
        Task<Result<productListDTO>> GetId(int id);
        Task<List<productListDTO>?> GetProductAllAsync();
        Task<Products> AddAsync([FromForm] ProductDTO DTO); 
        Task<List<string>> DeleteAsync(int id);
        Task<Result<Products>> UpdateAsync(int id, [FromForm] ProductDTO DTO);
     
    }
}
