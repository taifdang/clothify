namespace Application.Common.Models.Product;

public class AddProductRequest
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public string? Description { get; set; }
    public List<string> Options { get; set; }
}
