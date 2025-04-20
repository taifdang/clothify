using clothes_backend.DTO.ORDER;
using clothes_backend.Models;
using clothes_backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {
        private readonly OrderRepository _orderRepo;
        public orderController(OrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }
        [HttpPost]
        public async Task<IActionResult> add([FromForm]orderItemDTO DTO)
        {
            var data = await _orderRepo.add(DTO);
            return Ok(data);
        }
    }
}
