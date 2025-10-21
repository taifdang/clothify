namespace Infrastructure.Enitites;

public class Category
{
    public int Id { get; set; }
    public int ProductTypeId { get; set; }
    public string Value { get; set; }
    public string? Label { get; set; }
    public ProductType ProductTypes { get; set; }
    public ICollection<Product> Products { get; set; }
}
