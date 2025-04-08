using System.ComponentModel.DataAnnotations;

namespace clothes_backend.Models
{
    public class ProductTypes
    {
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        public string? label { get; set; }
        public ICollection<Categories> categories { get; set; }
    }
}
