using clothes_backend.DTO.USER;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly UserRepositpory _userRepo;
        public usersController(UserRepositpory userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromForm] loginDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Users>.Fail(ModelState.Values.ToString()));
            }
            Users data = (Users)await _userRepo.login(DTO);
            if (data == null) return BadRequest(GenericResponse<Users>.Fail(null));
            return Ok(GenericResponse<Users>.Success(data));
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
