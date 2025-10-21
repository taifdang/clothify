namespace Infrastructure.Enitites;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int? OptionValueId { get; set; }
    public string? Image { get; set; }
    public Product Products { get; set; }
    public OptionValue? OptionValues { get; set; }
}
