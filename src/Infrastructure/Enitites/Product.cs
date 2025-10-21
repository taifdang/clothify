
namespace Infrastructure.Enitites;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public string? Description { get; set; }
    public Category Categories { get; set; }
    public ICollection<ProductOption> ProductOptions { get; set; }
    public ICollection<ProductVariant> ProductVariants { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
}
