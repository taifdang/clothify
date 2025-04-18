using clothes_backend.DTO.CART;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cartController : ControllerBase
    {
        private readonly CartRepository _cartRepo;
        public cartController(CartRepository cartRepo)
        {
            _cartRepo = cartRepo;
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
        [HttpGet]
        public async Task<IActionResult> get()
        {
            var data = _cartRepo.getIsUser();
            return Ok(data);        
        }
        
    }
}
