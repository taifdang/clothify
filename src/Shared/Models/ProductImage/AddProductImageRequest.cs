using Microsoft.AspNetCore.Http;

namespace Shared.Models.ProductImage;

public class AddProductImageRequest
{
    public int? OptionValueId { get; set; }
    public IFormFile MediaFile { get; set; }
}
