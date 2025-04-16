using clothes_backend.Models;

namespace clothes_backend.Inteface.JWT
{
    public interface IAuthJWT
    {
        void verifyJWT(int user_id,string refresh_token);
        string generateToken();
        void generateAccessToken(Users user, out string accessToken);
        void generateRefreshToken(Users user, out string refreshToken);
    }
}
