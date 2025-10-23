using Application.Common.Models.Product;
using Infrastructure.Models;
using Shared.Models.ProductOption;

namespace Application.Common.Interface;

public interface IProductOptionService
{
    Task<List<ProductOptionDTO>> Get(int productId);
    Task<ProductOptionDTO> Add(AddProductOptionRequest request, CancellationToken token);
    Task<ProductOptionDTO> Update(UpdateProductOptionRequest request, CancellationToken token);
    Task<ProductOptionDTO> Delete(int id, CancellationToken token);
}
