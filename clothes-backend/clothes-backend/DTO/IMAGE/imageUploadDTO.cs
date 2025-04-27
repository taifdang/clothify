using clothes_backend.Utils.Validate;
using System.ComponentModel.DataAnnotations;
namespace clothes_backend.DTO.IMAGE
{
    public class imageUploadDTO
    {
        [Required]
        public int product_id { get; set; }
        [Required]
        public int option_value_id { get; set; }
        [AllowedExtensions(new[] {".jpg",".webp",".png"})]
        public IFormFile[] files { get; set; }
    }
}
