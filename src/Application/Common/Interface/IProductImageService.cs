using Shared.Models.ProductImage;

namespace Application.Common.Interface;

public interface IProductImageService
{
    Task<ProductImageDTO> Add(AddProductImageRequest request, CancellationToken token);
    Task<ProductImageDTO> Delete(int id, CancellationToken token);
}
