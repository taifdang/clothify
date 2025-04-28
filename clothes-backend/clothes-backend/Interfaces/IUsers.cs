using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces
{
    public interface IUsers
    {
        Task<Result<TokenReponse>> login([FromForm] loginDTO DTO);
        Task<Result<userInfoDTO>> register([FromForm] registerDTO DTO);
        Task<Result<TokenReponse>> verify([FromForm] refreshTokenDTO DTO);
        Task<Result<TokenReponse>> createToken(Users user);

    }
}
