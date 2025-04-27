using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces
{
    public interface IUsers
    {
        Task<PayloadDTO<TokenReponse>> login([FromForm] loginDTO DTO);
        Task<PayloadDTO<userInfoDTO>> register([FromForm] registerDTO DTO);
        Task<PayloadDTO<TokenReponse>> verify([FromForm] refreshTokenDTO DTO);
        Task<PayloadDTO<TokenReponse>> createToken(Users user);

    }
}
