using Application.Common.Models.ProductVariant;

namespace Application.Common.Interface;

public interface IProductVariantService
{
    Task<List<ProductVariantDTO>> Get(int id, Dictionary<string, string>? selectedOptions);
    Task<ProductVariantDTO> Add(AddProductVariantRequest request, CancellationToken token);
    Task<ProductVariantDTO> Delete(int id, CancellationToken token);
}
