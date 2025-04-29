using clothes_backend.DTO.General;
using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Interfaces.Repository
{
    public interface IUserRepository:IBaseRepository<Users>
    {
        //Task<Result<Users>> login([FromForm] loginDTO DTO);
        //Task<Result<userInfoDTO>> register([FromForm] registerDTO DTO);
        
        //Task<Result<TokenReponse>> verifyToken([FromForm] refreshTokenDTO DTO);
        //Task<Result<TokenReponse>> createToken(Users user);

    }
}
