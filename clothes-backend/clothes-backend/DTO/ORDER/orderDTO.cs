using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.ORDER
{
    public class orderDTO
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public int? session_id { get; set; }
        [Required]
        public string status { get; set; }
        public string? note { get; set; }         
        public decimal total { get; set; }    
        public string phone { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string payment_type { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime create_at { get; set; } = DateTime.Now;
    }
}
