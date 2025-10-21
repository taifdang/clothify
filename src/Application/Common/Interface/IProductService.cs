using Application.Common.Models.Product;
using Infrastructure.Models;

namespace Application.Common.Interface;

public interface IProductService
{
    Task<ProductDTO> Get(int id);
    Task<Pagination<ProductDTO>> Get(int pageIndex, int pageSize);
    Task<ProductDTO> Add(AddProductRequest request, CancellationToken token);
    Task<ProductDTO> Update(UpdateProductRequest request, CancellationToken token);
    Task<ProductDTO> Delete(int id, CancellationToken token);
}
