namespace Infrastructure.Enitites;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int Version { get; set; }
    public Cart Carts { get; set; }
    public ProductVariant ProductVariants { get; set; }
}
