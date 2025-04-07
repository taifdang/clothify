using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class ProductOptionImages
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int option_value_id { get; set; }
        public string? src { get; set; }    
        public Products products { get; set; }    
        public OptionValues options_values { get; set; }
    }
}
