namespace Infrastructure.Enitites;

public class Option
{
    public string Id { get; set; }
    public string Title { get; set; }
    public ICollection<ProductOption> ProductOptions { get; set; }
    public ICollection<OptionValue> OptionValues { get; set; }
}
