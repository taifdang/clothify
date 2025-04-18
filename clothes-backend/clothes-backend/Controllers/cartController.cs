using clothes_backend.DTO.CART;
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
        private readonly CartRepository _cartRepo;
        private readonly DatabaseContext _db;
        public cartController(CartRepository cartRepo, DatabaseContext db)
        {
            _cartRepo = cartRepo;
            _db = db;
        }
        [HttpGet("getCart")]
        public async Task<IActionResult> getId()
        {         
            var data = await _cartRepo.getCart();
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpPost("addCartItem")]
        public async Task<IActionResult> addCartItem([FromForm]CartItemDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<CartItems>.Fail(ModelState.Values.ToString()));
            }
            var data = await _cartRepo.addCartItem(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> deleteCartItem(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<CartItems>.Fail(ModelState.Values.ToString()));
            }
            var data = await _cartRepo.removeCartItem(id);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }     
    }
}
