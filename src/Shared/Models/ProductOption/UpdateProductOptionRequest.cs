namespace Shared.Models.ProductOption;

public class UpdateProductOptionRequest
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string OptionId { get; set; }
    public bool AllowImage { get; set; } = false;
}
