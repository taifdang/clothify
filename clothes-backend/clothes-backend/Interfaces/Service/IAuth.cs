namespace clothes_backend.Interfaces.Service
{
    public interface IAuth
    {
        bool verifyPassword(string password, string storeHash, byte[] storeSalt);
        void hashPassword(string password, out string passwordHash, out byte[] passwordSalt);
    }
}
