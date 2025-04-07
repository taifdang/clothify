using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class ProductVariants
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public double old_price { get; set; }
        public int quantity { get; set; }
        public double percent { get; set; }
        public string sku { get; set; }    
        public Products products { get; set; }
        public ICollection<Variants> variants { get; set; }
        public ICollection<CartItems> cart_items { get; set; }
        public ICollection<OrderDetails> order_details { get; set; }
    }
}
