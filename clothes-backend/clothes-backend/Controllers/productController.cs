using clothes_backend.Data;
using clothes_backend.DTO.General;
using clothes_backend.DTO.PRODUCT;
using clothes_backend.Interfaces.Service;
using clothes_backend.Models;
using clothes_backend.Repository;
using clothes_backend.Service;
using clothes_backend.Utils.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace clothes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : ControllerBase
    {  
        private readonly IProductService _productService;      
        public productController(IProductService productService)
        {                              
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAllProductAsync();
            return Ok(data);
        }       
        [HttpGet("id")]
        public async Task<IActionResult> GetId(int id)
        {
            var result = await _productService.GetId(id);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("filter")]
        public IActionResult filter(SortType type,List<Products> products)
        {         
            var sortStategy = SortTypeStategy.getSortType(type);
            var getListData = new SortService<Products>(sortStategy).GetList(products);
            return Ok(getListData);
        }       
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] ProductDTO DTO)
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
                    string.Join(" ;", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));           
                return BadRequest(Result<Products>.IsValid(fullErrorMessage));
            }
            var result = await _productService.AddAsync(DTO);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDTO DTO)
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
                    string.Join(" ;", errors.Select(error => $"{error.Key}: {string.Join(", ", error.Value)}"));
                return BadRequest(Result<Products>.IsValid(fullErrorMessage));
            }
            var result = await _productService.UpdateAsync(id, DTO);
            if (result.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(result);
            return Ok(result);

        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _productService.DeleteAsync(id);
            if (data.statusCode != Utils.Enum.StatusCode.Success) return BadRequest(data);
            return Ok(data);          
        }
    }
}
