using Application.Common.Models.Product;
using Infrastructure.Models;
using Shared.Models.Product;

namespace Application.Common.Interface;

public interface IProductService
{
    Task<ProductReadModel> GetById(int id);
    Task<Pagination<ProductReadModel>> GetList(int pageIndex, int pageSize);
    Task<ProductDTO> Add(AddProductRequest request, CancellationToken token);
    Task<ProductDTO> Update(int id, UpdateProductRequest request, CancellationToken token);
    Task<ProductDTO> Delete(int id, CancellationToken token);
    Task<ProductDTO> Publish(int id, CancellationToken token);
}
