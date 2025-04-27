using System.ComponentModel.DataAnnotations.Schema;
namespace clothes_backend.DTO.ORDER
{
    public class orderDetailDTO
    {
        public int id { get; set; }     
        public int order_id { get; set; }      
        public int product_variant_id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }     
        public int quantity { get; set; }     
    }
}
