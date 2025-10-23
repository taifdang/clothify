namespace Infrastructure.Enitites;

public class ProductOption
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string OptionId { get; set; }
    public bool AllowImages { get; set; } = false;
    public Product Products { get; set; }
    public Option Options { get; set; }
}
