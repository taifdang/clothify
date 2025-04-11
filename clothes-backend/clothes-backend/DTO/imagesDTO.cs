using clothes_backend.Utils;
using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO
{
    public class imagesDTO
    {
        [Required]
        public int product_id { get; set; }
        [Required]
        public int option_value_id { get; set; }
        [AllowedExtensions(new[] {".jpg",".webp",".png"})]
        public IFormFile[] files { get; set; }
    }
}
