using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Variants
    {
        [Required]
        public int product_variant_id { get; set; }
        [Required]
        public int option_value_id { get; set; }
        [JsonIgnore]
        public ProductVariants product_variants { get; set; }
        [JsonIgnore]
        public OptionValues option_values { get; set; }
    }
}
