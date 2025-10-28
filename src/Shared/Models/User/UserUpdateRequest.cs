using Microsoft.AspNetCore.Http;

namespace Shared.Models.User;

public class UserUpdateRequest
{
    public Guid UserId { get; set; }
    public string Avatar { get; set; }
    public IFormFile MediaFile { get; set; }
}
