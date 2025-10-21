namespace Infrastructure.Enitites;

public class OptionValue
{
    public int Id { get; set; }
    public string OptionId { get; set; }
    public string Value { get; set; }
    public string? Label { get; set; }
    public Option Options { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<Variant> Variants { get; set; }
}
