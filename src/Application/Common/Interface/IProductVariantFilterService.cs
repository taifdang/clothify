using Infrastructure.Enitites;

namespace Application.Common.Interface;

public interface IProductVariantFilterService : IBaseFilterService<ProductVariant>
{
    void FilterByOptionValues(Dictionary<string, string>? options);
    public void FilterByProductId(int ProductId);
    public void FilterByExactOptionValues(List<int> items);
    public void FilterByOptionCount(List<int> items);
}
