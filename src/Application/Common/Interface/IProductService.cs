using Application.Common.Models.Product;
using Infrastructure.Models;

namespace Application.Common.Interface;

public interface IProductService
{
    Task<ProductDTO> GetById(int id);
    Task<Pagination<ProductDTO>> GetList(int pageIndex, int pageSize);
    Task<ProductDTO> Add(AddProductRequest request, CancellationToken token);
    Task<ProductDTO> Update(int id, UpdateProductRequest request, CancellationToken token);
    Task<ProductDTO> Delete(int id, CancellationToken token);
    Task<ProductDTO> Publish(int id, CancellationToken token);
}
