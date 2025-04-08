using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Products
    {
        public int id { get; set; }
        [Required]
        public int category_id { get; set; }
        [Required]
        public string title { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal old_price { get; set; }
        public string? description { get; set; }
        [JsonIgnore]
        public Categories categories { get; set; }
        public ICollection<ProductOptions> product_options { get; set; }
        public ICollection<ProductVariants> product_variants { get; set; }
        public ICollection<ProductOptionImages> product_option_images { get; set; }
    }
}
