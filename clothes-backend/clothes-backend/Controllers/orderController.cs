using clothes_backend.Data;
using clothes_backend.DTO.ORDER;
using clothes_backend.Interfaces.Service;
using clothes_backend.Repository;
using clothes_backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {
        private readonly OrderRepository _orderRepo;
        private readonly DatabaseContext _db;
        private readonly ICacheService _cache;
        public orderController(OrderRepository orderRepository, DatabaseContext db, ICacheService cache)
        {
            _orderRepo = orderRepository;
            _db = db;
            _cache = cache;
        }
        [HttpPost]
        public async Task<IActionResult> add([FromForm]orderItemDTO DTO)
        {
            var data = await _orderRepo.add(DTO);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var data = await _orderRepo.getAll();
            return Ok(data);
        }
        [HttpGet("get-id")]
        public async Task<IActionResult> getId(int id)
        {
            var data = await _orderRepo.getId(id);
            if (data.statusCode == Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }

    }
}
