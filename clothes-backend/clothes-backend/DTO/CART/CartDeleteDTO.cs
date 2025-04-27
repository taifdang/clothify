using System.ComponentModel.DataAnnotations;
namespace clothes_backend.DTO.CART
{
    public class cartDeleteDTO
    {
        [Required]
        public int id { get; set; }
        [Required]
        public byte[] row_version { get; set; }
    }
}
