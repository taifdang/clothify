namespace Infrastructure.Enitites;

public class ProductOption
{
    public int ProductId { get; set; }
    public string OptionId { get; set; }
    public Product Products { get; set; }
    public Option Options { get; set; }
}
