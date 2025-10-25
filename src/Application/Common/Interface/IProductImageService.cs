using Shared.Models.ProductImage;

namespace Application.Common.Interface;

public interface IProductImageService
{
    Task<ProductImageDTO> Add(int productId, AddProductImageRequest request, CancellationToken token);
    Task<ProductImageDTO> Delete(int productId, int id, CancellationToken token);
}
