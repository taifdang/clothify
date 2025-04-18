using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class CartItems
    {
        public int id { get; set; }
        [Required]
        public int cart_id { get; set; }
        [Required]
        public int product_variant_id { get; set; }
        [Range(1,1000,ErrorMessage ="Quantity must be between 1 and 1000")]
        public int quantity { get; set; }
        [Timestamp]
        public byte[] row_version { get; set; }
        [JsonIgnore]
        public Carts carts { get; set; }
        [JsonIgnore]
        public ProductVariants product_variants { get; set; }
    }
}
