namespace clothes_backend.Models
{
    public class OrderHistory
    {
        public int id { get; set; }
        public int product_variant_id { get; set; }
        public int sold_quantity { get; set; }
    }
}
