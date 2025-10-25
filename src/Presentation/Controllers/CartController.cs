using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/carts")]
public class CartController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCart()
    {
        throw new NotImplementedException();
    }

    [HttpPost("items")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddCartItem(int id)
    {
        throw new NotImplementedException();
    }
    [HttpPut("items/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCartItem(int id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("items/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("clear")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ClearCart(int id)
    {
        throw new NotImplementedException();
    }
}
