namespace clothes_backend.DTO.PRODUCT
{
    public class ProductDTO
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public decimal old_price { get; set; }
        public string? description { get; set; }
        public List<string> options { get; set; }
    }
}
