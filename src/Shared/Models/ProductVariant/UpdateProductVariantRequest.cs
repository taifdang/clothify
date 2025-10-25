namespace Shared.Models.ProductVariant;

public class UpdateProductVariantRequest
{
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Percent { get; set; }
    public List<int>? OptionValueIds { get; set; }
}
