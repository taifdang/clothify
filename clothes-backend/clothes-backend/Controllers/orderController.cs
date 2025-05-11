using clothes_backend.Data;
using clothes_backend.DTO.General;
using clothes_backend.DTO.ORDER;
using clothes_backend.Interfaces;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
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
        //private readonly OrderRepositoryOld _orderRepo;
        //private readonly DatabaseContext _db;
        private readonly IOrderService _service;
        public orderController(OrderRepositoryOld orderRepository, DatabaseContext db, IOrderService service)
        {          
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> add([FromForm]orderItemDTO DTO)
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
            var data = await _service.add(DTO);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var data = await _service.getAll();
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }
        [HttpGet("getId")]
        public async Task<IActionResult> getId(int id)
        {
            var data = await _service.getId(id);
            if (data.statusCode == Utils.Enum.StatusCode.Success) return BadRequest(data);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);
        }

    }
}
