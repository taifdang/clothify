using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class OrderDetails
    {
        public int id { get; set; }
        [Required]
        public int order_id { get; set; }
        [Required]
        public int product_variant_id { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price is not be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Range(1,1000,ErrorMessage ="Quantity must be between 1 and 1000")]
        public int quantity { get; set; }
        [JsonIgnore]
        public Orders orders { get; set; }
        public ProductVariants product_variants { get; set; }
    }
}
