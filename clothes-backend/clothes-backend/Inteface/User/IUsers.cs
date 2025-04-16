using clothes_backend.DTO.USER;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Inteface.User
{
    public interface IUsers
    {
        Task<object?> login([FromForm]loginDTO DTO);
        Task<object?> register([FromForm]registerDTO DTO);

    }
}
