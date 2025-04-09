using clothes_backend.DTO;
using clothes_backend.Inteface;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Utils;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DatabaseContext _db;
        private readonly ProductRepository _productRepo;

        public productsController(IConfiguration configuration, DatabaseContext db, ProductRepository productRepository)
        {
            this.configuration = configuration;
            this._db = db;
            _productRepo = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> getList()
        {
            var data = await _productRepo.get();
            return Ok(data);
        }
        [HttpGet("id")]
        public async Task<IActionResult> getId(int id)
        {
            var data = await _productRepo.getId(id);
            if (data is null) return BadRequest(GenericResponse<Products>.Fail());

            return Ok(GenericResponse<Products>.OK(data));
        }
        [HttpGet("filter")]
        public IActionResult filter(SortType type)
        {
            var data = _db.products.ToList();
            var sortStategy = SortTypeStategy.getSortType(type);
            var getListData = new SortService<Products>(sortStategy).GetList(data);
            return Ok(getListData);
        }
        [HttpGet("pagination")]
        public async Task<IActionResult> pagination()
        {
            var product = await _productRepo.get();
            var data = _productRepo.pagination(product, 1, 2);
            return Ok(data);
        }
        [HttpPost("add")]
        public async Task<ActionResult<GenericResponse<Products>>> add([FromForm] productDTO DTO)
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
                return BadRequest(GenericResponse<Products>.Fail(fullErrorMessage));
            }
            Products? result = await _productRepo.add(DTO);
            if (result == null)
            {
                return BadRequest(GenericResponse<Products>.Fail());
            }
            return Ok(GenericResponse<Products>.OK(result));
        }
        [HttpPost("update")]
        public async Task<IActionResult> update(int id, [FromForm] productDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Products>.Fail("ModelState.IsValid"));
            }
            var result = await _productRepo.update(id, DTO);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products>.OK(result));

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Products>.Fail("ModelState.IsValid"));
            }
            var result = await _productRepo.delete(id);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products?>.OK(null));

        }
    }
}
