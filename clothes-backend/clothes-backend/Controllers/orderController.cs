using clothes_backend.DTO.ORDER;
using clothes_backend.Inteface;
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
            var data = await _orderRepo.addorder(DTO);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> test(int product_varinat_id, int slmua)
        {
            //sl ban
            int slban = await getCacheSold(product_varinat_id);
            var slkho = await _db.product_variants.AsNoTracking().Where(y=>y.id == product_varinat_id).Select(x => x.quantity).FirstOrDefaultAsync();             
            if(slban + slmua > slkho)
            {
                return BadRequest("HET HANG");
            }
            slban += slmua;
            if(slban > slkho) return BadRequest("LOI DAT HANG");          
            _db.SaveChanges();
            return Ok("DAT HANG THANH CONG");
        }
        [NonAction]
        public async Task<int> getCacheSold(int product_varinat_id)
        {
            string cacheKey = $"cache_variant_{product_varinat_id}";
            if (_cache.isCached(cacheKey))
            {
                return _cache.Get<int>(cacheKey);
            }
            //??
            var slban = await _db.order_history.AsNoTracking().Where(y => y.id == product_varinat_id).Select(x => x.sold_quantity).FirstOrDefaultAsync();
            _cache.Set(cacheKey, slban, TimeSpan.FromMinutes(60));
            return slban;
        }
    }
}
