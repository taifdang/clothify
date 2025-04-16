using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.USER
{
    public class loginDTO
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email address format")]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
