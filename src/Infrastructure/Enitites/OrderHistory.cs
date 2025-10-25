namespace Infrastructure.Enitites;

public class OrderHistory
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public int SoldQuantity { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public ProductVariant ProductVariants { get; set; }
}
