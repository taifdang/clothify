using Application.Common.Interface;
using Application.Common.Utilities;
using Infrastructure.Enitites;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Models.Auth;
using Shared.Models.User;
using System.Security.Claims;

namespace Application.Services;

public class AuthService(
    UserManager<User> userManager, 
    SignInManager<User> signInManager, 
    IUnitOfWork unitOfWork,
    ICookieService cookieService,
    ITokenService tokenService,
    AppSettings appSettings,
    ICurrentUserProvider currentUserProvider
    ) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICookieService _cookieService = cookieService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly AppSettings _appSettings = appSettings;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public async Task<UserReadModel> GetProfile(CancellationToken cancellationToken)
    {
        var userId = _currentUserProvider.GetCurrentUserId();
        var user = await _userManager.Users
            .Select(u => new UserReadModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Roles = u.UserRole.Select(ur => ur.Role.Name).ToList(),
                AvatarUrl = u.AvatarUrl
            })
            .SingleOrDefaultAsync(x => x.Id == userId,cancellationToken)
            ?? throw new Exception("not found user");

        return user;
    }
    public async Task<TokenResult> SignIn(SignInRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == request.Username, cancellationToken)
            ?? throw new Exception("Not found user");

        var checkpass = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if(!checkpass.Succeeded)
        {
            throw new Exception("invalid check pass");
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopes = userClaims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(' ') ?? Array.Empty<string>();

        var token = await _tokenService.GenerateToken(user, scopes, cancellationToken);

        _cookieService.Delete();
        _cookieService.Set(token.Token);

        return token;
    }

    public async Task SignOut()
    {
        _cookieService.Delete();
        await _signInManager.SignOutAsync();
    }

    public async Task SignUp(SignUpRequest request, CancellationToken cancellationToken)
    {
        if(await _userManager.FindByNameAsync(request.UserName) != null)
        {
            throw new Exception("exist username");
        }

        if(await _userManager.FindByEmailAsync(request.Email) != null)
        {
            throw new Exception("exist email");
        }

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var signup = await _userManager.CreateAsync(user, request.Password);

        if(!signup.Succeeded)
        {
            throw new Exception("occur error");
        }

        await _userManager.AddToRoleAsync(user, Shared.Enums.Role.User.ToString());

        string readScope = _appSettings.Identity.ScopeBaseDomain + "/read";
        string writeScope = _appSettings.Identity.ScopeBaseDomain + "/write";

        string[] scopes = [readScope, writeScope];

        var scopeClaim = new Claim("scope", string.Join(" ", scopes));

        await _userManager.AddClaimAsync(user, scopeClaim);
    }
    public async Task<TokenResult> RefreshToken(string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Id == _currentUserProvider.GetCurrentUserId());

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            throw new Exception("Token was expire");
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var userClaims = await _userManager.GetClaimsAsync(user);
        var scopeClaim = userClaims.FirstOrDefault(c => c.Type == "scope");

        var scopes = scopeClaim?.Value.Split(' ') ?? [];

        var result = await _tokenService.GenerateToken(user, scopes, cancellationToken);

        _cookieService.Delete();
        _cookieService.Set(result.Token);

        return result;
    }
    
    //verify email address
    public async Task SendResetPassword(SendResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new Exception("email not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var otp = StringHelper.Generate(10000, 99999);

        // save to db
        var forgotPassword = new ForgotPassword
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token,
            OTP = otp.ToString(),
            DateTime = DateTime.Now
        };

        await _unitOfWork.ExecuteTransactionAsync(async () => 
             await _unitOfWork.ForgotPasswordRepository.AddAsync(forgotPassword), cancellationToken);

        // send email => hangfire

    }

    public async Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new Exception("email not exist");

        var forgotPassword = await _unitOfWork.ForgotPasswordRepository
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.OTP == request.OTP);

        var expireTime = forgotPassword.DateTime.AddMinutes(3);

        if (expireTime < DateTime.Now)
        {
            throw new Exception("OTP was expire");
        }

        var result = await _userManager.ResetPasswordAsync(user, forgotPassword.Token, request.NewPassword);

        if (!result.Succeeded)
            throw new Exception("Error reset password");
    }
}
