namespace Infrastructure.Enitites;

public class CartDetail
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public byte[] RowVersion { get; set; }
    public Cart Carts { get; set; }
    public ProductVariant ProductVariants { get; set; }
}
