using Azure.Core;
using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clothes_backend.DTO.General;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly UserRepositpory _userRepo;
        private readonly AuthService _auth;
        public usersController(UserRepositpory userRepo, AuthService auth)
        {
            _userRepo = userRepo;
            _auth = auth;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromForm] loginDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Users>.Fail(ModelState.Values.ToString()));
            }
            var data = await _userRepo.login(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return Unauthorized(data);
          
            return Ok(data);
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] registerDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Users>.Fail(ModelState.Values.ToString()));
            }
            var data = await _userRepo.register(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> refreshToken([FromForm]refreshTokenDTO DTO)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Users>.Fail(ModelState.Values.ToString()));
            }
            var data = await _userRepo.verify(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("get-id")]
        public async Task<IActionResult> test_get_id(int id)
        {
            var data = await _userRepo.get_user(id);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
    }
}
