namespace Application.Common.Models.ProductVariant;

public class ProductVariantDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public decimal Percent {  get; set; }
    public decimal Quantity { get; set; }
    public string Sku { get; set; }
    public List<string> Options { get; set; }
}
