using Application.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Auth;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase 
{
    private readonly IAuthService _authService = authService;

    [HttpPost("sign-in")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.SignIn(request, cancellationToken);
        SetTokenInCookie(result.Token);

        return Ok(result);
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        await _authService.SignUp(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOut();
        RemoveTokenInCookie();

        return NoContent();
    }

    [HttpGet("refresh")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RefreshToken()
       => throw new NotImplementedException();

    [HttpPost("reset-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _authService.ResetPassword(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("send-reset-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SendResetPassword(SendResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _authService.SendResetPassword(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("profile")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        => Ok(await _authService.GetProfile(cancellationToken));

    #region Helpers
    private string GetTokenInCookie() => Request.Cookies["token_key"];

    private void SetTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("token_key", refreshToken, cookieOptions);
    }

    private void RemoveTokenInCookie()
    {
        Response.Cookies.Delete("token_key");
    }
    #endregion
}
