using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.USER
{
    public class registerDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string email { get; set; }
        [Required]
        public string name { get; set; }
        [Phone]
        public string phone { get; set; }
        [Required]
        [StringLength(
            100,
            ErrorMessage = "Password must be between 8 and 100 characters",
            MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Compare("password",ErrorMessage ="Password not match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string confirm_password { get; set; }
    }
}
