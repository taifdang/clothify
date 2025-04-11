using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class ProductVariants
    {
        public int id { get; set; }
        [Required]
        public int product_id { get; set; }      
        public string? title { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Old_Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal old_price { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Quantity cannot be negative value")]
        public int quantity { get; set; }
        [Range(0, 100, ErrorMessage = "Percent must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal percent { get; set; }
        public string? sku { get; set; }
        [JsonIgnore]
        public Products products { get; set; }
        public ICollection<Variants> variants { get; set; }
        public ICollection<CartItems> cart_items { get; set; }
        public ICollection<OrderDetails> order_details { get; set; }
    }
}
