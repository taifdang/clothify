using Microsoft.AspNetCore.Http;

namespace Shared.Models.ProductImage;

public class AddProductImageRequest
{
    public int ProductId { get; set; }
    public int? OptionValueId { get; set; }
    public IFormFile MediaFile { get; set; }
}
