namespace Shared.Models.Auth;

public class TokenResult
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expire {  get; set; }
}
