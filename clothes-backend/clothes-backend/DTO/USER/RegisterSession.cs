using clothes_backend.Models;

namespace clothes_backend.DTO.USER
{
    public class RegisterSession
    {
        public string otp { get; set; }
        public Users user { get; set; }
    }
}
