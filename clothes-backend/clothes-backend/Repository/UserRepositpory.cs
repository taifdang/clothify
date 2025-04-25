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
using clothes_backend.Utils.General;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Mail;
namespace clothes_backend.Repository
{
    public class UserRepositpory : GenericRepository<Users>,IUsers
    {
        private readonly VerifyHandleService _auth;
        private readonly IMapper _mapper;
        private readonly MailKitHandle _mailHandle;
        //
        private readonly IHttpContextAccessor _context;
        private readonly IDistributedCache _cache;
        public UserRepositpory(
            DatabaseContext db, 
            VerifyHandleService auth, 
            IMapper mapper, 
            MailKitHandle mailKitHandle,
            IHttpContextAccessor contextAccessor,
            IDistributedCache cache
            ) : base(db)
        {
            _auth = auth;
            _mapper = mapper;
            _mailHandle = mailKitHandle;
            _context = contextAccessor;
            _cache = cache;
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
            //if (_mailHandle.sendMail(DTO.email,DTO.name) == true) return PayloadDTO<userInfoDTO>.Error(StatusCode.NotFound);
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
        //# user dang ky => luu tam thong tin vao cache, khi xac thuc OTP thanh cong => callback => luu vao database
        public void registerCache([FromForm] registerDTO DTO)
        {
            var sessionId = Guid.NewGuid().ToString();
            _context.HttpContext.Session.SetString("user_test", sessionId);
            var OTP = Random.Shared.Next(1000, 9999).ToString();
            //1.sessionId(5 phut) + otp  =>cache , enqueue => gui otp                    
            //luu cache [sessionId, sessionValue(OTP,regiterDTO)]
            //hash password
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
            var sessionValue = new RegisterSession()
            {
                otp = OTP,
                user = user,
            };
            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            //save cache
            _cache.SetAsync($"signup_{sessionId}", sessionValue,cacheOptions);
            //enqueue => send mail (email,otp)....
        }
    }
}
