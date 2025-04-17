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
        private readonly DatabaseContext _db;
        public AuthService(IConfiguration configuration, DatabaseContext db)
        {
            _config = configuration;
            _db = db;
        }
        public void generateAccessToken(Users user, out string token)
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

        public void generateRefreshToken(Users user, out string refreshToken)
        {
            refreshToken = generateToken();
            //save
            user.refresh_token = refreshToken;
            user.expiry_time = DateTime.UtcNow.AddHours(1);
            _db.SaveChanges();
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

        public string generateToken()
        {
            var token_random = new byte[64];
            using (var number = RandomNumberGenerator.Create())
            {
                number.GetBytes(token_random);
                return Convert.ToBase64String(token_random);
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

        public object? verifyJWT(int user_id, string refresh_token)
        {
            var user = _db.users.FirstOrDefault(x => x.id == user_id);          
            if(user is null||user.refresh_token != refresh_token || user.expiry_time <= DateTime.UtcNow) return null;
            return user;
            
        }
    }
}
