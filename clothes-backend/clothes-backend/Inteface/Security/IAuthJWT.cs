using clothes_backend.Models;

namespace clothes_backend.Inteface.JWT
{
    public interface IAuthJWT
    {
        void verifyJWT();
        void hashToken();
        void generateToken(Users user, out string token);
    }
}
