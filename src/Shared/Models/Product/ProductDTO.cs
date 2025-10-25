using Application.Common.Models.Option;
using Shared.Models.ProductImage;

namespace Application.Common.Models.Product;

public class ProductDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }   
    public string Description { get; set; }
    public string Category { get; set; }
    public string ProductType { get; set; }
}
