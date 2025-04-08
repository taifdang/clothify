using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class ProductOptions
    {
        [Required]
        public int product_id { get; set; }
        [Required]
        public string option_id { get; set; }
        [JsonIgnore]
        public Products products { get; set; }
        [JsonIgnore]
        public Options options { get; set; }
    }
}
