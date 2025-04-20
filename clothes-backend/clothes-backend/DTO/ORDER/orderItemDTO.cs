using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.ORDER
{
    public class orderItemDTO
    {
        public int user_id { get; set; }     
        public string status { get; set; }
        public string? note { get; set; }   
        public string phone { get; set; }
        public string address { get; set; }
        public string payment_type { get; set; }
        public List<int> cartItem_id { get; set; }
    }
}
