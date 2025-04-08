using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Orders
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public int? session_id { get; set; }
        [Required]
        public string status { get; set; }
        public string? note { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Price is not be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal total { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string phone { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string payment_type { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime create_at { get; set; }
        [JsonIgnore]
        public Users users { get; set; }
        public ICollection<OrderDetails> order_details { get; set; }
    }
}
