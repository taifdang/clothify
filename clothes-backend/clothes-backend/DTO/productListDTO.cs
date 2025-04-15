namespace clothes_backend.DTO
{
    public class productListDTO
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public decimal old_price { get; set; }
        public string? description { get; set; }
        public List<productOptionsDTO> option_value { get; set; }
        public List<ProductGroupImageDTO> options { get; set; }

    }
}
