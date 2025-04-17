using clothes_backend.Models;

namespace clothes_backend.Inteface.JWT
{
    public interface IAuthJWT
    {    
        Task<Users?> verifyJWT(int user_id, string refresh_token);
        string generateToken();
        void generateAccessToken(Users user, out string accessToken);    
        Task<string> generateRefreshToken(Users user);
    }
}
