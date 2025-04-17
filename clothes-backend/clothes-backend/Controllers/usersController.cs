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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly UserRepositpory _userRepo;
        private readonly AuthService _auth;
        private readonly DatabaseContext _db;
        public usersController(UserRepositpory userRepo, AuthService auth, DatabaseContext database)
        {
            _userRepo = userRepo;
            _auth = auth;
            _db = database;
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
        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            var token_auth = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //check user have token_auth
            if (string.IsNullOrEmpty(token_auth)) return null;
            //read token => get expiry(datetime) of current access_token
            var access_token = new JwtSecurityTokenHandler().ReadToken(token_auth) as JwtSecurityToken;
            var expiry = access_token.ValidTo;
            try
            {
                var bl_token = new BlackListToken()
                {
                    token = string.Join("_","bl", token_auth),
                    create_at = expiry
                };
                _db.blacklist_token.Add(bl_token);
                await _db.SaveChangesAsync();
                return Ok(bl_token);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex);
            }
        }
    }
}
