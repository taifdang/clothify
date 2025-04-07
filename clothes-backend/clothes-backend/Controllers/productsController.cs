using clothes_backend.Inteface;
using clothes_backend.Models;
using clothes_backend.Service;
using clothes_backend.Utils;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _db;
       
        public productsController(IConfiguration configuration, DatabaseContext db)
        {
            this.configuration = configuration;
            this._db = db;          
        }
        [HttpGet]
        public IActionResult getList()
        {
            var data = _db.orders.ToList();          
            return Ok(data);
        }
        [HttpGet("filter")]
        public IActionResult filter(SortType type)
        {
            var data = _db.products.ToList();
            var sortStategy = SortTypeStategy.getSortType(type);
            var getListData = new SortService<Products>(sortStategy).GetList(data);
            return Ok(getListData);
        }
    }
}
