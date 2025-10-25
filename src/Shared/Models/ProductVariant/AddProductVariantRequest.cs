namespace Application.Common.Models.ProductVariant;

public class AddProductVariantRequest
{
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public int Quantity { get; set; }
    public List<int> OptionValueIds { get; set; }
}
