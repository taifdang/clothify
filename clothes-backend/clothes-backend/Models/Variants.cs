using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Variants
    {
        public int product_variant_id { get; set; }
        public int option_value_id { get; set; }       
        public ProductVariants product_variants { get; set; }       
        public OptionValues option_values { get; set; }
    }
}
