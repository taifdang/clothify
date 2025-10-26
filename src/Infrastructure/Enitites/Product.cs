
namespace Infrastructure.Enitites;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public decimal RegularPrice { get; set; } // sale, current
    public decimal ComparePrice { get; set; } // root
    public string? Description { get; set; }
    public string Status { get; set; } = "Hidden";
    public Category Categories { get; set; }
    public ICollection<ProductOption> ProductOptions { get; set; }
    public ICollection<ProductVariant> ProductVariants { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
}
