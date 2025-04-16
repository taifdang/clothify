using clothes_backend.Inteface.Security;
using System.Security.Cryptography;
using System.Text;

namespace clothes_backend.Service
{
    public class AuthService : IAuth
    {
        public void hashPassword(string password, out string passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(bytes));
                passwordSalt = hmac.Key;
            }
        }
        public bool verifyPassword(string password, string storeHash, byte[] storeSalt)//no key => random => salt
        {
            using (var hmac = new HMACSHA512(storeSalt))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] computeHash = hmac.ComputeHash(bytes);
                bool isMatch = computeHash.SequenceEqual(Convert.FromBase64String(storeHash));
                return isMatch;
            }
        }
    }
}
