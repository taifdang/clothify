using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.CART
{
    public class addCartDTO
    {
        public int product_variant_id { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity cart item must be between 1 and 100")]
        public int quantity { get; set; }
    }
}
