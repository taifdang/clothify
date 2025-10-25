using Application.Common.Models.ProductVariant;
using Shared.Models.ProductVariant;

namespace Application.Common.Interface;

public interface IProductVariantService
{
    Task<List<ProductVariantDTO>> GetList(int id, Dictionary<string, string>? selectedOptions);
    Task<ProductVariantDTO> GetById(int productId, int id);
    Task<ProductVariantDTO> Add(int productId, AddProductVariantRequest request, CancellationToken token);
    Task<ProductVariantDTO> Update(int productId, int id, UpdateProductVariantRequest request, CancellationToken token);
    Task<ProductVariantDTO> Delete(int productId, int id, CancellationToken token);
    Task<int> Generate(int id, GenerateVariantsRequest OptionValues, CancellationToken token);
}
