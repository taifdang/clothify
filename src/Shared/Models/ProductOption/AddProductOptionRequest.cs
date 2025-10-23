namespace Shared.Models.ProductOption;

public class AddProductOptionRequest
{
    public int ProductId { get; set; }
    public string OptionId { get; set; }
    public bool AllowImage { get; set; } = false;
}
