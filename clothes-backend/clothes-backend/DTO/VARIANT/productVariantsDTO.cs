using clothes_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using clothes_backend.Utils.Validate;

namespace clothes_backend.DTO.VARIANT
{
    public class productVariantsDTO
    {
        public int id { get; set; }
        [Required]
        public int product_id { get; set; }     
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Old_Price cannot be negative value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal old_price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative value")]
        public int quantity { get; set; }
        [Required]
        [NonEmptyListInt(ErrorMessage ="Not empty list")]
        public List<int> options { get; set; }
       
    }
}
