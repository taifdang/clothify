namespace Infrastructure.Enitites;

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductVariantId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
    public Order Orders { get; set; }
    public ProductVariant ProductVariants { get; set; }
}
