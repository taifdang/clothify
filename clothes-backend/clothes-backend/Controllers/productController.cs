using clothes_backend.Data;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {
           
        private readonly ProductRepository _productRepo;
        private readonly IProductService _productService;
        public productController(ProductRepository productRepository, IProductService productService)
        {            
          
            _productRepo = productRepository;
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> getList()
        {
            var data = await _productService.GetAllProductAsync();
            return Ok(data);
        }
        [HttpGet("test")]
        public async Task<IActionResult> getListTest()
        {
            var data = await _productRepo.getTest();
            return Ok(data);
        }
        [HttpGet("id")]
        public async Task<IActionResult> getId(int id)
        {
            var data = await _productRepo.getId(id);
            if (data is null) return BadRequest(GenericResponse<Products>.Fail());

            return Ok(GenericResponse<Products>.Success(data));
        }
        [HttpGet("filter")]
        public IActionResult filter(SortType type,List<Products> products)
        {         
            var sortStategy = SortTypeStategy.getSortType(type);
            var getListData = new SortService<Products>(sortStategy).GetList(products);
            return Ok(getListData);
        }
        [HttpGet("pagination")]
        public async Task<IActionResult> pagination()
        {
            //var product = await _productRepo.get();
            //var data = _productRepo.pagination(product, 1, 2);
            return Ok();
        }
        [HttpPost("add")]
        public async Task<ActionResult<GenericResponse<Products>>> add([FromForm] ProductDTO DTO)
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
            return Ok(GenericResponse<Products>.Success(result));
        }
        [HttpPost("update")]
        public async Task<IActionResult> update(int id, [FromForm] ProductDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Products>.Fail("ModelState.IsValid"));
            }
            var result = await _productRepo.update(id, DTO);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products>.Success(result));

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericResponse<Products>.Fail("ModelState.IsValid"));
            }
            var result = await _productService.DeleteProduct(id);
            if (result == null) return BadRequest(GenericResponse<Products>.Fail());
            return Ok(GenericResponse<Products?>.Success(null));
            
        }
    }
}
