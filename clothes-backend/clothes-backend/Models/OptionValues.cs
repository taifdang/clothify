using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class OptionValues
    {
        public int id { get; set; }
        [Required]
        public string option_id { get; set; }
        [Required]
        public string value { get; set; }
        public string? label { get; set; }
        [JsonIgnore]
        public Options options { get; set; }
        public ICollection<ProductOptionImages> product_option_images { get; set; }
        public ICollection<Variants> variants { get; set; }
    }
}
