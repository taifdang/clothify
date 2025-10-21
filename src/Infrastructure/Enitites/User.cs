namespace Infrastructure.Enitites;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
    public string Role { get; set; } = "User";
    public string? Avatar { get; set; }
    public bool IsLock { get; set; } = false;
    public string? RefreshToken { get; set; }
    public DateTime? ExpiryTime { get; set; }
    public ICollection<Cart> Carts { get; set; }
    public ICollection<Order> Orders { get; set; }
}
