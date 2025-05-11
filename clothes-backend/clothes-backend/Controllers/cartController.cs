using clothes_backend.DTO.CART;
using clothes_backend.DTO.General;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cartController : ControllerBase
    {
        private readonly CartRepositoryOld _cartRepo;      
        private readonly ICartService _service;
        public cartController(ICartService service, CartRepositoryOld cartRepo)
        {
            _service = service;
            _cartRepo = cartRepo;
        }
        [HttpGet("get")]
        public async Task<IActionResult> getCart()
        {         
            var data = await _service.getCart();
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("add")]
        public async Task<IActionResult> addCartItem([FromForm] cartAddDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .ToDictionary(
                       error => error.Key,
                       error => error.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var fullErrorMessage =
                    string.Join(";", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));
                return BadRequest(Result<CartItems>.IsValid(fullErrorMessage));               
            }
            var data = await _service.addCartItem(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("update")]
        public async Task<IActionResult> updateCartItem([FromForm]CartItemDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .ToDictionary(
                       error => error.Key,
                       error => error.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var fullErrorMessage =
                    string.Join(";", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));
                return BadRequest(GenericResponse<CartItems>.Fail(fullErrorMessage));
            }
            var data = await _service.updateCartItem(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> deleteCartItem([FromForm] cartDeleteDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                   .Where(x => x.Value?.Errors.Count > 0)
                   .ToDictionary(
                       error => error.Key,
                       error => error.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var fullErrorMessage =
                    string.Join(";", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));
                return BadRequest(GenericResponse<CartItems>.Fail(fullErrorMessage));
            }
            var data = await _service.removeCartItem(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }     
    }
}
