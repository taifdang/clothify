using Shared.Models.Option;
using Shared.Models.OptionValue;
using Shared.Models.ProductImage;

namespace Shared.Models.Product;

public class ProductReadModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string ProductType { get; set; }
    public List<ProductImageReadModel> Images { get; set; }
    public List<OptionValueReadModel> OptionValues { get; set; }
    public List<OptionReadModel> Options { get; set; }
}
