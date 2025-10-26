

namespace Infrastructure.Enitites;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public decimal RegularPrice { get; set; }
    public decimal ComparePrice { get; set; }
    public int Quantity { get; set; }
    public decimal Percent { get; set; }
    public string? Sku { get; set; }
    public Product Products { get; set; }
    public ICollection<Variant> Variants { get; set; }
    public ICollection<CartItem> CartDetails { get; set; }
    public ICollection<OrderItem> OrderDetails { get; set; }
}
