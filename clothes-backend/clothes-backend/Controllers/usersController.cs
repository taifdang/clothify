using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            Users data = (Users)await _userRepo.login(DTO);
            if (data == null) return Unauthorized("Invalid client");
            _auth.generateToken(data, out string access_token);
            return Ok(access_token);
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] registerDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Users>.Fail(ModelState.Values.ToString()));
            }
            Users data = (Users)await _userRepo.register(DTO);
            if (data == null) return BadRequest(GenericResponse<Users>.Fail(null));
            return Ok(GenericResponse<Users>.Success(data));
        }
    }
}
