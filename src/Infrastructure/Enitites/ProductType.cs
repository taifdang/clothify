namespace Infrastructure.Enitites;

public class ProductType
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Label { get; set; }
    public ICollection<Category> Categories { get; set; }
}
