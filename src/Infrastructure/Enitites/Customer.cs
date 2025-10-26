namespace Infrastructure.Enitites;

public class Customer
{
    public int Id { get; set; }
    public Guid? UserId { get; set; } //customer have account
    public string? SessionId { get; set; } //guest
    public User? User { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public ICollection<Cart> Carts { get; set; }
    public ICollection<Order> Orders { get; set; }
}
