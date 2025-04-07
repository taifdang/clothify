using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Categories
    {
        public int id { get; set; }
        public int product_types_id { get; set; }
        public string value { get; set; }
        public string? label { get; set; }
        public ProductTypes product_types { get; set; }
        public ICollection<Products> products { get; set; }
    }
}
