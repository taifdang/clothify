using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase 
{
    [HttpPost("sign-in")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SignIn()
        => throw new NotImplementedException();

    [HttpPost("sign-up")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SignUp()
        => throw new NotImplementedException();


    [HttpDelete("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout()
        => throw new NotImplementedException();

    [HttpGet("refresh")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RefreshToken()
       => throw new NotImplementedException();

    [HttpPost("reset-password")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ResetPassword()
       => throw new NotImplementedException();

    [HttpPost("verify-otp")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ResetPassword(int otp)
       => throw new NotImplementedException();

    [HttpGet("profile")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetProfile()
       => throw new NotImplementedException();
}
