using clothes_backend.DTO.PRODUCT_DTO;
using clothes_backend.Models;
using MimeKit.Tnef;

namespace clothes_backend.Interfaces.Repository
{
    public interface IProductRepository:IBaseRepository<Products>
    {
        Task<List<productListDTO>?> getProductAllAsync();
        Task<List<string>> deleteProduct(int id);
     
    }
}
