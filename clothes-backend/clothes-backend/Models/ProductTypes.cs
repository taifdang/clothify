namespace clothes_backend.Models
{
    public class ProductTypes
    {
        public int id { get; set; }
        public string title { get; set; }
        public string? label { get; set; }
        public ICollection<Categories> categories { get; set; }
    }
}
