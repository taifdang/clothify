using AutoMapper;
using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Interfaces.Repository;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Service;
using clothes_backend.Utils;
using clothes_backend.Utils.Enum;
using Hangfire;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace clothes_backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly VerifyHandleService _auth;
        private readonly IHttpContextAccessor _context;
        private readonly IDistributedCache _distributeCache;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        public UserService(
        IUserRepository userRepository,
        VerifyHandleService auth,
        IHttpContextAccessor context,
        IDistributedCache distributeCache,
        IBackgroundJobClient backgroundJobClient,
        IBackgroundJobService backgroundJobService,
        IUserContextService userContextService,
        IMapper mapper)
        {
            _userRepository = userRepository;
            _auth = auth;
            _context = context;
            _distributeCache = distributeCache;
            _backgroundJobClient = backgroundJobClient;
            _backgroundJobService = backgroundJobService;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public async Task<Result<TokenReponse>> createToken(Users user)
        {
            try
            {
                _auth.generateAccessToken(user, out string access_token);
                var refreshToken = await _auth.generateRefreshToken(user);
                var payload = new TokenReponse() { accessToken = access_token, refreshToken = refreshToken };
                return Result<TokenReponse>.Success(payload);
            }
            catch
            {
                return Result<TokenReponse>.Failure(StatusCode.Isvalid);
            }
        }

        public async Task<Result<userInfoDTO>> getUser()
        {
            var userId = _userContextService.convertToInt(_userContextService.getValueAuth());
            if (userId == 0) return Result<userInfoDTO>.Failure(StatusCode.Unauthorized);
            var user = await _userRepository.FindBase(x => x.id == userId);
            var userInfo = _mapper.Map<userInfoDTO>(user);
            return Result<userInfoDTO>.Success(userInfo);
        }

        public async Task<Result<TokenReponse>> login([FromForm] loginDTO DTO)
        {
            var result = await _userRepository.FindBase(x => x.email == DTO.email);
            if(result is null) return Result<TokenReponse>.Failure();
            if (!_auth.verifyPassword(DTO.password, result.password, result.passwordSalt)) return Result<TokenReponse>.Failure(StatusCode.Isvalid);
            var token = await createToken(result);
            return Result<TokenReponse>.Success(token.data);
        }

        public async Task<Result<Users>> logout()
        {
            try
            {
                var token = _context.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token)) return Result<Users>.Failure(StatusCode.Unauthorized);
                //luu token
                await _distributeCache.SetAsync($"bl_{token}", token, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                return Result<Users>.Success();
            }
            catch
            {
                return Result<Users>.Failure(StatusCode.Isvalid);
            }          
        }

        public async Task<Result<Users>> register([FromForm] registerDTO DTO)
        {          
            var IsEmail = await _userRepository.FindBase(x => x.email == DTO.email);//kiem tra email
            if (IsEmail is not null) return Result<Users>.Failure(StatusCode.Isvalid);        
            var sessionId = Guid.NewGuid().ToString(); //session-OTP
            _context.HttpContext?.Session.SetString("user_test", sessionId);
            var OTP = Random.Shared.Next(1000, 9999).ToString();//luu cache:[sessionId,(OTP,User)] - 5 phut
            _auth.hashPassword(DTO.password, out string passwordHash, out byte[] passwordSalt);//hash pass         
            var user = new Users() {email=DTO.email,name=DTO.name,phone=DTO.phone,password=passwordHash,passwordSalt=passwordSalt };
            var sessionValue = new SessionValue { otp = OTP, user = user };            
            var optionCache = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            await _distributeCache.SetAsync($"signup_{sessionId}", sessionValue, optionCache);//save cache
            _backgroundJobClient.Schedule(() => _backgroundJobService.DelayedJob(user.email, OTP),TimeSpan.FromSeconds(5));//background job
            return Result<Users>.Success();
        }
        public async Task<Result<Users>> verifyOTP(string OTP)
        {
            string sessionId = _context.HttpContext?.Session.GetString("user_test")?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(sessionId)) return Result<Users>.Failure(StatusCode.Unauthorized);
            //GetAllBase cache
            if (_distributeCache.TryGetValue($"signup_{sessionId}", out SessionValue? sessionValue))
            {
                if (sessionValue?.otp == OTP)
                {
                    try
                    {
                        await _userRepository.AddBase(sessionValue.user);//save user                       
                        _distributeCache.Remove($"signup_{sessionId}");//remove cache                   
                        _context.HttpContext?.Session.Remove("user_test");//remove session
                        return Result<Users>.Success();
                    }
                    catch
                    {
                        return Result<Users>.Failure(StatusCode.Isvalid);
                    }
                }
            }
            return Result<Users>.Failure(StatusCode.Unauthorized);
        }
        public async Task<Result<TokenReponse>> verifyToken([FromForm] refreshTokenDTO DTO)
        {
            var user = await _auth.verifyJWT(DTO.user_id, DTO.refreshToken);
            var data = await createToken(user!);
            return data;
        }
    }
}
