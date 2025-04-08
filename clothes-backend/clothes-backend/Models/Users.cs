using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace clothes_backend.Models
{
    public class Users
    {
            
        public int id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]      
        public string email { get; set; }   
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100,ErrorMessage ="The {0} must be at least {2} and at most {1} characters long.",MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; }
        [Required(ErrorMessage ="Role is required")]
        public string role { get; set; }
        public string? avatar { get; set; }
        [Required(ErrorMessage = "Lock status is required")]
        public bool is_lock { get; set; }
        public string? refresh_token { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? expiry_time { get; set; }       
        public ICollection<Carts> carts { get; set; }     
        public ICollection<Orders> orders { get; set; }
    }
}
