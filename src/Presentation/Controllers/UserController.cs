using Application.Common.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.User;

namespace Api.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> GetAvailable(CancellationToken cancellationToken)
       => Ok(await _userService.GetAvailable(cancellationToken));

    [HttpPut]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Update(UserUpdateRequest request, CancellationToken cancellationToken)
    {
        await _userService.Update(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Delete(string userId)
    {
        await _userService.Delete(userId);
        return NoContent();
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> AssignRole(AssignRoleRequest request, CancellationToken cancellationToken)
    {
        await _userService.AssignRole(request, cancellationToken);
        return NoContent();
    }
}
