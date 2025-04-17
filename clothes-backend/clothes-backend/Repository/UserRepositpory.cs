using AutoMapper;
using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Inteface;
using clothes_backend.Inteface.Security;
using clothes_backend.Inteface.User;
using clothes_backend.Models;
using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace clothes_backend.Repository
{
    public class UserRepositpory : GenericRepository<Users>,IUsers
    {
        private readonly AuthService _auth;
        private readonly IMapper _mapper;
        public UserRepositpory(DatabaseContext db, AuthService auth, IMapper mapper) : base(db)
        {
            _auth = auth;
            _mapper = mapper;
        }      
        public async Task<object?> login([FromForm] loginDTO DTO)
        {
            var user = await _db.users.FirstOrDefaultAsync(x => x.email == DTO.email);
            if (user == null) return null;
            //verify
            if (!_auth.verifyPassword(DTO.password, user.password, user.passwordSalt)) return null;          
            return user;
        }
        public async Task<object?> register([FromForm] registerDTO DTO)
        {
            if (await _db.users.AnyAsync(x => x.email == DTO.email)) return null;
            //hash password
            try
            {
                _auth.hashPassword(DTO.password, out string passwordHash, out byte[] passwordSalt);
                var user = new Users()
                {
                    email = DTO.email,
                    phone = DTO.phone,
                    password = passwordHash,
                    passwordSalt = passwordSalt,
                    name = DTO.name,
                    role = "User",
                    is_lock = false,
                };
                _db.users.Add(user);
                await _db.SaveChangesAsync();
                return user;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            } 
        }     
        public async Task<PayloadDTO<userInfoDTO>> get_user(int id)
        {           
            var data = await _db.users.FirstOrDefaultAsync(x => x.id == id);
            if (data == null) return PayloadDTO<userInfoDTO>.Error(StatusCode.NotFound);
            //tranfers data
            var result = _mapper.Map<userInfoDTO>(data);
            return PayloadDTO<userInfoDTO>.OK(result);
        }
    }
}
