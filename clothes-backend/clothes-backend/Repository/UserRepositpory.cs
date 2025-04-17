using AutoMapper;
using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Inteface;
using clothes_backend.Inteface.Security;
using clothes_backend.Inteface.User;
using clothes_backend.Models;
using clothes_backend.Service;
using clothes_backend.Utils;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PayloadDTO<TokenReponse>> login([FromForm] loginDTO DTO)
        {
            var user = await _db.users.FirstOrDefaultAsync(x => x.email == DTO.email);
            if (user == null) return PayloadDTO<TokenReponse>.Error(StatusCode.NotFound);
            //verify
            if (!_auth.verifyPassword(DTO.password, user.password, user.passwordSalt)) return PayloadDTO<TokenReponse>.Error(StatusCode.Isvalid);         
            var data = await createToken(user);
            return data;

        }
        public async Task<PayloadDTO<TokenReponse>> createToken(Users user)
        {
            try
            {
                _auth.generateAccessToken(user, out string access_token);
                var refreshToken = await _auth.generateRefreshToken(user);
                var payload = new TokenReponse() { accessToken = access_token, refreshToken = refreshToken };
                return PayloadDTO<TokenReponse>.OK(payload);
            }
            catch
            {
                return PayloadDTO<TokenReponse>.Error(StatusCode.Isvalid);
            }
        }
        public async Task<PayloadDTO<TokenReponse>> verify([FromForm] refreshTokenDTO DTO)
        {
            var user = await _auth.verifyJWT(DTO.user_id, DTO.refreshToken);
            var data = await createToken(user!);
            return data;
        }
        public async Task<PayloadDTO<userInfoDTO>> register([FromForm] registerDTO DTO)
        {
            if (await _db.users.AnyAsync(x => x.email == DTO.email)) return PayloadDTO<userInfoDTO>.Error(StatusCode.NotFound);
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
                //mapper
                var data = _mapper.Map<userInfoDTO>(user);

                return PayloadDTO<userInfoDTO>.OK(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return PayloadDTO<userInfoDTO>.Error(StatusCode.None);
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
        //public async Task<PayloadDTO<Users>> logout()
        //{
        //    //get token from header
        //    var token = 
        //    var bl_token = new BlackListToken()
        //    {
        //        token = string.Join("_", "bl",)
        //    };
        //    _db.blacklist_token.Add()
        //    return PayloadDTO<Users>.OK(user);


        //}
    }
}
