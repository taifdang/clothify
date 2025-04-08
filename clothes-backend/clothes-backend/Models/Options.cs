using System.ComponentModel.DataAnnotations;

namespace clothes_backend.Models
{
    public class Options
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        public ICollection<ProductOptions> product_options { get; set; }
        public ICollection<OptionValues> option_values { get; set; }

    }
}
