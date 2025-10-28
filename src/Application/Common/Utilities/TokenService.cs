using Application.Common.Interface;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Common.Utilities;

public class TokenService(AppSettings appSettings, IUnitOfWork unitOfWork, UserManager<User> userManager) : ITokenService
{
    private readonly AppSettings _appSettings = appSettings;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly Identity _jwt = appSettings.Identity;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<TokenResult> GenerateToken(User user, string[] scopes, CancellationToken cancellationToken)
    {
        #region
        //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
        //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //var claims = new[]
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(ClaimTypes.Email, user.Email),
        //    new Claim(ClaimTypes.Role, user.UserRole.ToString()),
        //};

        //var token = new JwtSecurityToken(
        //    claims: claims,
        //    expires: DateTime.UtcNow.AddDays(1),
        //    audience: _appSettings.Identity.Audience,
        //    issuer: _appSettings.Identity.Issuer,
        //    signingCredentials: credentials);

        //return new JwtSecurityTokenHandler().WriteToken(token);
        #endregion

        var result = new TokenResult();

        //token
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Uri, user?.AvatarUrl ?? "default.png"),
            new Claim(ClaimTypes.Role, roles == null ? Shared.Enums.Role.User.ToString() : string.Join(";", roles)),
            new Claim("scope", string.Join(" ", scopes)) // Adding scope claim
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddDays(_jwt.ExpiredTime);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        //set result
        result.Token = tokenResult;
        result.UserId = user.Id;
        result.Expire = expires;

        //refresh token  
        var refreshToken = new RefreshToken
        {
            Token = tokenResult,
            UserId = user.Id,
            Expires = expires,
            Created = DateTime.UtcNow
        };

        var existToken = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);

        if(existToken == null)
        {
            await _unitOfWork.ExecuteTransactionAsync(
             async () => await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken), cancellationToken);
        }
        else if(existToken.Expires > DateTime.UtcNow)
        {
            existToken.Token = tokenResult;
            existToken.Expires = expires;
            existToken.Created = DateTime.UtcNow;

            await _unitOfWork.ExecuteTransactionAsync(
              () => _unitOfWork.RefreshTokenRepository.Update(refreshToken), cancellationToken);
        }
        else
        {
            await _unitOfWork.ExecuteTransactionAsync(
               async () =>
               {
                   _unitOfWork.RefreshTokenRepository.Delete(refreshToken);
                   await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
               }, cancellationToken);
        }
        return result;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        TokenValidationParameters validationParameters = new()
        {
            ValidIssuer = _appSettings.Identity.Issuer,
            ValidAudience = _appSettings.Identity.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

        return principal;
    }
}
