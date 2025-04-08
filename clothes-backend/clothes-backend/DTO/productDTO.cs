namespace clothes_backend.DTO
{
    public class productDTO
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string title { get; set; }
        public double price { get; set; }
        public double old_price { get; set; }
        public string? description { get; set; }
    }
}
