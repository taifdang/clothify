using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Service
{
    public interface IUserService
    {
        Task<Result<TokenReponse>> login([FromForm] loginDTO DTO);
        Task<Result<Users>> register([FromForm] registerDTO DTO);
        Task<Result<TokenReponse>> verifyToken([FromForm] refreshTokenDTO DTO);
        Task<Result<TokenReponse>> createToken(Users user);
        Task<Result<Users>> verifyOTP(string OTP);
        Task<Result<Users>> logout();
        Task<Result<userInfoDTO>> getUser();
    }
}
