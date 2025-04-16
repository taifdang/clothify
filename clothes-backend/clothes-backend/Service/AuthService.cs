using clothes_backend.Inteface.JWT;
using clothes_backend.Inteface.Security;
using clothes_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace clothes_backend.Service
{
    public class AuthService : IAuth,IAuthJWT
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void generateToken(Users user, out string token)
        {
            var token_handle = new JwtSecurityTokenHandler();       
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //
            var token_info = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.id.ToString()),
                    new Claim(ClaimTypes.Email,user.email),
                    new Claim(ClaimTypes.Role,user.role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var access_token = token_handle.CreateToken(token_info);
            token = token_handle.WriteToken(access_token);        
        }

        public void hashPassword(string password, out string passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(bytes));
                passwordSalt = hmac.Key;
            }
        }

        public void hashToken()
        {
            throw new NotImplementedException();
        }

        public void verifyJWT()
        {
            throw new NotImplementedException();
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
