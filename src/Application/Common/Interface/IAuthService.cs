using Shared.Models.Auth;
using Shared.Models.User;

namespace Application.Common.Interface;

public interface IAuthService
{
    Task<TokenResult> SignIn(SignInRequest request, CancellationToken cancellationToken);
    Task SignUp(SignUpRequest request, CancellationToken cancellationToken);
    Task SignOut();
    Task<UserReadModel> GetProfile(CancellationToken cancellationToken);
    Task<TokenResult> RefreshToken(string token, CancellationToken cancellationToken);
    Task SendResetPassword(SendResetPasswordRequest request, CancellationToken cancellationToken); // send request
    Task ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken); // verify email address
}