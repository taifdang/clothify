namespace Infrastructure.Enitites;

public class Cart
{
    public int Id { get; set; }
    //public string? SessionId { get; set; }
    //public int? UserId { get; set; }
    //public User? Users { get; set; }
    public int CustomerId { get; set; }
    public Customer Customers { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}
