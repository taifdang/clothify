namespace clothes_backend.DTO.ORDER
{
    public class OrderValueDTO
    {
        public int id { get; set; }
        public int purchase_quantity { get; set; }
        public int stock_quantity { get; set; }
        public decimal price { get; set; }
    }
}
