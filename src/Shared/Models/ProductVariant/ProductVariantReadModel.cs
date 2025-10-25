using Shared.Models.OptionValue;

namespace Shared.Models.ProductVariant;

public class ProductVariantReadModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Percent { get; set; }
    public decimal Quantity { get; set; }
    public string Sku { get; set; }
    public List<OptionValueVariantReadModel> Options { get; set; }
}
