using Azure.Core;
using clothes_backend.DTO.USER;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clothes_backend.DTO.General;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using clothes_backend.Data;
using clothes_backend.Models;
using clothes_backend.Interfaces.Service;
using static System.Net.WebRequestMethods;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly UserRepositpory _userRepo;
        private readonly IUserService _userService;
      
        private readonly DatabaseContext _db;
        public usersController(UserRepositpory userRepo, DatabaseContext database,IUserService userService)
        {
            _userRepo = userRepo;          
            _db = database;
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromForm] loginDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<Users>.IsValid(ModelState.Values.ToString()));
            }
            var user = await _userService.login(DTO);
            if (user.statusCode != Utils.Enum.StatusCode.Success) return Unauthorized(user);
            return Ok(user);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> register([FromForm] registerDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<Users>.IsValid(ModelState.Values.ToString()));
            }
            var data = await _userService.register(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> refreshToken([FromForm]refreshTokenDTO DTO)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(Result<Users>.IsValid(ModelState.Values.ToString()));
            }
            var data = await _userService.verifyToken(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [NonAction]
        [HttpPost("get-id")]
        public async Task<IActionResult> test_get_id(int id)
        {
            var data = await _userRepo.get_user(id);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }     
        [HttpPost("logout")]
        public async Task<IActionResult> logout2()
        {
            var result = await _userService.logout();
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
        }
        [NonAction]
        [HttpGet("getSessionId")]
        public async Task<IActionResult> getSessionId()
        {
            return Ok(HttpContext.Session.GetString("user_test"));
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> verifyOPT(string OTP)
        {
            var result = await _userService.verifyOTP(OTP);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);         
            return Ok(result);

        }     
    }
}
