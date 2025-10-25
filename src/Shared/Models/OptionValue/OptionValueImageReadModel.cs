using Shared.Models.ProductImage;

namespace Shared.Models.OptionValue;

public class OptionValueImageReadModel
{
    public string Title { get; set; }
    public string? Label { get; set; }
    public List<ProductImageReadModel> Image { get; set; } = new();
}
