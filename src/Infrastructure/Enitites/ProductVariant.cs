

namespace Infrastructure.Enitites;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Percent { get; set; }
    public string? Sku { get; set; }
    public Product Products { get; set; }
    public ICollection<Variant> Variants { get; set; }
    public ICollection<CartDetail> CartDetails { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
