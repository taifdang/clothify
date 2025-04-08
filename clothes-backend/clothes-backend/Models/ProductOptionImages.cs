using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class ProductOptionImages
    {
        public int id { get; set; }
        [Required]
        public int product_id { get; set; }
        [Required]
        public int option_value_id { get; set; }
        public string? src { get; set; }
        [JsonIgnore]
        public Products products { get; set; }
        [JsonIgnore]
        public OptionValues options_values { get; set; }
    }
}
