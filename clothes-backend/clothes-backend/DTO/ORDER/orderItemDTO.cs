using System.ComponentModel.DataAnnotations;
namespace clothes_backend.DTO.ORDER
{
    public class orderItemDTO
    {       
        public string? note { get; set; }
        [Phone]
        public string phone { get; set; }
        public string address { get; set; }
        public string payment_type { get; set; }
        public List<int> cartItem { get; set; }
    }
}
