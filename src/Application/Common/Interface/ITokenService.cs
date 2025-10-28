using Infrastructure.Enitites;
using Shared.Models.Auth;
using System.Security.Claims;

namespace Application.Common.Interface;

public interface ITokenService
{
    Task<TokenResult> GenerateToken(User user, string[] scopes, CancellationToken cancellationToken);
    ClaimsPrincipal ValidateToken(string token);
}
