using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Models;

namespace clothes_backend.Interfaces.Service
{
    public interface IProductService
    {
        Task<PayloadDTO<List<productListDTO>?>> GetAllProductAsync();
        Task<PayloadDTO<Products>> DeleteProduct(int id);
    }
}
