namespace Infrastructure.Enitites;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? SessionId { get; set; }
    public string Status { get; set; } = "PENDING";
    public string? Note { get; set; }
    public decimal Total { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string PaymentType { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public User Users { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
