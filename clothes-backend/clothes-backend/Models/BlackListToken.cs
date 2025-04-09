namespace clothes_backend.Models
{
    public class BlackListToken
    {
        public int id { get; set; }
        public string token { get; set; } // bl_{token}
        public DateTime create_at { get; set; }
    }
}
