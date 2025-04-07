namespace clothes_backend.Models
{
    public class Orders
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public int? session_id { get; set; }
        public string status { get; set; }
        public string? note { get; set; }
        public double total { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string payment_type { get; set; }
        public DateTime create_at { get; set; }
        public Users users { get; set; }
        public ICollection<OrderDetails> order_details { get; set; }
    }
}
