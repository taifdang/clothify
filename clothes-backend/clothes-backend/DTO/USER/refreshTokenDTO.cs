using System.ComponentModel.DataAnnotations;

namespace clothes_backend.DTO.USER
{
    public class refreshTokenDTO
    {
        [Required(ErrorMessage = "user id is not empty")]
        public int user_id { get; set; }
        [Required(ErrorMessage ="Invalid refresh token")]
        public string refreshToken { get; set; }
    }
}
